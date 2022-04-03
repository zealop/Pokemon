using Battle;
using UnityEngine;

namespace Move.Damage
{
    public class Default : MoveDamage
    {
        private const string CritMessage = "A critical hit!";
        private const string NotEffectiveMessage = "It's super effective!";
        private const string SuperEffectiveMessage = "It's not very effective!";
        private const float CritMod = 1.5f;

        public override DamageDetail Apply(BattleUnit source, BattleUnit target)
        {
            float critMod = 1f;
            if (Random.value <= MoveCritical.CritChance(move.CritStage, source, target))
            {
                critMod = CritMod;
            }

            float randMod = Random.Range(0.85f, 1f);
            float typeMod = TypeChart.GetEffectiveness(move.Type, target.Types);
            float stab = source.Types.Contains(move.Type) ? 1.5f : 1f;
            
            int attack = move.Category == MoveCategory.Physical ? source.Attack : source.SpAttack;
            int defense = move.Category == MoveCategory.Special ? target.SpDefense : target.Defense;

            float a = (2 * source.Level + 10) / 250f;
            float d = a * move.Power * ((float) attack / defense) + 2;

            float attackMod = source.Modifier.AttackerMod(move, target);
            float defenseMod = target.Modifier.DefenderMod(move, source);

            int damage = Mathf.FloorToInt(d * stab * critMod * randMod * typeMod * attackMod * defenseMod);

            var detail = new DamageDetail(damage);

            if (critMod > 1f)
                detail.Messages.Add(CritMessage);
            if (typeMod > 1f)
                detail.Messages.Add(NotEffectiveMessage);
            if (typeMod < 1f)
                detail.Messages.Add(SuperEffectiveMessage);

            return detail;
        }
    }
}