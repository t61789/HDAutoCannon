using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace HDAC.HarmonyPatches
{
    [HarmonyPatch(typeof(Pawn), nameof(Pawn.GetGizmos))]
    public static class Verse_Pawn_GetGizmos
    {
        private static Dictionary<Mode, int> _modeCounts = new Dictionary<Mode, int>();
        private static int _frameCount = -1;
        
        //	public override IEnumerable<Gizmo> GetGizmos()
        public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> __result, Pawn __instance)
        {
            foreach (var r in __result)
                yield return r;
            
            if (Time.frameCount != _frameCount)
            {
                var mode = CreateCommand();
                if (mode != null)
                {
                    yield return mode;
                }
                _frameCount = Time.frameCount;
                _modeCounts.Clear();
            }

            Comp_AutoCannon comp = null;
            var needDraw =
                __instance.IsColonistPlayerControlled &&
                __instance.equipment.Primary != null &&
                __instance.equipment.Primary.TryGetComp(out comp);
            
            if (needDraw)
            {
                var mode = comp.curMode;
                if (!_modeCounts.ContainsKey(mode))
                {
                    _modeCounts[mode] = 0;
                }
                _modeCounts[mode]++;
            }
        }

        private static Command_SwitchMode CreateCommand()
        {
            var mode = Mode.Common;
            var max = 0;
            foreach (var pair in _modeCounts)
            {
                if (pair.Value > max)
                {
                    max = pair.Value;
                    mode = pair.Key;
                }
            }

            return max == 0 ? null : new Command_SwitchMode(mode);
        }
    }
}