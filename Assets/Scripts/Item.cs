using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Data")]
    public string itemId;

    [HideInInspector]
    public string originalScene;

    void Awake()
    {
        // Store the scene this item was originally authored in
        originalScene = gameObject.scene.name;
    }

    /// <summary>
    /// Called when the player picks this item up
    /// </summary>
    public void OnPickup()
    {
        if (WorldStateManager.Instance == null)
            return;

        // This item no longer belongs to its original scene
        WorldStateManager.Instance.MarkItemRemovedFromOriginalScene(itemId);

        // Register as currently held
        WorldStateManager.Instance.SetHeldItem(itemId);
    }

    /// <summary>
    /// Called when the player drops this item
    /// </summary>
    public void OnDrop()
    {
        // Nothing special needed here (yet)
        // Drop logic lives in PlayerInventory
    }
}
