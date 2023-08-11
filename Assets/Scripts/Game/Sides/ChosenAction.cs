using Game.Moves;
using Game.Pokemons;
using Game.Sides;

namespace Game.Battles
{
    public class ChosenAction
    {
        /// <summary>
        /// Action type
        /// </summary>
        public ActionType Choice { get; set; }

        /// <summary>
        /// The pokemon doing the action
        /// </summary>
        public Pokemon Pokemon { get; set; }

        /// <summary>
        /// Relative location of the target to pokemon (move action only)
        /// </summary>
        public int? TargetLoc { get; set; }

        /// <summary>
        /// A move to use (move action only)
        /// </summary>
        public string Moveid { get; set; }

        /// <summary>
        /// The active move corresponding to moveId (move action only)
        /// </summary>
        public ActiveMove Move { get; }

        /// <summary>
        /// The target of the action
        /// </summary>
        public Pokemon Target { get; set; }

        /// <summary>
        /// The chosen index in Team Preview
        /// </summary>
        public int? Index { get; }

        /// <summary>
        /// The action's side
        /// </summary>
        public Side Side { get; }

        /// <summary>
        /// true if megaing or ultra bursting
        /// </summary>
        public bool Mega { get; }

        /// <summary>
        /// if zmoving, the name of the zmove
        /// </summary>
        public string Zmove { get; }

        /// <summary>
        /// if dynamaxed, the name of the max move
        /// </summary>
        public string MaxMove { get; } // 

        /// <summary>
        /// if terastallizing, tera type
        /// </summary>
        public string Terastallize { get; }

        /// <summary>
        /// priority of the action
        /// </summary>
        public int? Priority { get; }
    }

    public enum ActionType
    {
        Move,
        Switch,
        InstaSwitch,
        RevivalBlessing,
        Team,
        Shift,
        Pass
    }
}