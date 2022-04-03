using UnityEngine;

namespace Data.Volatile
{
    public class Thrash : VolatileCondition
    {
        public override VolatileID ID => VolatileID.Thrash;

        private int counter;

        public Thrash()
        {
            counter = Random.Range(1, 3);
        }
        public override void OnStart()
        {
            unit.LockedAction = true;
        
            //Unit.OnTurnEndList.Add(Rampage);
        }

        public override void OnEnd()
        {
            unit.LockedAction = false;

            //Unit.OnTurnEndList.Remove(Rampage);
        }

        private void Rampage()
        {
            if (!unit.CanMove)
            {
                unit.RemoveVolatileCondition(ID);
            }
            else if (counter == 0)
            {
                unit.RemoveVolatileCondition(ID);
                unit.AddVolatileCondition(new VolatileConfused());
            }
            else
            {
                counter--;
            }
        }
    }
}