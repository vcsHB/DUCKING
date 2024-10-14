using UnityEngine;

namespace BlockManage
{
    
    [CreateAssetMenu(menuName = "SO/Block")]
    public class BlockSO : ScriptableObject
    {
        public Vector2Int blockSize;
        // 나중에 빌드 가능한 블록의 Type을 받아서 설치 수행
        // 재료를 추가 (Resource필요)

    }
}