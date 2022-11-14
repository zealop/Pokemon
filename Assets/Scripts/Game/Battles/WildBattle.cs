using Game.Pokemons;

namespace Game.Battles
{
    public class WildBattle : Battle
    {
        public WildBattle(BattleContext context, PokemonParty party, Pokemon wildPokemon)
            : base(context, party, new PokemonParty(wildPokemon), 1)
        {
        }
    }
}