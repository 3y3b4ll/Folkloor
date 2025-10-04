using UnityEngine;
using TMPro;
using System.Collections;

public class FeedLine : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;

    // Call once for instant line
    public void SetText(string richText)
    {
        label.text = richText;
        label.ForceMeshUpdate();
        label.maxVisibleCharacters = label.textInfo.characterCount;
    }

    // Optional: typewriter reveal
    public IEnumerator Reveal(string richText, float charsPerSecond = 60f)
    {
        label.text = richText;
        label.ForceMeshUpdate();
        int total = label.textInfo.characterCount;
        label.maxVisibleCharacters = 0;

        float t = 0f;
        while (label.maxVisibleCharacters < total)
        {
            t += Time.unscaledDeltaTime * charsPerSecond;
            label.maxVisibleCharacters = Mathf.Min(total, Mathf.FloorToInt(t));
            yield return null;
        }
    }
}
