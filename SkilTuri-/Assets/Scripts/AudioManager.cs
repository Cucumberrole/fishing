using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("AudioSource")]
    public AudioSource ambientSource;
    public AudioSource bgmSource;
    public AudioSource seSource;

    [Header("環境音")]
    public AudioClip seagullAmbient;

    [Header("BGM")]
    public AudioClip gameBGM1;
    public AudioClip gameBGM2;

    [Header("SE")]
    public AudioClip coinSE;
    public AudioClip buttonSE;
    public AudioClip lureSplashSE;
    public AudioClip catchSplashSE;
    public AudioClip rodCastSE;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    public void PlayAmbient(AudioClip clip)
    {
        if (ambientSource == null || clip == null) return;
        if (ambientSource.clip == clip && ambientSource.isPlaying) return;
        ambientSource.clip = clip;
        ambientSource.loop = true;
        ambientSource.Play();
    }

    public void StopAmbient()
    {
        if (ambientSource == null) return;
        ambientSource.Stop();
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource == null || clip == null) return;
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource == null) return;
        bgmSource.Stop();
    }

    public void PlaySE(AudioClip clip)
    {
        if (seSource == null || clip == null) return;
        seSource.PlayOneShot(clip);
    }

    public void PlaySeagullAmbient() { PlayAmbient(seagullAmbient); }
    public void PlayGameBGM1() { PlayBGM(gameBGM1); }
    public void PlayGameBGM2() { PlayBGM(gameBGM2); }
    public void PlayButtonSE() { PlaySE(buttonSE); }
    public void PlayCoinSE() { PlaySE(coinSE); }
    public void PlayLureSplashSE() { PlaySE(lureSplashSE); }
    public void PlayCatchSplashSE() { PlaySE(catchSplashSE); }
    public void PlayRodCastSE() { PlaySE(rodCastSE); }
}
