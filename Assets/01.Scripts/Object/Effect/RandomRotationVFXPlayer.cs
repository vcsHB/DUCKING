using UnityEngine;

public class RandomRotationVFXPlayer : VFXPlayer
{
    [SerializeField] private float _randomizeRotation;
    
    [ContextMenu("PlayRotationVFX")]
    public override void PlayVFX()
    {
        base.PlayVFX();
        transform.rotation = Quaternion.Euler(0,0, Random.Range(0, _randomizeRotation));

    }
}
