using Game.Battles;

namespace Game.Moves
{
    public interface IMoveAccuracy : IMoveComponent
    {
        /// <summary>Run accuracy check for move</summary>
        /// <returns>Returns true if pass accuracy check</returns>
        public abstract bool Apply(MoveBuilder move, Unit source, Unit target);
    }
}