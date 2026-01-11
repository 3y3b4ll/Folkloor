using UnityEngine;

public class WorldStateManager : MonoBehaviour
{
    public static WorldStateManager Instance;

    [Header("Player State")]
    public bool isHoldingItem;
    public string heldItemId;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetHeldItem(string itemId)
    {
        isHoldingItem = true;
        heldItemId = itemId;
    }

    public void ClearHeldItem()
    {
        isHoldingItem = false;
        heldItemId = null;
    }
}
