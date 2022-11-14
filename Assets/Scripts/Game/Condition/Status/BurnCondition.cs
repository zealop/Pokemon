using Game.Battles;
using Game.Moves;
using UnityEngine;

namespace Game.Condition.Status
{
    public class BurnCondition : StatusCondition
    {
        public override StatusConditionID ID => StatusConditionID.Burn;
        
        private const float DamageModifier = 1 / 16f;
        private const float AttackModifier = 1 / 2f;
        
        public override void OnStart()
        {
            Unit.Modifier.OnTurnEndList.Add(BurnDamage);
            Unit.Modifier.AttackerModList.Add(BurnMod);
            
            Log($"{Unit.Name} is burned!");
        }

        public override void OnEnd()
        {
            Unit.Modifier.OnTurnEndList.Remove(BurnDamage);
            Unit.Modifier.AttackerModList.Remove(BurnMod);
            
            Log($"{Unit.Name} is cured of its burn!");
        }
        
        private static float BurnMod(MoveBuilder move, Unit target)
        {
            return move.Category == MoveCategory.Physical ? AttackModifier : 1f;
        }
        
        private void BurnDamage()
        {
            var damage = Mathf.FloorToInt(Unit.MaxHp * DamageModifier);
            Unit.TakeDamage(new DamageDetail(damage));
            
            Log($"{Unit.Name} is hurt by its burn!");
        }
    }
}