using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : Building
{

    Vector2 a;

    public void Awake()
    {
        Vector3.Cross(Vector3.up, Vector3.right);
        Vector3.Dot(Vector3.up, Vector3.right);
    }
}
