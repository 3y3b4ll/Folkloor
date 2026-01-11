using UnityEngine;

public class Item : MonoBehaviour
{
    public void OnPickup()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
    }

    public void OnDrop()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = true;
    }
}
