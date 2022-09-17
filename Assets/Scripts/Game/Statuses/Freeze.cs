using UnityEngine;

namespace Game.Statuses
{
    public class Freeze : Status
    {
        protected override StatusID ID => StatusID.Freeze;
        private const float ThawChance = 0.2f;
    
        public override void OnStart()
        {
            Restart();

            Log($"{Unit.Name} is frozen solid!");
        }

        public override void OnEnd()
        {
            // Unit.Modifier.OnBeforeMoveList.Remove(FreezeCheck);
            // Unit.Modifier.OnHitList.Remove(FireHit);

            Log($"{Unit.Name} thawed out!");
        }

        private void FreezeCheck()
        {
            if (Random.value <= ThawChance)
            {
                // Unit.RemoveStatusCondition();
            }
            else
            {
                // Unit.CanMove = false;
                Log($"{Unit.Name} is frozen solid!");
            }
        }

        // private void FireHit(MoveBase move, Unit source, int damage)
        // {
        //     if (move.Type == PokemonType.Fire)
        //     {
        //         Unit.RemoveStatusCondition();
        //     }
        // }

        private void Restart()
        {
            // Unit.Modifier.OnBeforeMoveList.Add(FreezeCheck);
            // Unit.Modifier.OnHitList.Add(FireHit);
        }
    }
}