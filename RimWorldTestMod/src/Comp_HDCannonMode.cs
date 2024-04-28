using System;
using Verse;

namespace RimWorldTestMod
{
    public class CompProperties_HDCannonMode : CompProperties
    {
        public Mode initMode; 
        
        public CompProperties_HDCannonMode()
        {
            compClass = typeof(Comp_HDCannonMode);
        }
    }
    
    public class Comp_HDCannonMode : ThingComp
    {
        public Mode curMode;
        
        private CompProperties_HDCannonMode Props => props as CompProperties_HDCannonMode;

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