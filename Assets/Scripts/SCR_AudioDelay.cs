using UnityEngine;

public class AudioSourceWithDelay : MonoBehaviour
{
    public AudioSource audioSource; 
    [SerializeField] private float delay = 1f; 
    private bool canPlay = true; 
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                Debug.LogError("No AudioSource component found.");
            }
        }
    }

    public void PlayAudio()
    {
        if (canPlay && audioSource != null)
        {
            audioSource.Play();
            canPlay = false;
            StartCoroutine(PlayDelay());
        }
    }

    private System.Collections.IEnumerator PlayDelay()
    {
        yield return new WaitForSeconds(delay);
        canPlay = true;
    }
}