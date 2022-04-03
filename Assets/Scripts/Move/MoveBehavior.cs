using System;
using Battle;

namespace Move.Component
{
    public abstract class MoveBehavior : MoveComponent
    {
        /// <summary></summary>
        public abstract void Apply(BattleUnit source, BattleUnit target, Action consumePp);
    }
}