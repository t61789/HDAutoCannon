using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace HDAC
{
    public class Command_SwitchMode : Command
    {
        public Mode switchTargetMode;

        public Command_SwitchMode(Mode curMode)
        {
            icon = ModResources.textures[curMode];
            switchTargetMode = (Mode)((int)(curMode + 1) % Enum.GetValues(typeof(Mode)).Length);
            defaultLabel = "HDAC_Command_Label".Translate($"HDAC_ModeName_{curMode}".Translate());
            defaultDesc = $"HDAC_Command_Description_{curMode}".Translate();
        }
        
        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);

            var availablePawns = 
                from thing in Find.Selector.SelectedObjects
                where thing is Pawn
                let pawn = (Pawn)thing
                where pawn.IsColonistPlayerControlled 
                select pawn;
            
            foreach (var modes in Utils.GetAutoCannonComp(availablePawns))
            {
                modes.SwitchMode(switchTargetMode);
            }
        }
    }
}