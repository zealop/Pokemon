using Game.Battles;

namespace Game.Moves
{
    public interface IMoveDamage : IMoveComponent
    {
        DamageDetail Apply(MoveBuilder move, Unit source, Unit target);
    }
}