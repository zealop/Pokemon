using Battle;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Move
{
    // [CreateAssetMenu(fileName = "Move", menuName = "New move")]
    public class MoveBase : SerializedScriptableObject
    {
        [SerializeField] private string baseName;

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

        public string Name => baseName;
        public string Description => description;
        public PokemonType Type => type;
        public MoveCategory Category => category;
        public MoveTarget Target => target;
        public int Accuracy => accuracy;
        public int Pp => pp;
        public int Power => power;

        public MoveBuilder Builder()
        {
            return new MoveBuilder()
                .Base(this)
                .Name(baseName)
                .Type(type)
                .Category(category)
                .Power(power)
                .Accuracy(accuracy)
                .Priority(priority)
                .CritStage(critStage)
                .Damage(damage)
                .AccuracyCheck(accuracyCheck)
                .Effect(effect)
                .SecondaryEffect(secondaryEffect)
                .Behavior(behavior);
        }
    }

    public abstract class MoveComponent
    {
        protected MoveBuilder moveBuilder;

        protected static BattleManager BattleManager => BattleManager.i;

        public void Init(MoveBuilder move)
        {
            this.moveBuilder = move;
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