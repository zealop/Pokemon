using UnityEngine;

namespace Data.Condition.Statuses
{
    public class Paralyze : Status
    {
        public override StatusID ID => StatusID.PRZ;
        private const float SpeedModifier = 0.5f;
        private const float ParalyzeChance = 0.25f;
    
        public override void OnStart()
        {
            Restart();

            Log($"{unit.Name} is paralyzed!");
        }

        public override void OnEnd()
        {
            unit.Modifier.OnBeforeMoveList.Remove(ParalyzeCheck);
            unit.Modifier.SpeedModList.Remove(ParalyzeSlow);

            Log($"{unit.Name} is cured of its paralysis!");
        }

        private void ParalyzeCheck()
        {
            if (!(Random.value <= ParalyzeChance)) return;
            
            unit.CanMove = false;

            Log($"{unit.Name} is fully paralyzed!");
        }

        private static float ParalyzeSlow()
        {
            return SpeedModifier;
        }

        private void Restart()
        {
            unit.Modifier.OnBeforeMoveList.Add(ParalyzeCheck);
            unit.Modifier.SpeedModList.Add(ParalyzeSlow);
        }
    }
}