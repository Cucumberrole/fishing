using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jingle;

    public void PlayJingle()
    {
        audioSource.PlayOneShot(jingle);
    }
}