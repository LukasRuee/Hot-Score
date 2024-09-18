using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffector : MonoBehaviour
{
    static public CameraEffector Instance;
    [SerializeField] private CinemachineVirtualCamera _vCam;
    private CinemachineBasicMultiChannelPerlin _cineBMCP;
    private void Awake()
    {
        Instance = this;
        _cineBMCP = _vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void Start()
    {
        _cineBMCP.m_AmplitudeGain = 0;
    }
    public IEnumerator ShakeCamera(float duration, float intensity)
    {
        _cineBMCP.m_AmplitudeGain = intensity;

        yield return new WaitForSeconds(duration);

        _cineBMCP.m_AmplitudeGain = 0;
    }
}
