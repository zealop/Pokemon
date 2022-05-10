using Battle;
using UnityEngine;

namespace Move.Damage
{
    public class PsyWave : MoveDamage
    {
        private const float Min = 0.5f;
        private const float Max = 1.5f;
        public override DamageDetail Apply(Unit source, Unit target)
        {
            float r = Random.Range(Min, Max);
            int damage = Mathf.Max(Mathf.FloorToInt(source.Level * r), 1);

            return new DamageDetail(damage);
        }
    }
}