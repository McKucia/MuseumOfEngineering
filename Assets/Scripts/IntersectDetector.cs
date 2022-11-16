using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class IntersectDetector : MonoBehaviour
{
    [SerializeField] private LayerMask intersectionLayerMask;
    [SerializeField] private LayerMask rayLayerMask;
    [SerializeField] private Transform intersectionPoint;
    [SerializeField] private float sizeX, sizeY, sizeZ;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float rayLength;
    [SerializeField] private GameObject prompt;
    [SerializeField] private Image promptImage;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private Sprite defaultSprite;
    
    private IInteractable interactable;
    private Image crosshairImage;
    private Collider[] intersectedObjects;
    private int numFound = 0;
    private GameObject hitedObject;

    void Start()
    {
        intersectedObjects = new Collider[3];
    }

    void LateUpdate()
    {
        intersectedObjects = Physics.OverlapBox(intersectionPoint.position, new Vector3(sizeX / 2, sizeY / 2, sizeZ / 2), 
            Quaternion.identity, intersectionLayerMask);

        numFound = intersectedObjects.Length;

        if(numFound <= 0) 
        {
            ResetPrompt();
            return;
        }

        RaycastHit hit;
        bool hited = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayLength, rayLayerMask);
        if(!hited) 
        {
            ResetPrompt();
            return;
        }

        hitedObject = hit.collider.gameObject;

        if(Array.Exists(intersectedObjects, element => element.transform.parent.gameObject == hitedObject)) //hitedObject == intersectedObjects[0].transform.parent.gameObject)
        {
            interactable = hitedObject.GetComponent<IInteractable>();
            if(interactable != null)
            {
                var outline = hitedObject.GetComponent<Outline>();
                outline.enabled = true;
                // prompt.SetActive(true);
                // if(interactable.interactionSprite)
                //     promptImage.sprite = interactable.interactionSprite;
                // else
                //     promptImage.sprite = defaultSprite;
                // promptText.text = interactable.interactionPrompt;
                // if(Input.GetKeyDown("e"))
                // {
                //     interactable.Interact();
                // }
            }
        }
    }

    void ResetPrompt()
    {
        if(hitedObject != null)
        {
            hitedObject.GetComponent<Outline>().enabled = false;
            hitedObject = null;
        }
        // prompt.SetActive(false);
        // promptImage.sprite = null;
        // promptText.text = "";
        if(interactable != null) interactable = null;
    }

    // void OnDrawGizmos() 
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawCube(intersectionPoint.position, new Vector3(sizeX, sizeY, sizeZ));
    // }
}
