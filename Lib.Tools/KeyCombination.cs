using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Lib.Tools
{
    public class KeyCombination
    {
        public KeyCombination(Key key1, Key key2)
        {
            Keys = new List<Key>()
            {
                key1,
                key2
            };
        }

        public KeyCombination(Key key1, Key key2, Key key3)
        {
            Keys = new List<Key>()
            {
                key1,
                key2,
                key3
            };
        }

        public KeyCombination(List<Key> keys)
        {
            Keys = new List<Key>();
        }


        public List<Key> Keys { get; }

        public override string ToString()
        {
            string keys = "";
            foreach (var key in Keys)
            {
                keys += $"{key}+";
            }

            if (keys.EndsWith("+"))
                keys = keys.Remove(keys.Length - 1);

            return keys;
        }

        public static KeyCombination GetKeyCombinationForPressedKeys()
        {
            List<Key> keys = new List<Key>();
            foreach (var key in Enum.GetValues(typeof(Key)).OfType<Key>())
            {
                if (Keyboard.IsKeyDown(key))
                    keys.Add(key);
            }

            return new KeyCombination(keys);
        }
    }
}