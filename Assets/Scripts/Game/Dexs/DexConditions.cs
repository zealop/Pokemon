using System.Collections.Generic;
using Game.Conditions;
using OneOf;

namespace Game.Dexs
{
    public class DexConditions
    {
        private ModdedDex dex;
        private Dictionary<string, Condition> conditionCache;

        public DexConditions(ModdedDex dex)
        {
            this.dex = dex;
            this.conditionCache = new Dictionary<string, Condition>();
        }

        public Condition Get(OneOf<string, Condition>? name = null)
        {
            if (name is null)
            {
                return Condition.EmptyCondition;
            }

            if (name.Value.IsT1)
            {
                return name.Value.AsT1;
            }

            var nameString = name.Value.AsT0;
            return this.GetByID(
                nameString.StartsWith("item:") || nameString.StartsWith("ability:")
                    ? nameString
                    : Utils.Utils.ToID(nameString)
            );
        }

        public Condition GetByID(string id)
        {
            if (id is null)
            {
                return Condition.EmptyCondition;
            }

            this.conditionCache.TryGetValue(id, out var condition);
            if (condition is not null)
            {
                return condition;
            }

            bool found;

            // if (id.StartsWith("item:")) {
            //     var item = this.dex.Items.GetByID(id.slice(5) as ID);
            //     condition = {...item, id: 'item:' + item.id as ID} as any as Condition;
            // } else if (id.startsWith('ability:')) {
            //     const ability = this.dex.abilities.getByID(id.slice(8) as ID);
            //     condition = {...ability, id: 'ability:' + ability.id as ID} as any as Condition;
            // } else if (this.dex.data.Rulesets.hasOwnProperty(id)) {
            //     condition = this.dex.formats.get(id) as any as Condition;
            // } else if (this.dex.data.Conditions.hasOwnProperty(id)) {
            //     condition = new Condition({name: id, ...this.dex.data.Conditions[id]});
            // } else if (
            //     (this.dex.data.Moves.hasOwnProperty(id) && (found = this.dex.data.Moves[id]).condition) ||
            //     (this.dex.data.Abilities.hasOwnProperty(id) && (found = this.dex.data.Abilities[id]).condition) ||
            //     (this.dex.data.Items.hasOwnProperty(id) && (found = this.dex.data.Items[id]).condition)
            // ) {
            //     condition = new Condition({name: found.name || id, ...found.condition});
            // } else if (id === 'recoil') {
            //     condition = new Condition({name: 'Recoil', effectType: 'Recoil'});
            // } else if (id === 'drain') {
            //     condition = new Condition({name: 'Drain', effectType: 'Drain'});
            // } else {
            //     condition = new Condition({name: id, exists: false});
            // }
            //
            // this.conditionCache.set(id, condition);
            return condition;
        }
    }
}