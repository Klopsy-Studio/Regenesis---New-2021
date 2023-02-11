using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TheKiwiCoder {
    [System.Serializable]

    public class RandomSelector : CompositeNode {
        protected int current;
        bool updated = false;
        public bool test;
        protected override void OnStart() {
            int chosenChild = Random.Range(0, children.Count);                            
            current = chosenChild;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            var child = children[current];

            if (test)
            {
                Debug.Log(children[current].description);
            }
            updated = true;
            return child.Update();
        }
    }
}