using UnityEngine;

namespace Game.Statuses
{
    public class Toxic : Status
    {
        protected override StatusID ID => StatusID.Toxic;
        private const float DamageModifier = 1 / 16f;
        private int counter;
    
        public override void OnStart()
        {
            Restart();

            Log($"{Unit.Name} is badly poisoned!");
        }

        public override void OnEnd()
        {
            // Unit.Modifier.OnTurnEndList.Add(ToxicDamage);

            Log($"{Unit.Name} is cured of its poison!");
        }

        private void ToxicDamage()
        {
            int damage = Mathf.FloorToInt(Unit.MaxHp * DamageModifier * counter);
            counter++;
            // Unit.TakeDamage(damage, $"{Unit.Name} is hurt by poison!");
        }

        private void Restart()
        {
            counter = 1;
            // Unit.Modifier.OnTurnEndList.Add(ToxicDamage);
        }
    }
}