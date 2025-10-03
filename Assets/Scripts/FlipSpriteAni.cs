using UnityEngine;

public class FlipSpriteAni : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    [Tooltip("Horizontal input axis name")]
    public string horizontalAxis = "Horizontal";

    void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        float h = Input.GetAxis(horizontalAxis);
        if (h > 0.01f)
            spriteRenderer.flipX = false;
        else if (h < -0.01f)
            spriteRenderer.flipX = true;
    }
}