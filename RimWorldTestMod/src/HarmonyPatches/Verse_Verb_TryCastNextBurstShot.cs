using HarmonyLib;
using RimWorld;
using Verse;

namespace RimWorldTestMod.HarmonyPatches
{
    [HarmonyPatch(typeof(Verb), "TryCastNextBurstShot")]
    public class Verse_Verb_TryCastNextBurstShot
    {
        public static void Postfix(Verb __instance, int ___burstShotsLeft)
        {
            // Projectile_HDCannonBullet.Mote(__instance.EquipmentSource);
        }
    }
}