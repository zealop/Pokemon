using UnityEngine;

namespace Game.Statuses
{
    public class Burn : Status
    {
        protected override StatusID ID => StatusID.Burn;
        private const float DamageModifier = 1 / 16f;
        private const float AttackModifier = 1 / 2f;
    
        public override void OnStart()
        {
            Restart();

            Log($"{Unit.Name} is burned!");
        }

        public override void OnEnd()
        {
            // Unit.Modifier.AttackerModList.Remove(BurnMod);
            // Unit.Modifier.OnTurnEndList.Remove(BurnDamage);

            Log($"{Unit.Name} is cured of its burn!");
        }

        private void BurnDamage()
        {
            int damage = Mathf.FloorToInt(Unit.MaxHp * DamageModifier);
            // Unit.TakeDamage(damage, $"{Unit.Name} is hurt by its burn!");
        }

        // private static float BurnMod(MoveBuilder move, Unit target)
        // {
        //     return move.category == MoveCategory.Physical ? AttackModifier : 1f;
        // }

        private void Restart()
        {
            // Unit.Modifier.AttackerModList.Add(BurnMod);
            // Unit.Modifier.OnTurnEndList.Add(BurnDamage);
        }
    }
}