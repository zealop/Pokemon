using UnityEngine;

namespace Game.Condition.Status
{
    public class ParalyzeCondition : StatusCondition
    {
        public override StatusConditionID ID => StatusConditionID.Paralyze;
        private const float SpeedModifier = 0.5f;
        private const float ParalyzeChance = 0.25f;
    
        public override void OnStart()
        {
            Unit.Modifier.OnBeforeMoveList.Add(ParalyzeCheck);
            Unit.Modifier.SpeedModList.Add(ParalyzeSlow);

            Log($"{Unit.Name} is paralyzed!");
        }

        public override void OnEnd()
        {
            Unit.Modifier.OnBeforeMoveList.Remove(ParalyzeCheck);
            Unit.Modifier.SpeedModList.Remove(ParalyzeSlow);

            Log($"{Unit.Name} is cured of its paralysis!");
        }

        private void ParalyzeCheck()
        {
            if (!(Random.value <= ParalyzeChance)) return;
            
            // Unit.CanMove = false;

            Log($"{Unit.Name} is fully paralyzed!");
        }

        private static float ParalyzeSlow()
        {
            return SpeedModifier;
        }
    }
}