using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using System;

public class CinemachineShakeManager : MonoBehaviour
{
    public static CinemachineShakeManager Instance { get; private set; }

    private Coroutine shakeCoroutine;

    void Awake()
    {
        Instance = this;
    }
     public enum ShakeStrength { LIGHT, MID, STRONG }
    public void Shake(ShakeStrength strength)
    {
        float amplitude = 0f;
        float frequency = 0f;
        float duration = 0f;

        switch (strength)
        {
            case ShakeStrength.LIGHT:
                amplitude = 1f;
                frequency = 1f;
                duration = 0.1f;
                break;
            case ShakeStrength.MID:
                amplitude = 1.5f;
                frequency = 1.5f;
                duration = 0.15f;
                break;
            case ShakeStrength.STRONG:
                amplitude = 2.5f;
                frequency = 2f;
                duration = 0.3f;
                break;
        }
        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(DoShake(amplitude, frequency, duration));
    }
  
    private IEnumerator DoShake(float amplitude, float frequency, float duration)
    {

        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        CinemachineCamera currentVCam = brain.ActiveVirtualCamera as CinemachineCamera;

        if (currentVCam != null)
        {
            var noise = currentVCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
            if (noise != null)
            {
                noise.AmplitudeGain = amplitude;
                noise.FrequencyGain = frequency;

                float elapsed = 0f;
                while (elapsed < duration)
                {
                    elapsed += Time.unscaledDeltaTime;
                    yield return null;
                }

                noise.AmplitudeGain = 0f;
                noise.FrequencyGain = 0f;
            }
        }

        shakeCoroutine = null;
    }
}
