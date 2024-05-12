using HarmonyLib;
using Verse;

namespace HDAC.HarmonyPatches
{
    [HarmonyPatch(typeof(Verse.VerbUtility), "UsesExplosiveProjectiles")]
    public class Verse_VerbUtility_UsesExplosiveProjectiles
    {
        public static void Postfix(ref bool __result, Verb verb)
        {
            if (verb.caster is Pawn pawn)
            {
                var primary = pawn.equipment.Primary;
                if (primary != null)
                {
                    var comp = primary.TryGetComp<Comp_AutoCannon>();
                    if (comp != null)
                    {
                        __result = comp.curMode == Mode.Common;
                    }
                }
            }
        }
    }
}