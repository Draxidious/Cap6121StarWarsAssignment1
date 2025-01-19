using UnityEngine;

public class PlayParticleOnEvent : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public AudioSource sound;

    // Function to play the particle system
    public void PlayOnce()
    {
        if (particleSystem == null)
        {
            Debug.LogWarning("Particle System is not assigned.");
            return;
        }

        // Restart the particle system to ensure it plays from the beginning
        particleSystem.Stop();
        particleSystem.Play();
        sound.Play();

    }
}
