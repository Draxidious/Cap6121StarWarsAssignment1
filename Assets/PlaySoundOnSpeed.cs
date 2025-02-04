using UnityEngine;

public class PlaySoundOnSpeed : MonoBehaviour
{
    public Transform parentTransform; // Reference to the parent object's Transform
    public float speedThreshold = 5f; // Speed at which the sound will play
    public AudioSource audioSource;

    private bool soundPlayed = false; // To ensure the sound is not repeatedly played
    private Vector3 lastPosition; // To track the object's last position

    void Start()
    {
        // Initialize the last position
        lastPosition = parentTransform.position;
    }

    void Update()
    {
        if (parentTransform == null)
            return;

        // Calculate the translation speed of the parent Transform
        float speed = (parentTransform.position - lastPosition).magnitude / Time.deltaTime;

        // Check if the speed exceeds the threshold
        if (speed >= speedThreshold && !soundPlayed)
        {
            audioSource.Play();
            soundPlayed = true; // Prevent repeated playing
        }
        else if (speed < speedThreshold && soundPlayed && !audioSource.isPlaying)
        {
            soundPlayed = false; // Reset when speed goes below the threshold
        }

        // Update the last position
        lastPosition = parentTransform.position;
    }
}
