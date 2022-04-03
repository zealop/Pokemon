using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Move.Component;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Move
{
    [CreateAssetMenu(fileName = "Move", menuName = "New move")]
    public class MoveBase : SerializedScriptableObject
    {
        // ReSharper disable once InconsistentNaming
        [SerializeField] public string _name;

        [TextArea] [SerializeField] public string description;

        [SerializeField] public PokemonType type;
        [SerializeField] public MoveCategory category;
        [SerializeField] public MoveTarget target;

        [SerializeField] public int pp;
        [SerializeField] public int power;
        [SerializeField] public int accuracy;

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

        public void Execute(BattleUnit source, BattleUnit target, Action consumePp)
        {
            Init();
            behavior?.Apply(source, target, consumePp);
        }
    }

    public class DamageDetail
    {
        public readonly int Value;
        public readonly List<string> Messages = new List<string>();

        public DamageDetail(int value)
        {
            Value = value;
        }

        public DamageDetail(int value, string message) : this(value)
        {
            Messages.Add(message);
        }
    }

    public enum MoveCategory
    {
        Physical,
        Special,
        Status
    }

    public enum MoveTarget
    {
        Foe,
        Self
    }

    public abstract class MoveComponent
    {
        protected MoveBase move;
    
        protected static BattleManager BattleManager => BattleManager.I;

        public void Init(MoveBase move)
        {
            this.move = move;
        }
        
        protected static void Log(string message, BattleUnit source = null, BattleUnit target = null)
        {
            BattleManager.Log(Format(message, source, target));
        }

        protected static string Format(string message, BattleUnit source = null, BattleUnit target = null)
        {
            return string.Format(message, source?.Name, target?.Name);
        }
    }
}