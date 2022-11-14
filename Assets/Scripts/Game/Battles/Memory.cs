using Game.Moves;

namespace Game.Battles
{
    public class Memory
    {
        public Memory(Unit unit)
        {
            Unit = unit;
        }

        public Unit Unit { get; }
        
        public MoveBase LastUsed { get; set; }
    }
}