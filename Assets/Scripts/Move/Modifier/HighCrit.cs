using UnityEngine;

public class HighCrit : MoveModifier
{
    [SerializeField] int crit = 1;
    public override void ModifyMove()
    {
        _base.CritStage = (s, t) => crit;
    }
}

