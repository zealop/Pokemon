using System;
using Game.Battles;
using Game.Constants;
using Random = UnityEngine.Random;

namespace Game.Moves.Damage
{
    public class Default : IMoveDamage
    {
        private const string CritMessage = "A critical hit!";
        private const string NotEffectiveMessage = "It's super effective!";
        private const string SuperEffectiveMessage = "It's not very effective!";
        private const float CritMod = 1.5f;
        
        private static readonly Func<Unit, BattleContext> Context = u => u.Battle.Context;
        private static readonly Func<Unit, StageModifier> StageModifier = u => Context(u).StageModifier;
        private static readonly Func<Unit, TypeChart> TypeChart = u => Context(u).TypeChart;
        
        public DamageDetail Apply(MoveBuilder move, Unit source, Unit target)
        {
            var isCrit = StageModifier(source).IsCrit(move.CritStage + source.CritStage);
            var critMod = isCrit ? StageModifier(source).CritBonus : 1;
            
            var randMod = Random.Range(0.85f, 1f);
            var typeMod = TypeChart(source).GetEffectiveness(move.Type, target.Types);

            var isStab = source.Types.Contains(move.Type);
            var stabMod = isStab ? TypeChart(source).StabBonus : 1;

            var attack = move.Category == MoveCategory.Physical ? source.TotalAttack : source.TotalSpAttack;
            var defense = move.Category == MoveCategory.Special ? target.TotalSpDefense : target.TotalDefense;

            var a = (2 * source.Level + 10) / 250f;
            var d = a * move.Power * ((float)attack / defense) + 2;

            var attackMod = source.Modifier.AttackerMod(move, target);
            var defenseMod = target.Modifier.DefenderMod(move, source);

            var value = d * randMod * attackMod * defenseMod;
            
            return new DamageDetail(value, critMod, stabMod, typeMod);
        }
    }
}