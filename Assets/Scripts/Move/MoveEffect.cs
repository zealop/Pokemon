using Battle;

namespace Move
{
    public abstract class MoveEffect : MoveComponent
    {
        public abstract void Apply(BattleUnit source, BattleUnit target);

        protected static void OnFail()
        {
            Log("But it failed!");
        }
    }
}