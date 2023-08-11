using System.Collections.Generic;

namespace Game.Battles
{
    public class Choice
    {
        /// <summary>
        /// true if the choice can't be cancelled because of the maybeTrapped issue
        /// </summary>
        public bool CantUndo { get; set; }

        /// <summary>
        /// Contains error text in the case of a choice error
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Array of chosen actions
        /// </summary>
        public List<ChosenAction> Actions { get; set; }

        /// <summary>
        /// Number of switches left that need to be performed
        /// </summary>
        public int ForcedSwitchesLeft { get; set; }

        /// <summary>
        /// number of passes left that need to be performed
        /// </summary>
        public int ForcedPassesLeft { get; set; } // 

        /// <summary>
        /// Indexes of pokemon chosen to switch in
        /// </summary>
        public HashSet<int> SwitchIns { get; set; }

        /// <summary>
        /// true if a Z-move has already been selected
        /// </summary>
        public bool ZMove { get; set; }

        /// <summary>
        /// true if a mega evolution has already been selected
        /// </summary>
        public bool Mega { get; set; }

        /// <summary>
        /// true if an ultra burst has already been selected
        /// </summary>
        public bool Ultra { get; set; }

        /// <summary>
        /// true if a dynamax has already been selected
        /// </summary>
        public bool Dynamax { get; set; }

        /// <summary>
        /// true if a terastallization has already been inputted
        /// </summary>
        public bool Terastallize { get; set; }
    }
}