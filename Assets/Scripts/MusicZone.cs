using UnityEngine;

public class MusicZone : MonoBehaviour
{
    [Header("Music Manager")]
    public MusicManager musicManager;

    [Header("Zone Music")]
    public AudioClip zoneMusic;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something entered zone: " + gameObject.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered zone: " + gameObject.name);

            if (musicManager != null && zoneMusic != null)
            {
                musicManager.ChangeMusic(zoneMusic);

                Debug.Log("Changing music to: " + zoneMusic.name);
            }
            else
            {
                Debug.LogError("MusicManager or ZoneMusic is missing in: " + gameObject.name);
            }
        }
    }
}