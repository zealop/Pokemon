using Game.Battles;

namespace Game.Statuses
{
    public abstract class Status
    {
        protected abstract StatusID ID { get; }
        protected Unit Unit { get; private set; }
        protected static void Log(string message)
        {
            // BattleManager.i.Log(message);
        }

        private static T GetChild<T>(Unit unit) where T:Status, new()
        {
            var condition = new T
            {
                Unit = unit
            };
            return condition;
        }

        public static Status Create(StatusID id, Unit unit)
        {
            return id switch
            {
                StatusID.Poison => GetChild<Poison>(unit),
                StatusID.Burn => GetChild<Burn>(unit),
                StatusID.Sleep => GetChild<Sleep>(unit),
                StatusID.Paralysis => GetChild<Paralyze>(unit),
                StatusID.Freeze => GetChild<Freeze>(unit),
                StatusID.Toxic => GetChild<Toxic>(unit),
                _ => null,
            };
        }

        public static float CatchBonus(Status condition)
        {
            if (condition is null) return 1;

            return condition.ID switch
            {
                StatusID.Poison => 1.5f,
                StatusID.Burn => 1.5f,
                StatusID.Sleep => 2f,
                StatusID.Paralysis => 2f,
                StatusID.Freeze => 1.5f,
                StatusID.Toxic => 1.5f,
                _ => 1f,
            };
        }

        public abstract void OnStart();
        public abstract void OnEnd();
    }
}