using UnityEngine;
using TMPro; // if you use TextMeshPro

public class EncounterTrigger : MonoBehaviour
{
    [SerializeField] private DialogueFeedUI dialogueUI;  // assign in Inspector
    [SerializeField] private GameObject talkPrompt;      // your "[E] Talk" UI
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool triggered = false;
    private bool playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            talkPrompt.SetActive(true);

            if (!triggered)
            {
                triggered = true;
                talkPrompt.SetActive(false); // hide prompt while dialogue is running
                dialogueUI.gameObject.SetActive(true);
                dialogueUI.StartDialogue();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            talkPrompt.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && triggered && Input.GetKeyDown(interactKey))
        {
            talkPrompt.SetActive(false);
            dialogueUI.gameObject.SetActive(true);
            dialogueUI.StartDialogue();
        }
    }
}