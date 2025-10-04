using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueFeedUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    [SerializeField] private FeedLine feedLinePrefab;
    [SerializeField] private GameObject nextIndicator;

    [Header("Demo Data")]
    [TextArea]
    public string[] lines = {
        "<b>Detective</b>: Another day, another body.",
        "Narration: The rain taps like keys on a broken typewriter.",
        "<i>Thought Cabinet</i>: Maybe don’t say that out loud.",
        "<color=#85C1E9>Informant</color>: You came. Good."
    };

    private int index = 0;
    private bool waitingForSpace = false;

    void Awake()
    {
        if (nextIndicator != null) nextIndicator.SetActive(false);
    }

    void OnEnable()
    {
        // clear previous content
        for (int i = content.childCount - 1; i >= 0; i--)
            Destroy(content.GetChild(i).gameObject);

        index = 0;
        StartCoroutine(AppendNext()); // show first line
    }

    void Update()
    {
        if (waitingForSpace && Input.GetKeyDown(KeyCode.Space))
        {
            waitingForSpace = false;
            if (nextIndicator != null) nextIndicator.SetActive(false);

            if (index < lines.Length)
                StartCoroutine(AppendNext());
            else
                gameObject.SetActive(false); // close box at end
        }
    }

    IEnumerator AppendNext()
    {
        if (index >= lines.Length) yield break;

        var line = Instantiate(feedLinePrefab, content);

        yield return StartCoroutine(line.Reveal(lines[index], 80f));

        yield return null;
        Canvas.ForceUpdateCanvases();

        // Important: 0f = bottom
        scrollRect.verticalNormalizedPosition = 0f;

        index++;
        waitingForSpace = true;
        nextIndicator.SetActive(true);
    }

}
