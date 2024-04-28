using HarmonyLib;
using Verse;

namespace RimWorldTestMod
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