using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightPower : MoveModifier
{

    static int[] weights =  {100,   250,    500,    1000,   2000,   10000 };
    static int[] powers =   {20,    40,     60,     80,     100,    120 };
    public override void ModifyMove()
    {
        _base.Power = WeightBased;
    }

    int WeightBased(BattleUnit source, BattleUnit target)
    {
        int i = 0;
        while(target.Weight > weights[i])
        {
            i++;
        }
        return powers[i];
    }
}
