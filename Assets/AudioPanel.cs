using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioPanel : MonoBehaviour
{
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider bgmVolume;
    [SerializeField] private Slider sfxVolume;

    private void Start()
    {
        // 初始化 Slider 數值（讀取 PlayerPrefs）
        masterVolume.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        bgmVolume.value = PlayerPrefs.GetFloat("BgmVolume", 0.75f);
        sfxVolume.value = PlayerPrefs.GetFloat("SfxVolume", 0.75f);


        masterVolume.onValueChanged.AddListener(value => {
            AudioManager.Instance.SetVolume("MasterVolume", value);
        });

        bgmVolume.onValueChanged.AddListener(value => {
            AudioManager.Instance.SetVolume("BgmVolume", value);
        });

        sfxVolume.onValueChanged.AddListener(value => {
            AudioManager.Instance.SetVolume("SfxVolume", value);
        });
    }
}
