using Assets.Scripts.Simulation;
using Assets.Scripts.Simulation.Tracking;
using Assets.Scripts.Unity.UI_New.InGame;
using Assets.Scripts.Utils;
using BTD_Mod_Helper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                c = -c;
            }
        }
        if (Globals.GlobalVar.Class == "Commander")
        {
            if (source != Simulation.CashSource.CoopTransferedCash || source != Simulation.CashSource.TowerSold)
            {
                c *= .05f;
                c = -c;
            }
        }
        if (Globals.GlobalVar.Class == "Economist")
        {
            if (source != Simulation.CashSource.CoopTransferedCash || source != Simulation.CashSource.TowerSold)
            {
                c *= .2f;  
            }
        }
        InGame.instance.AddCash(c);
        return true;
    }
}