using UnityEngine;

namespace Game.Statuses
{
    public class Sleep : Status
    {
        protected override StatusID ID => StatusID.Sleep;
        private int counter;

        public void InitCounter(int turn = 0)
        {
            counter = turn == 0 ? Random.Range(1, 4) : turn;
        }
        public override void OnStart()
        {
            Restart();

            Log($"{Unit.Name} fell asleep!");
        }

        public override void OnEnd()
        {
            // Unit.Modifier.OnBeforeMoveList.Remove(SleepCheck);

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
                // Unit.RemoveStatusCondition();
            }
        }

        private void Restart()
        {
            // Unit.Modifier.OnBeforeMoveList.Add(SleepCheck);
        }
    }
}