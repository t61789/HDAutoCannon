using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorldTestMod
{
    public enum Mode
    {
        Common,
        Piercing
    }
    
	[StaticConstructorOnStartup]
    public static class ModResources
    {
	    public static Dictionary<Mode, Texture2D> textures = new Dictionary<Mode, Texture2D>
	    {
		    { Mode.Common, ContentFinder<Texture2D>.Get("Common") },
		    { Mode.Piercing, ContentFinder<Texture2D>.Get("Piercing") }
	    };
    }
    
    public static class Utils
    {
        public static Comp_HDCannonMode GetHdCannonModes(Pawn pawn)
        {
            if (pawn == null)
            {
                return null;
            }
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