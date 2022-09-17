using System;
using Battle;

namespace Move
{
    public class MoveBuilder
    {
        public MoveSlot moveSlot;
        public MoveBase moveBase;

        public string name;

        public PokemonType type;
        public MoveCategory category;
        public MoveTarget moveTarget;

        public int power;
        public int accuracy;

        public int priority;
        public int critStage;
        
        public MoveDamage damage;
        public MoveAccuracy accuracyCheck;
        public MoveEffect effect;
        public SecondaryEffect secondaryEffect;
        public MoveBehavior behavior;
        
        public Action consumePp;

        public void Execute(Unit source, Unit target)
        {
            behavior?.Apply(source, target);
        }
        
        public void Prepare(Unit source)
        {
            throw new NotImplementedException();
        }
        
        public MoveBuilder Base(MoveBase _base)
        {
            this.moveBase = _base;
            return this;
        }

        public MoveBuilder Name(string name)
        {
            this.name = name;
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
            this.damage = damage;
            return this;
        }

        public MoveBuilder AccuracyCheck(MoveAccuracy accuracyCheck)
        {
            this.accuracyCheck = accuracyCheck;
            accuracyCheck.Init(this);
            return this;
        }

        public MoveBuilder Effect(MoveEffect effect)
        {
            this.effect = effect;
            effect?.Init(this);
            return this;
        }

        public MoveBuilder SecondaryEffect(SecondaryEffect secondaryEffect)
        {
            this.secondaryEffect = secondaryEffect;
            secondaryEffect?.Init(this);
            return this;
        }

        public MoveBuilder Behavior(MoveBehavior behavior)
        {
            this.behavior = behavior;
            behavior.Init(this);
            return this;
        }

        public MoveBuilder Move(MoveSlot moveSlot)
        {
            this.moveSlot = moveSlot;
            this.consumePp = () => moveSlot.Pp--;
            return this;
        }
    }
}