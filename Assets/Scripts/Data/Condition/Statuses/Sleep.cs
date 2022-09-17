using UnityEngine;

namespace Data.Condition.Statuses
{
    public class Sleep : Status
    {
        public override StatusID ID => StatusID.SLP;
        private int counter;

        public void InitCounter(int turn = 0)
        {
            counter = turn == 0 ? Random.Range(1, 4) : turn;
        }
        public override void OnStart()
        {
            Restart();

            Log($"{unit.Name} fell asleep!");
        }

        public override void OnEnd()
        {
            unit.Modifier.OnBeforeMoveList.Remove(SleepCheck);

            Log($"{unit.Name} woke up!");
        }

        private void SleepCheck()
        {
            if (counter > 0)
            {
                counter--;
                unit.CanMove = false;

                Log($"{unit.Name} is fast asleep!");
            }
            else
            {
                unit.RemoveStatusCondition();
            }
        }

        private void Restart()
        {
            unit.Modifier.OnBeforeMoveList.Add(SleepCheck);
        }
    }
}