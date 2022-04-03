using Battle;
using UnityEngine;

namespace Move.Damage
{
    public class Flat : MoveDamage
    {
        [SerializeField] private int damage;
        public override DamageDetail Apply(BattleUnit source, BattleUnit target)
        {
            return new DamageDetail(damage);
        }
    }
}