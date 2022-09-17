using Battle;
using Move;
using UnityEngine;

namespace Data.Condition.Statuses
{
    public class Freeze : Status
    {
        public override StatusID ID => StatusID.FRZ;
        private const float ThawChance = 0.2f;
    
        public override void OnStart()
        {
            Restart();

            Log($"{unit.Name} is frozen solid!");
        }

        public override void OnEnd()
        {
            unit.Modifier.OnBeforeMoveList.Remove(FreezeCheck);
            unit.Modifier.OnHitList.Remove(FireHit);

            Log($"{unit.Name} thawed out!");
        }

        private void FreezeCheck()
        {
            if (Random.value <= ThawChance)
            {
                unit.RemoveStatusCondition();
            }
            else
            {
                unit.CanMove = false;
                Log($"{unit.Name} is frozen solid!");
            }
        }

        private void FireHit(MoveBase move, Unit source, int damage)
        {
            if (move.Type == PokemonType.Fire)
            {
                unit.RemoveStatusCondition();
            }
        }

        private void Restart()
        {
            unit.Modifier.OnBeforeMoveList.Add(FreezeCheck);
            unit.Modifier.OnHitList.Add(FireHit);
        }
    }
}