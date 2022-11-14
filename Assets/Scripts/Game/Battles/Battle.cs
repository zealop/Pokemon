using Game.Moves;
using Game.Pokemons;

namespace Game.Battles
{
    public abstract class Battle
    {
        public BattleContext Context { get; }
        public Side PlayerSide { get; }
        public Side EnemySide { get; }
        public MoveQueue MoveQueue { get; }
        // public bool isWild { get; }

        protected Battle(BattleContext context, PokemonParty player, PokemonParty enemy, int size)
        {
            Context = context;
            PlayerSide = new Side(this, player, size);
            EnemySide = new Side(this, enemy, size);
            MoveQueue = new MoveQueue();
        }

        public void Enqueue(MoveBuilder move, Unit source, Unit target)
        {
            MoveQueue.Enqueue(move, source, target);
        }

        public void RunTurn()
        {
            while (!MoveQueue.IsEmpty)
            {
                var node = MoveQueue.Dequeue();
                RunMove(node);
            }
        }
        
        private static void RunMove(MoveNode node)
        {
            var move = node.Move;
            var source = node.Source;
            var target = node.Target;

            if (!source.IsFainted)
            {
                source.UseMove(move, target);
            }
        }
    }
}