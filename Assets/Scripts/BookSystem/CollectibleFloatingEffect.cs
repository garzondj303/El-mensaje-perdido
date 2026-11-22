using UnityEngine;

public class CollectibleFloatingEffect : MonoBehaviour
{
    [Header("Floating Movement")]
    [SerializeField] private float floatingHeight = 0.15f;
    [SerializeField] private float floatingSpeed = 2f;

    [Header("Rotation")]
    [SerializeField] private bool useRotation = true;
    [SerializeField] private float rotationAngle = 4f;
    [SerializeField] private float rotationSpeed = 1.5f;

    private Vector3 startingPosition;
    private float randomPhase;

    private void Start()
    {
        // Store the original position of the collectible.
        startingPosition = transform.position;

        // Give each collectible a different animation phase.
        randomPhase = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        // Calculate a smooth vertical floating movement.
        float verticalOffset = Mathf.Sin(
            Time.time * floatingSpeed + randomPhase
        ) * floatingHeight;

        transform.position = startingPosition +
                             Vector3.up * verticalOffset;

        if (useRotation)
        {
            // Calculate a subtle rotation around the Z axis.
            float currentAngle = Mathf.Sin(
                Time.time * rotationSpeed + randomPhase
            ) * rotationAngle;

            transform.rotation =
                Quaternion.Euler(0f, 0f, currentAngle);
        }
    }
}