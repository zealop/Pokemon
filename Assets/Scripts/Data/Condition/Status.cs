using Battle;
using Data.Condition.Statuses;

namespace Data.Condition
{
    public abstract class Status
    {
        public abstract StatusID ID { get; }
        protected Unit unit { get; private set; }
        protected static void Log(string message)
        {
            BattleManager.i.Log(message);
        }

        public void Bind(Unit unit)
        {
            this.unit = unit;
        }

        private static T GetChild<T>(Unit unit) where T:Status, new()
        {
            var condition = new T
            {
                unit = unit
            };
            return condition;
        }

        public static Status Create(StatusID id, Unit unit)
        {
            return id switch
            {
                StatusID.PSN => GetChild<Poison>(unit),
                StatusID.BRN => GetChild<Burn>(unit),
                StatusID.SLP => GetChild<Sleep>(unit),
                StatusID.PRZ => GetChild<Paralyze>(unit),
                StatusID.FRZ => GetChild<Freeze>(unit),
                StatusID.TOX => GetChild<Toxic>(unit),
                _ => null,
            };
        }

        public static float CatchBonus(Status condition)
        {
            if (condition is null) return 1;

            return condition.ID switch
            {
                StatusID.PSN => 1.5f,
                StatusID.BRN => 1.5f,
                StatusID.SLP => 2f,
                StatusID.PRZ => 2f,
                StatusID.FRZ => 1.5f,
                StatusID.TOX => 1.5f,
                _ => 1f,
            };
        }

        public abstract void OnStart();
        public abstract void OnEnd();
    }
}