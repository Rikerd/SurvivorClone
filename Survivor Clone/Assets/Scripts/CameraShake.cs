using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour {
    public float shakeDuration = 1f;
    public float shakeAmp = 3f;
    public float shakeFreq = 2f;

    public CinemachineVirtualCamera virtualCamera;

    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    private float currentShakeDuration;
    private bool isShaking = false;

    private void Awake()
    {
        virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();

        virtualCameraNoise.m_AmplitudeGain = 0;
        virtualCameraNoise.m_FrequencyGain = shakeFreq;
        isShaking = false;
    }

    // Update is called once per frame
    void Update () {
		if (isShaking)
        {
            currentShakeDuration -= Time.deltaTime;

            if (currentShakeDuration <= 0)
            {
                virtualCameraNoise.m_AmplitudeGain = 0;

                isShaking = false;
            }
        }
	}

    public void StartShake()
    {
        currentShakeDuration = shakeDuration;
        virtualCameraNoise.m_AmplitudeGain = shakeAmp;

        isShaking = true;
    }
}
