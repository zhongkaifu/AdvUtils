using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdvUtils
{
    public class PriorityQueue<TKey, TValue> where TKey : IComparable<TKey>
    {
        private List<TKey> Keys;
        private List<TValue> Values;

        public PriorityQueue()
        {
            this.Keys = new List<TKey>();
            this.Values = new List<TValue>();
        }

        public void Enqueue(TKey key, TValue value)
        {
            Keys.Add(key);
            Values.Add(value);
            int ci = Keys.Count - 1; // child index; start at end
            while (ci > 0)
            {
                int pi = (ci - 1) / 2; // parent index
                if (Keys[ci].CompareTo(Keys[pi]) >= 0) break; // child item is larger than (or equal) parent so we're done
                TKey tmp = Keys[ci]; Keys[ci] = Keys[pi]; Keys[pi] = tmp;
                TValue tmp2 = Values[ci]; Values[ci] = Values[pi]; Values[pi] = tmp2;
                ci = pi;
            }
        }

        public TKey Dequeue()
        {
            // assumes pq is not empty; up to calling code
            int li = Keys.Count - 1; // last index (before removal)
            TKey frontKey = Keys[0];   // fetch the front
            TValue frontValue = Values[0];

            Keys[0] = Keys[li];
            Keys.RemoveAt(li);

            Values[0] = Values[li];
            Values.RemoveAt(li);

            --li; // last index (after removal)
            int pi = 0; // parent index. start at front of pq
            while (true)
            {
                int ci = pi * 2 + 1; // left child index of parent
                if (ci > li) break;  // no children so done
                int rc = ci + 1;     // right child
                if (rc <= li && Keys[rc].CompareTo(Keys[ci]) < 0) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
                    ci = rc;
                if (Keys[pi].CompareTo(Keys[ci]) <= 0) break; // parent is smaller than (or equal to) smallest child so done
                TKey tmp = Keys[pi]; Keys[pi] = Keys[ci]; Keys[ci] = tmp; // swap parent and child
                TValue tmp2 = Values[pi]; Values[pi] = Values[ci]; Values[ci] = tmp2; // swap parent and child
                pi = ci;
            }
            return frontKey;
        }

        public KeyValuePair<TKey, TValue> Peek()
        {
            TKey frontKey = Keys[0];
            TValue frontValue = Values[0];
            return new KeyValuePair<TKey, TValue>(frontKey, frontValue);
        }

        public int Count()
        {
            return Keys.Count;
        }


    } // PriorityQueue
}
