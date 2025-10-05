using UnityEngine;

public class FootstepsAudio : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    
    private void OnTriggerEnter(Collider other) // use OnTriggerEnter2D for 2D physics
    {
        // play sound
        if (audioSource) audioSource.Play();
    }
}
