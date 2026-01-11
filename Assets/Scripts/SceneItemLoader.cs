using UnityEngine;

public class SceneItemLoader : MonoBehaviour
{
    void Start()
    {
        if (WorldStateManager.Instance == null) return;

        // If the player is holding an item, do NOT touch it
        if (WorldStateManager.Instance.isHoldingItem)
        {
            GameObject heldItem = GameObject.Find(WorldStateManager.Instance.heldItemId);
            if (heldItem != null)
                return;
        }

        // Otherwise, do nothing for now
        // (future logic like respawning or save loading goes here)
    }
}
