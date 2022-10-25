using Assets.Scripts.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[HarmonyPatch(typeof(Simulation), "AddCash")]
public class NoCash
{
    [HarmonyPrefix]
    public static bool Prefix(ref double c, ref Simulation.CashSource source)
    {
        if (Globals.GlobalVar.Class == "Necromancer")
        {
            if (source != Simulation.CashSource.CoopTransferedCash || source != Simulation.CashSource.TowerSold)
            {
                c *= .85f;
            }
        }
        if (Globals.GlobalVar.Class == "Commander")
        {
            if (source != Simulation.CashSource.CoopTransferedCash || source != Simulation.CashSource.TowerSold)
            {
                c *= .95f;
            }
        }
        if (Globals.GlobalVar.Class == "Economist")
        {
            if (source != Simulation.CashSource.CoopTransferedCash || source != Simulation.CashSource.TowerSold)
            {
                c *= 1.20f;
            }
        }
        return true;
    }
}