using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace HDAC
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
        public static Comp_AutoCannon GetAutoCannonComp(Pawn pawn)
        {
            if (pawn == null)
            {
                return null;
            }
            return GetAutoCannonComp(Enumerable.Repeat(pawn, 1)).FirstOrDefault();
        }
        
        public static IEnumerable<Comp_AutoCannon> GetAutoCannonComp(IEnumerable<Pawn> pawns)
        {
            var comps = 
                from pawn in pawns
                let primary = pawn.equipment.Primary
                where primary != null && primary.HasComp<Comp_AutoCannon>()
                select primary.GetComp<Comp_AutoCannon>();
            
            foreach (var hdCannonMode in comps)
            {
                yield return hdCannonMode;
            }
        }
    }
}