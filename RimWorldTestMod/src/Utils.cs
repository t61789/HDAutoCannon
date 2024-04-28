using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorldTestMod
{
    public enum Mode
    {
        Common,
        AntiPersonnel,
        Piercing
    }
    
    public static class Utils
    {
        public static Comp_HDCannonMode GetHdCannonModes(Pawn pawn)
        {
            return GetHdCannonModes(Enumerable.Repeat(pawn, 1)).FirstOrDefault();
        }
        
        public static IEnumerable<Comp_HDCannonMode> GetHdCannonModes(IEnumerable<Pawn> pawns)
        {
            var comps = 
                from pawn in pawns
                let primary = pawn.equipment.Primary
                where primary != null && primary.HasComp<Comp_HDCannonMode>()
                select primary.GetComp<Comp_HDCannonMode>();
            
            foreach (var hdCannonMode in comps)
            {
                yield return hdCannonMode;
            }
        }
    }
}