using UnityEngine;

namespace Data.Condition.Statuses
{
    public class Poison : Status
    {
        public override StatusID ID => StatusID.PSN;
        private const float DamageModifier = 1 / 8f;
    
        public override void OnStart()
        {
            unit.Modifier.OnTurnEndList.Add(PoisonDamage);

            Log($"{unit.Name} is poisoned!");
        }

        public override void OnEnd()
        {
            unit.Modifier.OnTurnEndList.Remove(PoisonDamage);

            Log($"{unit.Name} is cured of its poison!");
        }

        private void PoisonDamage()
        {
            int damage = Mathf.FloorToInt(unit.MaxHp * DamageModifier);
            unit.TakeDamage(damage, $"{unit.Name} is hurt by poison!");
        }
    }
}