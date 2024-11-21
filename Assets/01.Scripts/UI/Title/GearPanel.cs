using UnityEngine;

namespace UI.TitleScene
{

    public class GearPanel : MonoBehaviour
    {
        [SerializeField] private float _spinSpeed;

        private void Update()
        {


            Rotate();
        }

        private void Rotate()
        {
            float z = transform.rotation.eulerAngles.z + _spinSpeed;
            transform.rotation = Quaternion.Euler(0, 0, z);
        }
    }

}