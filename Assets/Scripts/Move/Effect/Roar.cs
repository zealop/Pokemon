using System.Collections.Generic;
using System.Linq;
using Battle;
using Pokemons;
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

        public override void Apply(Unit source, Unit target)
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

        private void ApplyTrainerBattle(Unit target)
        {
            var party = target.party;
            var candidate = GetForceSwitchTarget(party.Pokemons, target.Pokemon);
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

        private void ApplyWildBattle(Unit source, Unit target)
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
            return party.FirstOrDefault(pokemon => pokemon.Hp > 0 && pokemon != target);
        }
    }
}