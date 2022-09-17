using System;
using System.Collections.Generic;
using System.Linq;
using Data.Condition;
using Pokemons;

namespace Battle
{
    public class Side
    {
        private readonly Dictionary<SideConditionID, SideCondition> conditions = new();

        public readonly List<Action> onTurnEndList = new();
        
        public IBattle Battle { get; }
        public PokemonParty Party { get; }
        public Unit[] Units { get; }
        
        public Side(IBattle battle, PokemonParty party, int size)
        {
            Battle = battle;
            Party = party;
            Units = new Unit[size];
        }

        public void AddCondition(SideCondition condition)
        {
            if (conditions.ContainsKey(condition.id)) return;

            condition.Bind(this);
            condition.Start();
        }

        public void RemoveSideCondition(SideConditionID id)
        {
            conditions[id].End();
            conditions.Remove(id);
        }

        public void OnTurnEnd()
        {
            onTurnEndList.ForEach(a => a());
        }
    }
}