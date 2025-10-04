using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button START;                 // assign the START button
    [SerializeField] private CanvasGroup blackpanel;       // CanvasGroup on blackpanel
    [SerializeField] private TextMeshProUGUI QuoteText;    // TMP text inside blackpanel
    [SerializeField] private Image SlideImage;             // one Image inside blackpanel

    [Header("Slideshow Frames")]
    [SerializeField] private Sprite[] frames;              // assign 3 sprites
    [SerializeField] private float frameDuration = 0.6f;
    [SerializeField] private float frameFade = 0.2f;

    [Header("Timings")]
    [SerializeField] private float fadeToBlackTime = 0.6f;
    [SerializeField] private float quoteDelay = 0.3f;
    [SerializeField] private float quoteHold = 3.0f;

    [Header("Next Scene")]
    [SerializeField] private string nextSceneName = "Tasand1";

    private void Awake()
    {
        if (blackpanel) blackpanel.alpha = 0f;
        if (QuoteText)  QuoteText.gameObject.SetActive(false);
        if (SlideImage) SlideImage.gameObject.SetActive(false);

        if (START) START.onClick.AddListener(OnStartClicked);
    }

    private void OnDestroy()
    {
        if (START) START.onClick.RemoveListener(OnStartClicked);
    }

    private void OnStartClicked()
    {
        if (START) START.interactable = false;
        StartCoroutine(BeginSequence());
    }

    private IEnumerator BeginSequence()
    {
        // fade to black
        yield return StartCoroutine(FadeCanvasGroup(blackpanel, 0f, 1f, fadeToBlackTime));

        // quote
        yield return new WaitForSeconds(quoteDelay);
        const string quote = "“When a person goes to sleep, the soul wanders. When the soul doesn’t return, the person remains inanimate… forever.” (Viljandi)";
        if (QuoteText)
        {
            QuoteText.text = quote;
            QuoteText.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(quoteHold);
        if (QuoteText) QuoteText.gameObject.SetActive(false);

        // slideshow
        if (frames != null && frames.Length > 0 && SlideImage != null)
        {
            SlideImage.color = new Color(1, 1, 1, 0);
            SlideImage.gameObject.SetActive(true);

            for (int i = 0; i < frames.Length; i++)
            {
                SlideImage.sprite = frames[i];
                yield return StartCoroutine(FadeImage(SlideImage, 0f, 1f, frameFade));
                yield return new WaitForSeconds(frameDuration);
                if (i < frames.Length - 1)
                    yield return StartCoroutine(FadeImage(SlideImage, 1f, 0f, frameFade));
            }
        }

        yield return new WaitForSeconds(0.2f);

        // load Tasand1
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);
        while (!op.isDone)
            yield return null;
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float from, float to, float duration)
    {
        if (!cg) yield break;
        float t = 0f;
        cg.alpha = from;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        cg.alpha = to;
    }

    private IEnumerator FadeImage(Image img, float from, float to, float duration)
    {
        if (!img) yield break;
        float t = 0f;
        Color c = img.color;
        c.a = from; img.color = c;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(from, to, t / duration);
            img.color = c;
            yield return null;
        }
        c.a = to; img.color = c;
    }
}
