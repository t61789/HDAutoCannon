using HarmonyLib;
using RimWorld;
using Verse;

namespace HDAC.HarmonyPatches
{
    [HarmonyPatch(typeof(StatExtension), nameof(StatExtension.GetStatValue))]
    public static class RimWorld_StatExtension_GetStatValue
    {
        public static void Postfix(ref float __result, Thing thing, StatDef stat, bool applyPostProcess = true, int cacheStaleAfterTicks = -1)
        {
            if (thing.def.defName != "Gun_HDAC_AutoCannon" || stat != StatDefOf.RangedWeapon_Cooldown)
            {
                return;
            }
            
            if (!(thing.holdingOwner?.Owner is Pawn_EquipmentTracker tracker))
            {
                return;
            }
                
            if (Comp_AutoCannon.HasBackpack(tracker.pawn))
            {
                __result = 0;
            }
        }
    }
}