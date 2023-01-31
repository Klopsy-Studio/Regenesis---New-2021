using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
namespace Weasel.Utils
{
    public class GUIUtils
    {
        public static float GetArraySize(SerializedProperty array, string caption = null)
        {
            return EditorGUIUtility.singleLineHeight * (1 + array.arraySize);
        }
        public static void DisplayArray(SerializedProperty array, string caption = null)
        {
            EditorGUILayout.LabelField((caption == null) ? array.name : caption);
            EditorGUI.indentLevel++;
            if(array == null) { EditorGUI.indentLevel--; return; }
            for (int i = 0; i < array.arraySize; i++)
            {
                EditorGUILayout.PropertyField(array.GetArrayElementAtIndex(i));
            }
            EditorGUI.indentLevel--;
            //other possible methods:
            //array.InsertArrayElementAtIndex;
            //array.DeleteArrayElementAtIndex;
            //array.MoveArrayElement;
        }

        [Serializable]
        public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
        {
            [SerializeField] private List<SerializableKeyValuePair<TKey, TValue>> _keyValuePairs = new List<SerializableKeyValuePair<TKey, TValue>>();
            int size = 0;

            // save the dictionary to lists
            public void OnBeforeSerialize()
            {
                _keyValuePairs.Clear();
                foreach (KeyValuePair<TKey, TValue> pair in this)
                {
                    _keyValuePairs.Add(new SerializableKeyValuePair<TKey, TValue>(pair.Key, pair.Value));
                }
                for (int i = _keyValuePairs.Count; i < size; i++)
                {
                    _keyValuePairs.Add(new SerializableKeyValuePair<TKey, TValue>());
                }
            }

            // load dictionary from lists
            public void OnAfterDeserialize()
            {
                this.Clear();
                size = _keyValuePairs.Count;
                for (int i = 0; i < _keyValuePairs.Count; i++)
                {
                    this[_keyValuePairs[i].Key] = _keyValuePairs[i].Value;
                }
            }
        }

        [Serializable]
        public class SerializableKeyValuePair<TKey, TValue> : IEquatable<SerializableKeyValuePair<TKey, TValue>>
        {
            [SerializeField]
            TKey _key;
            public TKey Key { get { return _key; } }

            [SerializeField]
            TValue _value;
            public TValue Value { get { return _value; } }

            public SerializableKeyValuePair()
            {

            }

            public SerializableKeyValuePair(TKey key, TValue value)
            {
                this._key = key;
                this._value = value;
            }

            public bool Equals(SerializableKeyValuePair<TKey, TValue> other)
            {
                var comparer1 = EqualityComparer<TKey>.Default;
                var comparer2 = EqualityComparer<TValue>.Default;

                return comparer1.Equals(_key, other._key) &&
                    comparer2.Equals(_value, other._value);
            }

            public override int GetHashCode()
            {
                var comparer1 = EqualityComparer<TKey>.Default;
                var comparer2 = EqualityComparer<TValue>.Default;

                int h0;
                h0 = comparer1.GetHashCode(_key);
                h0 = (h0 << 5) + h0 ^ comparer2.GetHashCode(_value);
                return h0;
            }

            public override string ToString()
            {
                return String.Format("(Key: {0}, Value: {1})", _key, _value);
            }
        }

    }
}
#else
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue> { };
#endif