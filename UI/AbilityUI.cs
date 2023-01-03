
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api;
using UnityEngine;
using BTD_Mod_Helper.Extensions;
using BloonsTDClass;
using TaskScheduler = BTD_Mod_Helper.Api.TaskScheduler;
using MelonLoader;
using Il2CppAssets.Scripts.Utils;
using Il2CppAssets.Scripts.Unity.Menu;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Unity.UI_New;

public static class AbilityUI
{
    private static ModHelperPanel panel;
    public static ModHelperButton button;
    public static ModHelperText text;
    public static SpriteReference NotReadyImage => ModContent.GetSpriteReference<MelonMain>("AbilityNotReady");
    public static SpriteReference ReadyImage => ModContent.GetSpriteReference<MelonMain>("AbilityReady");

    public static SpriteReference ActiveImage => ModContent.GetSpriteReference<MelonMain>("AbilityActive");

    public static void AbilityPressed()
    {
        if (GlobalVar.abilitycooldown < 1)
        {
            MenuManager.instance.buttonClickSound.Play("ClickSounds");
            switch (GlobalVar.Class)
            {
                case "Commander":
                    GlobalVar.abilitycooldown = 120 + InGame.instance.bridge.GetCurrentRound();
                    GlobalVar.abilitydur = 20;
                    foreach(var tower in InGame.instance.bridge.GetAllTowers())
                    {
                        var towerModel = tower.tower.rootModel.Clone().Cast<TowerModel>();
                        if (towerModel.GetWeapons() != null)
                        {
                            foreach (var weapon in towerModel.GetWeapons())
                            {
                                weapon.projectile.GetDamageModel().damage += 3;
                                weapon.Rate *= .5f;
                                weapon.projectile.pierce *= 3;
                                if (towerModel.baseId == "BombShooter" || towerModel.baseId == "MortarMonkey")
                                {
                                    weapon.Rate *= .5f;
                                }
                            }
                        }
                        tower.tower.UpdateRootModel(towerModel);
                    }
                    MelonLogger.Msg("Pressed");
                    GlobalVar.abilityactive = true;
                    break;
                default:
                    break;
            }
            button.Image.SetSprite(NotReadyImage);
        }
        else
        {
            MenuManager.instance.buttonClick2Sound.Play("ClickSounds");
        }
    }
    private static void CreatePanel(GameObject screen)
    {
        var Icon = Globals.GlobalVar.Image;
        panel = screen.AddModHelperPanel(new Info("AbilityPanel")
        {
            Anchor = new Vector2(1, 0),
            Pivot = new Vector2(1, 0)
        });
        button = panel.AddButton(new Info("AbilityButton", -2600, 2300, 250, 250, new Vector2(1, 0), new Vector2(0.5f, 0)), NotReadyImage.guidRef, new Action(() =>
        {
            AbilityPressed();
        }));
        text = button.AddText(new Info("Text", 0, -120, 600, 200), GlobalVar.abilitycooldown + "s", 90);

        var animator = panel.AddComponent<Animator>();
        animator.runtimeAnimatorController = Animations.PopupAnim;
        animator.speed = .55f;
    }

    private static void Init()
    {
        var screen = CommonForegroundScreen.instance.transform;
        var ModSavePanel = screen.FindChild("AbilityPanel");
        if (ModSavePanel == null)
            CreatePanel(screen.gameObject);
    }


    private static void HideButton()
    {
        panel.GetComponent<Animator>().Play("PopupSlideOut");
        TaskScheduler.ScheduleTask(() => panel.SetActive(false), ScheduleType.WaitForFrames, 13);
    }

    public static void Show()
    {
        Init();
        panel.SetActive(true); 
        panel.GetComponent<Animator>().Play("PopupSlideIn");

        GlobalVar.abilityshowing = true;
    }

    public static void Hide()
    {
        var screen = CommonForegroundScreen.instance.transform;
        var ModSavePanel = screen.FindChild("AbilityPanel");
        GlobalVar.abilityshowing = false;
        if (ModSavePanel != null)
            HideButton();
    }
    
}