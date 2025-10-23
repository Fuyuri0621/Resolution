using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Mixer")]
    [SerializeField] private AudioMixer gameMixer;
    [SerializeField] private AudioMixerGroup bgmMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    [Header("BGM")]
    private AudioSource bgmSourceA;
    private AudioSource bgmSourceB;
    private AudioSource activeSource;   // タ冀
    private AudioSource inactiveSource; // 非称睭
    private Coroutine bgmSwitchCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // ミㄢ BGM AudioSource
            bgmSourceA = gameObject.AddComponent<AudioSource>();
            bgmSourceA.loop = true;
            bgmSourceA.outputAudioMixerGroup = bgmMixerGroup;

            bgmSourceB = gameObject.AddComponent<AudioSource>();
            bgmSourceB.loop = true;
            bgmSourceB.outputAudioMixerGroup = bgmMixerGroup;

            activeSource = bgmSourceA;
            inactiveSource = bgmSourceB;

            // 更秖
            SetVolume("MasterVolume",PlayerPrefs.GetFloat("MasterVolume",0.75f));
            SetVolume("BgmVolume", PlayerPrefs.GetFloat("BgmVolume", 0.75f));
            SetVolume("SfxVolume", PlayerPrefs.GetFloat("SfxVolume", 0.75f));
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ミ冀 BGM礚睭睭
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;

        activeSource.clip = clip;
        activeSource.volume = 1f;
        activeSource.Play();
    }

    // 睭睭ち传 BGM
    public void SwitchBGM(AudioClip newClip, float fadeTime = 3)
    {
        if (bgmSwitchCoroutine != null)
            StopCoroutine(bgmSwitchCoroutine);

        bgmSwitchCoroutine = StartCoroutine(CrossfadeBGM(newClip, fadeTime));
    }

    public void EndBGM(float fadeTime = 1.5f)
    {
        if (bgmSwitchCoroutine != null)
            StopCoroutine(bgmSwitchCoroutine);

        bgmSwitchCoroutine = StartCoroutine(FadeOutBGM(fadeTime));
    }

    private IEnumerator FadeOutBGM(float fadeTime)
    {
        if (activeSource == null || !activeSource.isPlaying)
            yield break;

        float startVolume = activeSource.volume;
        float timer = 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            float t = timer / fadeTime;
            activeSource.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }

        activeSource.Stop();
        activeSource.clip = null;
    }

    private IEnumerator CrossfadeBGM(AudioClip newClip, float fadeTime)
    {
        if (newClip == null) yield break;

        // 砞﹚ inactiveSource
        inactiveSource.clip = newClip;
        inactiveSource.volume = 0f;
        inactiveSource.Play();

        float timer = 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            float t = timer / fadeTime;

            activeSource.volume = Mathf.Lerp(1f, 0f, t);
            inactiveSource.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        // ユ传 active/inactive
        AudioSource temp = activeSource;
        activeSource = inactiveSource;
        inactiveSource = temp;

        inactiveSource.Stop();
    }

    // 冀 SFX
    public void PlaySFX(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;

        GameObject obj = new GameObject("TempSFX");
        obj.transform.position = position;

        AudioSource source = obj.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.outputAudioMixerGroup = sfxMixerGroup;
        source.Play();

        Destroy(obj, clip.length);
    }

    // 砞﹚ 秖 (0 ~ 1)
    public void SetVolume(string volumeName , float value)
    {
        gameMixer.SetFloat(volumeName, Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat(volumeName, value);
    }

}
