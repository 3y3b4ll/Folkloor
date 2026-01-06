using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class PlayerPickup : MonoBehaviour
{
    [Header("Pickup")]
    public KeyCode pickupKey = KeyCode.E;

    [Header("UI")]
    public TMP_Text pickupPrompt;

    [System.Serializable]
    public class BodyPartAttachRule
    {
        public string slotKey;
        public List<string> allowedTags;
        public Vector3 localPosition;
        public Vector3 localRotation;
    }

    public List<BodyPartAttachRule> attachRules = new List<BodyPartAttachRule>();

    private Dictionary<string, BodyPartAttachRule> tagToRule;
    private Dictionary<string, GameObject> slotToObject;

    private GameObject nearbyPickup;
    private BodyPartAttachRule nearbyRule;


    class OriginalTransform
    {
        public Quaternion rotation;
        public float yPosition;
    }

    private Dictionary<GameObject, OriginalTransform> originalTransforms =
        new Dictionary<GameObject, OriginalTransform>();


    void Start()
    {

        tagToRule = new Dictionary<string, BodyPartAttachRule>();
        slotToObject = new Dictionary<string, GameObject>();

        foreach (var rule in attachRules)
        {
            foreach (var tag in rule.allowedTags)
            {
                if (!tagToRule.ContainsKey(tag))
                    tagToRule.Add(tag, rule);
            }
        }

        if (pickupPrompt)
            pickupPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        if (nearbyPickup && Input.GetKeyDown(pickupKey))
        {
            HandlePickupOrSwitch();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsValidPickup(other.gameObject, out BodyPartAttachRule rule))
            return;

        nearbyPickup = other.gameObject;
        nearbyRule = rule;

        UpdatePrompt();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == nearbyPickup)
            ClearNearby();
    }

    void HandlePickupOrSwitch()
    {
        string slot = nearbyRule.slotKey;

        // Slot already has an object - > switch
        if (slotToObject.TryGetValue(slot, out GameObject oldObj))
        {
            DropObject(oldObj);
        }

        AttachObject(nearbyPickup, nearbyRule);
        ClearNearby();
    }

    void AttachObject(GameObject obj, BodyPartAttachRule rule)
    {
        // Store original transform if not already stored
        if (!originalTransforms.ContainsKey(obj))
        {
            originalTransforms[obj] = new OriginalTransform
            {
                rotation = obj.transform.rotation,
                yPosition = obj.transform.position.y
            };
        }

        obj.transform.SetParent(transform);
        obj.transform.localPosition = rule.localPosition;
        obj.transform.localRotation = Quaternion.Euler(rule.localRotation);

        slotToObject[rule.slotKey] = obj;

        DisablePhysics(obj);
    }


    void DropObject(GameObject obj)
    {
        obj.transform.SetParent(null);

        Vector3 pos = transform.position + transform.forward * 0.5f;

        // Restore original Z
        if (originalTransforms.TryGetValue(obj, out OriginalTransform data))
        {
            pos.y = data.yPosition;
            obj.transform.rotation = data.rotation;
        }

        obj.transform.position = pos;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }

        Collider col = obj.GetComponent<Collider>();
        if (col)
            col.enabled = true;
    }


    void DisablePhysics(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        Collider col = obj.GetComponent<Collider>();
        if (col)
            col.enabled = false;
    }

    bool IsValidPickup(GameObject obj, out BodyPartAttachRule rule)
    {
        return tagToRule.TryGetValue(obj.tag, out rule);
    }

    void UpdatePrompt()
    {
        if (!pickupPrompt) return;

        string slot = nearbyRule.slotKey;

        pickupPrompt.text = slotToObject.ContainsKey(slot)
            ? "Press [E] to switch"
            : "Press [E] to pick up";

        pickupPrompt.gameObject.SetActive(true);
    }

    void ClearNearby()
    {
        nearbyPickup = null;
        nearbyRule = null;

        if (pickupPrompt)
            pickupPrompt.gameObject.SetActive(false);
    }
}

