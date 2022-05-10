using System.Collections;
using System.Collections.Generic;
using Battle;

namespace Data
{
    public abstract class VolatileCondition
    {
        public abstract VolatileID ID { get; }
        protected Unit unit;
        protected static Queue<IEnumerator> AnimationQueue => BattleManager.I.AnimationQueue;
        protected static DialogBox DialogBox => BattleManager.I.DialogBox;
        public static T Create<T>(Unit unit) where T : VolatileCondition, new()
        {
            var condition = new T
            {
                unit = unit
            };
            return condition;
        }

        public abstract void OnStart();
        public abstract void OnEnd();
    }

    public enum VolatileID
    {
        TwoTurnMove,
        Dig,
        Dive,
        Fly,
        Recharge,
        Bide,
        Bound,
        Flinch,
        Thrash,
        Confused,
        Disabled,
        Seeded,
        Counter,
        Rage,
        Minimize,
        Transform,
    }
}