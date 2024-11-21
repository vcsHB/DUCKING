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
        int errorCount = 0; // 연산 한계치
        // A* 알고리즘 초기화
        path.Clear();
        //pathIndex = 0;

        Vector3Int startTile = _tilemap.WorldToCell(start);
        Vector3Int targetTile = _tilemap.WorldToCell(target);
        Vector3Int targetPosss = new Vector3Int((int)target.x, (int)target.y);


        HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>();
        PriorityQueue<Vector3Int> openSet = new PriorityQueue<Vector3Int>();

        openSet.Enqueue(startTile, 0);

        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        Dictionary<Vector3Int, float> gScore = new Dictionary<Vector3Int, float> { [startTile] = 0 };

        while (openSet.Count > 0)
        {
            errorCount++;
            if (errorCount > 1000)
            {
                // 이쯤되면 못찾은겨
                break;
            }
            Vector3Int current = openSet.Dequeue();
            //Debug.Log($"Current Node: {current}, OpenSet Count: {openSet.Count}");

            if (Vector3.Distance(current, targetTile) < 2f)
            {
                ReconstructPath(cameFrom, current);
                return;
            }
            if (!closedSet.Contains(current))
            {
                closedSet.Add(current);
            }

            foreach (Vector3Int neighbor in GetNeighbors(current))
            {

                if (closedSet.Contains(neighbor) || IsWall(neighbor)) 
                {
                    //Debug.Log($"거긴 벽이다 애송이. {current}");
                    continue; // 이미 처리된 노드 건너뜀
                }


                if (closedSet.Contains(neighbor) || IsWall(neighbor)) continue;

                float tentativeGScore = gScore[current] + 1;

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    float fScore = tentativeGScore + GetHeuristic(neighbor, targetTile);

                    if (!closedSet.Contains(neighbor) && !openSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, fScore);
                    }
                }
            }
        }
        // openSet이 비었는데도 목표에 도달하지 못한 경우 (경로 없음)
        Debug.LogWarning("Pathfinding failed : CANT GO");
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
    private static List<Vector3Int> GetNeighbors(Vector3Int position)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>
        {
            position + Vector3Int.up,
            position + Vector3Int.down,
            position + Vector3Int.left,
            position + Vector3Int.right
        };
        // 유효한 타일만 반환 (벽 필터링)
        neighbors.RemoveAll(neighbor => IsWall(neighbor));
        return neighbors;
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