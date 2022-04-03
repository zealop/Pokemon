using System;
using Battle;

namespace Move.Behaviour
{
    public class JumpKick : Default
    {
        private const string Message = "{0} kept going and crashes!";
        public override void Apply(BattleUnit source, BattleUnit target, Action consumePp)
        {
            source.Modifier.OnMissList.Add(CrashDamage);
            base.Apply(source, target, consumePp);
            source.Modifier.OnMissList.Remove(CrashDamage);
        }

        private static void CrashDamage(BattleUnit unit)
        {
            int damage = unit.MaxHp / 2;
            unit.TakeDamage(damage, Format(Message, unit));
        }
    }
}