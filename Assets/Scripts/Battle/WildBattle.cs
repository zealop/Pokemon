using Pokemons;

namespace Battle
{
    public class WildBattle : IBattle
    {
        public Side PlayerSide { get; }
        public Side EnemySide { get; }
        private MoveQueue MoveQueue { get; }

        public WildBattle(PokemonParty party, Pokemon wildPokemon)
        {
            PlayerSide = new Side(this, party, 1);
            EnemySide = new Side(this, new PokemonParty(wildPokemon), 1);
            MoveQueue = new MoveQueue();
        }

        public void ChooseMove(int moveSlot)
        {
        }
    }
}