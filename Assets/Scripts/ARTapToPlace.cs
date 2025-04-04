using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;

public class ARTapTo : MonoBehaviour
{
    public GameObject objectToPlace;
    private ARRaycastManager raycastManager;
    private Vector2 touchPosition;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField] LineRenderer lineRenderer;

    GameObject selectedObject = null;
    private bool isDragging = false;


    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        if (raycastManager == null)
        {
            Debug.LogError("ARRaycastManager not found! Make sure you have an ARRaycastManager in your scene.");
        }

        // Setup LineRenderer
      //  lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.positionCount = 2; // Start and end point

    }

    void Update()
    {
        // Debug.Log("Update called.");
        // Debug.Log($"Touch detected at position: {touch.position}");


        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                TrySelectObject(touchPosition);
            }

            if (isDragging && touch.phase == TouchPhase.Moved)
            {
                MoveSelectedObject(touchPosition);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
                selectedObject = null;
            }
        }
    }

    void TrySelectObject(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.CompareTag("Movable"))
            {
                selectedObject = hit.collider.gameObject;
                isDragging = true;
                Debug.Log("Selected object for moving: " + selectedObject.name);
            }
        }
    }

     void MoveSelectedObject(Vector2 position)
     {
         if (raycastManager.Raycast(position, hits, TrackableType.Planes))
         {
             Pose hitPose = hits[0].pose;
             selectedObject.transform.position = hitPose.position;
             Debug.Log("Moved object to: " + hitPose.position);
         }
     }


  /*  void MoveSelectedObject(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Move the object to the raycast hit point (which includes depth/z movement)
            selectedObject.transform.position = hit.point;
            Debug.Log("Moved object to: " + hit.point);
        }
    }*/
    void DrawRay(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
