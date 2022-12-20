global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
global using HarmonyLib;
global using Globals;
using BTD_Mod_Helper.Api.Enums;

namespace Globals;
class GlobalVar
{
    public static string Class = "Default";
    public static string Image = VanillaSprites.PrimaryBtn2;
    public static string About = "The Default Class, this class has nothing special";
    public static string Desc = "Pros" + "\n" + "- None" + "\n" + "Cons" + "\n" + "- None";
    public static int DescSize = 90;
    public static int Width = 250;
    public static int Height = 250;
    public static bool ability = false;
    public static float abilitycooldown = 0;
    public static float abilitydur = 0;
    public static bool abilityactive = false;

    public static bool ingame = false;
    public static bool abilityshowing = false;
}
public class SaveInfo
{
    public string? Class { get; set; }
    public string? Image { get; set; }
    public string? About { get; set; }
    public string? Desc { get; set; }
    public int DescSize { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}
