using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class StackRing_Game : MonoBehaviour
{
    [SerializeField] ARRaycastManager raycastManager;
    private Vector2 touchPosition;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject selectedRing = null;
    [SerializeField] BoxCollider boxCollider;
    private bool isDragging = false;


    [SerializeField]Material[] normalMaterials;
    [SerializeField]Material[] outlineMaterials;
    [SerializeField] GameObject fadeInUI;
    [SerializeField] TextMeshProUGUI uiText;
    [SerializeField] Button restartBtn;
    [SerializeField] Transform setPoint1;
    [SerializeField] Transform setPoint2;
    [SerializeField] Transform setPoint3;
    [SerializeField] Transform setPoint4;
    public Transform poleTop; // Reference to the top of the pole
    private float ringSpacing = 0.1f; // Space between stacked rings
    private List<GameObject> stackedRings = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        restartBtn.gameObject.SetActive(false);
        uiText.text = "Stack the rings from largest to smallest!";
        //  restartBtn.onClick.AddListener(() => Debug.Log("Button Clicked!"));
    }

    public void RestartBtn()
    {
        fadeInUI.SetActive(true);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                TrySelectRing(touchPosition);
            }

            if (isDragging && touch.phase == TouchPhase.Moved)
            {
                MoveSelectedRing(touchPosition);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                PlaceRing();
                

               // selectedRing = null; // Reset selected ring

            }
        }

       
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ring1") || other.CompareTag("Ring2") || other.CompareTag("Ring3") || other.CompareTag("Ring4"))
        {
            /* Debug.Log("Ring collided with SetPoint!");
             other.transform.SetParent(setPoint1);
             other.transform.position = setPoint1.transform.position;*/
            if (setPoint1.childCount == 0) // If setPoint1 has no rings
            {
                other.transform.SetParent(setPoint1);
                other.transform.position = setPoint1.position;
                other.transform.gameObject.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
                Debug.Log($"{other.name} placed at SetPoint1");
            }
            else if (setPoint2.childCount == 0) // If setPoint1 is occupied, use setPoint2
            {
                other.transform.SetParent(setPoint2);
                other.transform.position = setPoint2.position;
                other.transform.gameObject.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
                Debug.Log($"{other.name} placed at SetPoint2");
            }
            else if (setPoint3.childCount == 0) // If setPoint1 is occupied, use setPoint2
            {
                other.transform.SetParent(setPoint3);
                other.transform.position = setPoint3.position;
                other.transform.gameObject.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
                Debug.Log($"{other.name} placed at SetPoint2");
            }
            else if (setPoint4.childCount == 0) // If setPoint1 is occupied, use setPoint2
            {
                other.transform.SetParent(setPoint4);
                other.transform.position = setPoint4.position;
                other.transform.gameObject.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
                Debug.Log($"{other.name} placed at SetPoint2");
            }
            else
            {
                Debug.Log("No available set points!");
            }

            isDragging = false; // Stop dragging when the ring is placed
            // Set the ring as a child of the box collider
            Debug.Log("Ring entered trigger: " + other.name);

            removeMaterials();
            CheckWinConditions();
            // Handle ring entering the trigger area if needed
        }
    }

   void removeMaterials()
    {
        MeshRenderer outLineMaterial = selectedRing.transform.gameObject.GetComponent<MeshRenderer>();
        if (outLineMaterial != null)
        {
            if (selectedRing.CompareTag("Ring1"))
            {
                outLineMaterial.material = normalMaterials[0];
            }
            else if (selectedRing.CompareTag("Ring2"))
            {
                outLineMaterial.material = normalMaterials[1];
            }
            else if (selectedRing.CompareTag("Ring3"))
            {
                outLineMaterial.material = normalMaterials[2];
            }
            else if (selectedRing.CompareTag("Ring4"))
            {
                outLineMaterial.material = normalMaterials[3];
            }
        }
    }
    

    void CheckWinConditions()
    {
        bool set1 = setPoint1.childCount > 0 && setPoint1.GetChild(0).CompareTag("Ring1");
        bool set2 = setPoint2.childCount > 0 && setPoint2.GetChild(0).CompareTag("Ring2");
        bool set3 = setPoint3.childCount > 0 && setPoint3.GetChild(0).CompareTag("Ring3");
        bool set4 = setPoint4.childCount > 0 && setPoint4.GetChild(0).CompareTag("Ring4");

       
        if (set1 && set2 && set3 && set4)
        {
            Debug.Log("All rings are in the correct order!");
            uiText.text = " Game Won ";
            restartBtn.gameObject.SetActive(true);
            StartCoroutine(CheckPoints());
        }
      
        else if (setPoint1.childCount == 1 &&
                 setPoint2.childCount == 1 &&
                 setPoint3.childCount == 1 &&
                 setPoint4.childCount == 1)
        {
            Debug.Log("All rings are in the wrong order!");
            uiText.text = " Game Lost ";
            restartBtn.gameObject.SetActive(true);
            StartCoroutine(CheckPoints());
        }
        else
        {
            StartCoroutine(CheckPoints());
        }
    }

    IEnumerator CheckPoints()
    {

        bool correct = true;

        if (setPoint1.childCount > 0 && !setPoint1.GetChild(0).CompareTag("Ring1"))
            correct = false;
        if (setPoint2.childCount > 0 && !setPoint2.GetChild(0).CompareTag("Ring2"))
            correct = false;
        if (setPoint3.childCount > 0 && !setPoint3.GetChild(0).CompareTag("Ring3"))
            correct = false;
        if (setPoint4.childCount > 0 && !setPoint4.GetChild(0).CompareTag("Ring4"))
            correct = false;

        if (correct)
        {
            uiText.text = "Correct order! 🎉";
            yield return new WaitForSeconds(2f);
            uiText.text = "Great job!";
        }
        else
        {
            uiText.text = "Wrong order!";
            yield return new WaitForSeconds(3f);
            uiText.text = "Stack the rings from largest to smallest!";
        }
    }

    void TrySelectRing(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.CompareTag("Ring1") || hit.collider.gameObject.CompareTag("Ring2") ||
                hit.collider.gameObject.CompareTag("Ring3") || hit.collider.gameObject.CompareTag("Ring4"))
            {
                selectedRing = hit.collider.gameObject;
                isDragging = true;

                MeshRenderer outLineMaterial = hit.transform.gameObject.GetComponent<MeshRenderer>();
                if (outLineMaterial != null)
                {
                    if (selectedRing.CompareTag("Ring1"))
                    {
                      outLineMaterial.material = outlineMaterials[0];
                    }
                    else if (selectedRing.CompareTag("Ring2"))
                    {
                        outLineMaterial.material = outlineMaterials[1];
                    }
                    else if (selectedRing.CompareTag("Ring3"))
                    {
                        outLineMaterial.material = outlineMaterials[2];
                    }
                    else if (selectedRing.CompareTag("Ring4"))
                    {
                        outLineMaterial.material = outlineMaterials[3];
                    }
                }
                Debug.Log("Picked up: " + selectedRing.name);
            }
        }
    }
    

    void MoveSelectedRing(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {

            float distance = Vector3.Distance(Camera.main.transform.position, hit.point);
            
            if (distance < 3f)
            {
                // Move the position further along the ray direction
                selectedRing.transform.position = Camera.main.transform.position + ray.direction * 5f;
            }
            else
            {
                selectedRing.transform.position = hit.point;
            }
           // selectedRing.transform.position = hit.point;
        }
    }

    void PlaceRing()
    {
        if (selectedRing == null) return;

        float ringSize = selectedRing.transform.localScale.x; // Assume x-scale represents size

        // Check if ring is dropped near the pole
        float distanceToPole = Vector3.Distance(selectedRing.transform.position, poleTop.position);
        if (distanceToPole < 0.2f)
        {
            // Ensure correct stacking order (largest at bottom)
            if (stackedRings.Count == 0 || ringSize < stackedRings[stackedRings.Count - 1].transform.localScale.x)
            {
                Vector3 newPosition = poleTop.position + Vector3.up * (stackedRings.Count * ringSpacing);
                selectedRing.transform.position = newPosition;
                stackedRings.Add(selectedRing);
                Debug.Log("Ring placed: " + selectedRing.name);

                if (stackedRings.Count == 4) // All rings placed
                {
                    Debug.Log("🎉 You stacked all rings correctly! 🎉");
                }
            }
            else
            {
                Debug.Log("❌ Cannot place this ring on a smaller ring!");
            }
        }
        else
        {
            Debug.Log("⚠ Drop the ring closer to the pole!");
        }

        isDragging = false;
        selectedRing = null;
    }
}
