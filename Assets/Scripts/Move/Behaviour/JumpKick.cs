using System;
using Battle;

namespace Move.Behaviour
{
    public class JumpKick : Default
    {
        private const string Message = "{0} kept going and crashes!";
        public override void Apply(Unit source, Unit target)
        {
            source.Modifier.OnMissList.Add(CrashDamage);
            base.Apply(source, target);
            source.Modifier.OnMissList.Remove(CrashDamage);
        }

        private static void CrashDamage(Unit unit)
        {
            int damage = unit.MaxHp / 2;
            unit.TakeDamage(damage, Format(Message, unit));
        }
    }
}