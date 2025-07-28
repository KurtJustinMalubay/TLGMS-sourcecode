using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    void Awake()
    {
#if UNITY_ANDROID
        QualitySettings.vSyncCount = 0;
        float refreshRate = (float)Screen.currentResolution.refreshRateRatio.value;
        Application.targetFrameRate = Mathf.RoundToInt(refreshRate);
#else
        QualitySettings.vSyncCount = 1;
#endif
    }
}
