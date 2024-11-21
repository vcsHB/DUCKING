using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class PathFinder
{
    private static Tilemap _tilemap; // Tilemap 벽이 있는 레이어
    private static LayerMask _wallLayer = LayerMask.GetMask("Wall");    //부술 수 없음
    private static LayerMask _barrier = LayerMask.GetMask("Barrier");   //부술 수 있음
    public Vector3 _targetPosition; // 목표 위치

    public static List<Vector2> path = new List<Vector2>(); // A* 경로

    public static void Initialize(Tilemap wallTilemap)
    {
        _tilemap = wallTilemap;
    }

    public static List<Vector2> GetPath { get => new List<Vector2>(path); }


    public static void FindPath(Vector2 start, Vector2 target)
    {
        // A* 알고리즘 초기화
        path.Clear();
        //pathIndex = 0;

        Vector3Int startTile = _tilemap.WorldToCell(start);
        Vector3Int targetTile = _tilemap.WorldToCell(target);

        HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>();
        PriorityQueue<Vector3Int> openSet = new PriorityQueue<Vector3Int>();

        openSet.Enqueue(startTile, 0);

        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        Dictionary<Vector3Int, float> gScore = new Dictionary<Vector3Int, float> { [startTile] = 0 };

        while (openSet.Count > 0)
        {
            Vector3Int current = openSet.Dequeue();

            if (current == targetTile)
            {
                ReconstructPath(cameFrom, current);
                return;
            }

            closedSet.Add(current);

            foreach (Vector3Int neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor) || IsWall(neighbor)) continue;

                float tentativeGScore = gScore[current] + Vector3Int.Distance(current, neighbor);

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    float fScore = tentativeGScore + GetHeuristic(neighbor, targetTile);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, fScore);
                    }
                }
            }
        }
    }

    private static bool IsWall(Vector3Int position)
    {
        Vector3 worldPosition = _tilemap.CellToWorld(position) + _tilemap.cellSize / 2;
        return Physics2D.BoxCast(worldPosition, _tilemap.cellSize, 0f, Vector2.zero, 0.1f, _wallLayer); // 박스 캐스트에 Vec.one을 바꾸면 판정을 조정할 수 있다.
    }

    private static void ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        path.Clear();
        while (cameFrom.ContainsKey(current))
        {
            path.Insert(0, _tilemap.CellToWorld(current));
            current = cameFrom[current];
        }

        path.Insert(0, _tilemap.CellToWorld(current));
    }



    private static float GetHeuristic(Vector3Int a, Vector3Int b) // 
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // 맨해튼거리 사용
    }

    // Get사방
    private static Vector3Int[] GetNeighbors(Vector3Int position)
    {
        Vector3Int[] neighbors = new Vector3Int[4]
        {
                position + Vector3Int.up,
                position + Vector3Int.down,
                position + Vector3Int.left,
                position + Vector3Int.right
        };

        return neighbors;
        //
    }
}

// 우선순위 큐...
public class PriorityQueue<T>
{
    private List<(T item, float priority)> elements = new List<(T, float)>();

    public int Count => elements.Count;

    public void Enqueue(T item, float priority)
    {
        elements.Add((item, priority));
        elements.Sort((a, b) => a.priority.CompareTo(b.priority)); // 정렬해주...
    }

    public T Dequeue()
    {
        if (elements.Count > 0)
        {
            T bestItem = elements[0].item;
            elements.RemoveAt(0);
            return bestItem;
        }
        return default(T);
    }

    public bool Contains(T item)
    {
        return elements.Exists(element => EqualityComparer<T>.Default.Equals(element.item, item));
    }
}