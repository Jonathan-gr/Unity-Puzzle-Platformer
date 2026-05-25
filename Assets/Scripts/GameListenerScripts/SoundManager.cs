using UnityEngine;
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource sfxSource;
    public AudioSource musicSource;


    void Awake() => Instance = this;

    public void PlaySFX(AudioClip clip, float volume = 0.5f)
    {
        sfxSource.pitch = Random.Range(0.9f, 1.1f);
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }
    public AudioSource PlayLoopingSFX(AudioClip clip, float volume = 0.5f)
    {
        // Create a temporary GameObject to act as a separate channel
        GameObject go = new GameObject("LoopingChannel_" + clip.name);
        go.transform.SetParent(transform); // Keep it organized under the manager

        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.loop = true;
        source.Play();

        return source; // Return this so the caller can stop it later
    }
}
