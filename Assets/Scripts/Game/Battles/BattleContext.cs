using Game.Constants;
using UnityEngine;

namespace Game.Battles
{
    [CreateAssetMenu(menuName = "Pokemon/Create new game context")]
    public class BattleContext : ScriptableObject
    {
        [SerializeField] private TypeChart typeChart;
        [SerializeField] private ExpChart expChart;
        [SerializeField] private StageModifier stageModifier;
        public TypeChart TypeChart => typeChart;
        public ExpChart ExpChart => expChart;
        public StageModifier StageModifier => stageModifier;
    }
}