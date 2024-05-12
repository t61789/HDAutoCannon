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
        private float _savedExplosionRadius = 0;

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);

            if (GetShootVerbProperties(out var shootVerbProperties))
            {
                _savedExplosionRadius = shootVerbProperties.defaultProjectile.projectile.explosionRadius;
            }
            
            SwitchMode(Props.initMode);
        }

        public void SwitchMode(Mode mode)
        {
            curMode = mode;
            if (!GetShootVerbProperties(out var shootVerbProperties))
            {
                return;
            }

            var projectile = shootVerbProperties.defaultProjectile.projectile;
            if (mode == Mode.Piercing && projectile.explosionRadius != 0)
            {
                projectile.explosionRadius = 0;
            }
            else if (mode == Mode.Common && projectile.explosionRadius == 0)
            {
                projectile.explosionRadius = _savedExplosionRadius;
            }
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

        private bool GetShootVerbProperties(out VerbProperties shootVerbProperties)
        {
            shootVerbProperties = parent.def.Verbs.Find(v => v.verbClass == typeof(Verb_Shoot));
            return shootVerbProperties != null;
        }
    }
}