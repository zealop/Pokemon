using System;
using System.Collections.Generic;
using Game.Conditions;
using Game.Dexs;
using Game.Effects;
using Game.Pokemons;
using Game.Sides;
using Game.Utils;
using OneOf;
using UnityEngine;
using Effect = OneOf.OneOf<Game.Conditions.Condition>;

namespace Game.Battles
{
    public class Battle
    {
        private PRNG prng;
        public ModdedDex Dex { get; set; }
        public bool ended;
        public GameType GameType { get; set; }
        public RuleTable RuleTable { get; set; }
        public RequestState RequestState { get; set; }
        public int Gen { get; set; }
        public int Turn { get; set; }
        public Side[] Sides { get; set; }
        public Dictionary<string, object> Event { get; set; }
        public BattleActions Actions { get; set; }

        public Action<string, string[]> send;
        public bool strictChoices;
        
        public Battle(object options)
        {
        }

        public bool Choose(SideID sideId, string input)
        {
            var side = getSide(sideId);

            if (!side.Choose(input)) return false;

            if (!side.IsChoiceDone())
            {
                side.EmitChoiceError($"Incomplete choice: {input} - missing other pokemon");
                return false;
            }

            if (AllChoicesDone()) CommitDecisions();
            return true;
        }

        private void CommitDecisions()
        {
            throw new System.NotImplementedException();
        }

        private bool AllChoicesDone()
        {
            throw new System.NotImplementedException();
        }

        private Side getSide(SideID sideId)
        {
            throw new System.NotImplementedException();
        }

        public T Sample<T>(List<T> items)
        {
            return this.prng.Sample(items.ToArray());
        }

        public bool SingleEvent(
            string eventId, 
            Effect effect, 
            AnyObject state,
            OneOf<string, Pokemon, Side, Battle> target, 
            OneOf<string, Pokemon, Effect, bool>? source = null,
            OneOf<Effect, string>? sourceEffect = null, 
            object relayVar = null, 
            dynamic customCallback = null
        )
        {
            throw new NotImplementedException();
        }

        public void RunEvent(
            string eventId,
            OneOf<Pokemon, Pokemon[], Side, Battle>? target,
            OneOf<string, Pokemon, bool>? source,
            Effect? sourceEffect,
            object relayVar = null,
            bool? onEffect = null,
            bool? fastExi = null
        )
        {
            throw new NotImplementedException();
        }

        public bool ValidTargetLoc(int targetLoc, Pokemon pokemon, string targetType)
        {
            throw new NotImplementedException();
        }
    }

    public class BattleActions
    {
        public string GetZMove(Move move, Pokemon pokemon, bool skipCheck = false)
        {
            throw new NotImplementedException();
        }

        public Move GetMaxMove(Move move, Pokemon pokemon)
        {
            throw new NotImplementedException();
        }

        public bool TargetTypeChoices(string targetType)
        {
            throw new NotImplementedException();
        }
    }
}