using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    [SerializeField] private float baseIntensity = 30f;   // your normal light brightness
    [SerializeField] private float flickerStrength = 100f; // how much it varies
    [SerializeField] private float flickerSpeed = 1f;     // speed of change

    private Light _light;
    private float _seed;

    void Awake()
    {
        _light = GetComponent<Light>();
        _seed = Random.Range(0f, 100f); // each light gets a unique pattern
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(_seed, Time.time * flickerSpeed);
        _light.intensity = baseIntensity + (noise - 0.5f) * flickerStrength;
    }
}