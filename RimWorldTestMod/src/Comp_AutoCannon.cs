using System;
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