using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimWorldTestMod
{
    public class CompProperties_HDCannonMode : CompProperties
    {
        public Mode initMode;
        public Vector3 muzzleOffset = new Vector3(0, 0, 0);
        public FleckDef muzzleFleckDef = FleckDefOf.ShotFlash;
        public float muzzleFleckScale = 2;
        
        public CompProperties_HDCannonMode()
        {
            compClass = typeof(Comp_HDCannonMode);
        }
    }
    
    public class Comp_HDCannonMode : ThingComp
    {
        public Mode curMode;
        
        public CompProperties_HDCannonMode Props => props as CompProperties_HDCannonMode;

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            
            curMode = Props.initMode;
        }

        public void SwitchMode(Mode mode)
        {
            curMode = mode;
        }
    }
}