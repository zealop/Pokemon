using Game.Moves;

namespace Game.Battles
{
    public class MoveNode
    {
        public MoveBuilder Move { get; }
        public  Unit Source { get; }
        public  Unit Target { get; }

        public MoveNode(MoveBuilder move, Unit source, Unit target)
        {
            Move = move;
            Source = source;
            Target = target;
        }
    }
}