using System;
using System.Collections.Generic;
using Game.Effects;
using Game.Sides;
using Game.Utils;
using OneOf;

namespace Game.Pokemons
{
    public class Pokemon
    {
        private bool isActive;

        public Pokemon(PokemonSet pokemonSet, Side side1)
        {
            throw new System.NotImplementedException();
        }

        public bool MaybeTrapped { get; set; }
        public int Position { get; set; }
        public bool SwitchFlag { get; set; }
        public bool Fainted { get; set; }
        public Dictionary<string, EffectState> Volatiles { get; set; }
        public string Name { get; set; }
        public Specie Specie { get; set; }
        public bool Trapped { get; set; }
        public string Item { get; set; }
        public Side Side { get; set; }
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int? LastMoveTargetLoc { get; set; }

        public bool IsLastActive()
        {
            if (!isActive)
            {
                return false;
            }

            var allyActive = Side.Active;
            for (var i = Position + 1; i < allyActive.Length; i++)
            {
                if (allyActive[i] is not null && !allyActive[i].Fainted)
                {
                    return false;
                }
            }

            return true;
        }

        public object GetSwitchRequestData(bool forAlly = false)
        {
            throw new System.NotImplementedException();
        }

        public string GetSlot()
        {
            var positionOffset = this.Side.N / 2 * this.Side.Active.Length;
            var positionLetter = "abcdef"[this.Position + positionOffset];
            return this.Side.ID.ToString() + positionLetter;
        }

        public MoveRequestData GetMoveRequestData()
        {
            throw new NotImplementedException();
        }

        public MoveData[] GetMoves(string lockedMove = null, bool restrictData = false)
        {
            throw new NotImplementedException();
        }

        public class MoveData
        {
            public string Move { get; set; }
            public string Id { get; set; }
            public OneOf<string, bool>? Disabled { get; set; }
            public string DisabledSource { get; set; }
            public string Target { get; set; }
            public int? Pp { get; set; }
            public int? MaxPp { get; set; }
        }

        public string GetLockedMove()
        {
            throw new NotImplementedException();
        }
    }

    public class MoveRequestData
    {
        public MoveData[] Moves { get; set; }
        public bool? MaybeDisabled { get; set; }
        public bool? Trapped { get; set; }
        public bool? MaybeTrapped { get; set; }
        public bool? CanMegaEvo { get; set; }
        public bool? CanUltraBurst { get; set; }
        public AnyObject CanZMove { get; set; }
        public bool? CanDynamax { get; set; }
        public DynamaxOption MaxMoves { get; set; }
        public string CanTerastallize { get; set; }

        public class MoveData
        {
            public string Move { get; set; }
            public string Id { get; set; }
            public string Target { get; set; }
            public OneOf<string, bool>? Disabled { get; set; }
        }
    }

    public class DynamaxOption
    {
        public MoveData[] MaxMoves { get; set; }
        public string Gigantamax { get; set; }

        public class MoveData
        {
            public string Move { get; set; }
            public MoveTarget Target { get; set; }
            public bool? Disabled { get; set; }
        }
    }

    public enum MoveTarget
    {
    }
}