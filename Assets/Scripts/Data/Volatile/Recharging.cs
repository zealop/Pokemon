namespace Data.Volatile
{
    public class Recharging : VolatileCondition
    {
        public override VolatileID ID => VolatileID.Recharging;

        private int counter;

        public override void OnStart()
        {
            unit.LockedAction = true;
            counter = 2;
            unit.Modifier.OnTurnEndList.Add(Recharge);
        }

        public override void OnEnd()
        {
            unit.LockedAction = false;
            unit.Modifier.OnTurnEndList.Remove(Recharge);
        }

        private void Recharge()
        {
            counter--;
            if (counter > 0)
            {
                Log($"{unit.Name} must recharge!");
            }
            else
            {
                unit.RemoveVolatileCondition(ID);
            }
        }
    }
}