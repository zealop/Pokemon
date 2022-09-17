using Battle;
using Move;

namespace Data.Volatile
{
    public class Counter : VolatileCondition
    {
        public override VolatileID ID => VolatileID.Counter;

        public int storedDamage;

        public override void OnStart()
        {
            unit.Modifier.OnHitList.Add(StoreDamage);
            unit.Modifier.OnTurnEndList.Add(Clear);
        }

        public override void OnEnd()
        {
            unit.Modifier.OnHitList.Remove(StoreDamage);
            unit.Modifier.OnTurnEndList.Remove(Clear);
        }

        private void StoreDamage(MoveBase move, Unit source, int damage)
        {
            if (move.Category == MoveCategory.Physical)
                storedDamage += damage;
        }

        private void Clear()
        {
            unit.RemoveVolatileCondition(ID);
        }
    }
}