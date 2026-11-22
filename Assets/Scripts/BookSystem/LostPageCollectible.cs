using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LostPageCollectible : MonoBehaviour
{
    [Header("Book Page")]
    [Tooltip("Index of the book page unlocked by this collectible.")]
    [SerializeField] private int pageIndex;

    [Header("Player Detection")]
    [Tooltip("Only objects with this tag can collect the page.")]
    [SerializeField] private string playerTag = "Player";

    [Header("Collection Settings")]
    [Tooltip("Optional visual effect instantiated when the page is collected.")]
    [SerializeField] private GameObject collectionEffectPrefab;

    [Tooltip("Optional sound played when the page is collected.")]
    [SerializeField] private AudioClip collectionSound;

    [Tooltip("Volume used for the collection sound.")]
    [Range(0f, 1f)]
    [SerializeField] private float collectionSoundVolume = 1f;

    private Collider2D collectibleCollider;
    private bool hasBeenCollected;

    private void Awake()
    {
        collectibleCollider = GetComponent<Collider2D>();

        // The collectible collider must work as a trigger.
        collectibleCollider.isTrigger = true;
    }

    private void Start()
    {
        // Remove this collectible if its page was already unlocked
        // during a previous play session.
        if (BookManager.Instance != null &&
            BookManager.Instance.IsPageUnlocked(pageIndex))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore collisions after the page has already been collected.
        if (hasBeenCollected)
        {
            return;
        }

        // Only allow the player to collect this page.
        if (!other.CompareTag(playerTag))
        {
            return;
        }

        CollectPage();
    }

    private void CollectPage()
    {
        if (BookManager.Instance == null)
        {
            Debug.LogError(
                "The page could not be collected because no BookManager " +
                "was found in the scene."
            );

            return;
        }

        hasBeenCollected = true;

        // Disable the collider immediately to prevent duplicate triggers.
        collectibleCollider.enabled = false;

        // Unlock and save the corresponding book page.
        BookManager.Instance.UnlockPage(pageIndex);

        // Create the optional collection effect.
        if (collectionEffectPrefab != null)
        {
            Instantiate(
                collectionEffectPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        // Play the optional collection sound.
        if (collectionSound != null)
        {
            AudioSource.PlayClipAtPoint(
                collectionSound,
                transform.position,
                collectionSoundVolume
            );
        }

        Debug.Log(
            "Lost book page collected. Page index: " + pageIndex
        );

        // Remove the collectible from the map.
        Destroy(gameObject);
    }

    private void OnValidate()
    {
        // Prevent negative page indexes in the Inspector.
        if (pageIndex < 0)
        {
            pageIndex = 0;
        }

        // Automatically configure the collider as a trigger.
        Collider2D currentCollider = GetComponent<Collider2D>();

        if (currentCollider != null)
        {
            currentCollider.isTrigger = true;
        }
    }
}