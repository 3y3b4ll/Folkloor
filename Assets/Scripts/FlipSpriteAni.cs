using UnityEngine;

public class FlipSpriteAni : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Camera worldCamera; // assign Main Camera in inspector

    void Awake()
    {
        if (!spriteRenderer) 
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (!worldCamera)
            worldCamera = Camera.main;
    }

    void Update()
    {
        // Convert mouse position to world space
        Vector3 mouseWorld = worldCamera.ScreenToWorldPoint(Input.mousePosition);

        // If the mouse is right of player, face right; left of player, face left
        if (mouseWorld.x > transform.position.x)
            spriteRenderer.flipX = true;   // facing right (flipped)
        else if (mouseWorld.x < transform.position.x)
            spriteRenderer.flipX = false;  // facing left (not flipped)
    }
}