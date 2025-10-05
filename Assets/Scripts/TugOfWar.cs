using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TugOfWar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider bar;           // 0..1, start at 0.5
    public TextMeshProUGUI quoteText;
    public TextMeshProUGUI fightText;     // The "Press Space!" text (to hide)
    public GameObject image1;             // First image
    public GameObject image2;             // Second image
    public GameObject sliderObj;          // The tug-of-war slider
    public Button continueButton; // assign your button here

    [Header("Controls")]
    [SerializeField] private KeyCode mashKey = KeyCode.Space;

    [Header("Balance")]
    [SerializeField] private float pushPerPress = 0.06f;  // player gain per key press
    [SerializeField] private float mobPullPerSec = 0.25f; // AI pull per second
    [SerializeField] private float responsiveness = 6f;   // higher = snappier
    [SerializeField] private float friction = 4f;         // damps velocity feel

    [Header("FX (optional)")]
    [SerializeField] private AudioSource sfxPress;
    [SerializeField] private AudioClip pressClip;

    [Header("Text Files")]
    public TextAsset playerWinFile;
    public TextAsset mobWinFile;

    [Header("Audio")]
    public AudioSource fightAudio;  // assign your audio source here

    float vel; // “momentum” for nicer feel

    void Start()
    {
        if (bar) bar.value = 0.5f;
    }

    void Update()
    {
        if (!bar) return;

        // Player input
        if (Input.GetKeyDown(mashKey))
        {
            vel += pushPerPress;
            if (sfxPress && pressClip) { sfxPress.pitch = Random.Range(0.95f, 1.05f); sfxPress.PlayOneShot(pressClip, 0.9f); }
            // TODO: tiny screen shake or pulse UI here if you want
        }

        // AI pull + damping
        vel -= mobPullPerSec * Time.deltaTime;
        vel -= vel * friction * Time.deltaTime;

        // Apply to bar
        bar.value = Mathf.Clamp01(bar.value + vel * Time.deltaTime * responsiveness);

        // Win/Lose
        if (bar.value >= 1f) PlayerWins();
        else if (bar.value <= 0f) MobWins();
    }

    public void PlayerWins()
    {
        enabled = false;
        Debug.Log("Player wins!");
        // TODO: play win anim, load next scene, etc.
        if (quoteText && playerWinFile)
        {
            quoteText.gameObject.SetActive(true);   // show it
            GetComponent<TypewriterText>().ShowText(playerWinFile.text);
            CleanupUI();
            MuteAudio();
            ShowButton();
        }
    }

    public void MobWins()
    {
        enabled = false;
        Debug.Log("Mob wins!");
        // TODO: fail state, retry, etc.
        if (quoteText && mobWinFile)
        {
            quoteText.gameObject.SetActive(true);   // show it
            GetComponent<TypewriterText>().ShowText(mobWinFile.text);
            CleanupUI();
            MuteAudio();
            ShowButton();
        }
    }

    private void CleanupUI()
    {
        if (fightText) fightText.gameObject.SetActive(false);
        if (image1) image1.SetActive(false);
        if (image2) image2.SetActive(false);
        if (sliderObj) sliderObj.SetActive(false);
    }

    private void MuteAudio()
    {
        if (fightAudio)
        {
            fightAudio.mute = true; // mute the audio source
            // Alternatively, you can stop it completely:
            // fightAudio.Stop();
        }
    }

    private void ShowButton()
    {
        if (continueButton)
            continueButton.gameObject.SetActive(true); // activate the button
    }
}