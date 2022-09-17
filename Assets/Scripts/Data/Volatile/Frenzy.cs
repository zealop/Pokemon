using UnityEngine;

namespace Data.Volatile
{
    public class Frenzy : VolatileCondition
    {
        public override VolatileID ID => VolatileID.Frenzy;

        private int counter;

        public Frenzy()
        {
            counter = Random.Range(1, 3);
        }
        public override void OnStart()
        {
            unit.LockedAction = true;   
        
            unit.Modifier.OnTurnEndList.Add(Rampage);
        }

        public override void OnEnd()
        {
            unit.LockedAction = false;

            unit.Modifier.OnTurnEndList.Remove(Rampage);
        }

        private void Rampage()
        {
            if (counter == 0)
            {
                unit.RemoveVolatileCondition(ID);
                unit.AddVolatileCondition(new Confused());
            }
            else
            {
                counter--;
            }
        }
    }
}