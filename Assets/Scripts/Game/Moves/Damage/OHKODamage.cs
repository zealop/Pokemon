using Game.Battles;

namespace Game.Moves.Damage
{
    public class OHKODamage : IMoveDamage
    {
        public DamageDetail Apply(MoveBuilder move, Unit source, Unit target)
        {
            return new DamageDetail(target.MaxHp);
        }
    }
}