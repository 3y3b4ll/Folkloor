using System.Collections.Generic;
using UnityEngine;

public class WorldStateManager : MonoBehaviour
{
    public static WorldStateManager Instance;

    [Header("Player State")]
    public bool isHoldingItem;
    public string heldItemId;

    [System.Serializable]
    public class ItemWorldData
    {
        public string itemId;
        public string sceneName;
        public Vector3 position;
    }

    // itemId -> world data
    public Dictionary<string, ItemWorldData> droppedItems =
        new Dictionary<string, ItemWorldData>();
    public HashSet<string> removedFromOriginalScene = new HashSet<string>();


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /* =========================
     * HELD ITEM
     * ========================= */

    public void SetHeldItem(string itemId)
    {
        isHoldingItem = true;
        heldItemId = itemId;

        // If item was previously dropped somewhere, remove it
        if (droppedItems.ContainsKey(itemId))
            droppedItems.Remove(itemId);
    }

    public void ClearHeldItem(string currentScene)
    {
        isHoldingItem = false;
        heldItemId = string.Empty;
    }

    /* =========================
     * DROPPED ITEMS
     * ========================= */

    public void RegisterDroppedItem(string itemId, string sceneName, Vector3 position)
    {
        droppedItems[itemId] = new ItemWorldData
        {
            itemId = itemId,
            sceneName = sceneName,
            position = position
        };
    }

    public ItemWorldData GetItemInScene(string itemId, string sceneName)
    {
        if (droppedItems.TryGetValue(itemId, out ItemWorldData data))
        {
            if (data.sceneName == sceneName)
                return data;
        }

        return null;
    }

    public List<ItemWorldData> GetAllItemsInScene(string sceneName)
    {
        List<ItemWorldData> result = new List<ItemWorldData>();

        foreach (var kvp in droppedItems)
        {
            if (kvp.Value.sceneName == sceneName)
                result.Add(kvp.Value);
        }

        return result;
    }

    /* =========================
     * DEBUG (OPTIONAL)
     * ========================= */

    public void DebugLogState()
    {
        Debug.Log($"Holding Item: {isHoldingItem} ({heldItemId})");
        foreach (var item in droppedItems.Values)
        {
            Debug.Log(
                $"Item {item.itemId} in {item.sceneName} at {item.position}"
            );
        }
    }
    public void MarkItemRemovedFromOriginalScene(string itemId)
    {
        removedFromOriginalScene.Add(itemId);
    }

    public bool IsRemovedFromOriginalScene(string itemId)
    {
        return removedFromOriginalScene.Contains(itemId);
    }

}
