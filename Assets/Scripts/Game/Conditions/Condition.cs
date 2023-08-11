using System;
using Game.Battles;
using Game.Pokemons;
using OneOf;

namespace Game.Conditions
{
    public class Condition : BasicEffect
    {
        public static readonly Condition EmptyCondition = new();
        public Func<Battle, Pokemon, Pokemon, OneOf<Condition>?, int> durationCallback;
        public Func<Battle, Pokemon, Pokemon, OneOf<Condition>, bool> onRestart;

    }

    public class BasicEffect
    {
        public string Id { get; set; }
        public int? Duration { get; set; }
    }
}