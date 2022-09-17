namespace Data.Volatile
{
    public class Flinch : VolatileCondition
    {
        public override VolatileID ID => VolatileID.Flinch;

        public override void OnStart()
        {
            unit.Modifier.OnBeforeMoveList.Add(Flinched);
            unit.Modifier.OnTurnEndList.Add(Unflinching);
        }

        public override void OnEnd()
        {
            unit.Modifier.OnBeforeMoveList.Remove(Flinched);
            unit.Modifier.OnTurnEndList.Remove(Unflinching);
        }

        private void Flinched()
        {
            unit.CanMove = false;
            Log($"{unit.Name} flinched!");
        }

        private void Unflinching()
        {
            unit.RemoveVolatileCondition(ID);
        }
    }
}