using UnityEngine;
using TMPro;  // TextMeshPro namespace

public class TriggerMessage : MonoBehaviour
{
    public TMP_Text messageText;     // Assign in Inspector
    public float displayTime = 3f;   // Time to show message

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
