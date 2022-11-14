using Game.Battles;
using UnityEngine;

namespace Game.Condition.Status
{
    public class PoisonCondition : StatusCondition
    {
        public override StatusConditionID ID => StatusConditionID.Poison;

        private const float DamageModifier = 1 / 8f;

        public override void OnStart()
        {
            Unit.Modifier.OnTurnEndList.Add(PoisonDamage);

            Log($"{Unit.Name} is poisoned!");
        }

        public override void OnEnd()
        {
            Unit.Modifier.OnTurnEndList.Remove(PoisonDamage);

            Log($"{Unit.Name} is cured of its poison!");
        }

        private void PoisonDamage()
        {
            var damage = Mathf.FloorToInt(Unit.MaxHp * DamageModifier);
            Unit.TakeDamage(new DamageDetail(damage));


            Log($"{Unit.Name} is hurt by poison!");
        }
    }
}