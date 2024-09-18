using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlocker : MonoBehaviour
{
    public GameObject objectToPlace; // The GameObject to place above the camera
    public float distanceAboveCamera = 1f; // Distance above the camera to place the object

    private Camera mainCamera;

    void Start()
    {
        // Find the main camera in the scene
        mainCamera = Camera.main;

        // If objectToPlace is not assigned, create a simple cube
        if (objectToPlace == null)
        {
            objectToPlace = GameObject.CreatePrimitive(PrimitiveType.Cube);
            objectToPlace.name = "PlacedObject";
        }
    }

    void Update()
    {
        if (mainCamera != null)
        {
            // Calculate the position above the upper end of the camera
            Vector3 cameraTop = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 1f, mainCamera.nearClipPlane));
            Vector3 targetPosition = cameraTop + mainCamera.transform.up * distanceAboveCamera;

            // Set the position of the objectToPlace
            objectToPlace.transform.position = targetPosition;
        }
    }
}
