using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BomberTimer : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;

    public void StartTimer()
    {
        _text.gameObject.SetActive(true);
    }

    public void EndTimer()
    {
        _text.gameObject.SetActive(false);
    }

    public void SetTimer(int time)
    {
        _text.SetText(time.ToString());
    }
}
