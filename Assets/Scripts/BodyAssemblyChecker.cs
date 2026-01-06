using UnityEngine;
using System.Collections.Generic;

public class BodyAssemblyChecker : MonoBehaviour
{
    [Header("Player Tag")]
    public string playerTag = "Player";

    [Header("Required Slots")]
    public List<string> requiredSlots = new List<string>
    {
        "P_kasi",
        "V_kasi",
        "P_jalg",
        "V_jalg",
        "torso",
        "pea"
    };

    [Header("Colors")]
    public Color passColor = Color.green;
    public Color incompleteColor = Color.yellow;
    public Color badColor = Color.red;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        EvaluatePlayer(other.transform);
    }

    void EvaluatePlayer(Transform player)
    {
        Dictionary<string, string> slotToTag = new Dictionary<string, string>();

        // Gather equipped parts
        foreach (Transform child in player)
        {
            string tag = child.tag;

            foreach (string slot in requiredSlots)
            {
                // Match slot or slot_halb
                if (tag == slot || tag == slot + "_halb")
                {
                    slotToTag[slot] = tag;
                    break;
                }
            }
        }

        // Case 1: Missing slots - > YELLOW
        foreach (string slot in requiredSlots)
        {
            if (!slotToTag.ContainsKey(slot))
            {
                SetColor(incompleteColor);
                return;
            }
        }

        // Case 2: All slots filled, but any _halb - > RED
        foreach (var kvp in slotToTag)
        {
            if (kvp.Value.EndsWith("_halb"))
            {
                SetColor(badColor);
                return;
            }
        }

        // Case 3: All slots filled, no _halb - > GREEN
        SetColor(passColor);
    }

    void SetColor(Color color)
    {
        if (rend)
            rend.material.color = color;
    }
}
