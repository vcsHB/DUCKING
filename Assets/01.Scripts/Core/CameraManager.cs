using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] private CinemachineVirtualCamera _virCam;
    [SerializeField] private float _defaultZoomValue = 8f;
    private bool _isZooming;

    public void Zoom(float zoom, float duration)
    {
        if (_isZooming) return;
        _isZooming = true;
        StartCoroutine(ZoomCoroutine(zoom, duration));
    }

    [ContextMenu("ZoomDefault")]
    private void DebugZoom()
    {
        ZoomDefault(1f);   
    }

    public void ZoomDefault(float duration)
    {
        Zoom(_defaultZoomValue, duration);
    }


    private IEnumerator ZoomCoroutine(float target, float duration)
    {
        float currentTime = 0f;
        float before = _virCam.m_Lens.OrthographicSize;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float ratio = currentTime / duration;
            _virCam.m_Lens.OrthographicSize = Mathf.Lerp(before, target, ratio);
            yield return null;
        }
        _virCam.m_Lens.OrthographicSize = target;
        _isZooming = false;
    }

}
