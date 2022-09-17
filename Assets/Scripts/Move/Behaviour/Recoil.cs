using Battle;
using Sirenix.Serialization;
using UnityEngine;

namespace Move.Behaviour
{
    public class Recoil : Default
    {
        private const string Message = "{0} was damaged by the recoil!";

        [OdinSerialize] private readonly float ratio;

        public Recoil(float ratio)
        {
            this.ratio = ratio;
        }

        public override void Apply(Unit source, Unit target)
        {
            source.Modifier.OnApplyDamage += RecoilDamage;
            base.Apply(source, target);
            source.Modifier.OnApplyDamage -= RecoilDamage;
        }

        private void RecoilDamage(Unit unit, int damage)
        {
            damage = Mathf.FloorToInt(damage * ratio);
            unit.TakeDamage(damage, Format(Message, unit));
        }
    }
}