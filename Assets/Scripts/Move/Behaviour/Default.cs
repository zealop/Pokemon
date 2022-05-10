using System;
using Battle;
using UnityEngine;

namespace Move.Behaviour
{
    public class Default : MoveBehavior
    {
        protected void RegisterMove(Unit source, Action consumePp)
        {
            Log($"{source.Name} used {move.Name}!");
            source.LastUsedMove = move;
            consumePp();
        }
        public override void Apply(Unit source, Unit target, Action consumePp = null)
        {
            RegisterMove(source, consumePp);

            bool isHit = move.accuracyCheck(source, target);
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
        protected void ApplyDamage(Unit source, Unit target)
        {
            var damage = move.Damage.Apply(source, target);
            
            Debug.Log(damage.Value);
            source.ApplyDamage(target, damage);
        }
    }
}