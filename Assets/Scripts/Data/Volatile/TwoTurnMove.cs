using Battle;

namespace Data.Volatile
{
    public class TwoTurnMove : VolatileCondition
    {
        public override VolatileID ID => VolatileID.TwoTurnMove;

        public void BindToUnit(BattleUnit unit)
        {
            this.unit = unit;
        }
        public override void OnStart()
        {
            unit.LockedAction = true;
        }

        public override void OnEnd()
        {
            unit.LockedAction = false;
        }
    }
}