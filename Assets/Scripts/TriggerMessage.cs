using UnityEngine;
using TMPro;  // TextMeshPro namespace

public class TriggerMessage : MonoBehaviour
{
    public TMP_Text messageText;     // Assign in Inspector
    public float displayTime = 3f;   // Time to show message
    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;  // assign in Inspector

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // play sound
            if (audioSource) audioSource.Play();
            
            StartCoroutine(ShowMessage());
        }
    }

    private System.Collections.IEnumerator ShowMessage()
    {
        messageText.gameObject.SetActive(true);   // Show text
        yield return new WaitForSeconds(displayTime);
        messageText.gameObject.SetActive(false);  // Hide text
    }
}
