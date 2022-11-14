using System.Collections.Generic;
using System.Linq;
using Game.Condition;
using Game.Pokemons;

namespace Game.Battles
{
    public class Side
    {
        // private readonly Dictionary<SideConditionID, SideCondition> conditions = new();

        // public readonly List<Action> onTurnEndList = new();
        
        public Battle Battle { get; }
        public PokemonParty Party { get; }
        public Unit[] Units { get; }
        public Dictionary<SideConditionID, SideCondition> Conditions { get; }
        public Side(Battle battle, PokemonParty party, int size)
        {
            Battle = battle;
            Party = party;
            Units = party
                .GetHealthyPokemon(size)
                .Select(p => new Unit(this, p))
                .ToArray();
            Conditions = new Dictionary<SideConditionID, SideCondition>();
        }

        // public void AddCondition(SideCondition condition)
        // {
        //     if (conditions.ContainsKey(condition.id)) return;
        //
        //     condition.Bind(this);
        //     condition.Start();
        // }
        //
        // public void RemoveSideCondition(SideConditionID id)
        // {
        //     conditions[id].End();
        //     conditions.Remove(id);
        // }
        //
        // public void OnTurnEnd()
        // {
        //     onTurnEndList.ForEach(a => a());
        // }
    }
}