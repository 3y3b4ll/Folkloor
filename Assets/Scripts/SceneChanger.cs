using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // set this in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // make sure player has "Player" tag
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
