using UnityEngine;

public class ForceField : MonoBehaviour
{
    public float pushForce = 10f;       // initial strength
    public float pushDuration = 0.3f;   // how long the push lasts
    [SerializeField] private AudioSource audioSource;  // assign in Inspector

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            // play sound
            if (audioSource) audioSource.Play();

            CharacterController controller = other.GetComponent<CharacterController>();
            if (controller != null)
            {
                // Start push coroutine
                StartCoroutine(PushPlayer(controller, other.transform));
            }
        }
    }

    private System.Collections.IEnumerator PushPlayer(CharacterController controller, Transform playerTransform)
    {
        float timer = 0f;

        // Push direction (from field center outwards)
        Vector3 pushDir = (playerTransform.position - transform.position).normalized;

        while (timer < pushDuration)
        {
            // strength decreases over time
            float strength = Mathf.Lerp(pushForce, 0f, timer / pushDuration);

            controller.Move(pushDir * strength * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }
    }
}
