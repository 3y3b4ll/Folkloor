using UnityEngine;

public class EncounterTrigger : MonoBehaviour
{
    [Header("Shared Dialogue UI (one instance in scene)")]
    [SerializeField] private DialogueFeedUI dialogueUI;     // assign the shared DialogueCanvas's DialogueFeedUI
    [SerializeField] private GameObject talkPrompt;         // optional: [E] Talk prompt
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Dialogue Source (choose one)")]
    [SerializeField] private TextAsset dialogueFile;        // .txt in Assets (optional)
    [TextArea, SerializeField] private string[] dialogueLinesInspector; // inline fallback

    [Header("Trigger Behavior")]
    [SerializeField] private bool triggerOnce = false;      // auto-fire only the first time

    private string[] lines;          // resolved lines (from file or inspector)
    private bool triggeredOnce = false;
    private bool playerInRange = false;

    private void Awake()
    {
        // Prefer TextAsset if provided, else use inspector array
        if (dialogueFile != null)
            lines = SplitLines(dialogueFile.text);
        else
            lines = dialogueLinesInspector;

        if (lines == null || lines.Length == 0)
            Debug.LogWarning($"{name}: No dialogue lines set (file and inspector are empty).");
    }

    private static string[] SplitLines(string text)
    {
        // handle \r\n (Windows), \n (Unix), \r (old Mac)
        return text.Split(new[] { "\r\n", "\n", "\r" }, System.StringSplitOptions.RemoveEmptyEntries);
    }

    private void OnTriggerEnter(Collider other) // use OnTriggerEnter2D for 2D physics
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        // Show prompt unless we're about to auto-start and only once
        if (!(triggerOnce && triggeredOnce) && talkPrompt) talkPrompt.SetActive(true);

        // Auto-start FIRST time only (if desired)
        if (!triggeredOnce)
        {
            triggeredOnce = true;
            if (talkPrompt) talkPrompt.SetActive(false);
            dialogueUI.gameObject.SetActive(true);
            dialogueUI.StartDialogue(lines); // pass the resolved lines
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        if (talkPrompt) talkPrompt.SetActive(false);
    }

    private void Update()
    {
        // Allow manual restarts while in range (press E)
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            if (talkPrompt) talkPrompt.SetActive(false);
            dialogueUI.gameObject.SetActive(true);
            dialogueUI.StartDialogue(lines);
        }
    }
}
