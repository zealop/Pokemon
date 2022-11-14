using System;
using Game.Battles;
using Game.Constants;

namespace Game.Moves
{
    public class MoveBuilder
    {
        public MoveBase Base { get;  }
        public string Name { get; set; }

        public PokemonType Type { get; set; }

        public MoveCategory Category { get; set; }

        public MoveTarget MoveTarget { get; set; }

        public int Power { get; set; }

        public int Accuracy { get; set; }

        public int Priority { get; set; }

        public int CritStage { get; set; }

        public IMoveDamage Damage { get; set; }

        public IMoveAccuracy AccuracyCheck { get; set; }

        public IMoveEffect Effect { get; set; }

        public SecondaryEffect SecondaryEffect { get; set; }

        public IMoveBehavior Behavior { get; set; }

        public Action ConsumePp { get; set; }

        public MoveBuilder(MoveBase moveBase, Action consumePp)
        {
            Base = moveBase;
            
            Name = moveBase.Name;
            Power = moveBase.Power;
            Accuracy = moveBase.Accuracy;
            Type = moveBase.Type;

            CritStage = moveBase.CritStage;
            Priority = moveBase.Priority;

            Behavior = moveBase.Behavior;
            Damage = moveBase.Damage;
            AccuracyCheck = moveBase.AccuracyCheck;
            Effect = moveBase.Effect;
            SecondaryEffect = moveBase.SecondaryEffect;
            ConsumePp = consumePp;
        }

        public void Execute(Unit source, Unit target)
        {
            Behavior.Apply(this ,source, target);
        }

        public bool IsAccurate(Unit source, Unit target)
        {
            return AccuracyCheck.Apply(this, source, target);
        }

        public DamageDetail GetDamage(Unit source, Unit target)
        {
            return Damage.Apply(this, source, target);
        }
    }
}