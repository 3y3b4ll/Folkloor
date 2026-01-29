using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneItemLoader : MonoBehaviour
{
    [Tooltip("All item prefabs that can exist in the world")]
    public GameObject[] itemPrefabs;

    void Start()
    {
        if (WorldStateManager.Instance == null) return;

        string currentScene = SceneManager.GetActiveScene().name;

        var itemsToSpawn =
            WorldStateManager.Instance.GetAllItemsInScene(currentScene);

        foreach (var itemData in itemsToSpawn)
        {
            foreach (GameObject prefab in itemPrefabs)
            {
                Item item = prefab.GetComponent<Item>();
                if (item != null && item.itemId == itemData.itemId)
                {
                    Instantiate(
                        prefab,
                        itemData.position,
                        Quaternion.identity
                    );
                    break;
                }
            }
        }
    }
}
