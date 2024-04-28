using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorldTestMod
{
	[StaticConstructorOnStartup]
    public static class ModResources
    {
	    public static Dictionary<Mode, Texture2D> textures = new Dictionary<Mode, Texture2D>
	    {
		    { Mode.Common, ContentFinder<Texture2D>.Get("Common") },
		    { Mode.AntiPersonnel, ContentFinder<Texture2D>.Get("AntiPersonnel") },
		    { Mode.Piercing, ContentFinder<Texture2D>.Get("Piercing") }
	    };
    }
}