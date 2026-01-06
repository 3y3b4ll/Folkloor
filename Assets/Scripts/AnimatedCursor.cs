using UnityEngine;

public class AnimatedCursor : MonoBehaviour
{
    public Texture2D[] cursorFrames; // Assign your cursor frames in the Inspector
    public float frameRate = 0.1f;  // Time between frames

    private int currentFrame;
    private float timer;

    void SetFrame(Texture2D tex)
    {
        Vector2 hotspot = new Vector2(tex.width / 2f, tex.height / 2f);
        Cursor.SetCursor(tex, hotspot, CursorMode.Auto);
    }

    void Start()
    {
        SetFrame(cursorFrames[0]); // Set initial cursor
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer -= frameRate;

            // Update the cursor frame
            currentFrame = (currentFrame + 1) % cursorFrames.Length;
            SetFrame(cursorFrames[currentFrame]);
        }
    }
}