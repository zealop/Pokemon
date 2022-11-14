using Game.Battles;
using UnityEngine;

namespace Game.Condition.Status
{
    public class ToxicCondition : StatusCondition
    {
        public override StatusConditionID ID => StatusConditionID.Toxic;
        private const float DamageModifier = 1 / 16f;
        private int counter;
    
        public override void OnStart()
        {
            
            Unit.Modifier.OnTurnEndList.Add(ToxicDamage);

            Log($"{Unit.Name} is badly poisoned!");
        }

        public override void OnEnd()
        {
            Unit.Modifier.OnTurnEndList.Add(ToxicDamage);

            Log($"{Unit.Name} is cured of its poison!");
        }

        private void ToxicDamage()
        {
            var damage = Mathf.FloorToInt(Unit.MaxHp * DamageModifier * ++counter);
            Unit.TakeDamage(new DamageDetail(damage));
            
            Log($"{Unit.Name} is hurt by poison!");
        }
    }
}