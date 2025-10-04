using UnityEngine;
using TMPro;
using System.Collections;

public class FeedLine : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    public bool IsRevealing { get; private set; }

    void Awake()
    {
        if (!label) label = GetComponentInChildren<TextMeshProUGUI>(true);
        if (!label) Debug.LogError("[FeedLine] Assign a TextMeshProUGUI to 'label' on the prefab.");
    }

    // Instant set
    public void SetText(string richText)
    {
        if (!label) return;
        label.text = richText ?? string.Empty;
        label.ForceMeshUpdate();
        label.maxVisibleCharacters = label.textInfo.characterCount;
        IsRevealing = false;
    }

    // Typewriter
    public IEnumerator Reveal(string richText, float charsPerSecond = 80f)
    {
        if (!label) yield break;

        IsRevealing = true;
        label.text = richText ?? string.Empty;
        label.ForceMeshUpdate();

        int total = label.textInfo.characterCount;
        label.maxVisibleCharacters = 0;

        float visible = 0f;
        while (label.maxVisibleCharacters < total)
        {
            visible += Time.unscaledDeltaTime * charsPerSecond;
            label.maxVisibleCharacters = Mathf.Min(total, Mathf.FloorToInt(visible));
            yield return null;
        }
        IsRevealing = false;
    }

    public void SkipReveal()
    {
        if (!label) return;
        label.maxVisibleCharacters = label.textInfo.characterCount;
        IsRevealing = false;
    }
}

