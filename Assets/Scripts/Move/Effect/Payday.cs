using Battle;

namespace Move.Effect
{
    public class Payday : MoveEffect
    {
        private const string Message = "Coins scattered on the ground!";
        public override void Apply(BattleUnit source, BattleUnit target)
        {
            // BattleManager.Payday(source.Pokemon);
            Log(Message);
        }
    }
}