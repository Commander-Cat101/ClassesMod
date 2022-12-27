using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api;
using UnityEngine;
using BTD_Mod_Helper.Extensions;
using BTD_Mod_Helper.UI.Menus;
using BloonsTDClass;
using Random = System.Random;
using ClassesMenuUI;
using Il2CppAssets.Scripts.Unity.UI_New;

public static class InGamePanel
{
    private static ModHelperPanel panel;
    public static ModHelperImage image;

    private static void CreatePanel(GameObject screen)
    {
        panel = screen.AddModHelperPanel(new Info("InGamePanel")
        {
            Anchor = new Vector2(1, 0),
            Pivot = new Vector2(1, 0)
        });
    }

    private static void Init()
    {
        var screen = CommonForegroundScreen.instance.transform;
        var ModSavePanel = screen.FindChild("InGamePanel");
        if (ModSavePanel == null)
            CreatePanel(screen.gameObject);
    }


    private static void HideButton()
    {
        panel.SetActive(false);
    }

    public static void Show()
    {
        Init();
        panel.SetActive(true);
    }

    public static void Hide()
    {
        var screen = CommonForegroundScreen.instance.transform;
        var ModSavePanel = screen.FindChild("InGamePanel");
        if (ModSavePanel != null)
            HideButton();
    }
}