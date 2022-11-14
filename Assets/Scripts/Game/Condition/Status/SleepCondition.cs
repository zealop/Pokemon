using UnityEngine;

namespace Game.Condition.Status
{
    public class SleepCondition : StatusCondition
    {
        public override StatusConditionID ID => StatusConditionID.Sleep;
        private int counter;

        public void InitCounter(int turn = 0)
        {
            counter = turn == 0 ? Random.Range(1, 4) : turn;
        }
        public override void OnStart()
        {
            Unit.Modifier.OnBeforeMoveList.Add(SleepCheck);

            Log($"{Unit.Name} fell asleep!");
        }

        public override void OnEnd()
        {
            Unit.Modifier.OnBeforeMoveList.Remove(SleepCheck);

            Log($"{Unit.Name} woke up!");
        }

        private void SleepCheck()
        {
            if (counter > 0)
            {
                counter--;
                // Unit.CanMove = false;

                Log($"{Unit.Name} is fast asleep!");
            }
            else
            {
                Unit.RemoveStatusCondition();
            }
        }
    }
}