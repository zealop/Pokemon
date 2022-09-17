using Battle;

namespace Data
{
    public abstract class VolatileCondition
    {
        public abstract VolatileID ID { get; }
        protected Unit unit;

        protected static void Log(string message)
        {
            BattleManager.i.Log(message);
        }

        public void Bind(Unit unit)
        {
            this.unit = unit;
        }

        public abstract void OnStart();
        public abstract void OnEnd();
    }

    public enum VolatileID
    {
        TwoTurnMove,
        Recharging,
        Bide,
        Bound,
        Flinch,
        Frenzy,
        Confused,
        Disabled,
        Seeded,
        Counter,
        Rage,
        Minimize,
        Transform,
    }
}