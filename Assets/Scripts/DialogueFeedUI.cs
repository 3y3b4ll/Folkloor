using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueFeedUI : MonoBehaviour
{
    [Header("UI Refs")]
    [SerializeField] private ScrollRect scrollRect;       // DialogueFeed (has ScrollRect)
    [SerializeField] private RectTransform content;       // DialogueFeed/Viewport/Content
    [SerializeField] private FeedLine feedLinePrefab;
    [SerializeField] private Button continueButton;       // ← Create this Button
    [SerializeField] private TextMeshProUGUI buttonLabel; // ← TMP child of the Button
    [Header("Button Appearance")]
    [SerializeField] private Color continueColor = new Color(1f, 0.5f, 0f); // orange
    [SerializeField] private Color endColor = Color.red;


    [Header("Button Text")]
    [SerializeField] private string continueText = "Continue";
    [SerializeField] private string endText = "End";

    [Header("Demo Data")]
    [TextArea] public string[] lines = {
        "<b>Detective</b>: Another day, another body.",
        "Narration: The rain taps like keys on a broken typewriter.",
        "<i>Thought Cabinet</i>: Maybe don’t say that out loud.",
        "<color=#85C1E9>Informant</color>: You came. Good."
    };

    private int index = 0;                // next line to append
    private FeedLine currentLine = null;  // the line being revealed
    private Coroutine revealCo = null;

    void Awake()
    {
        if (continueButton) continueButton.onClick.AddListener(OnContinueClicked);
    }

    void OnDestroy()
    {
        if (continueButton) continueButton.onClick.RemoveListener(OnContinueClicked);
    }

    void OnEnable()
    {
        // Optional: auto-start; otherwise call StartDialogue() from your trigger
        // StartDialogue();
    }

    public void StartDialogue(string[] source = null)
    {
        if (source != null) lines = source;

        // Clear previous content
        for (int i = content.childCount - 1; i >= 0; i--)
            Destroy(content.GetChild(i).gameObject);

        index = 0;
        gameObject.SetActive(true);

        UpdateButtonLabel();  // sets Continue/End correctly for 1-line cases
        AppendNext();
    }

    private void AppendNext()
    {
        if (index >= lines.Length)
        {
            CloseDialogue();
            return;
        }

        // Create new line at bottom
        currentLine = Instantiate(feedLinePrefab, content);

        // Start reveal
        if (revealCo != null) StopCoroutine(revealCo);
        revealCo = StartCoroutine(RevealAndScroll(lines[index]));

        index++;
        UpdateButtonLabel(); // If we just queued the last line, button becomes "End"
    }

    private IEnumerator RevealAndScroll(string text)
    {
        yield return StartCoroutine(currentLine.Reveal(text, 80f));

        // Ensure scrolled to bottom
        yield return null; // wait one frame for layout
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();
    }

    private void OnContinueClicked()
    {
        // First click while typing -> finish instantly
        if (currentLine != null && currentLine.IsRevealing)
        {
            currentLine.SkipReveal();
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
            return;
        }

        // Otherwise advance or end
        if (index < lines.Length)
            AppendNext();
        else
            CloseDialogue();
    }

    private void UpdateButtonLabel()
    {
        bool isLastClick = (index >= lines.Length);

        if (buttonLabel)
            buttonLabel.text = isLastClick ? endText : continueText;

        if (continueButton && continueButton.image != null)
        {
            continueButton.image.color = isLastClick ? endColor : continueColor;
        }
    }


    private void CloseDialogue()
    {
        // TODO: play a fade/SFX if you like, then hide wrapper/panel
        gameObject.SetActive(false);
    }
}
