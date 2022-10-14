using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectDetector : MonoBehaviour
{
    [SerializeField] private Transform intersectionPoint;
    [SerializeField] private float sizeX, sizeY, sizeZ;
    [SerializeField] private LayerMask intersectionLayerMask;
    
    private InteractionPrompt interactionPrompt;
    private Collider[] intersectedObjects;
    private int numFound = 0;
    private IInteractable interactable;

    void Start()
    {
        intersectedObjects = new Collider[3];
    }

    void Update()
    {
        intersectedObjects = Physics.OverlapBox(intersectionPoint.position, new Vector3(sizeX / 2, sizeY / 2, sizeZ / 2), 
            Quaternion.identity, intersectionLayerMask);

        numFound = intersectedObjects.Length;

        if(numFound > 0)
        {
            interactable = GetComponent<IInteractable>();

            if(interactable != null)
            {
                interactionPrompt = transform.GetChild(0).GetComponent<InteractionPrompt>();
                if(!interactionPrompt.isDisplayed) interactionPrompt.SetUp(interactable.interactionPrompt);
                if(Input.GetKeyDown("e"))
                {
                    interactable.Interact(this);
                }
            }
        }
        else
        {
            if(interactable != null) interactable = null;
            if(interactionPrompt && interactionPrompt.isDisplayed) interactionPrompt.Close();
        }
    }

    // void OnDrawGizmos() 
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawCube(intersectionPoint.position, new Vector3(sizeX, sizeY, sizeZ));
    // }
}
