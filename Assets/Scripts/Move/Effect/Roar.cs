using System.Collections.Generic;
using System.Linq;
using Battle;
using UnityEngine;

namespace Move.Effect
{
    public class Roar : MoveEffect
    {
        [SerializeField] private readonly string message;

        public Roar(string message)
        {
            this.message = message;
        }

        public override void Apply(BattleUnit source, BattleUnit target)
        {
            if (BattleManager.IsTrainerBattle)
            {
                ApplyTrainerBattle(target);
            }
            else
            {
                ApplyWildBattle(source, target);
            }
        }

        private void ApplyTrainerBattle(BattleUnit target)
        {
            var party = target.Party;
            var candidate = GetForceSwitchTarget(party.Party, target.Pokemon);
            if (candidate is null)
            {
                OnFail();
            }
            else
            {
                Log(message, null, target);
                BattleManager.SwitchTrainerPokemon(candidate, target);
                Log($"{target.Name} is forced out!");
            }
        }

        private void ApplyWildBattle(BattleUnit source, BattleUnit target)
        {
            if (source.Level < target.Level)
            {
                OnFail();
            }
            else
            {
                Log(message, null, target);
                BattleManager.ForcedEndBattle();
            }
        }

        private static Pokemon GetForceSwitchTarget(IEnumerable<Pokemon> party, Pokemon target)
        {
            return party.FirstOrDefault(pokemon => pokemon.HP > 0 && pokemon != target);
        }
    }
}