using Battle;

namespace Move.Behaviour
{
    public class Default : MoveBehavior
    {
        protected void RegisterMove(Unit source)
        {
            Log($"{source.Name} used {moveBuilder.name}!");
            source.LastUsedMove = moveBuilder.moveBase;
            moveBuilder.consumePp?.Invoke();
        }

        public override void Apply(Unit source, Unit target)
        {
            RegisterMove(source);

            bool isHit = moveBuilder.accuracyCheck.Apply(source, target);
            if (!isHit)
            {
                source.Modifier.OnMiss();
                return;
            }

            if (moveBuilder.category != MoveCategory.Status)
            {
                ApplyDamage(source, target);
                moveBuilder.secondaryEffect?.Apply(source, target);
            }
            moveBuilder.effect?.Apply(source, target);
        }

        protected void ApplyDamage(Unit source, Unit target)
        {
            var damage = moveBuilder.damage.Apply(source, target);
            source.ApplyDamage(target, damage);
        }
    }
}