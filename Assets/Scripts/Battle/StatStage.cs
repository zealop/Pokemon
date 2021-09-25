using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StatStage
{
    static float[] common = { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };
    static BoostableStat[] commonStat = { BoostableStat.Attack, BoostableStat.Defense, BoostableStat.SpAttack, BoostableStat.SpDefense, BoostableStat.Speed };

    static float[] aim = { 1f, 4 / 3f, 5 / 3f, 2f, 7 / 3f, 8 / 3f, 3f };
    static BoostableStat[] aimStat = { BoostableStat.Accuracy, BoostableStat.Evasion };

    public Dictionary<BoostableStat, int> Value { get; private set; } = new Dictionary<BoostableStat, int>()
    {
        {BoostableStat.Attack, 0 },
        {BoostableStat.Defense, 0 },
        {BoostableStat.SpAttack, 0 },
        {BoostableStat.SpDefense, 0 },
        {BoostableStat.Speed, 0 },

        {BoostableStat.Accuracy, 0 },
        {BoostableStat.Evasion, 0 },
    };

    public int CritStage;

    public float this[BoostableStat stat]
    {
        get
        {
            int stage = Value[stat];

            var array = aimStat.Contains(stat) ? aim : common;
            return stage >= 0 ? array[stage] : 1 / array[-stage];
                
        }
    }
}