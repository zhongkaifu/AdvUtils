using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace AdvUtils
{
    public class Trie
    {    
        public class Node
        {
            public Node()
            {
                bHasData = false;
                _data = 0;
                _children = null;
                _childChars = null;
            }


            public void AddChild(byte[] str, int idx, int data)
            {
                if (_children == null)
                {
                    _children = new List<Node>(1);
                    _childChars = new List<byte>(1);
                }

                int childIdx = _childChars.IndexOf(str[idx]);
                if (childIdx < 0)
                {
                    childIdx = _children.Count;
                    _children.Add(new Node());
                    _childChars.Add(str[idx]);
                }

                if (idx < str.Length - 1)
                {
                    _children[childIdx].AddChild(str, idx + 1, data);
                }
                else
                {
                    _children[childIdx].SetData(data);
                }
            }

            void SetData(int data)
            {
                if (bHasData)
                {
                    throw new Exception("Warning: Duplicate keys in trie. Previous one will be overwritten.");
                }

                _data = data;
                bHasData = true;
            }

            public bool bHasData;
            public int _data;
            public List<Node> _children;
            public List<byte> _childChars;
        }

        Node _root;
        public Trie()
        {
            _root = new Node();
        }
        public void Add(string key, int value)
        {
            if (key.Length > 0)
            {
                _root.AddChild(Encoding.UTF8.GetBytes(key), 0, value);
            }
        }

        public bool Match(string str, out int data)
        {
            byte[] strData = Encoding.UTF8.GetBytes(str);

            data = 0;
            Node node = _root;
            for (int i = 0; i < strData.Length; i++)
            {
                int childIndex = node._childChars.IndexOf(strData[i]);
                if (childIndex < 0)
                {
                    return false;
                }
                node = node._children[childIndex];
            }

            if (node.bHasData == true)
            {
                data = node._data;
                return true;
            }
            return false;
        }
    }
}