

using Il2Cpp;
using Il2CppAssets.Scripts.Unity.Menu;
using Il2CppAssets.Scripts.Unity.UI_New.Main.DifficultySelect;
using Il2CppAssets.Scripts.Unity.UI_New.Main.MapSelect;

[HarmonyPatch(typeof(MenuManager), nameof(MenuManager.OpenMenu))]
internal static class MenuManager_OpenMenu
{
    [HarmonyPostfix]
    private static void Postfix(MenuManager __instance, string menuName)
    {
        if (menuName == "MapSelectScreen")
            ClassesPanel.Show();
    }
}

[HarmonyPatch(typeof(DifficultySelectScreen), nameof(DifficultySelectScreen.Open))]
internal static class DifficultySelectScreen_Open
{
    [HarmonyPostfix]
    private static void Postfix()
    {
        ClassesPanel.Hide();
    }
}

[HarmonyPatch(typeof(DifficultySelectScreen), nameof(DifficultySelectScreen.OpenModeSelectUi))]
internal static class DifficultySelectScreen_OpenModeSelectUi
{
    [HarmonyPostfix]
    private static void Postfix()
    {
        ClassesPanel.Hide();
    }
}

[HarmonyPatch(typeof(ContinueGamePanel), nameof(ContinueGamePanel.ContinueClicked))]
internal static class ContinueGamePanel_ContinueClicked
{
    [HarmonyPostfix]
    private static void Postfix()
    {
        ClassesPanel.Hide();
    }
}

[HarmonyPatch(typeof(MapSelectScreen), nameof(MapSelectScreen.Open))]
internal static class MapSelectScreen_Open
{
    [HarmonyPostfix]
    private static void Postfix()
    {
        ClassesPanel.Show();
    }
}

[HarmonyPatch(typeof(MapSelectScreen), nameof(MapSelectScreen.Close))]
internal static class MapSelectScreen_Close
{
    [HarmonyPostfix]
    private static void Postfix()
    {
        ClassesPanel.Hide();
    }
}