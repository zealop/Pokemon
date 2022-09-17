using Battle;
using Move;
using UnityEngine;

namespace Data.Condition.Statuses
{
    public class Burn : Status
    {
        public override StatusID ID => StatusID.BRN;
        private const float DamageModifier = 1 / 16f;
        private const float AttackModifier = 1 / 2f;
    
        public override void OnStart()
        {
            Restart();

            Log($"{unit.Name} is burned!");
        }

        public override void OnEnd()
        {
            unit.Modifier.AttackerModList.Remove(BurnMod);
            unit.Modifier.OnTurnEndList.Remove(BurnDamage);

            Log($"{unit.Name} is cured of its burn!");
        }

        private void BurnDamage()
        {
            int damage = Mathf.FloorToInt(unit.MaxHp * DamageModifier);
            unit.TakeDamage(damage, $"{unit.Name} is hurt by its burn!");
        }

        private static float BurnMod(MoveBuilder move, Unit target)
        {
            return move.category == MoveCategory.Physical ? AttackModifier : 1f;
        }

        private void Restart()
        {
            unit.Modifier.AttackerModList.Add(BurnMod);
            unit.Modifier.OnTurnEndList.Add(BurnDamage);
        }
    }
}