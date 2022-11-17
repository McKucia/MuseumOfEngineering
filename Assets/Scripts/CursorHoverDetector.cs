using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CursorHoverDetector : MonoBehaviour
{
    [SerializeField] private LayerMask rayLayerMask;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float rayLength;
    
	private GameObject hitedObject;
    private IInteractable interactable;
    private bool isInteracting = false;


    void LateUpdate()
    {
        if(isInteracting)
        {
            if(Input.GetKeyDown("e"))
            {
                interactable.Interact();
                isInteracting = false;
            }
            else return;
        }

        RaycastHit hit;
        bool hited = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayLength, rayLayerMask);

		// if there's no interactable object in front of us
		// or if we point or another interactable object
        if(!hited)
        {
            ResetPrompt();
            return;
        }
		
        if(hitedObject && hitedObject != hit.collider.gameObject)
            ResetPrompt();

        hitedObject = hit.collider.gameObject;
        interactable = hitedObject.GetComponent<IInteractable>();

		// object must implement IInteractable interface 
        if(interactable != null)
        {
            interactable.SetHover(true);
            if(Input.GetKeyDown("e"))
            {
                interactable.Interact();
                isInteracting = true;
            }
        }
		else
		{
			ResetPrompt();
		}
    }

    void ResetPrompt()
    {
        if(interactable != null) 
		{
			interactable.SetHover(false);
            interactable.Reset();
			hitedObject = null;
			interactable = null;
		}
    }
}
