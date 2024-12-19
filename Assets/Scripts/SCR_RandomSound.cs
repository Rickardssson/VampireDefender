using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    public AudioClip[] audioClips;

    [SerializeField] private float soundDelay = 0f;
    [SerializeField] private float soundCooldown = 0f;

    private AudioSource audioSource;
    private int lastPlayedIndex = -1;
    private float lastSoundTime = -Mathf.Infinity;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found.");
        }
    }

    public void PlayRandomSound()
    {
        if (Time.time - lastSoundTime < soundCooldown)
        {
            return;
        }

        if (audioClips.Length == 0)
        {
            Debug.LogWarning("No audio clips assigned.");
            return;
        }

        int randomIndex;
        // Prevent the same sound from being played twice in a row
        do { randomIndex = Random.Range(0, audioClips.Length); } while (randomIndex == lastPlayedIndex);

        lastPlayedIndex = randomIndex;
        lastSoundTime = Time.time;

        AudioClip selectedClip = audioClips[randomIndex];

        if (soundDelay > 0f)
        {
            Invoke(nameof(PlaySoundDelayed), soundDelay);
        }
        else
        {
            PlaySoundNow(selectedClip);
        }
    }

    private void PlaySoundNow(AudioClip selectedClip)
    {
        audioSource.clip = selectedClip;
        audioSource.Play();
    }

    private void PlaySoundDelayed()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}