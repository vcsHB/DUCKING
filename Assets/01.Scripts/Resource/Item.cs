using UnityEngine;

namespace ResourceSystem
{
    // 필드에 드랍될 오브젝트 클래스
    public class Item : MonoBehaviour
    {
        private Resource _resource;
        public void Initialize(Resource resource)
        {
            _resource = resource;
        }


        public Resource GetResource()
        {
            // 얻었을때 풀링에서 파괴시킴과 동시에 지급
            // 파괴시키는거 개발해야됨
            return _resource;
        }
        
        
    }
}