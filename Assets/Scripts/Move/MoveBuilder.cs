using System;
using Battle;

namespace Move
{
    public class MoveBuilder
    {
        private string name;
        private string description;

        private PokemonType type;
        private MoveCategory category;
        private MoveTarget target;

        private int pp;
        private int power;
        private int accuracy;

        private int priority;
        private int critStage;

        private MoveDamage damage;
        public Func<Unit, Unit, bool> accuracyCheck { get; private set; }
        private MoveEffect effect;
        private SecondaryEffect secondaryEffect;
        private MoveBehavior behavior;

        private Move move;

        public void Execute(Unit source, Unit target)
        {
            behavior?.Apply(source, target);
        }
        public MoveBuilder Name(string name)
        {
            this.name = name;
            return this;
        }

        public MoveBuilder Description(string description)
        {
            this.description = description;
            return this;
        }

        public MoveBuilder Type(PokemonType type)
        {
            this.type = type;
            return this;
        }

        public MoveBuilder Category(MoveCategory category)
        {
            this.category = category;
            return this;
        }

        public MoveBuilder Pp(int pp)
        {
            this.pp = pp;
            return this;
        }

        public MoveBuilder Power(int power)
        {
            this.power = power;
            return this;
        }

        public MoveBuilder Accuracy(int accuracy)
        {
            this.accuracy = accuracy;
            return this;
        }

        public MoveBuilder Priority(int priority)
        {
            this.priority = priority;
            return this;
        }

        public MoveBuilder CritStage(int critStage)
        {
            this.critStage = critStage;
            return this;
        }

        public MoveBuilder Damage(MoveDamage damage)
        {
            damage.Init(this);
            this.damage = damage.Apply;
            return this;
        }

        public MoveBuilder AccuracyCheck(MoveAccuracy accuracyCheck)
        {
            accuracyCheck.Init(this);
            this.accuracyCheck = accuracyCheck.Apply;
            return this;
        }

        public MoveBuilder Effect(MoveEffect effect)
        {
            this.effect = effect;
            return this;
        }

        public MoveBuilder SecondaryEffect(SecondaryEffect secondaryEffect)
        {
            this.secondaryEffect = secondaryEffect;
            return this;
        }

        public MoveBuilder Behavior(MoveBehavior behavior)
        {
            this.behavior = behavior;
            return this;
        }

        public MoveBuilder Move(Move move)
        {
            this.move = move;
            return this;
        }
    }
}