using Battle;

namespace Move.Effect
{
    public class Payday : MoveEffect
    {
        private const string Message = "Coins scattered on the ground!";
        public override void Apply(Unit source, Unit target)
        {
            // BattleManager.Payday(source.Pokemon);
            Log(Message);
        }
    }
}