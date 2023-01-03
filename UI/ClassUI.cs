
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Extensions;
using UnityEngine;
using BTD_Mod_Helper.Api.Enums;
using BloonsTDClass;
using TaskScheduler = BTD_Mod_Helper.Api.TaskScheduler;
using Il2CppAssets.Scripts.Unity.UI_New.ChallengeEditor;
using Il2CppAssets.Scripts.Utils;
using Il2CppAssets.Scripts.Unity.UI_New;
using Il2CppAssets.Scripts.Unity.Menu;
using Il2CppTMPro;

namespace ClassesMenuUI;
public class ClassesMenu : ModGameMenu<ExtraSettingsScreen>
{
    public static SpriteReference ExtraClasses => ModContent.GetSpriteReference<MelonMain>("ExtraClassesButton");
    public static SpriteReference NecroButton => ModContent.GetSpriteReference<MelonMain>("Necromancer");
    public static SpriteReference PyroButton => ModContent.GetSpriteReference<MelonMain>("Pyrotechnic");
    public static SpriteReference EtherealButton => ModContent.GetSpriteReference<MelonMain>("Etherial");
    public static SpriteReference EconomistButton => ModContent.GetSpriteReference<MelonMain>("Economist");
    public static SpriteReference CommanderButton => ModContent.GetSpriteReference<MelonMain>("Commander");

    public static ModHelperPanel? ClassesMenuPanel;
    public static ModHelperImage? image;
    public static ModHelperImage? indicator;
    public static ModHelperImage? indicator1;
    public static ModHelperImage? indicator2;
    public static ModHelperImage? indicator3;
    public static ModHelperImage? indicator4;
    public static ModHelperImage? indicator5;
    public static ModHelperText? selectedclassname;
    public static ModHelperText? abouttext;

    public static ModHelperScrollPanel? scrollpanel;
    public static ModHelperText? desctext;
    public static ModHelperPanel? bloontonium;
    public static ModHelperText? bloontoniumtext;
    private static ModHelperScrollPanel? ClassesList;
    // private static ClassesMenuClasses modTemplate;
    private static ModHelperScrollPanel? modsList;
    internal const int Padding = 50;

    internal const int MenuWidth = 3600;
    internal const int MenuHeight = 1900;

    internal const int LeftMenuWidth = 1750;
    internal const int RightMenuWidth = 1750;
    public override bool OnMenuOpened(Il2CppSystem.Object data)
    {
        CommonForegroundScreen.instance.heading.SetActive(true);
        CommonForegroundHeader.SetText("Classes");
        var panelTransform = GameMenu.gameObject.GetComponentInChildrenByName<RectTransform>("Panel");
        var panel = panelTransform.gameObject;
        panel.DestroyAllChildren();
        ClassesMenuPanel = panel.AddModHelperPanel(new Info("ClassesMenu", 3600, 1900));
        CreateLeftMenu(ClassesMenuPanel);
        CreateRightMenu(ClassesMenuPanel);
        CreateExtraButtons(ClassesMenuPanel);
        return false;
    }
    private void CreateLeftMenu(ModHelperPanel ClassesMenu)
    {
        var leftmenu = ClassesMenu.AddPanel(new Info("LeftMenu", (MenuWidth - LeftMenuWidth) / -2f, 0, LeftMenuWidth, MenuHeight), VanillaSprites.MainBGPanelBlue, RectTransform.Axis.Vertical, Padding, Padding);
        var topRow = leftmenu.AddPanel(new Info("TopRow")
        {
            Height = 50,
            FlexWidth = 0
        }, null, RectTransform.Axis.Horizontal, 50);
        leftmenu.AddText(new Info("Text", 0, 1150, 1000, 200), "Classes", 150f);
        //ClassesMenu.AddImage(new Info("BloontoniumImage", -475, 750, 200, 200), ModContent.GetSpriteReference<MelonMain>("Bloontonium").guidRef);
        ClassesList = leftmenu.AddScrollPanel(new Info("ClassesListScroll", InfoPreset.Flex), RectTransform.Axis.Vertical, VanillaSprites.BlueInsertPanelRound, Padding, Padding);
        var Button1 = ClassesMenu.AddButton(new Info("Class1", -3400, 1600, 250, 250, new Vector2(1, 0), new Vector2(0.5f, 0)), VanillaSprites.PrimaryBtn2, new Action(() =>
        {
            SetSelectedClass("Default", VanillaSprites.PrimaryBtn2, "The Default Class, this class has nothing special", "Pros" + "\n" + "- None" + "\n" + "Cons" + "\n" + "- None", 125, 125);
        }));
        Button1.AddText(new Info("Text", 0, -130, 1000, 200), "None", 70f);
        indicator = Button1.AddImage(new Info("SelectedButton", 100, 60, 100, 100), VanillaSprites.SelectedTick);
        indicator.gameObject.SetActive(Globals.GlobalVar.Class == "Default");

        var Button2 = ClassesMenu.AddButton(new Info("Class2", -2725, 100, 125 * 3, 125 * 3, new Vector2(1, 0), new Vector2(0.5f, 0)), NecroButton.guidRef, new Action(() =>
        {
            SetSelectedClass("Necromancer", NecroButton.guidRef, "The power of the Necromancer Class gives many abilities. One being your fellow necromancers gain the power of sacrifice and the other being the power of strength at the loss of money", "Pros" + "\n" + "- Ezili gets a 50% attack speed buff" + "\n" + "- 004 Wizard Monkey Gets Double The Space In Its Graveyard" + "\n" + "- 004 & 005 Wizard Monkeys Get Double Attack Speed" + "\n" +"- All 'Evil' Towers get a small buff" + "\n" + "\n" + "Cons" + "\n" + "- ALL Cash Gain is cut by 15%", 125, 125, 75);

        }));
        Button2.AddText(new Info("Text", 0, -160, 1000, 200), "Necromancer", 70f);
        indicator1 = Button2.AddImage(new Info("SelectedButton", 100, 100, 150, 150), VanillaSprites.SelectedTick);
        indicator1.gameObject.SetActive(Globals.GlobalVar.Class == "Necromancer");

        var Button3 = ClassesMenu.AddButton(new Info("Class3", -2150, 100, 125 * 3, 125 * 3, new Vector2(1, 0), new Vector2(0.5f, 0)), EconomistButton.guidRef, new Action(() =>
        {
            SetSelectedClass("Economist", EconomistButton.guidRef, "The fiscally-responsible Economist Class makes cash generation and tax evasion insanely easy, but as a result some monkeys are less powerful", "Pros" + "\n" + "- Cash gain for all sources is increased by 20%" + "\n" + "- Gain a free banana farm every 15 rounds" + "\n" + "- Benjamin's bank hack gives an extra 10% cash gain" + "\n" + "\n" + "Cons" + "\n" + "- All Towers Lose 1 damage (1 minimum damage)" + "\n" + "- All Towers Lose 2 pierce (1 minimum pierce)" + "\n" + "- All T4 and up towers get 10% slower attack speed" + "\n", 125, 125, 65);

        }));
        Button3.AddText(new Info("Text", 0, -160, 1000, 200), "Economist", 70f);
        indicator2 = Button3.AddImage(new Info("SelectedButton", 100, 100, 150, 150), VanillaSprites.SelectedTick);
        indicator2.gameObject.SetActive(Globals.GlobalVar.Class == "Economist");

        var Button4 = ClassesMenu.AddButton(new Info("Class4", -3275, 100, 150 * 3, 125 * 3, new Vector2(1, 0), new Vector2(0.5f, 0)), CommanderButton.guidRef, new Action(() =>
        {
            SetSelectedClass("Commander", CommanderButton.guidRef, "Awarded multiple medals in the monkey military, the commander will lead your monkey to victory with an inspiring banner. Unfortunely, bloons have gotten faster", "Pros" + "\n" + "- Flanking Tactics - Ability (20 Second Uptime, (120 + round) Second Cooldown)" + "\n" + "During Ability each tower gets" + "\n" + "- 2x Attack Speed, 4x for Bomb Shooters and Mortar Monkeys" + "\n" + "- 3x Pierce and an extra 3 damage" + "\n\n" + "Cons" + "\n" + "- Bloons are %30 faster", 150, 125, 75);

        }));
        Button4.AddText(new Info("Text", 0, -160, 1000, 200), "Commander", 70f);
        indicator3 = Button4.AddImage(new Info("SelectedButton", 100, 100, 150, 150), VanillaSprites.SelectedTick);
        indicator3.gameObject.SetActive(Globals.GlobalVar.Class == "Commander");

        var Button5 = ClassesMenu.AddButton(new Info("Class5", -3300, 600, 125 * 3, 125 * 3, new Vector2(1, 0), new Vector2(0.5f, 0)), EtherealButton.guidRef, new Action(() =>
        {
            SetSelectedClass("Ethereal", EtherealButton.guidRef, "From back in the days the ghostly ethereal class turns all monkeys into spooky scary ghosts allowing towers to see through walls, but beware your hearts are even more fragile than before", "Pros" + "\n" + "- All towers can see through walls \n- Camo bloons are visable by all monkeys \n- All Towers get a small buff in attack range \n\nCons \n- Life loss from all sources are doubled\n- Losing lives makes all towers lose 5% attack speed", 125, 125, 70);

        }));
        Button5.AddText(new Info("Text", 0, -160, 1000, 200), "Ethereal", 70f);
        indicator4 = Button5.AddImage(new Info("SelectedButton", 100, 100, 150, 150), VanillaSprites.SelectedTick);
        indicator4.gameObject.SetActive(Globals.GlobalVar.Class == "Ethereal");

        /*var Button6 = ClassesMenu.AddButton(new Info("Class6", -2725, 600, 125 * 3, 125 * 3, new Vector2(1, 0), new Vector2(0.5f, 0)), PyroButton.guidRef, new Action(() =>
        {
            SetSelectedClass("Pyrotechnic", PyroButton.guidRef, "The Fiery Pyrotechnic Class is ready to heat up the game. Some attacks are enhanced by fire and fire attacks are improved greatly, but your monkeys are not used to being hot and are weakened.", "Pros\n- Gwendolyn gets a %50 attack speed buff and her cocktail ability lasts longer\n- All fire based upgrades deal an extra %40 damage and get a bonus %20 attack speed\n\nCons\n- All water towers get half range (unless using fire upgrade)\n- Towers without fire upgrades get %10 less attack speed", 125, 125, 70);

        }));
        Button6.AddText(new Info("Text", 0, -160, 1000, 200), "Pyrotechnic", 70f);
        indicator5 = Button6.AddImage(new Info("SelectedButton", 100, 100, 150, 150), VanillaSprites.SelectedTick);
        indicator5.gameObject.SetActive(Globals.GlobalVar.Class == "Pyrotechnic");*/

    }
    private static void SetSelectedClass(String Class, String Image, String About, String Desc, int width, int height, int DescSize = 90)
    {
        try
        {
            MenuManager.instance.buttonClickSound.Play("ClickSounds");
            Globals.GlobalVar.Class = Class;
            Globals.GlobalVar.Image = Image;
            Globals.GlobalVar.About = About;
            Globals.GlobalVar.Desc = Desc;
            Globals.GlobalVar.DescSize = DescSize;
            GlobalVar.Width = width * 2;
            GlobalVar.Height = height * 2;
            indicator1?.gameObject.SetActive(Globals.GlobalVar.Class == "Necromancer");
            indicator2?.gameObject.SetActive(Globals.GlobalVar.Class == "Economist");
            indicator?.gameObject.SetActive(Globals.GlobalVar.Class == "Default");
            indicator3?.gameObject.SetActive(Globals.GlobalVar.Class == "Commander");
            indicator4?.gameObject.SetActive(Globals.GlobalVar.Class == "Ethereal");
            //indicator5?.gameObject.SetActive(Globals.GlobalVar.Class == "Pyrotechnic");
            selectedclassname?.SetText(Class);
            image?.Image.SetSprite(Image);
            image?.SetInfo(new Info("SelectedImage", 250, 800, width * 2, height * 2));
            abouttext?.SetText(About);
            desctext?.SetText(Desc);
            desctext.Text.fontSize = DescSize;
            ClassesPanel.image.Image.SetSprite(Image);
            ClassesPanel.image.SetInfo(new Info("ClassesMenuButton", 0, 0, width * 3, height * 3, new Vector2(1, 0), new Vector2(0.5f, 0)));
            InGamePanel.image.Image.SetSprite(Image);
            if (Class == "Default")
            {

            }
        }
        catch { }
    }
    public static void CreateRightMenu(ModHelperPanel modsMenu)
    {
        var leftMenu = modsMenu.AddPanel(
            new Info("RightMenu", (RightMenuWidth - MenuWidth) / -2f, 0, RightMenuWidth, MenuHeight),
            VanillaSprites.MainBGPanelBlue, RectTransform.Axis.Vertical, Padding, Padding
        );
        var topRow = leftMenu.AddPanel(new Info("TopRow")
        {
            Height = 200,
            FlexWidth = 1
        }, null, RectTransform.Axis.Horizontal, 50);
        selectedclassname = topRow.AddText(new Info("Text", 300, 1800, LeftMenuWidth - 100, 200), Globals.GlobalVar.Class, 120f);
        var About = leftMenu.AddPanel(new Info("AboutPanel", (RightMenuWidth - (MenuWidth + - 500)) / -2f, 0, LeftMenuWidth - 100, 450), VanillaSprites.BlueInsertPanelRound, RectTransform.Axis.Vertical, Padding, Padding);
        abouttext = About.AddText(new Info("Text", -300, 0, LeftMenuWidth - 200, 400), Globals.GlobalVar.About, 60f, TextAlignmentOptions.Top);
        image = modsMenu.AddImage(new Info("SelectedImage", 250, 800, GlobalVar.Width, GlobalVar.Height), Globals.GlobalVar.Image);
        var Description = leftMenu.AddPanel(new Info("DescriptionPanel", (RightMenuWidth - (MenuWidth + 150)) / -2f, 0, LeftMenuWidth - 100, MenuHeight - 850), VanillaSprites.BlueInsertPanelRound, RectTransform.Axis.Vertical, Padding, Padding);
        desctext = Description.AddText(new Info("Text", 0, 0, LeftMenuWidth - 200, 1000), Globals.GlobalVar.Desc, Globals.GlobalVar.DescSize, TextAlignmentOptions.TopLeft);
    }
    private static void CreateExtraButtons(ModHelperPanel modsMenu)
    {
        var ExtraClassesButton = modsMenu.AddButton(new Info("ExtraClassesButton", -1950, -1000, 400, 400), ExtraClasses.guidRef, new Action(() => ExtraClassesPress()));
        ExtraClassesButton.AddText(new Info("ExtraClassesText", 0, -200, 600, 300), "Extra Classes", 80);

        scrollpanel = modsMenu.AddScrollPanel(new Info("ScrollPanel", -1950, 1000, 500, 1500), RectTransform.Axis.Vertical);
        scrollpanel.ScrollContent.LayoutGroup.childAlignment = TextAnchor.LowerCenter;
        scrollpanel.ScrollContent.RectTransform.pivot = new Vector2(0.5f, 0);
        scrollpanel.AddScrollContent(CreateTemplate("Empty"));
        scrollpanel.SetActive(false);

        var animator = scrollpanel.AddComponent<Animator>();
        animator.runtimeAnimatorController = Animations.PopupAnim;
        animator.speed = .75f;


    }
    private static void ExtraClassesPress()
    {
        switch (scrollpanel?.isActiveAndEnabled)
        {
            case false:
                ExtraClassesOpen();
                break;
            case true:
                ExtraClassesClose();
                break;
        }
    }
    private static void ExtraClassesClose()
    {
        scrollpanel?.GetComponent<Animator>().Play("PopupSlideOut");
        TaskScheduler.ScheduleTask(() => scrollpanel?.SetActive(false), ScheduleType.WaitForFrames, 13);
    }
    private static void ExtraClassesOpen()
    {
        scrollpanel?.SetActive(true);
        scrollpanel?.GetComponent<Animator>().Play("PopupSlideIn");
    }
    public static ModHelperButton CreateTemplate(string name)
    {
        var classes = ModHelperButton.Create(new Info("Empty", width: 300, height: 300), VanillaSprites.WoodenRoundButton, null);
        switch (name)
        {
            case "Empty":
                classes = ModHelperButton.Create(new Info("Empty", width: 300, height: 300), VanillaSprites.WoodenRoundButton, null);
                classes.AddText(new Info("Text", 0, -125, 500, 200), "Empty", 70);
                break;
        }
        return classes;
    }
}
