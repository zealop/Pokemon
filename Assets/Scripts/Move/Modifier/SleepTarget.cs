public class SleepTarget : MoveModifier
{
    public override void ModifyMove()
    {
        _base.ExtraImmunityCheck = RequireSleepTarget;
    }

    bool RequireSleepTarget(BattleUnit source, BattleUnit target)
    {
        if (target.Status.ID == StatusID.SLP)
            return false;

        return true;
    }
}
