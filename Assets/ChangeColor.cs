using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    // Reference to the object's Renderer
    private Renderer objectRenderer;

    // Public boolean to toggle between red and blue
    public bool toggle = false;
    public bool isRed = false; 

    void Start()
    {
        // Get the Renderer component attached to the object
        objectRenderer = GetComponent<Renderer>();

        // Ensure the object has a material to modify
        if (objectRenderer == null || objectRenderer.material == null)
        {
            Debug.LogError("No Renderer or Material found on this object!");
        }
    }

    void Update()
    {
        if(toggle)
        {
            toggle = false;
            ToggleColor();
        }
    }

    // Function to toggle the material color
    public void ToggleColor()
    {
        if (objectRenderer != null && objectRenderer.material != null)
        {
            if(objectRenderer.enabled) objectRenderer.enabled = false;
            else objectRenderer.enabled = true;
            
        }
        else
        {
            Debug.LogError("Cannot change color: No Renderer or Material available!");
        }
    }
}
