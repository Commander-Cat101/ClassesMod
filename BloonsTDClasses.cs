using MelonLoader;
using BTD_Mod_Helper;
using BloonsTDClasses;
using BTD_Mod_Helper.Api.ModOptions;
using Assets.Scripts.Unity.UI_New.InGame;
using BTD_Mod_Helper.UI.BTD6;
using BTD_Mod_Helper.Extensions;
using UnityEngine.UI;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Models;
using Assets.Scripts.Unity;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Simulation.Towers.Emissions;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Unity.Towers.Mods;
using BTD_Mod_Helper.Api;
using ClassesMenuUI;
using Assets.Scripts.Models.Towers.Behaviors;
using BTD_Mod_Helper.Api.Enums;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Bloons.Behaviors;
using Assets.Scripts.Simulation.Objects;
using Assets.Scripts.Unity.Towers;
using Tower = Assets.Scripts.Simulation.Towers.Tower;
using Assets.Scripts.Simulation.Towers.Projectiles;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Simulation.Bloons;
using Assets.Scripts.Simulation.Towers.Weapons;
using Assets.Scripts.Models.Towers.Pets;
using Assets.Scripts.Models.Bloons;
using Assets.Scripts.Utils;
using Assets.Scripts.Simulation;
using Assets.Scripts.Simulation.Input;
using Assets.Scripts.Unity.Bridge;

[assembly: MelonInfo(typeof(BloonsTDClass.MelonMain), ModHelperData.Name, ModHelperData.Version, ModHelperData.Author)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace BloonsTDClass;

public class MelonMain : BloonsTD6Mod
{
    public static bool bloonspeedchange = false;
    public static int freefarmcount = 0;
    public override void OnApplicationStart()
    {
        MelonLogger.Msg("BloonsTDClasses loaded!");
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
        InGamePanel.Hide();
        AbilityUI.Hide();
        base.OnMatchEnd();
        GlobalVar.ingame = false;
        GlobalVar.ability = false;
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
                InGame.instance.GetTowerInventory().AddFreeTowers(TowerType.BananaFarm, 1, "", 0);
                freefarmcount = 0;
            }
        }
        base.OnRoundEnd();
    }

    /*public override void OnTowerModelChanged(Tower tower, Model newModel)
    {
        
        if (GlobalVar.Class == "Necromancer")
        {      
            switch (tower.towerModel.baseId)
            {
                case "WizardMonkey":
                    if (tower.towerModel.tiers[2] == 4)
                    {
                        var newEmi = new NecromancerEmissionModel("EmissionModel", 1000, 50, 1, 5, 10, 50, 5, null, null, null, 5, 100, 10, 200, 2);
                        tower.towerModel.behaviors.First(a => a.name.Contains("Necromancer_")).Cast<AttackModel>().weapons[0].emission = newEmi;
                    }
                    if (tower.towerModel.tiers[2] == 5)
                    {
                        var newEmi = new NecromancerEmissionModel("EmissionModel", 6000, 300, 1, 8, 15, 50, 5, null, null, null, 5, 100, 10, 400, 3);
                        tower.towerModel.behaviors.First(a => a.name.Contains("Necromancer_")).Cast<AttackModel>().weapons[0].emission = newEmi;
                    }
                    break;
                case "Ezili":
                    foreach (var weapon in tower.towerModel.GetWeapons())
                    {
                        weapon.Rate *= .75f;
                    }
                    foreach (var abilitymod in tower.towerModel.GetAbilities())
                    {
                        abilitymod.Cooldown *= .75f;
                    }
                    break;
                case "IceMonkey":
                    if (tower.towerModel.tiers[0] >= 3)
                    {
                        foreach (var weapon in tower.towerModel.GetWeapons())
                        {
                            weapon.Rate *= .85f;
                        }
                    }
                    break;
                case "MonkeyBuccaneer":
                    if (tower.towerModel.tiers[1] >= 3)
                    {
                        foreach (var weapon in tower.towerModel.GetWeapons())
                        {
                            weapon.Rate *= .85f;
                        }
                    }
                    break;
                case "SuperMonkey":
                    if (tower.towerModel.tiers[1] >= 3)
                    {
                        foreach (var weapon in tower.towerModel.GetWeapons())
                        {
                            weapon.Rate *= .85f;
                        }
                    }
                    break;
                case "NinjaMonkey":
                    if (tower.towerModel.tiers[2] >= 3)
                    {
                        foreach (var weapon in tower.towerModel.GetWeapons())
                        {
                            weapon.Rate *= .85f;
                        }
                    }
                    break;
                case "Alchemist":
                    if (tower.towerModel.tiers[1] >= 4)
                    {
                        foreach (var weapon in tower.towerModel.GetWeapons())
                        {
                            weapon.Rate *= .85f;
                        }
                    }
                    break;
                case "Druid":
                    if (tower.towerModel.tiers[2] >= 2)
                    {
                        foreach (var weapon in tower.towerModel.GetWeapons())
                        {
                            weapon.Rate *= .85f;
                        }
                    }
                    break;
                case "SpikeFactory":
                    if (tower.towerModel.tiers[0] >= 3)
                    {
                        foreach (var weapon in tower.towerModel.GetWeapons())
                        {
                            weapon.Rate *= .85f;
                        }
                    }
                    break;
            }
        }
    }*/
    public override void OnTowerCreated(Tower tower, Entity target, Model modelToUse)
    {
        if (GlobalVar.Class == "Necromancer")
        {
            NecroTowers(tower);
        }
        CommanderTowersAbility(tower, modelToUse.Cast<TowerModel>());
        EconomistTowers(tower, modelToUse.Cast<TowerModel>());
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
    }
    public static void NecroTowers(Tower tower, string upgradeName = null, TowerModel newBaseTowerModel = null)
    {
        var towerModel = tower.rootModel.Clone().Cast<TowerModel>();
        switch (towerModel.baseId)
        {
            case "WizardMonkey":
                if (towerModel.tiers[2] == 4)
                {
                    var newEmi = new NecromancerEmissionModel("EmissionModel", 1000, 50, 1, 5, 10, 50, 5, null, null, null, 5, 100, 10, 200, 2);
                    towerModel.behaviors.First(a => a.name.Contains("Necromancer_")).Cast<AttackModel>().weapons[0].emission = newEmi;
                }
                if (towerModel.tiers[2] == 5)
                {
                    var newEmi = new NecromancerEmissionModel("EmissionModel", 6000, 300, 1, 8, 15, 50, 5, null, null, null, 5, 100, 10, 400, 3);
                    towerModel.behaviors.First(a => a.name.Contains("Necromancer_")).Cast<AttackModel>().weapons[0].emission = newEmi;
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
    public override void OnBloonCreated(Bloon bloon)
    {
        if (GlobalVar.Class == "Commander")
        {
            try
            {
                bloon.bloonModel.Speed *= 1.3f;
            }
            catch { }
        }
    }
}
