using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInventory : MonoBehaviour
{
    public Transform holdPoint;
    public float dropY = 0f;

    private GameObject heldItem;
    private readonly List<Item> nearbyItems = new();

    void Start()
    {
        RestoreHeldItem();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem == null)
                PickupClosestItem();
            else
                DropItem();
        }

        if (heldItem != null)
        {
            heldItem.transform.position = holdPoint.position;
            heldItem.transform.rotation = holdPoint.rotation;
        }
    }

    void RestoreHeldItem()
    {
        if (WorldStateManager.Instance == null) return;
        if (!WorldStateManager.Instance.isHoldingItem) return;

        GameObject item = GameObject.Find(WorldStateManager.Instance.heldItemId);
        if (item != null)
        {
            heldItem = item;
        }
    }

    void PickupClosestItem()
    {
        if (nearbyItems.Count == 0) return;

        Item closest = null;
        float minDist = float.MaxValue;

        foreach (Item item in nearbyItems)
        {
            if (item == null) continue;

            float dist = Vector3.Distance(transform.position, item.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = item;
            }
        }

        if (closest == null) return;

        heldItem = closest.gameObject;
        closest.OnPickup();

        heldItem.transform.SetParent(null);
        DontDestroyOnLoad(heldItem);

        WorldStateManager.Instance.SetHeldItem(heldItem.name);
        nearbyItems.Remove(closest);
    }

    void DropItem()
    {
        if (heldItem == null) return;

        string currentScene = SceneManager.GetActiveScene().name;
        Item item = heldItem.GetComponent<Item>();

        Vector3 pos = heldItem.transform.position;
        pos.y = dropY;
        heldItem.transform.position = pos;

        WorldStateManager.Instance.RegisterDroppedItem(
            item.itemId,
            currentScene,
            pos
        );

        // IMPORTANT:
        // Only destroy if we are NOT in the same scene it originated from
        if (item.originalScene != currentScene)
        {
            Destroy(heldItem);
        }

        heldItem = null;
        WorldStateManager.Instance.ClearHeldItem(currentScene);
    }


    private void OnTriggerEnter(Collider other)
    {
        Item item = other.GetComponent<Item>();
        if (item != null && !nearbyItems.Contains(item))
            nearbyItems.Add(item);
    }

    private void OnTriggerExit(Collider other)
    {
        Item item = other.GetComponent<Item>();
        if (item != null)
            nearbyItems.Remove(item);
    }
}
