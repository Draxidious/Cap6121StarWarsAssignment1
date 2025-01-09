using UnityEngine;

public class ToggleLightsaber : MonoBehaviour
{
    public float speed = 0.5f; // Speed at which the lightsaber shrinks or grows (units per second)
    public bool isGrowing = false; // Toggle between growing and shrinking

    private Vector3 initialScale;

    void Start()
    {
        // Store the initial scale of the lightsaber
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (isGrowing)
        {
            GrowLightsaber();
        }
        else
        {
            ShrinkLightsaber();
        }
    }

    void ShrinkLightsaber()
    {
        // Check if the lightsaber's Y scale is greater than zero
        if (transform.localScale.y > 0)
        {
            // Calculate the amount to shrink this frame
            float shrinkAmount = speed * Time.deltaTime;

            // Reduce the Y scale, but ensure it doesn't go below zero
            float newYScale = Mathf.Max(0, transform.localScale.y - shrinkAmount);

            // Calculate the amount the lightsaber needs to move down
            float moveAmount = (transform.localScale.y - newYScale) / 2;

            // Apply the new scale
            transform.localScale = new Vector3(transform.localScale.x, newYScale, transform.localScale.z);

            // Move the lightsaber down to keep the bottom stationary
            transform.position -= new Vector3(0, moveAmount, 0);
        }
    }

    void GrowLightsaber()
    {
        // Check if the lightsaber's Y scale is less than the initial scale
        if (transform.localScale.y < initialScale.y)
        {
            // Calculate the amount to grow this frame
            float growAmount = speed * Time.deltaTime;

            // Increase the Y scale, but ensure it doesn't exceed the initial scale
            float newYScale = Mathf.Min(initialScale.y, transform.localScale.y + growAmount);

            // Calculate the amount the lightsaber needs to move up
            float moveAmount = (newYScale - transform.localScale.y) / 2;

            // Apply the new scale
            transform.localScale = new Vector3(transform.localScale.x, newYScale, transform.localScale.z);

            // Move the lightsaber up to keep the bottom stationary
            transform.position += new Vector3(0, moveAmount, 0);
        }
    }
}
