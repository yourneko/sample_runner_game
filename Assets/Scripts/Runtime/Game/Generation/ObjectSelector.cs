using System.Collections.Generic;
using Runner.Misc;
using UnityEngine;

namespace Runner.Game
{
    // Implements the rules for filling the location with coins and obstacles. 
    // Putting object generation logics in the block generator is not the best practice  (unless it is forced by needs of visual design)
    struct ObjectSelector
    {
        readonly IReadOnlyList<int[]> configs;
        IEnumerator<int> e;

        public ObjectSelector(params int[][] availableConfigs) {
            configs = availableConfigs;
            e       = GetRandomEnumerator(availableConfigs);
        }

        public void Reset() => e = GetRandomEnumerator(configs);
        
        public ObjectType[] GetNextRow() {
            if (!e.MoveNext()) 
                (e = GetRandomEnumerator(configs)).MoveNext();
            return ReadObjectRow(e.Current);
        }
        
        ObjectType[] ReadObjectRow(int rowValue) { // not the most human-readable format, but i keep it
            int value = rowValue;
            var result = new ObjectType[Configuration.TRACK_COUNT];
            for (int i = 0; i < Configuration.TRACK_COUNT; i++) {
                result[i] =   (ObjectType)(value & 3); // taking the last 2 bits from value
                value     >>= 2;
            }
            return result;
        }

        static IEnumerator<int> GetRandomEnumerator(IReadOnlyList<int[]> configs) {
            int index = Random.Range(0, configs.Count - 1);
            return ((IEnumerable<int>)configs[index]).GetEnumerator();
        }
    }
}
