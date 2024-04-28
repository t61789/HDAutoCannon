using HarmonyLib;
using Verse;

namespace RimWorldTestMod
{
    [StaticConstructorOnStartup]
    public static class HDCannon
    {
        static HDCannon()
        {
            Harmony.DEBUG = true;
            var harmony = new Harmony("rimworldtestmod.hd_cannon");
            harmony.PatchAll();
        }
    }
}