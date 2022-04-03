using System;
using Battle;
using Move.Component;
using UnityEngine;

namespace Move.Behaviour
{
    public class Default : MoveBehavior
    {
        protected void RegisterMove(BattleUnit source, Action consumePp)
        {
            Log($"{source.Name} used {move.Name}!");
            source.LastUsedMove = move;
            consumePp();
        }
        public override void Apply(BattleUnit source, BattleUnit target, Action consumePp = null)
        {
            RegisterMove(source, consumePp);
        
            bool isHit = move.AccuracyCheck.Apply(source, target);
            if (!isHit)
            {
                source.Modifier.OnMiss();
                return;
            }
        
            if (move.Category == MoveCategory.Status)
            {
                move.Effect.Apply(source, target);
            }
            else
            {
                ApplyDamage(source, target);
                move.SecondaryEffect?.Apply(source, target);
            }
        }
        protected void ApplyDamage(BattleUnit source, BattleUnit target)
        {
            var damage = move.Damage.Apply(source, target);
            
            Debug.Log(damage.Value);
            source.ApplyDamage(target, damage);
        }
    }
}