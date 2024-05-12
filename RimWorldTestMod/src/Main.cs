using HarmonyLib;
using Verse;

namespace HDAC
{
    [StaticConstructorOnStartup]
    public static class Main
    {
        static Main()
        {
            // #if DEBUG
            // Harmony.DEBUG = true;
            // #endif
            var harmony = new Harmony("rimworldtestmod.hd_cannon");
            harmony.PatchAll();
        }
    }
}