using UnityEngine;

public class TestDictionary : MonoBehaviour
{
    public int code;
    public SerializeDictionary<string, int> dictionary;

    private void Start()
    {
        foreach (var kvp in dictionary)
        {
            Debug.Log($"{kvp.Key} : {kvp.Value}");
        }

        //add testValue
        dictionary.Add("Hello", 20);
        dictionary.Add("GGM", 20);
        dictionary.Add("Test", 20);
    }

}

