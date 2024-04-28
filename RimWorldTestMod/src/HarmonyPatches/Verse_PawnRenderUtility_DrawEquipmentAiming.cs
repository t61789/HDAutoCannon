using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimWorldTestMod.HarmonyPatches
{
    [HarmonyPatch(typeof(PawnRenderUtility), "DrawEquipmentAiming")]
    public class Verse_PawnRenderUtility_DrawEquipmentAiming
    {
        public static void Postfix(Thing eq, Vector3 drawLoc, float aimAngle)
        {
            // Projectile_HDCannonBullet.SetPos(eq, drawLoc);
        }
    }
}