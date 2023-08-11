using Game.Pokemons;

namespace Game.Dexs
{
    public class ModdedDex
    {
        public DexTableData Data { get; }
        public DexConditions Conditions { get; set; }
        public DexMoves Moves { get; set; }
    }

    public class DexMoves
    {
        public Move Get(Move name)
        {
            return name ?? Get("");
        }

        public Move Get(string name)
        {
            name = (name ?? "").Trim();
            var id = Utils.Utils.ToID(name);
            return this.GetById(id);
        }

        public Move GetById(string id)
        {
            return null;
        }
    }

    public class Move : MoveData
    {
    }

    public class MoveData
    {
        public string Name { get; set; }
        public MoveTarget Target { get; set; }
    }
}