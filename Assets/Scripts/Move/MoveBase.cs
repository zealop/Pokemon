using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "New move")]
public class MoveBase : SerializedScriptableObject
{
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

    [FoldoutGroup("B")] [OdinSerialize] private MoveDamage moveDamage;
    [FoldoutGroup("B")] [OdinSerialize] private MoveAccuracy moveAccuracy;
    [FoldoutGroup("B")] [OdinSerialize] private MoveEffect moveEffect;
    [FoldoutGroup("B")] [OdinSerialize] private SecondaryEffect secondaryEffect;

    public MoveDamage MoveDamage
    {
        get => moveDamage;
        set => moveDamage = value;
    }

    public MoveAccuracy MoveAccuracy
    {
        get => moveAccuracy;
        set => moveAccuracy = value;
    }

    public MoveEffect MoveEffect
    {
        get => moveEffect;
        set => moveEffect = value;
    }

    public string Name => _name;
    public string Description => description;
    public PokemonType Type => type;
    public MoveCategory Category => category;
    public MoveTarget Target => target;
    public int Accuracy => accuracy;
    public int PP => pp;
    public int Power => power;
    public int Priority => priority;
    public int CritStage => critStage;

    private void OnEnable()
    {
        Debug.Log("enabled");
        moveDamage.Init(this);
        moveAccuracy.Init(this);
    }

    public void Run(BattleUnit source, BattleUnit target)
    {
        if (Category != MoveCategory.Status)
        {
            bool isHit = moveAccuracy.Apply(source, target);
            if (!isHit)
            {
                source.OnMiss();
                return;
            }
            
            var damage = moveDamage.Apply(source, target);
            target.TakeDamage(damage);
        }
        else
        {
        }
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

    public void Init(MoveBase move)
    {
        this.move = move;
    }
}