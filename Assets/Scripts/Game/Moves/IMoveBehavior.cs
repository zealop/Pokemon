using Game.Battles;

namespace Game.Moves
{
    public interface IMoveBehavior : IMoveComponent
    {
        /// <summary></summary>
        public abstract void Apply(MoveBuilder move, Unit source, Unit target);
    }
}