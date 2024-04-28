﻿using System;
using System.Collections.Generic;
using LudeonTK;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimWorldTestMod
{
    public class HDCannonExtension : DefModExtension
    {
        public float piercingModeArmorPenetration = 1.3f;
        public int piercingModeDamageAmount = 40;
        public int commonModeDamageAmount = 20;
        public float commonModeExplosionRadius = 1.1f;
        public float commonModeArmorPenetration = 0.2f;
        public float commonModeArmorDirectHitPenetration = 0.5f;
        public bool explosionDamageFallOff = true;
        
        public static readonly HDCannonExtension DEFAULT = new HDCannonExtension();
    }
    
    public class Projectile_HDCannonBullet : Projectile
    {
        private Comp_HDCannonMode _hdCannonMode;
        
        public override void Launch(Thing launcher,
            Vector3 origin,
            LocalTargetInfo usedTarget,
            LocalTargetInfo intendedTarget,
            ProjectileHitFlags hitFlags, 
            bool preventFriendlyFire = false, 
            Thing equipment = null,
            ThingDef targetCoverDef = null)
        {
            base.Launch(launcher, origin, usedTarget, intendedTarget, hitFlags, preventFriendlyFire, equipment, targetCoverDef);
            
            _hdCannonMode = Utils.GetHdCannonModes(launcher as Pawn);
            if (_hdCannonMode == null)
            {
                return;
            }
            var originOffset = _hdCannonMode.Props.muzzleOffset;
            var fromOriginToDest = (intendedTarget.CenterVector3 - origin).normalized;
            var muzzlePos = origin + Quaternion.LookRotation(fromOriginToDest) * originOffset;
            
            FleckMaker.ThrowSmoke(muzzlePos, MapHeld, 1);
            
            FleckMaker.Static(muzzlePos, MapHeld, FleckDefOf.ShotFlash, 10);
            
            var fleck = FleckMaker.GetDataStatic(muzzlePos, MapHeld, _hdCannonMode.Props.muzzleFleckDef, _hdCannonMode.Props.muzzleFleckScale);
            fleck.rotation = fromOriginToDest.ToAngleFlat();
            MapHeld.flecks.CreateFleck(fleck);
        }

        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            var extension = def.GetModExtension<HDCannonExtension>() ?? HDCannonExtension.DEFAULT;

            if (hitThing != null)
            {
                EffecterDefOf.Mine.Spawn(hitThing, hitThing.MapHeld, 1);
            }
            
            var mode = GetMode();
            if (mode == Mode.Common)
            {
                var armorPenetration = hitThing == null
                    ? extension.commonModeArmorPenetration
                    : extension.commonModeArmorDirectHitPenetration;
                GenExplosion.DoExplosion(
                    PositionHeld,
                    MapHeld,
                    extension.commonModeExplosionRadius,
                    DamageDefOf.Bomb,
                    launcher,
                    extension.commonModeDamageAmount,
                    armorPenetration,
                    def.projectile.soundExplode,
                    equipmentDef,
                    def,
                    intendedTarget.Thing,
                    def.projectile.postExplosionSpawnThingDef ?? def.projectile.filth,
                    def.projectile.postExplosionSpawnChance,
                    def.projectile.postExplosionSpawnThingCount,
                    def.projectile.postExplosionGasType,
                    true,
                    def.projectile.preExplosionSpawnThingDef,
                    def.projectile.preExplosionSpawnChance,
                    def.projectile.preExplosionSpawnThingCount,
                    0,
                    extension.explosionDamageFallOff,
                    origin.AngleToFlat(destination),
                    null,
                    null,
                    true,
                    def.projectile.damageDef.expolosionPropagationSpeed,
                    0,
                    true,
                    def.projectile.postExplosionSpawnThingDefWater,
                    def.projectile.screenShakeFactor,
                    null,
                    null);
            }
            else
            {
                // spray effects
                if (hitThing != null)
                {
                    var dInfo = new DamageInfo(
                        DamageDefOf.Bullet,
                        extension.piercingModeDamageAmount,
                        extension.piercingModeArmorPenetration,
                        0,
                        launcher,
                        null,
                        def
                    );
                    hitThing.TakeDamage(dInfo).AssociateWithLog(new BattleLogEntry_RangedImpact(launcher, hitThing, intendedTarget.Thing, equipmentDef, def, targetCoverDef));
                }
                else
                {
                    EffecterDefOf.Deflect_General.Spawn(PositionHeld, MapHeld);
                }
            }
            
            Destroy();
        }

        private Mode GetMode()
        {
            const Mode defaultMode = Mode.Common;
            if (!(launcher is Pawn pawn))
            {
                return defaultMode;
            }

            var comp = Utils.GetHdCannonModes(pawn);
            if (comp == null)
            {
                return defaultMode;
            }

            return comp.curMode;
        }
    }
}