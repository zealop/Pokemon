using Move;
using UnityEngine;

namespace Data.Volatile
{
    public class Confused : VolatileCondition
    {
        public override VolatileID ID => VolatileID.Confused;

        private const int Power = 40;

        private int counter;

        public Confused()
        {
            counter = Random.Range(2, 6);
        }
        
        public override void OnStart()
        {
            unit.Modifier.OnBeforeMoveList.Add(Confusion);
            unit.Modifier.OnTurnEndList.Add(Count);
            Log($"{unit.Name} became confused!");
        }

        public override void OnEnd()
        {
            unit.Modifier.OnBeforeMoveList.Remove(Confusion);
            unit.Modifier.OnTurnEndList.Remove(Count);
            Log($"{unit.Name} snapped out of its confusion!");
        }

        private void Count()
        {
            counter--;
        }
        private void Confusion()
        {
            if (counter == 0)
            {
                unit.RemoveVolatileCondition(ID);
                return;
            }
            
            Log($"{unit.Name} is confused!");
            
            if (!(Random.value < 0.33f)) return;
            
            Log("It hurt it self in its confusion!");
            // unit.CanMove = false;
            unit.TakeDamage(damage());
        }
        
        
        private DamageDetail damage()
        {
            float randMod = Random.Range(0.85f, 1f);

            int attack = unit.Attack;
            int defense = unit.Defense;

            float a = (2 * unit.Level + 10) / 250f;
            float d = a * Power * ((float) attack / defense) + 2;

            int damage = Mathf.FloorToInt(d * randMod);

            return new DamageDetail(damage);
        }
    }
}