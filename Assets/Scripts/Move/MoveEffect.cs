using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveEffect
{
    protected MoveBase _base;

    public void LoadEffect(MoveBase move)
    {
        _base = move;
    }
    public virtual IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        yield return null;
    }

}
