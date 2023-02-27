
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Simulation;
using Il2CppAssets.Scripts.Simulation.Tracking;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;

[HarmonyPatch(typeof(AnalyticsTrackerSimManager), nameof(AnalyticsTrackerSimManager.OnCashEarned))]
public class NoCash
{
    [HarmonyPrefix]
    public static bool Prefix(ref double cash,ref Simulation.CashSource source)
    {
        var c = cash;
        if (Globals.GlobalVar.Class == "Necromancer")
        {
            if (source != Simulation.CashSource.CoopTransferedCash || source != Simulation.CashSource.TowerSold)
            {
                c *= .15f;
            }

            InGame.instance.AddCash(-c);
        }
        if (Globals.GlobalVar.Class == "Commander")
        {
            if (source != Simulation.CashSource.CoopTransferedCash || source != Simulation.CashSource.TowerSold)
            {
                c *= .05f;
                c = -c;
            }

            InGame.instance.AddCash(c);
        }
        if (Globals.GlobalVar.Class == "Economist")
        {
            if (source != Simulation.CashSource.CoopTransferedCash || source != Simulation.CashSource.TowerSold)
            {
                c *= .2f;  
            }

            InGame.instance.AddCash(c);
        }
        return true;
    }
}