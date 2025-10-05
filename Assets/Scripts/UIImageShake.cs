using UnityEngine;

public class UIImageShake : MonoBehaviour
{
    [Header("Shake Settings")]
    public float amplitude = 5f;    // Max distance from original position
    public float speed = 2f;        // Base rotation speed
    public float randomness = 2f;   // How erratic the motion is

    private Vector3 originalPos;
    private float angle = 0f;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        // Increase angle over time (circular motion)
        angle += speed * Time.deltaTime;

        // Base circular offsets
        float offsetX = Mathf.Cos(angle) * amplitude;
        float offsetY = Mathf.Sin(angle) * amplitude;

        // Add erratic noise
        offsetX += (Random.value * 2f - 1f) * randomness;
        offsetY += (Random.value * 2f - 1f) * randomness;

        transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);
    }

    public void ResetPosition()
    {
        transform.localPosition = originalPos;
    }
}
