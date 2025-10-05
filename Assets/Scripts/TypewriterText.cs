using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterText : MonoBehaviour
{
    public TMP_Text textMesh;
    [Tooltip("Time in seconds per character")]
    public float typeSpeed = 0.05f;

    private Coroutine typingCoroutine;

    public void ShowText(string message)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(message));
    }

    private IEnumerator TypeText(string message)
    {
        textMesh.text = "";
        foreach (char c in message)
        {
            textMesh.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }
}
