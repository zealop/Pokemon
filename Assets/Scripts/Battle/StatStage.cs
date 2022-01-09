using System.Collections.Generic;
using System.Linq;

public class StatStage
{
    private static readonly float[] common = {1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f};
    private static readonly float[] aim = {1f, 4 / 3f, 5 / 3f, 2f, 7 / 3f, 8 / 3f, 3f};
    private static readonly BoostableStat[] AimStat = {BoostableStat.Accuracy, BoostableStat.Evasion};

    public Dictionary<BoostableStat, int> Value { get; } = new Dictionary<BoostableStat, int>()
    {
        {BoostableStat.Attack, 0},
        {BoostableStat.Defense, 0},
        {BoostableStat.SpAttack, 0},
        {BoostableStat.SpDefense, 0},
        {BoostableStat.Speed, 0},

        {BoostableStat.Accuracy, 0},
        {BoostableStat.Evasion, 0},
    };

    public int CritStage { get; set; }

    public float this[BoostableStat stat]
    {
        get
        {
            int stage = Value[stat];

            float[] array = AimStat.Contains(stat) ? aim : common;
            return stage >= 0 ? array[stage] : 1 / array[-stage];
        }
    }
}

public enum BoostableStat
{
    Attack,
    Defense,
    SpAttack,
    SpDefense,
    Speed,
    Accuracy,
    Evasion
}