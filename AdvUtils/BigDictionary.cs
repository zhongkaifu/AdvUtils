using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace AdvUtils
{
    public class BigDictionary<TKey, TValue>
    {
        public List<Dictionary<TKey, TValue>> _dic;
        private int _nMaxItemPerPart = 40000000;
        private IEqualityComparer<TKey> m_compare;


        public BigDictionary()
        {
            _dic = new List<Dictionary<TKey, TValue>>();
            m_compare = null;
        }

        public BigDictionary(int bucketSize, IEqualityComparer<TKey> compare)
        {
            _dic = new List<Dictionary<TKey, TValue>>();
            _nMaxItemPerPart = bucketSize;
            m_compare = compare;
        }

        #region property
        public int MaxItemPerPart
        {
            get { return _nMaxItemPerPart; }
            set { _nMaxItemPerPart = value; }
        }

        public long Count
        {
            get
            {
                long result = 0;
                foreach (Dictionary<TKey, TValue> pair in _dic)
                {
                    result += pair.Count;
                }

                return result;
            }
        }

        public TValue this[TKey Key]
        {
            get
            {
                TValue otv;
                foreach (Dictionary<TKey, TValue> pair in _dic)
                {
                    if (pair.TryGetValue(Key, out otv) == true)
                    {
                        return otv;
                    }
                }

                KeyNotFoundException e = new KeyNotFoundException();
                throw e;
            }
            set
            {
                foreach (Dictionary<TKey, TValue> pair in _dic)
                {
                    if (pair.ContainsKey(Key) == true)
                    {
                        pair[Key] = value;
                        return;
                    }
                }

                KeyNotFoundException e = new KeyNotFoundException();
                throw e;
            }
        }
        #endregion

        public IEnumerator GetEnumerator()
        {
            foreach (Dictionary<TKey, TValue> pair in _dic)
            {
                foreach (KeyValuePair<TKey, TValue> subpair in pair)
                {
                    yield return subpair;
                }
            }
        }


        public void Add(TKey Key, TValue Value, bool bCheckKey = true)
        {
            //First check all sub-dictionary
            if (bCheckKey == true)
            {
                if (ContainsKey(Key) == true)
                {
                    ArgumentException e = new ArgumentException("Key " + Key.ToString() + " has exists.");
                    throw e;
                }
            }

            //Then, if given key is not existed, add it
            foreach (Dictionary<TKey, TValue> pair in _dic)
            {
                if (pair.Count < _nMaxItemPerPart)
                {
                    pair.Add(Key, Value);
                    return;
                }
            }

            if (m_compare == null)
            {
                _dic.Add(new Dictionary<TKey, TValue>());
            }
            else
            {
                _dic.Add(new Dictionary<TKey, TValue>(m_compare));
            }

            _dic[_dic.Count - 1].Add(Key, Value);
        }

        public bool Remove(TKey Key)
        {
            for (int i = 0; i < _dic.Count; i++)
            {
                if (_dic[i].Remove(Key) == true)
                {
                    if (_dic[i].Count == 0)
                    {
                        _dic.RemoveAt(i);
                    }
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            _dic.Clear();
        }

        public bool TryGetValue(TKey key, out TValue tv)
        {
            tv = default(TValue);
            try
            {
                for (int i = 0; i < _dic.Count; i++)
                {
                    if (_dic[i].TryGetValue(key, out tv) == true)
                    {
                        return true;
                    }
                }
            }
            catch (System.Exception){}

            return false;
        }

        public bool ContainsKey(TKey Key)
        {
            foreach (Dictionary<TKey, TValue> pair in _dic)
            {
                if (pair.ContainsKey(Key) == true)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
