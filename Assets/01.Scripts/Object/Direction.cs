using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Direction
{
    public static Vector3[] directions = new Vector3[4]
    {
        new Vector3(0,0,0),
        new Vector3(0,0,90),
        new Vector3(0,0,180),
        new Vector3(0,0,270)
    };

    public static Vector2Int[] directionsInt = new Vector2Int[4]
    {
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(-1, 0),
    };

    public static Vector3 GetDirection(DirectionEnum dir)
        => directions[(int)dir];
    public static Vector2Int GetTileDirection(DirectionEnum dir)
        => directionsInt[(int)dir];

    public static DirectionEnum GetOpposite(DirectionEnum dir) => (DirectionEnum)(((int)dir + 2) % 4);
}

public enum DirectionEnum
{
    Down = 0,
    Right = 1,
    Up = 2,
    Left = 3,
}
