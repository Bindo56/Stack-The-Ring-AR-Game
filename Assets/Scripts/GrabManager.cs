using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabManager : MonoBehaviour
{

  private GameObject selectedObject;
   [SerializeField] private Camera arCamera;

    void Start()
    {
        arCamera = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch1.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Grabbable"))
                    {
                        selectedObject = hit.collider.gameObject;
                    }
                }
            }

            if (selectedObject != null)
            {
                Vector3 newPos = arCamera.ScreenToWorldPoint(new Vector3(touch1.position.x, touch1.position.y, 1f));
                selectedObject.transform.position = newPos;
            }
        }
        else
        {
            selectedObject = null;
        }
    }
}
