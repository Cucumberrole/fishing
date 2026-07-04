using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jingle;

    public void PlayJingle()
    {
        if (AudioManager.Instance != null) { AudioManager.Instance.PlayButtonSE(); return; }
        if (audioSource != null && jingle != null) audioSource.PlayOneShot(jingle);
    }
}
