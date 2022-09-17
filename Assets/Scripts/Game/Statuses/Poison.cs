using UnityEngine;

namespace Game.Statuses
{
    public class Poison : Status
    {
        protected override StatusID ID => StatusID.Poison;
        private const float DamageModifier = 1 / 8f;
    
        public override void OnStart()
        {
            // Unit.Modifier.OnTurnEndList.Add(PoisonDamage);

            Log($"{Unit.Name} is poisoned!");
        }

        public override void OnEnd()
        {
            // Unit.Modifier.OnTurnEndList.Remove(PoisonDamage);

            Log($"{Unit.Name} is cured of its poison!");
        }

        private void PoisonDamage()
        {
            int damage = Mathf.FloorToInt(Unit.MaxHp * DamageModifier);
            // Unit.TakeDamage(damage, $"{Unit.Name} is hurt by poison!");
        }
    }
}