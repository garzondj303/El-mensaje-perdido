using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Music Player")]
    public AudioSource audioSource;

    [Header("Zone Music")]
    public AudioClip jungleMusic;
    public AudioClip villageMusic;
    public AudioClip templeMusic;

    private AudioClip currentMusic;

    public void ChangeMusic(AudioClip newMusic)
    {
        if (newMusic == null || audioSource == null)
            return;

        if (currentMusic == newMusic && audioSource.isPlaying)
            return;

        audioSource.Stop();
        audioSource.clip = newMusic;
        audioSource.loop = true;
        audioSource.Play();

        currentMusic = newMusic;
    }

    public void PlayJungleMusic()
    {
        ChangeMusic(jungleMusic);
    }

    public void PlayVillageMusic()
    {
        ChangeMusic(villageMusic);
    }

    public void PlayTempleMusic()
    {
        ChangeMusic(templeMusic);
    }
}