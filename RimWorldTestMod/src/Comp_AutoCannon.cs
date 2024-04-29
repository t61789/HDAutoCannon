using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace HDAC
{
    public class CompProperties_AutoCannon : CompProperties
    {
        public Mode initMode;
        public Vector3 muzzleOffset = new Vector3(0, 0, 0);
        public FleckDef muzzleFleckDef = FleckDefOf.ShotFlash;
        public float muzzleFleckScale = 2;
        
        public CompProperties_AutoCannon()
        {
            compClass = typeof(Comp_AutoCannon);
        }
    }
    
    public class Comp_AutoCannon : ThingComp
    {
        public Mode curMode;
        
        public CompProperties_AutoCannon Props => props as CompProperties_AutoCannon;

        private class BackpackCheckCache
        {
            public bool hasBackpack;
            public int checkedTick = -999;
        }
        private static readonly Dictionary<Thing, BackpackCheckCache> _backpackCheckCache = new Dictionary<Thing, BackpackCheckCache>(); 
        private static readonly List<Thing> _needGcThings = new List<Thing>();
        private const int OUTDATED_TICK = 30;

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            
            curMode = Props.initMode;
        }

        public void SwitchMode(Mode mode)
        {
            curMode = mode;
        }

        public static bool HasBackpack(Thing thing)
        {
            if (!(thing is Pawn pawn))
            {
                return false;
            }

            var curTick = Find.TickManager.TicksGame;
            if (!_backpackCheckCache.TryGetValue(thing, out var cache))
            {
                cache = new BackpackCheckCache();
                _backpackCheckCache.Add(thing, cache);
            }

            if (curTick - cache.checkedTick > OUTDATED_TICK)
            {
                cache.checkedTick = curTick;
                cache.hasBackpack = pawn.apparel.WornApparel.Any(x=>x.def.defName == "Apparel_HDAC_AutoCannonBackpack");
            }
            var result = cache.hasBackpack;

            if (curTick % OUTDATED_TICK == 0)
            {
                _needGcThings.AddRange(
                    from x in _backpackCheckCache 
                    where curTick - x.Value.checkedTick > OUTDATED_TICK * 2 
                    select x.Key);
                _backpackCheckCache.RemoveRange(_needGcThings);
                _needGcThings.Clear();
            }

            return result;
        }
    }
}