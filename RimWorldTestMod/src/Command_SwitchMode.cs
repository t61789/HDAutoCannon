using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorldTestMod
{
    public class Command_SwitchMode : Command
    {
        public Mode switchTargetMode;
        
        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);

            var availablePawns = 
                from thing in Find.Selector.SelectedObjects
                where thing is Pawn
                let pawn = (Pawn)thing
                where pawn.IsColonistPlayerControlled 
                select pawn;
            
            foreach (var modes in Utils.GetHdCannonModes(availablePawns))
            {
                modes.SwitchMode(switchTargetMode);
            }
        }
    }
}