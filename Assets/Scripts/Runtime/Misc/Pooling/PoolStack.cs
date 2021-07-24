using System.Collections.Generic;
using UnityEngine;

namespace Runner.Misc
{
    class PoolStack
    {
        readonly Stack<GameObject> stack = new Stack<GameObject>();
        readonly GameObject sample;
        
        public PoolStack(GameObject sample) {
            this.sample = sample;
        }

        public GameObject Take() {
            return stack.Count > 0
                       ? stack.Pop()
                       : Object.Instantiate(sample, sample.transform.position, sample.transform.rotation, sample.transform.parent);
        }

        public void Put(GameObject go) => stack.Push(go);

        public void Clear() {
            foreach (var element in stack) 
                Object.Destroy(element);
            stack.Clear();
        }
    }
}
