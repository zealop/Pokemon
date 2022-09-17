using Move;

namespace Data.Volatile
{
    public class Disabled : VolatileCondition
    {
        public override VolatileID ID => VolatileID.Disabled;

        public readonly MoveBase move;
        private int counter;

        public Disabled(MoveBase move)
        {
            this.move = move;
            this.counter = 4;
        }
        public override void OnStart()
        {
            unit.Modifier.OnTurnEndList.Add(CountDown);
            Log($"{unit.Name}'s {move.Name} was disabled!");
        }

        public override void OnEnd()
        {
            unit.Modifier.OnTurnEndList.Remove(CountDown);
            Log($"{unit.Name}'s {move.Name} is no longer disabled!");
        }

        private void CountDown()
        {
            counter--;
            if (counter == 0)
            {
                unit.RemoveVolatileCondition(ID);
            }
        }
    }
}