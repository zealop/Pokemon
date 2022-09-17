using UnityEngine;

namespace Data.Condition.Statuses
{
    public class Toxic : Status
    {
        public override StatusID ID => StatusID.TOX;
        private const float DamageModifier = 1 / 16f;
        private int counter;
    
        public override void OnStart()
        {
            Restart();

            Log($"{unit.Name} is badly poisoned!");
        }

        public override void OnEnd()
        {
            unit.Modifier.OnTurnEndList.Add(ToxicDamage);

            Log($"{unit.Name} is cured of its poison!");
        }

        private void ToxicDamage()
        {
            int damage = Mathf.FloorToInt(unit.MaxHp * DamageModifier * counter);
            counter++;
            unit.TakeDamage(damage, $"{unit.Name} is hurt by poison!");
        }

        private void Restart()
        {
            counter = 1;
            unit.Modifier.OnTurnEndList.Add(ToxicDamage);
        }
    }
}