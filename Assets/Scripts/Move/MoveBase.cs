using System;
using Battle;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Move
{
    [CreateAssetMenu(fileName = "Move", menuName = "New move")]
    public class MoveBase : SerializedScriptableObject
    {
        // ReSharper disable once InconsistentNaming
        [SerializeField] private string _name;

        [TextArea] [SerializeField] private string description;

        [SerializeField] private PokemonType type;
        [SerializeField] private MoveCategory category;
        [SerializeField] private MoveTarget target;

        [SerializeField] private int pp;
        [SerializeField] private int power;
        [SerializeField] private int accuracy;

        [FoldoutGroup("A")] [SerializeField] private int priority;
        [FoldoutGroup("A")] [SerializeField] private int critStage;

        [FoldoutGroup("B")] [OdinSerialize] private MoveDamage damage;
        [FoldoutGroup("B")] [OdinSerialize] private MoveAccuracy accuracyCheck;
        [FoldoutGroup("B")] [OdinSerialize] private MoveEffect effect;
        [FoldoutGroup("B")] [OdinSerialize] private SecondaryEffect secondaryEffect;
        [FoldoutGroup("B")] [OdinSerialize] private MoveBehavior behavior;

        public string Name => _name;
        public string Description => description;
        public PokemonType Type => type;
        public MoveCategory Category => category;
        public MoveTarget Target => target;
        public int Accuracy => accuracy;
        public int Pp => pp;
        public int Power => power;
        public MoveDamage Damage => damage;
        public MoveAccuracy AccuracyCheck => accuracyCheck;
        public MoveEffect Effect => effect;
        public SecondaryEffect SecondaryEffect => secondaryEffect;
        public MoveBehavior Behavior => behavior;
        public int Priority => priority;
        public int CritStage => critStage;

        private void Init()
        {
            damage.Init(this);
            accuracyCheck.Init(this);
            effect?.Init(this);
            secondaryEffect?.Init(this);
            behavior.Init(this);
        }

        public void Execute(Unit source, Unit target, Move move)
        {
            Init();
            behavior?.Apply(source, target);
        }

        public MoveBuilder Builder()
        {
            return new MoveBuilder()
                .Name(_name)
                .Description(description);
        }
    }

    public abstract class MoveComponent
    {
        protected MoveBuilder move;

        protected static BattleManager BattleManager => BattleManager.I;

        public void Init(MoveBuilder move)
        {
            this.move = move;
        }

        protected static void Log(string message, Unit source = null, Unit target = null)
        {
            BattleManager.Log(Format(message, source, target));
        }

        protected static string Format(string message, Unit source = null, Unit target = null)
        {
            return string.Format(message, source?.Name, target?.Name);
        }
    }
}