using MelonLoader;
using BTD_Mod_Helper;
using BloonsTDClasses;
using BTD_Mod_Helper.Extensions;
using System.IO;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Utils;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu.TowerSelectionMenuThemes;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;

[assembly: MelonInfo(typeof(BloonsTDClass.MelonMain), ModHelperData.Name, ModHelperData.Version, ModHelperData.Author)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace BloonsTDClass;

public class MelonMain : BloonsTD6Mod
{
    public static bool bloonspeedchange = false;
    public static int freefarmcount = 0;
    public static string filepath = MelonHandler.ModsDirectory;
    public static string txtpath = filepath + @"\ClassesMod\ClassesData.txt";
    public override void OnApplicationStart()
    {

        MelonLogger.Msg("BloonsTDClasses loaded!");
        if (!Directory.Exists(filepath + @"\ClassesMod"))
        {
            Directory.CreateDirectory(filepath + @"\ClassesMod");
            MelonLogger.Msg(System.ConsoleColor.Green, "Created Data Folder");
        }
        if (!File.Exists(filepath + @"\ClassesMod\ClassesData.json"))
        {
            SaveInfo saveInfo = new SaveInfo
            {
                Class = "Default",
                Image = VanillaSprites.PrimaryBtn2,
                About = "The Default Class, this class has nothing special",
                Desc = "Pros" + "\n" + "- None" + "\n" + "Cons" + "\n" + "- None",
                DescSize = 90,
                Width = 250,
                Height = 250
            };
            MelonLogger.Msg(System.ConsoleColor.Green, "Created SaveData File");
            JsonSerializer.instance.SaveToFile<SaveInfo>(saveInfo, filepath + @"\ClassesMod\ClassesData.json");
        }
        else
        {
        }
        if (File.Exists(filepath + @"\ClassesMod\ClassesData.json"))
        {
            SaveInfo info = JsonSerializer.instance.LoadFromFile<SaveInfo>(filepath + @"\ClassesMod\ClassesData.json");
            try
            {
                GlobalVar.About = info.About;
                GlobalVar.Class = info.Class;
                GlobalVar.Desc = info.Desc;
                GlobalVar.DescSize = info.DescSize;
                GlobalVar.Height = info.Height;
                GlobalVar.Width = info.Width;
                GlobalVar.Image = info.Image;
                MelonLogger.Msg(System.ConsoleColor.Green, "Loaded Data From Save File");
            }
            catch
            {
                MelonLogger.Msg(System.ConsoleColor.Red, "Failed To Load From Save File");
                SaveInfo saveInfo = new SaveInfo
                {
                    Class = "Default",
                    Image = VanillaSprites.PrimaryBtn2,
                    About = "The Default Class, this class has nothing special",
                    Desc = "Pros" + "\n" + "- None" + "\n" + "Cons" + "\n" + "- None",
                    DescSize = 90,
                    Width = 250,
                    Height = 250
                };
                File.Delete(filepath + @"\ClassesMod\ClassesData.json");
                MelonLogger.Msg(System.ConsoleColor.Green, "Deleted Faulty Save File");

                JsonSerializer.instance.SaveToFile<SaveInfo>(saveInfo, filepath + @"\ClassesMod\ClassesData.json");
                MelonLogger.Msg(System.ConsoleColor.Green, "Created New SaveData File");
            }
        }
        else
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "Failed To Locate Save File");
        }
    }
    public override void OnDeinitializeMelon()
    {
        base.OnDeinitializeMelon();
        Save();
    }
    public static void Save()
    {
        SaveInfo info = new SaveInfo()
        {
            Class = GlobalVar.Class,
            Image = GlobalVar.Image,
            About = GlobalVar.About,
            Desc = GlobalVar.Desc,
            DescSize = GlobalVar.DescSize,
            Width = GlobalVar.Width,
            Height = GlobalVar.Height
        };
        JsonSerializer.instance.SaveToFile(info, "Mods/ClassesMod/ClassesData.json");
        MelonLogger.Msg("Saved Data Successfully :)");
    }
    public override void OnMatchStart()
    {
        InGamePanel.Show();
        if (GlobalVar.Class == "Necromancer")
        {
        }
        if (GlobalVar.Class == "Commander")
        {
            AbilityUI.Show();
            GlobalVar.ability = true;
            GlobalVar.abilitycooldown = 30;
        }
        else { AbilityUI.Hide(); }
        base.OnMatchStart();
        GlobalVar.ingame = true;
        freefarmcount = 0;
    }
    public override void OnMatchEnd()
    {
        switch (GlobalVar.Class)
        {
            case "Commander":
                if (GlobalVar.abilityactive == true)
                {
                    foreach (var tower in InGame.instance.bridge.GetAllTowers())
                    {
                        var newModel = tower.tower.rootModel.Clone().Cast<TowerModel>();
                        if (newModel.GetWeapons() != null)
                        {
                            foreach (var weapon in newModel.GetWeapons())
                            {
                                weapon.projectile.GetDamageModel().damage -= 3;
                                weapon.Rate *= 2f;
                                weapon.projectile.pierce /= 3;
                                if (newModel.baseId == "BombShooter" || newModel.baseId == "MortarMonkey")
                                {
                                    weapon.Rate *= 2f;
                                }
                            }
                        }
                        tower.tower.UpdateRootModel(newModel);
                    }
                    MelonLogger.Msg("Expired");
                    GlobalVar.abilityactive = false;
                }
                break;
        }
        InGamePanel.Hide();
        AbilityUI.Hide();
        base.OnMatchEnd();
        GlobalVar.ingame = false;
        GlobalVar.ability = false;
    }
    public override void OnVictory()
    {
        Save();
    }
    public override void OnUpdate()
    {
        
        if (GlobalVar.ingame == true)
        {
            if (GlobalVar.ability)
            {
                if (GlobalVar.abilityshowing == true && TimeManager.gamePaused == true) { AbilityUI.Hide(); }
                if (GlobalVar.abilityshowing == false && TimeManager.gamePaused == false) { AbilityUI.Show(); }
                var time = UnityEngine.Time.deltaTime;
                if (GlobalVar.abilitycooldown > 0.001)
                {
                    GlobalVar.abilitycooldown -= time;
                }
                else if (GlobalVar.abilitycooldown != 0)
                {
                    GlobalVar.abilitycooldown = 0;
                    AbilityUI.button.Button.image.SetSprite(AbilityUI.ReadyImage);
                }
                if (GlobalVar.abilityactive == true)
                {
                    GlobalVar.abilitydur -= time;
                }
                if (GlobalVar.abilitydur < 1)
                {
                    if (GlobalVar.abilityactive == true)
                    {
                        foreach (var tower in InGame.instance.bridge.GetAllTowers())
                        {
                            var newModel = tower.tower.rootModel.Clone().Cast<TowerModel>();
                            if (newModel.GetWeapons() != null)
                            {
                                foreach (var weapon in newModel.GetWeapons())
                                {
                                    weapon.projectile.GetDamageModel().damage -= 3;
                                    weapon.Rate *= 2f;
                                    weapon.projectile.pierce /= 3;
                                    if (newModel.baseId == "BombShooter" || newModel.baseId == "MortarMonkey")
                                    {
                                        weapon.Rate *= 2f;
                                    }
                                }
                            }
                            tower.tower.UpdateRootModel(newModel);
                        }
                        MelonLogger.Msg("Expired");
                        GlobalVar.abilityactive = false;
                    }
                    GlobalVar.abilitydur = 0;
                }

                AbilityUI.text.SetText((int)GlobalVar.abilitycooldown + "s");
            }
        }
    }
    public override void OnRoundEnd()
    {
        if (GlobalVar.Class == "Economist")
        {
            freefarmcount++;
            if (freefarmcount == 15)
            {
                InGame.instance.GetTowerInventory().AddFreeTowers(TowerType.BananaFarm, 1, "", 0);//
                freefarmcount = 0;
            }
        }
        base.OnRoundEnd();
    }
    public override void OnTowerCreated(Tower tower, Entity target, Model modelToUse)
    {
        if (GlobalVar.Class == "Necromancer")
        {
            NecroTowers(tower);
        }
        CommanderTowersAbility(tower, modelToUse.Cast<TowerModel>());
        EconomistTowers(tower, modelToUse.Cast<TowerModel>());
        EtherealTower(tower, modelToUse.Cast<TowerModel>());
        //PyrotechnicTower(tower, modelToUse.Cast<TowerModel>());
    }
    public override void OnTowerUpgraded(Tower tower, string upgradeName, TowerModel newBaseTowerModel)
    {
        base.OnTowerUpgraded(tower, upgradeName, newBaseTowerModel);
        if (GlobalVar.Class == "Necromancer")
        {
            NecroTowers(tower, upgradeName, newBaseTowerModel);
        }
        CommanderTowersAbility(tower, newBaseTowerModel);
        EconomistTowers(tower, newBaseTowerModel);
        EtherealTower(tower, newBaseTowerModel);
        //PyrotechnicTower(tower, newBaseTowerModel);
    }
    public static void NecroTowers(Tower tower, string upgradeName = null, TowerModel newBaseTowerModel = null)
    {
        var towerModel = tower.rootModel.Clone().Cast<TowerModel>();
        switch (towerModel.baseId)
        {
            case "WizardMonkey":
                if (tower.towerModel.tiers[2] == 4)
                {
                    var newEmi = new NecromancerEmissionModel("EmissionModel", 1000, 50, 1, 5, 10, 50, 5, null, null, null, 5, 100, 10, 200, 2);
                    tower.towerModel.behaviors.First(a => a.name == "AttackModel_Attack Necromancer_").Cast<AttackModel>().weapons[0].emission.Cast<NecromancerEmissionModel>().maxRbeStored = 1000;
                    foreach (var weapon in towerModel.GetWeapons())
                    {
                        weapon.Rate *= .5f;
                    }
                }
                if (tower.towerModel.tiers[2] == 5)
                {
                    foreach (var weapon in towerModel.GetWeapons())
                    {
                        weapon.Rate *= .5f;
                    }
                }
                break;
            case "Ezili":
                foreach (var weapon in towerModel.GetWeapons())
                {
                    weapon.Rate *= .75f;
                }
                foreach (var abilitymod in towerModel.GetAbilities())
                {
                    abilitymod.Cooldown *= .75f;
                }
                break;
            case "IceMonkey":
                if (towerModel.tiers[0] >= 3)
                {
                    foreach (var weapon in towerModel.GetWeapons())
                    {
                        weapon.Rate *= .85f;
                    }
                }
                break;
            case "MonkeyBuccaneer":
                if (towerModel.tiers[1] >= 3)
                {
                    foreach (var weapon in towerModel.GetWeapons())
                    {
                        weapon.Rate *= .85f;
                    }
                }
                break;
            case "DartlingGunner":
                if (towerModel.tiers[1] >= 3)
                {
                    foreach (var weapon in towerModel.GetWeapons())
                    {
                        weapon.Rate *= .85f;
                    }
                }
                if (towerModel.tiers[0] >= 4)
                {
                    foreach (var weapon in towerModel.GetWeapons())
                    {
                        weapon.Rate *= .85f;
                    }
                }
                break;
            case "SuperMonkey":
                if (towerModel.tiers[1] >= 3)
                {
                    foreach (var weapon in towerModel.GetWeapons())
                    {
                        weapon.Rate *= .85f;
                    }
                }
                break;
            case "NinjaMonkey":
                if (towerModel.tiers[2] >= 3)
                {
                    foreach (var weapon in towerModel.GetWeapons())
                    {
                        weapon.Rate *= .85f;
                    }
                }
                break;
            case "Alchemist":
                if (towerModel.tiers[1] >= 4)
                {
                    foreach (var weapon in towerModel.GetWeapons())
                    {
                        weapon.Rate *= .85f;
                    }
                }
                break;
            case "Druid":
                if (towerModel.tiers[2] >= 2)
                {
                    foreach (var weapon in towerModel.GetWeapons())
                    {
                        weapon.Rate *= .85f;
                    }
                }
                break;
            case "SpikeFactory":
                if (towerModel.tiers[0] >= 3)
                {
                    foreach (var weapon in towerModel.GetWeapons())
                    {
                        weapon.Rate *= .85f;
                    }
                }
                break;
            case "MonkeyVillage":
                foreach (var attackmodel in towerModel.GetAttackModels())
                {
                    attackmodel.range *= .7f;
                }
                break;

        }
        tower.UpdateRootModel(towerModel);
    }
    public static void CommanderTowersAbility(Tower tower, TowerModel towerModel)
    {
        var Model = towerModel.Duplicate();
        if (GlobalVar.Class == "Commander")
        {
            if (GlobalVar.abilitydur > 0)
            {
                if (Model.GetWeapons() != null)
                {
                    foreach (var weapon in Model.GetWeapons())
                    {
                        if (weapon.projectile.GetDamageModel() != null)
                        {
                            weapon.projectile.GetDamageModel().damage += 3;
                            weapon.Rate *= .5f;
                            weapon.projectile.pierce *= 3;
                            if (Model.baseId == "BombShooter" || towerModel.baseId == "MortarMonkey")
                            {
                                weapon.Rate *= .5f;
                            }
                        }
                    }
                }
            }
        }
        tower.UpdateRootModel(Model);
        
    }
    public static void EconomistTowers(Tower tower, TowerModel towerModel)
    {
        var Model = towerModel.Duplicate();
        if (GlobalVar.Class == "Economist")
        {
            if (Model.GetWeapons() != null)
            {
                foreach (var weapon in Model.GetWeapons())
                {
                    if (weapon.projectile.GetDamageModel() != null)
                    {
                        if (weapon.projectile.GetDamageModel().damage > 1)
                        {
                            weapon.projectile.GetDamageModel().damage -= 1;
                        }
                        if (weapon.projectile.pierce > 2)
                        {
                            weapon.projectile.pierce -= 2;
                        }
                    }
                    if (!Model.IsHero())
                    {
                        if (Model.tiers.Max() > 3)
                        {
                            weapon.Rate *= 1.1f;
                        }
                    }
                }
            }
            if (Model.baseId == TowerType.Benjamin)
            {
                if (Model.tiers[0] > 4)
                {
                    Model.GetBehavior<BananaCashIncreaseSupportModel>().multiplier += .1f;
                }
            }
        }
        tower.UpdateRootModel(Model);

    }
    public static void TinyTower(Tower tower, TowerModel towerModel)
    {
        var Model = towerModel.Duplicate();
        if (GlobalVar.Class == "Tiny")
        {
            var radius = Model.GetBehavior<CircleFootprintModel>().radius;
            if (radius > 0 && radius < 7)
            {

            }
            else if (radius < 9)
            {

            }
            else
            {

            }
        }
        tower.UpdateRootModel(Model);
    }
    public override void OnBloonCreated(Bloon bloon)
    {
        if (GlobalVar.Class == "Commander")
        {
            try
            {
                bloon.trackSpeedMultiplier = 1.3f;
            }
            catch { }
            
        }
        if (GlobalVar.Class == "Ethereal")
        {
            try
            {
                bloon.trackSpeedMultiplier = 1.5f;
            }
            catch { }

        }
    }
    public static void EtherealTower(Tower tower, TowerModel towerModel)
    {
        var Model = towerModel.Duplicate();
        if (GlobalVar.Class == "Ethereal")
        {
            Model.AddBehavior(new OverrideCamoDetectionModel("EtherealClassCamoOverride", true));
            foreach (var am in Model.GetAttackModels())
            {
                am.attackThroughWalls = true;
                foreach (var proj in am.GetAllProjectiles())
                {
                    proj.ignoreBlockers = true;
                }
                am.range += 6;
            }
            Model.range += 6;
            Model.ignoreBlockers = true;
        }
        tower.UpdateRootModel(Model);
    }
    public static void PyrotechnicTower(Tower tower, TowerModel towerModel)
    {
        
        if (GlobalVar.Class == "Pyrotechnic")
        {
            var Model = tower.towerModel.Duplicate();
            switch(Model.baseId)
            {
                case "Gwendolin":
                    foreach(var am in Model.GetAttackModels())
                    {
                        foreach(var wp in am.weapons)
                        {
                            wp.rate *= 0.75f;
                        }
                    }
                    try
                    {
                        Model.GetAbility(0).GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AgeModel>().Lifespan *= 2;
                    }
                    catch
                    {

                    }
                    break;
            }
            tower.UpdateRootModel(Model);
        }
    }

    public override void PostBloonLeaked(Bloon bloon)
    {
        if (GlobalVar.Class == "Ethereal")
        {
            InGame.instance.AddHealth(-bloon.GetModifiedTotalLeakDamage());
            foreach (var tower in InGame.instance.bridge.GetAllTowers())
            {
                var Model = tower.tower.towerModel.Duplicate();
                foreach(var am in Model.GetAttackModels())
                {
                    foreach(var wp in am.weapons)
                    {
                        wp.rate *= 1.05f;
                    }
                }
                tower.tower.UpdateRootModel(Model);
            }
        }
    }
    
}
