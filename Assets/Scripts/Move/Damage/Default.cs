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

        public override DamageDetail Apply(Unit source, Unit target)
        {
            var critMod = 1f;
            if (Random.value <= MoveCritical.CritChance(moveBuilder.critStage, source, target))
            {
                critMod = CritMod;
            }

            float randMod = Random.Range(0.85f, 1f);
            float typeMod = TypeChart.GetEffectiveness(moveBuilder.type, target.Types);
            float stab = source.Types.Contains(moveBuilder.type) ? 1.5f : 1f;
            
            int attack = moveBuilder.category == MoveCategory.Physical ? source.Attack : source.SpAttack;
            int defense = moveBuilder.category == MoveCategory.Special ? target.SpDefense : target.Defense;

            float a = (2 * source.Level + 10) / 250f;
            float d = a * moveBuilder.power * ((float) attack / defense) + 2;

            float attackMod = source.Modifier.AttackerMod(moveBuilder, target);
            float defenseMod = target.Modifier.DefenderMod(moveBuilder, source);

            int damage = Mathf.FloorToInt(d * stab * critMod * randMod * typeMod * attackMod * defenseMod);

            var detail = new DamageDetail(damage);

            if (critMod > 1f)
                detail.Messages.Add(CritMessage);
            switch (typeMod)
            {
                case > 1f:
                    detail.Messages.Add(NotEffectiveMessage);
                    break;
                case < 1f:
                    detail.Messages.Add(SuperEffectiveMessage);
                    break;
            }

            return detail;
        }
    }
}