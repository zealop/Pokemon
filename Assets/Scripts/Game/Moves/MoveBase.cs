using Game.Constants;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game.Moves
{
    [CreateAssetMenu(menuName = "Pokemon/Create new move")]
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

        [FoldoutGroup("B")] [OdinSerialize] private IMoveDamage damage;
        [FoldoutGroup("B")] [OdinSerialize] private IMoveAccuracy accuracyCheck;
        [FoldoutGroup("B")] [OdinSerialize] private IMoveEffect effect;
        [FoldoutGroup("B")] [OdinSerialize] private SecondaryEffect secondaryEffect;
        [FoldoutGroup("B")] [OdinSerialize] private IMoveBehavior behavior;

        public string Name => baseName;
        public string Description => description;
        public PokemonType Type => type;
        public MoveCategory Category => category;
        public MoveTarget Target => target;
        public int Pp => pp;
        public int Power => power;
        public int Accuracy => accuracy;
        public int Priority => priority;
        public int CritStage => critStage;
        public IMoveDamage Damage => damage;
        public IMoveAccuracy AccuracyCheck => accuracyCheck;
        public IMoveEffect Effect => effect;
        public SecondaryEffect SecondaryEffect => secondaryEffect;
        public IMoveBehavior Behavior => behavior;
    }
}