using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializeDictionary<K, V> : Dictionary<K, V>, ISerializationCallbackReceiver
{
    public K TempKey;
    public V TempValue;

    public List<K> SerializedKeys = new List<K>();
    public List<V> SerializedValues = new List<V>();

    public void OnAfterDeserialize()
    {
        SyncToSerializeData();
    }

    //리스트에 있는 걸 딕셔너리에 밀어 넣는다.
    private void SyncToSerializeData()
    {
        this.Clear();
        if(SerializedKeys != null && SerializedValues != null)
        {
            int maxCount = Mathf.Min(SerializedKeys.Count, SerializedValues.Count);
            for(int i = 0; i < maxCount; i++)
            {
                if (SerializedKeys[i] == null) continue;

                this[SerializedKeys[i]] = SerializedValues[i]; 
                //키가 중복되면 알아서 겹쳐들어간다.
            }
        }else
        {
            SerializedKeys = new List<K>();
            SerializedValues = new List<V>();
        }

        if(SerializedKeys.Count != SerializedValues.Count)
        {
            SerializedKeys = new List<K>(Keys);
            SerializedValues = new List<V>(Values);
        }
    }

    public void OnBeforeSerialize(){
        SerializedKeys.Clear();
        SerializedValues.Clear();

        foreach(K key in Keys)
        {
            SerializedKeys.Add(key);
        }
        foreach(V val in Values)
        {
            SerializedValues.Add(val);
        }
    }
}
