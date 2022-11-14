using Game.Battles;

namespace Game.Moves
{
    public interface IMoveEffect : IMoveComponent
    {
        void Apply(MoveBuilder move, Unit source, Unit target);

        protected static void OnFail()
        {
            // Log("But it failed!");
        }
    }
}