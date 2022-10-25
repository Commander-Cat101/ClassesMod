using Assets.Scripts.Unity.Menu;
using Assets.Scripts.Unity.UI_New;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api;
using UnityEngine;
using BTD_Mod_Helper.Extensions;
using BTD_Mod_Helper.UI.Menus;
using BloonsTDClass;
using Random = System.Random;
using ClassesMenuUI;
using Assets.Scripts.Unity.UI_New.Main;
using MelonLoader;
using TaskScheduler = BTD_Mod_Helper.Api.TaskScheduler;

public static class ClassesPanel
{
    private static ModHelperPanel panel;
    public static ModHelperButton image;

    private static void openclassesmenu()
    {
        MenuManager.instance.buttonClickSound.Play("ClickSounds");
        ModGameMenu.Open<ClassesMenu>();
    }
    public static String Icon = Globals.GlobalVar.Image;

    public static void CreatePanel(GameObject screen)
    {
        //Random rnd = new Random();
        //int Randomnumber = rnd.Next(1, 5);
        /*switch (Randomnumber)
        {
            case 1:
                Icon = VanillaSprites.PrimaryBtn2;
                break;
            case 2:
                Icon = VanillaSprites.MilitaryBtn2;
                break;
            case 3:
                Icon = VanillaSprites.MagicBtn2;
                break;
            case 4:
                Icon = VanillaSprites.SupportBtn;
                break;
        }*/
        panel = screen.AddModHelperPanel(new Info("ClassesPanel")
        {
            Anchor = new Vector2(1, 0),
            Pivot = new Vector2(1, 0)
        });

        var animator = panel.AddComponent<Animator>();
        animator.runtimeAnimatorController = Animations.PopupAnim;
        animator.speed = .75f;

        image = panel.AddButton(new Info("ClassesMenuButton", 0, 0, 370, 370, new Vector2(1, 0), new Vector2(0.5f, 0)), Icon, new Action(openclassesmenu));
        image.AddText(new Info("Text", 0, -200, 1000, 200), "Classes", 70f);


        var mainMenuTransform = screen.transform.Cast<RectTransform>();
        var matchLocalPosition = image.transform.gameObject.AddComponent<MatchLocalPosition>();
        var bottomGroup = mainMenuTransform.FindChild("Friends");
        matchLocalPosition.transformToCopy = bottomGroup.transform.GetChild(0);

        var rect = mainMenuTransform.rect;
        var aspectRatio = rect.width / rect.height;
        if (aspectRatio < 1.5)
        {
            matchLocalPosition.offset = new Vector3(0, 0);
            matchLocalPosition.scale = new Vector3(1, 3.33f, 1);
            MelonLogger.Msg("1");
        }
        else if (aspectRatio < 1.7)
        {
            matchLocalPosition.offset = new Vector3(-700, 60, 0);
            matchLocalPosition.scale = new Vector3(1, 3f, 1);
            MelonLogger.Msg("2");
        }
        else
        {
            matchLocalPosition.offset = new Vector3(-750, 70);
            MelonLogger.Msg("3");
        }
        /*var mainMenuTransform = screen.transform.Cast<RectTransform>();
        var matchLocalPosition = image.transform.gameObject.AddComponent<MatchLocalPosition>();
        var bottomGroup = mainMenuTransform.FindChild("RoundSetChangerPanel");
        matchLocalPosition.transformToCopy = bottomGroup.transform.GetChild(0);

        var rect = mainMenuTransform.rect;
        var aspectRatio = rect.width / rect.height;
        if (aspectRatio < 1.5)
        {
            matchLocalPosition.offset = new Vector3(-390, -100);
            matchLocalPosition.scale = new Vector3(1, 3.33f, 1);
        }
        else if (aspectRatio < 1.7)
        {
            matchLocalPosition.offset = new Vector3(-500, -80);
            matchLocalPosition.scale = new Vector3(1, 3f, 1);
        }
        else
        {
            matchLocalPosition.offset = new Vector3(-500, 20);
        }*/
    }

    private static void Init()
    {
        var screen = CommonForegroundScreen.instance.transform;
        var ModSavePanel = screen.FindChild("ClassesPanel");
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
    }

    public static void Hide()
    {
        var screen = CommonForegroundScreen.instance.transform;
        var ModSavePanel = screen.FindChild("ClassesPanel");
        if (ModSavePanel != null)
            HideButton();
    }
    
}