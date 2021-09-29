using UnityEngine;

public class MultiStrike : MoveModifier
{
    [SerializeField] int count;
    [SerializeField] bool accuracyCheck;


    public override void ModifyMove()
    {
        _base.HitCount = MultiHit;
    }

    int MultiHit(BattleUnit source, BattleUnit target)
    {
        return count > 0 ? count : TwoFiveHit(Random.value);

    }

    int TwoFiveHit(float value)
    {
        if (value < 0.35)
            return 2;
        if (value < 0.7)
            return 3;
        if (value < 0.85)
            return 4;
        return 5;
    }
}
