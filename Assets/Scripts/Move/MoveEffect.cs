using Battle;

namespace Move
{
    public abstract class MoveEffect : MoveComponent
    {
        public abstract void Apply(Unit source, Unit target);

        protected static void OnFail()
        {
            Log("But it failed!");
        }
    }
}