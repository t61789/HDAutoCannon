using HarmonyLib;
using Verse;

namespace HDAC
{
    [StaticConstructorOnStartup]
    public static class Main
    {
        static Main()
        {
            Harmony.DEBUG = true;
            var harmony = new Harmony("rimworldtestmod.hd_cannon");
            harmony.PatchAll();
        }
    }
}