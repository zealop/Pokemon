using Game.Battles;

namespace Game.Moves.Behavior
{
    public class Default : IMoveBehavior
    {
        public virtual void Apply(MoveBuilder move, Unit source, Unit target)
        {
            RegisterMove(move, source);

            var isHit = move.IsAccurate(source, target);
            if (!isHit)
            {
                source.Modifier.OnMiss();
                return;
            }

            if (move.Category != MoveCategory.Status)
            {
                ApplyDamage(move, source, target);
                move.SecondaryEffect?.Apply(move, source, target);
            }

            move.Effect?.Apply(move ,source, target);
        }

        protected virtual void ApplyDamage(MoveBuilder move, Unit source, Unit target)
        {
            var damage = move.GetDamage(source, target);
            source.ApplyDamage(target, damage);
        }
        
        private void RegisterMove(MoveBuilder move, Unit source)
        {
            // Log($"{source.Name} used {moveBuilder.name}!");
            // source.LastUsedMove = moveBuilder.moveBase;
            // moveBuilder.consumePp?.Invoke();
        }
    }
}