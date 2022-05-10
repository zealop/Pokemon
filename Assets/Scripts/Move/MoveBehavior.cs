using System;
using Battle;

namespace Move
{
    public abstract class MoveBehavior : MoveComponent
    {
        /// <summary></summary>
        public abstract void Apply(Unit source, Unit target, Action consumePp);
    }
}