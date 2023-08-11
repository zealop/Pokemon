using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Game.Battles;
using Game.Conditions;
using Game.Effects;
using Game.Moves;
using Game.Pokemons;
using Game.Utils;
using OneOf;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.Sides
{
    public class Side
    {
        private Battle battle;
        public SideID ID { get; }

        /// <summary>
        /// Index in `battle.sides`: `battle.sides[side.n] === side`
        /// </summary>
        public int N { get; }

        private string name;
        private string avatar;
        private Side foe;
        private Side allySide;
        private List<PokemonSet> team;
        private List<Pokemon> pokemon;
        public Pokemon[] Active { get; }
        private int pokemonLeft;
        private bool zMoveUsed;

        /*
         * This will be true in any gen before 8 or if the player (or their battle partner) has dynamaxed once already
         * Use Side.canDynamaxNow() to check if a side can dynamax instead of this property because only one
         * player per team can dynamax on any given turn of a gen 8 Multi Battle.
         */
        private bool dynamaxUsed;

        private Pokemon faintedLastTurn;
        private Pokemon faintedThisTurn;
        private int totalFainted;

        /** only used by Gen 1 Counter */
        private string lastSelectedMove;

        /** these point to the same object as the ally's, in multi battles */
        private Dictionary<string, EffectState> sideConditions;

        private Dictionary<string, EffectState>[] slotConditions;
        private AnyObject activeRequest;
        private Choice choice;

        /**
         * In gen 1, all lastMove stuff is tracked on Side rather than Pokemon
         * (this is for Counter and Mirror Move)
         * This is also used for checking Self-KO clause in Pokemon Stadium 2.
        */
        private Move lastMove;

        public Side(string name, Battle battle, int sideNum, List<PokemonSet> team)
        {
            battle.Dex.Data.Scripts.TryGetValue("side", out var sideScripts);
            if (sideScripts is not null)
            {
                //copy props
                // Object.assign(this, sideScripts);
            }

            this.battle = battle;
            this.ID = new[] { SideID.P1, SideID.P2, SideID.P3, SideID.P4 }[sideNum];
            this.N = sideNum;
            this.name = name;
            this.avatar = "";
            this.team = team;
            this.pokemon = new List<Pokemon>();

            for (var i = 0; i < this.team.Count && i < 24; i++)
            {
                // console.log("NEW POKEMON: " + (this.team[i] ? this.team[i].name : '[unidentified]'));
                this.pokemon.Add(new Pokemon(this.team[i], this));
                this.pokemon[i].Position = i;
            }

            var size = this.battle.GameType switch
            {
                GameType.Doubles => 2,
                GameType.Triples or GameType.Rotation => 3,
                _ => 1
            };
            this.Active = new Pokemon[size];
            this.pokemonLeft = this.pokemon.Count;
            this.faintedLastTurn = null;
            this.faintedThisTurn = null;
            this.totalFainted = 0;
            this.zMoveUsed = false;
            this.dynamaxUsed = this.battle.Gen != 8;

            this.sideConditions = new Dictionary<string, EffectState>();
            this.slotConditions = new Dictionary<string, EffectState>[size];
            for (var i = 0; i < this.Active.Length; i++)
            {
                this.slotConditions[i] = new Dictionary<string, EffectState>();
            }

            this.activeRequest = null;
            this.choice = new Choice()
            {
                CantUndo = false,
                Error = "",
                Actions = new List<ChosenAction>(),
                ForcedSwitchesLeft = 0,
                ForcedPassesLeft = 0,
                SwitchIns = new HashSet<int>(),
                ZMove = false,
                Mega = false,
                Ultra = false,
                Dynamax = false,
                Terastallize = false,
            };

            // old-gens
            this.lastMove = null;
        }

        private string ToJson()
        {
            return null;
        }

        private RequestState RequestState
        {
            get
            {
                if (activeRequest == null || activeRequest.ContainsKey("wait"))
                {
                    return RequestState.Blank;
                }

                if (activeRequest.ContainsKey("teamPreview"))
                {
                    return RequestState.TeamPreview;
                }

                if (activeRequest.ContainsKey("forceSwitch"))
                {
                    return RequestState.Switch;
                }

                return RequestState.Move;
            }
        }

        private bool CanDynamaxNow
        {
            get
            {
                if (this.battle.Gen != 8)
                {
                    return false;
                }

                // In multi battles, players on a team are alternatingly given the option to dynamax each turn
                // On turn 1, the players on their team's respective left have the first chance (p1 and p2)
                if (this.battle.GameType == GameType.Multi && this.battle.Turn % 2 != new int[] { 1, 1, 0, 0 }[this.N])
                {
                    return false;
                }

                return !this.dynamaxUsed;
            }
        }

        private string GetChoice()
        {
            if (this.choice.Actions.Count > 1 && this.choice.Actions.TrueForAll(action => action.Choice == ActionType.Team))
            {
                return "team " + string.Join(", ", this.choice.Actions.Select(action => action.Pokemon!.Position + 1));
            }

            return string.Join(", ", this.choice.Actions.Select(action =>
            {
                switch (action.Choice)
                {
                    case ActionType.Move:
                        var details = "";
                        if (action.TargetLoc is not null && this.Active.Length > 1)
                        {
                            details += $" {(action.TargetLoc > 0 ? "+" : "")}{action.TargetLoc}";
                        }

                        if (action.Mega)
                        {
                            details += (action.Pokemon!.Item == "ultranecroziumz" ? " ultra" : " mega");
                        }

                        if (action.Zmove is not null)
                        {
                            details += " zmove";
                        }

                        if (action.MaxMove is not null)
                        {
                            details += " dynamax";
                        }

                        if (action.Terastallize is not null)
                        {
                            details += " terastallize";
                        }

                        return $"move {action.Moveid}${details}";
                    case ActionType.Switch:
                    case ActionType.InstaSwitch:
                    case ActionType.RevivalBlessing:
                        return $"switch {action.Target!.Position + 1}";
                    case ActionType.Team:
                        return $"team ${action.Pokemon!.Position + 1}";
                    case ActionType.Shift:
                    case ActionType.Pass:
                    default:
                        return action.Choice.ToString();
                }
            }));
        }

        public override string ToString()
        {
            return $"{this.ID}: {this.name}";
        }

        private object GetRequestData(bool forAlly = false)
        {
            var data = new
            {
                Name = this.name,
                Id = this.ID,
                Pokemon = new List<object>(),
            };
            foreach (var curPokemon in this.pokemon)
            {
                data.Pokemon.Add(curPokemon.GetSwitchRequestData(forAlly));
            }

            return data;
        }

        private Pokemon RandomFoe()
        {
            var actives = this.Foes();
            return actives.IsNullOrEmpty() ? null : this.battle.Sample(actives);
        }

        /** Intended as a way to iterate through all foe side conditions - do not use for anything else. */
        private List<Side> FoeSidesWithConditions()
        {
            return this.battle.GameType == GameType.FreeForAll
                ? this.battle.Sides.Where(side => side != this).ToList()
                : new List<Side> { this.foe };
        }

        private int FoePokemonLeft()
        {
            if (this.battle.GameType == GameType.FreeForAll)
            {
                return this.battle.Sides.Where(side => side != this).Select(side => side.pokemonLeft).Aggregate((a, b) => a + b);
            }

            if (this.foe.allySide is not null)
            {
                return this.foe.pokemonLeft + this.foe.allySide.pokemonLeft;
            }

            return this.foe.pokemonLeft;
        }

        private List<Pokemon> Allies(bool all = false)
        {
            // called during the first switch-in, so `active` can still contain nulls at this point
            var allies = this.ActiveTeam().Where(ally => ally is not null);
            if (!all)
            {
                allies = allies.Where(ally => ally.Hp > 0);
            }

            return allies.ToList();
        }

        private List<Pokemon> Foes(bool all = false)
        {
            if (this.battle.GameType == GameType.FreeForAll)
            {
                return this.battle.Sides.Select(side => side.Active[0])
                    .Where(p => p is not null && p.Side != this && (all || p.Hp > 0))
                    .ToList();
            }

            return this.foe.Allies(all);
        }

        private List<Pokemon> ActiveTeam()
        {
            return this.battle.GameType != GameType.Multi
                ? this.Active.ToList()
                : this.battle.Sides[this.N % 2].Active.Concat(this.battle.Sides[this.N % 2 + 2].Active).ToList();
        }

        private bool HasAlly(Pokemon curPokemon)
        {
            return curPokemon.Side == this || curPokemon.Side == this.allySide;
        }

        private bool AddSideCondition(OneOf<string, Condition> status, OneOf<Pokemon, string>? source = null, OneOf<Condition>? sourceEffect = null)
        {
            this.battle.Event.TryGetValueAs("target", out Pokemon eventTarget);
            if (source is null && eventTarget is not null)
            {
                source = eventTarget;
            }

            if ("debug".Equals(source?.AsT1))
            {
                source = this.Active[0];
            }

            if (source is null)
            {
                throw new SystemException("Setting sidecond without a source");
            }

            if (!source.HasMethod("GetSlot"))
            {
                // source = (source as Side)!.Active[0];
            }

            var statusCondition = this.battle.Dex.Conditions.Get(status);
            var sourcePokemon = source.Value.AsT0;

            if (this.sideConditions.ContainsKey(statusCondition.Id))
            {
                if (!statusCondition.HasMethod("OnSideRestart"))
                {
                    return false;
                }

                return this.battle.SingleEvent(
                    "SideRestart", statusCondition,
                    this.sideConditions[statusCondition.Id].ToAnyObject(), this,
                    sourcePokemon, sourceEffect
                );
            }

            var effectState = new EffectState()
            {
                { "id", statusCondition.Id },
                { "target", this },
                { "source", sourcePokemon },
                { "sourceSlot", sourcePokemon.GetSlot() },
            };
            effectState.Duration = statusCondition.Duration;
            this.sideConditions[statusCondition.Id] = effectState;


            if (statusCondition.durationCallback is not null)
            {
                effectState.Duration = statusCondition.durationCallback.Invoke(this.battle, this.Active[0], sourcePokemon, sourceEffect);
            }

            if (!this.battle.SingleEvent(
                    "SideStart",
                    statusCondition,
                    effectState,
                    this,
                    sourcePokemon,
                    sourceEffect)
               )
            {
                this.sideConditions.Remove(statusCondition.Id);
                return false;
            }

            this.battle.RunEvent(
                "SideConditionStart",
                sourcePokemon,
                sourcePokemon,
                statusCondition
            );

            return true;
        }

        public OneOf<Condition>? GetSideCondition(OneOf<string, Condition> status)
        {
            var ret = this.battle.Dex.Conditions.Get(status);

            return this.sideConditions.ContainsKey(ret.Id) ? ret : null;
        }

        public AnyObject GetSideConditionData(OneOf<string, Condition> status)
        {
            var ret = this.battle.Dex.Conditions.Get(status);

            return this.sideConditions.TryGetValue(ret.Id, out var value) ? value : null;
        }

        public bool RemoveSideCondition(OneOf<string, Condition> status)
        {
            var ret = this.battle.Dex.Conditions.Get(status);
            if (!this.sideConditions.ContainsKey(ret.Id))
            {
                return false;
            }

            this.battle.SingleEvent("SideEnd", ret, this.sideConditions[ret.Id], this);
            this.sideConditions.Remove(ret.Id);

            return true;
        }

        public bool AddSlotCondition(
            OneOf<Pokemon, int> target,
            OneOf<string, Condition> status,
            OneOf<Pokemon, string>? source = null,
            OneOf<Condition>? sourceEffect = null
        )
        {
            if (source is null && this.battle.Event?["target"] is not null)
            {
                source = this.battle.Event["target"] as Pokemon;
            }

            if ("debug".Equals(source?.Value))
            {
                source = this.Active[0];
            }


            if (target.IsT0)
            {
                target = target.AsT0.Position;
            }

            if (source is null)
            {
                throw new SystemException("setting side condition without a source");
            }

            status = this.battle.Dex.Conditions.Get(status);

            if (this.slotConditions[target.AsT1].ContainsKey(status.AsT1.Id))
            {
                if (status.AsT1.onRestart is null)
                {
                    return false;
                }

                return this.battle.SingleEvent(
                    "Restart",
                    status.AsT1,
                    this.slotConditions[target.AsT1][status.AsT1.Id],
                    this,
                    source.Value.AsT0,
                    sourceEffect
                );
            }

            var conditionState = new EffectState()
            {
                { "id", status.AsT1.Id },
                { "target", this },
                { "source", source.Value.AsT0 },
                { "sourceSlot", source.Value.AsT0.GetSlot() },
            };
            conditionState.Duration = status.AsT1.Duration;
            this.sideConditions[status.AsT1.Id] = conditionState;

            if (status.AsT1.durationCallback is not null)
            {
                conditionState.Duration =
                    status.AsT1.durationCallback.Invoke(this.battle, this.Active[0], source.Value.AsT0, sourceEffect);
            }

            if (this.battle.SingleEvent(
                    "Start",
                    status.AsT1,
                    conditionState, this.Active[target.AsT1],
                    source.Value.AsT1,
                    sourceEffect)
               )
            {
                return true;
            }

            this.slotConditions[target.AsT1].Remove(status.AsT1.Id);
            return false;
        }

        public OneOf<Condition> GetSlotCondition(OneOf<Pokemon, int> target, OneOf<string, Condition> status)
        {
            if (target.IsT0)
            {
                target = target.AsT0.Position;
            }

            status = this.battle.Dex.Conditions.Get(status);

            return this.slotConditions[target.AsT1].ContainsKey(status.AsT1.Id) ? status.AsT1 : null;
        }

        public bool RemoveSlotCondition(OneOf<Pokemon, int> target, OneOf<string, Condition> status)
        {
            if (target.IsT0)
            {
                target = target.AsT0.Position;
            }

            status = this.battle.Dex.Conditions.Get(status);


            if (!this.slotConditions[target.AsT1].ContainsKey(status.AsT1.Id))
            {
                return false;
            }

            this.battle.SingleEvent("End", status.AsT1, this.slotConditions[target.AsT1][status.AsT1.Id], this.Active[target.AsT1]);
            this.slotConditions[target.AsT1].Remove(status.AsT1.Id);

            return true;
        }

        public void Send(params OneOf<string, int, Func<Side, string>, AnyObject>[] parts)
        {
            var sideUpdate = '|' + string.Join('|', parts.Select(
                part => part.Match(
                    str => str,
                    num => num.ToString(),
                    fun => fun.Invoke(this),
                    ao => ao.ToString() 
                )));

            this.battle.send("sideupdate", new[] { $"{this.ID}\n{sideUpdate}" });
        }

        public void EmitRequest(AnyObject update)
        {
            this.battle.send("sideupdate", new[] { $"{this.ID}\n|request|{update}" });
            this.activeRequest = update;
        }

        public bool EmitChoiceError(string message, bool unavailable = false)
        {
            choice.Error = message;
            var type = $"[{(unavailable ? "Unavailable" : "Invalid")} choice]";

            battle.send("sideupdate", new[] { $"{ID}\n|error|{type} {message}" });
            if (battle.strictChoices)
            {
                throw new SystemException($"{type} {message}");
            }

            return false;
        }

        private bool ChooseMove(OneOf<string, int>? moveText, int targetLoc = 0, ChoiceEvent eventType = ChoiceEvent.Blank)
        {
            if (this.RequestState != RequestState.Move)
            {
                return this.EmitChoiceError($"Can't move: You need a {this.RequestState} response");
            }

            var index = this.GetChoiceIndex();
            if (index >= this.Active.Length)
            {
                return this.EmitChoiceError("Can't move: You sent more choices than unfainted Pokémon.");
            }

            var autoChoose = moveText is null;
            var pokemon = this.Active[index];

            // Parse moveText (name or index)
            // If the move is not found, the action is invalid without requiring further inspection.

            var request = pokemon.GetMoveRequestData();
            var moveId = "";
            var targetType = "";
            if (autoChoose)
            {
                moveText = 1;
            }

            if (moveText.Value.IsT0 && int.TryParse(moveText.Value.AsT0, out var num))
            {
                moveText = num;
            }

            if (moveText.Value.IsT1)
            {
                // Parse a one-based move index.
                var moveIndex = moveText.Value.AsT1 - 1;
                if (moveIndex < 0 || moveIndex >= request.Moves.Length || request.Moves[moveIndex] is null)
                {
                    return this.EmitChoiceError($"Can't move: Your {pokemon.Name} doesn't have a move {moveIndex + 1}");
                }

                moveId = request.Moves[moveIndex].Id;
                targetType = request.Moves[moveIndex].Target!;
            }
            else
            {
                // Parse a move ID.
                // Move names are also allowed, but may cause ambiguity (see client issue #167).
                moveId = Utils.Utils.ToID(moveText);
                if (moveId.StartsWith("hiddenpower"))
                {
                    moveId = "hiddenpower";
                }

                foreach (var move_1 in request.Moves)
                {
                    if (move_1.Id != moveId) continue;
                    targetType = move_1.Target ?? "normal";
                    break;
                }

                if (string.IsNullOrEmpty(targetType) && new[] { ChoiceEvent.Blank, ChoiceEvent.Dynamax }.Contains(eventType) && request.MaxMoves is not null)
                {
                    for (var i = 0; i < request.MaxMoves.MaxMoves.Length; i++)
                    {
                        var moveRequest = request.MaxMoves.MaxMoves[i];
                        if (!moveId.Equals(moveRequest.Move)) continue;
                        moveId = request.Moves[i].Id;
                        targetType = moveRequest.Target.ToString().ToLower();
                        eventType = ChoiceEvent.Dynamax;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(targetType) && new[] { ChoiceEvent.Blank, ChoiceEvent.ZMove }.Contains(eventType) && request.CanZMove is not null)
                {
                    foreach (var (i, moveRequest) in request.CanZMove)
                    {
                        if (moveRequest is null) continue;

                        if (!moveId.Equals(Utils.Utils.ToID(((AnyObject)moveRequest)["move"]))) continue;
                        moveId = request.Moves[int.Parse(i)].Id;
                        targetType = (string)((AnyObject)moveRequest)["target"];
                        eventType = ChoiceEvent.ZMove;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(targetType))
                {
                    return this.EmitChoiceError($"Can't move: Your {pokemon.Name} doesn't have a move matching {moveid}");
                }
            }

            var moves = pokemon.GetMoves();
            if (autoChoose)
            {
                for (var i = 0; i < request.Moves.Length; i++)
                {
                    var move_2 = request.Moves[i];
                    if (move_2.Disabled is { IsT1: true, AsT1: true }) continue;

                    if (i < moves.Length && move_2.Id == moves[i].Id && moves[i].Disabled is { IsT1: true, AsT1: true }) continue;
                    moveId = move_2.Id;
                    targetType = move_2.Target;
                    break;
                }
            }

            var move = this.battle.Dex.Moves.Get(moveId);

            // Z-move

            var zMove = eventType == ChoiceEvent.ZMove ? this.battle.Actions.GetZMove(move, pokemon) : null;
            if (eventType == ChoiceEvent.ZMove && zMove is null)
            {
                return this.EmitChoiceError($"Can't move: ${pokemon.Name} can't use ${move.Name} as a Z-move");
            }

            if (zMove is not null && this.choice.ZMove)
            {
                return this.EmitChoiceError("Can't move: You can't Z-move more than once per battle");
            }

            if (zMove is not null)
            {
                targetType = this.battle.Dex.Moves.Get(zMove).Target.ToString().ToLower();
            }

            // Dynamax
            // Is dynamaxed or will dynamax this turn.
            var maxMove = (eventType == ChoiceEvent.Dynamax || pokemon.Volatiles.ContainsKey("dynamax"))
                ? this.battle.Actions.GetMaxMove(move, pokemon)
                : null;

            if (eventType == ChoiceEvent.Dynamax && maxMove is null)
            {
                return this.EmitChoiceError($"Can't move: {pokemon.Name} can't use {move.Name} as a Max Move");
            }

            if (maxMove != null) targetType = this.battle.Dex.Moves.Get(maxMove).Target.ToString().ToLower();

            // Validate targetting

            if (autoChoose)
            {
                targetLoc = 0;
            }
            else if (this.battle.Actions.TargetTypeChoices(targetType))
            {
                if (targetLoc == 0 && this.Active.Length >= 2)
                {
                    return this.EmitChoiceError($"Can't move: {move.Name} needs a target");
                }

                if (!this.battle.ValidTargetLoc(targetLoc, pokemon, targetType))
                {
                    return this.EmitChoiceError($"Can't move: Invalid target for ${move.Name}");
                }
            }
            else if (targetLoc == 0)
            {
                return this.EmitChoiceError($"Can't move: You can't choose a target for {move.Name}");
            }

            var lockedMove = pokemon.GetLockedMove();

            if (!string.IsNullOrEmpty(lockedMove))
            {
                var lockedMoveTargetLoc = pokemon.LastMoveTargetLoc ?? 0;
                var lockedMoveID = Utils.Utils.ToID(lockedMove);
                if (pokemon.Volatiles.ContainsKey(lockedMoveID) && pokemon.Volatiles[lockedMoveID].ContainsKey("targetLoc")) {
                    lockedMoveTargetLoc = (int) pokemon.Volatiles[lockedMoveID]["targetLoc"];
                }
                this.choice.Actions.Add(new ChosenAction() {
                    Choice = ActionType.Move,
                    Pokemon = pokemon,
                    TargetLoc = lockedMoveTargetLoc,
                    Moveid = lockedMoveID,
                });
                return true;
            } else if (moves.Length == 0 && string.IsNullOrEmpty(zMove)) {
                // Override action and use Struggle if there are no enabled moves with PP
                // Gen 4 and earlier announce a Pokemon has no moves left before the turn begins, and only to that player's side.
                if (this.battle.Gen <= 4) this.Send("-activate", pokemon, "move: Struggle");
                moveid = 'struggle';
            }
        }

        public bool IsChoiceDone()
        {
            if (RequestState == RequestState.Blank) return true;
            if (choice.ForcedSwitchesLeft > 0) return false;

            if (RequestState == RequestState.TeamPreview)
            {
                return choice.Actions.Count >= PickedTeamSize;
            }

            // current request is move/switch
            GetChoiceIndex(); // auto-pass
            return choice.Actions.Count >= Active.Length;
        }

        /// <summary>
        /// The number of pokemon you must choose in Team Preview.
        /// Note that PS doesn't support choosing fewer than this number of pokemon.
        /// In the games, it is sometimes possible to bring fewer than this, but
        /// since that's nearly always a mistake, we haven't gotten around to
        /// supporting it.
        /// </summary>
        private int PickedTeamSize => Math.Min(pokemon.Count, battle.RuleTable.PickedTeamSize ?? int.MaxValue);

        public bool Choose(string input)
        {
            if (RequestState == RequestState.Blank)
            {
                return EmitChoiceError(battle.ended ? "Can't do anything: The game is over" : "Can't do anything: It's not your turn");
            }

            if (choice.CantUndo)
            {
                return EmitChoiceError("Can't undo: A trapping / disabling effect would cause undo to leak information");
            }

            ClearChoice();

            var choiceStrings = input.StartsWith("team ")
                ? new[] { input }
                : input.Split(',');

            if (choiceStrings.Length > Active.Length)
            {
                return EmitChoiceError($"Can't make choices: You sent choices for $ {choiceStrings.Length} Pokémon, but this is a ${battle.GameType} game!");
            }

            foreach (var choiceString in choiceStrings)
            {
                var parts = choiceString.Trim().Split(' ');
                string choiceType = parts[0], data = parts[1].Trim();

                switch (choiceType)
                {
                    case "move":
                        var original = data;
                        bool Error() => EmitChoiceError($"Conflicting arguments for \"move\": ${original}");
                        int? targetLoc = null;
                        var eventType = ChoiceEvent.Blank;

                        while (true)
                        {
                            var pattern = new Regex(@"\s[-+]?[1-3]$");
                            // If data ends with a number, treat it as a target location.
                            // We need to special case 'Conversion 2' so it doesn't get
                            // confused with 'Conversion' erroneously sent with the target
                            // '2' (since Conversion targets 'self', targetLoc can't be 2).
                            if (pattern.IsMatch(data) && !string.Equals(data, "conversion2"))
                            {
                                if (targetLoc is not null)
                                {
                                    return Error();
                                }

                                targetLoc = int.Parse(data[Math.Max(0, data.Length - 2)..]);
                                data = data[..Math.Max(0, data.Length - 2)].Trim();
                            }
                            else if (data.EndsWith(" mega"))
                            {
                                if (eventType != ChoiceEvent.Blank)
                                {
                                    return Error();
                                }

                                eventType = ChoiceEvent.Mega;
                                data = data[..Math.Max(0, data.Length - 5)];
                            }
                            else if (data.EndsWith(" zmove"))
                            {
                                if (eventType != ChoiceEvent.Blank)
                                {
                                    return Error();
                                }

                                eventType = ChoiceEvent.ZMove;
                                data = data[..Math.Max(0, data.Length - 6)];
                            }
                            else if (data.EndsWith(" ultra"))
                            {
                                if (eventType != ChoiceEvent.Blank)
                                {
                                    return Error();
                                }

                                eventType = ChoiceEvent.Ultra;
                                data = data[..Math.Max(0, data.Length - 6)];
                            }
                            else if (data.EndsWith(" dynamax"))
                            {
                                if (eventType != ChoiceEvent.Blank)
                                {
                                    return Error();
                                }

                                eventType = ChoiceEvent.Dynamax;
                                data = data[..Math.Max(0, data.Length - 8)];
                            }
                            else if (data.EndsWith(" gigantamax"))
                            {
                                if (eventType != ChoiceEvent.Blank)
                                {
                                    return Error();
                                }

                                eventType = ChoiceEvent.Dynamax;
                                data = data[..Math.Max(0, data.Length - 11)];
                            }
                            else if (data.EndsWith(" max"))
                            {
                                if (eventType != ChoiceEvent.Blank)
                                {
                                    return Error();
                                }

                                eventType = ChoiceEvent.Dynamax;
                                data = data[..Math.Max(0, data.Length - 4)];
                            }
                            else if (data.EndsWith(" terastal"))
                            {
                                if (eventType != ChoiceEvent.Blank)
                                {
                                    return Error();
                                }

                                eventType = ChoiceEvent.Terastallize;
                                data = data[..Math.Max(0, data.Length - 9)];
                            }
                            else if (data.EndsWith(" terastallize"))
                            {
                                if (eventType != ChoiceEvent.Blank)
                                {
                                    return Error();
                                }

                                eventType = ChoiceEvent.Terastallize;
                                data = data[..Math.Max(0, data.Length - 13)];
                            }
                            else
                            {
                                break;
                            }
                        }


                        if (!ChooseMove(data, targetLoc, eventType))
                        {
                            return false;
                        }

                        break;
                    case "switch":
                        this.ChooseSwitch(data);
                        break;
                        //         case 'shift':
                        //             if (data) return this.emitChoiceError(`
                        //             Unrecognized data after "shift": ${
                        //             data
                        //         }`);
                        //             if (!this.chooseShift()) return false;
                        //             break;
                        //         case 'team':
                        //             if (!this.chooseTeam(data)) return false;
                        //             break;
                        //         case 'pass':
                        //         case 'skip':
                        //             if (data) return this.emitChoiceError(`
                        //             Unrecognized data after "pass": ${
                        //             data
                        //         }`);
                        //             if (!this.choosePass()) return false;
                        //             break;
                        //         case 'auto':
                        //         case 'default':
                        //             this.autoChoose();
                        //             break;
                        //         default:
                        //             this.emitChoiceError(`Unrecognized choice: ${
                        //             choiceString
                        //         }`);
                        break;
                }
            }

            return choice.Error is null;
        }

        private bool ChooseSwitch(string slotText = null)
        {
            if (RequestState != RequestState.Move && RequestState != RequestState.Switch)
            {
                return EmitChoiceError($"Can't switch: You need a ${RequestState} response");
            }

            var index = GetChoiceIndex();
            if (index >= Active.Length)
            {
                return EmitChoiceError(RequestState == RequestState.Switch
                    ? "Can't switch: You sent more switches than Pokémon that need to switch"
                    : "Can't switch: You sent more choices than unfainted Pokémon");
            }

            var curPokemon = Active[index];
            int slot;

            if (slotText is null)
            {
                if (RequestState != RequestState.Switch)
                {
                    return EmitChoiceError("Can't switch: You need to select a Pokémon to switch in");
                }

                if (slotConditions[curPokemon.Position].ContainsKey("revivalblessing"))
                {
                    slot = 0;
                    while (!pokemon[slot].Fainted)
                    {
                        slot++;
                    }
                }
                else
                {
                    if (choice.ForcedSwitchesLeft == 0)
                    {
                        return ChoosePass();
                    }

                    slot = Active.Length;
                    while (choice.SwitchIns.Contains(slot) || pokemon[slot].Fainted)
                    {
                        slot++;
                    }
                }
            }
            else
            {
                int.TryParse(slotText, out slot);
                slot--;
            }

            if (slot < 0)
            {
                // maybe it's a name/species id!
                slot = -1;
                for (var i = 0; i < pokemon.Count; i++)
                {
                    var mon = pokemon[i];
                    if (slotText!.ToLower().Equals(mon.Name.ToLower()) || slotText.Equals(mon.Specie.Id))
                    {
                        slot = i;
                        break;
                    }
                }

                if (slot < 0)
                {
                    return EmitChoiceError($"Can't switch: You do not have a Pokémon named \"${slotText}\" to switch to");
                }
            }

            if (slot >= pokemon.Count)
            {
                return EmitChoiceError($"Can't switch: You do not have a Pokémon in slot {slot + 1} to switch to");
            }

            if (slot < Active.Length && !slotConditions[curPokemon.Position].ContainsKey("revivalblessing"))
            {
                return EmitChoiceError("Can't switch: You can't switch to an active Pokémon");
            }

            if (choice.SwitchIns.Contains(slot))
            {
                return EmitChoiceError($"Can't switch: The Pokémon in slot {slot + 1} can only switch in once");
            }

            var targetPokemon = pokemon[slot];

            if (slotConditions[curPokemon.Position].ContainsKey("revivalblessing"))
            {
                if (!targetPokemon.Fainted)
                {
                    return EmitChoiceError("Can't switch: You have to pass to a fainted Pokémon");
                }

                // Should always subtract, but stop at 0 to prevent errors.
                choice.ForcedSwitchesLeft = Math.Max(choice.ForcedSwitchesLeft - 1, 0);
                curPokemon.SwitchFlag = false;
                choice.Actions.Add(new ChosenAction()
                {
                    Choice = ActionType.RevivalBlessing,
                    Pokemon = curPokemon,
                    Target = targetPokemon
                });
                return true;
            }

            if (targetPokemon.Fainted)
            {
                return EmitChoiceError("Can't switch: You can't switch to a fainted Pokémon");
            }

            if (RequestState == RequestState.Move)
            {
                if (curPokemon.Trapped)
                {
                    var includeRequest = UpdateRequestForPokemon(curPokemon, req =>
                    {
                        var updated = false;
                        // if (req.maybeTrapped)
                        // {
                        //     delete req.maybeTrapped;
                        //     updated = true;
                        // }
                        //
                        // if (!req.trapped)
                        // {
                        //     req.trapped = true;
                        //     updated = true;
                        // }

                        return updated;
                    });
                    var status = EmitChoiceError("Can't switch: The active Pokémon is trapped", includeRequest);
                    if (includeRequest)
                    {
                        EmitRequest(activeRequest!);
                    }

                    return status;
                }

                if (curPokemon.MaybeTrapped)
                {
                    choice.CantUndo = choice.CantUndo || curPokemon.IsLastActive();
                }
            }
            else if (RequestState == RequestState.Switch)
            {
                if (choice.ForcedSwitchesLeft == 0)
                {
                    throw new SystemException("Player somehow switched too many Pokemon");
                }

                choice.ForcedSwitchesLeft--;
            }

            choice.SwitchIns.Add(slot);

            choice.Actions.Add(new ChosenAction()
            {
                Choice = RequestState == RequestState.Switch ? ActionType.InstaSwitch : ActionType.Switch,
                Pokemon = curPokemon,
                Target = targetPokemon,
            });

            return true;
        }

        private bool UpdateRequestForPokemon(Pokemon curPokemon, Func<AnyObject, bool> update)
        {
            if (activeRequest?.ContainsKey("active") != true)
            {
                throw new SystemException("Can't update a request without active Pokemon");
            }

            var req = ((AnyObject[])activeRequest["active"])[curPokemon.Position];

            if (req is null)
            {
                throw new SystemException("Pokemon not found in request's active field");
            }

            return update(req);
        }

        private void ClearChoice()
        {
            var forcedSwitches = 0;
            var forcedPasses = 0;
            if (battle.RequestState == RequestState.Switch)
            {
                var canSwitchOut = Active.Count(p => p.SwitchFlag);
                var canSwitchIn = pokemon.Skip(Active.Length).Count(p => p is not null && !p.Fainted);

                forcedSwitches = Math.Min(canSwitchOut, canSwitchIn);
                forcedPasses = canSwitchOut - forcedSwitches;
            }

            choice = new Choice()
            {
                CantUndo = false,
                Error = "",
                Actions = new List<ChosenAction>(),
                ForcedSwitchesLeft = forcedSwitches,
                ForcedPassesLeft = forcedPasses,
                SwitchIns = new HashSet<int>()
            };
        }


        private int GetChoiceIndex(bool isPass = false)
        {
            var index = choice.Actions.Count;

            if (isPass)
            {
                return index;
            }

            switch (RequestState)
            {
                case RequestState.Move:
                    // auto-pass
                    while (
                        index < Active.Length &&
                        (Active[index].Fainted || Active[index].Volatiles.ContainsKey(EffectID.Commanding))
                    )
                    {
                        ChoosePass();
                        index++;
                    }

                    break;
                case RequestState.Switch:
                    while (index < Active.Length && !Active[index].SwitchFlag)
                    {
                        ChoosePass();
                        index++;
                    }

                    break;
                case RequestState.Blank:
                case RequestState.TeamPreview:
                default:
                    break;
            }

            return index;
        }

        private bool ChoosePass()
        {
            var index = GetChoiceIndex(true);
            if (index >= Active.Length)
            {
                return false;
            }

            var curPokemon = Active[index];

            switch (RequestState)
            {
                case RequestState.Switch:
                    if (curPokemon.SwitchFlag) // This condition will always happen if called by Battle#choose()
                    {
                        if (choice.ForcedPassesLeft == 0)
                        {
                            return EmitChoiceError($"Can't pass: You need to switch in a Pokémon to replace ${curPokemon.Name}");
                        }

                        choice.ForcedPassesLeft--;
                    }

                    break;
                case RequestState.Move:
                    if (!curPokemon.Fainted && !curPokemon.Volatiles.ContainsKey(EffectID.Commanding))
                    {
                        return
                            EmitChoiceError($"Can't pass: Your ${curPokemon.Name} must make a move(or switch)");
                    }

                    break;
                case RequestState.Blank:
                case RequestState.TeamPreview:
                default:
                    return EmitChoiceError("Can't pass: Not a move or switch request");
            }

            choice.Actions.Add(new ChosenAction()
            {
                Choice = ActionType.Pass
            });

            return true;
        }
    }
}