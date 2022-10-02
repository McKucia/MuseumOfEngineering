using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectDetector : MonoBehaviour
{
    [SerializeField] private Transform intersectionPoint;
    [SerializeField] private float intersectionPointRadius;
    [SerializeField] private LayerMask intersectionLayerMask;
    [SerializeField] private InteractionPrompt interactionPrompt;

    private Collider[] intersectedObjects;
    private PlayerPullTheLever playerPullTheLever;
    private int numFound = 0;
    private IInteractable interactable;

    void Start()
    {
        intersectedObjects = new Collider[3];
        playerPullTheLever = GetComponent<PlayerPullTheLever>();
    }

    void Update()
    {
        numFound = Physics.OverlapSphereNonAlloc(intersectionPoint.position, intersectionPointRadius, 
            intersectedObjects, intersectionLayerMask);

        if(numFound > 0)
        {
            interactable = intersectedObjects[0].GetComponent<IInteractable>();

            if(interactable != null)
            {
                if(!interactionPrompt.isDisplayed) interactionPrompt.SetUp(interactable.interactionPrompt);
                if(Input.GetKeyDown("e"))
                {
                    interactable.Interact(this);
                    playerPullTheLever.Pull();
                }
            }
        }
        else
        {
            if(interactable != null) interactable = null;
            if(interactionPrompt.isDisplayed) interactionPrompt.Close();
        }
    }

    // void OnDrawGizmos() 
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(intersectionPoint.position, intersectionPointRadius);
    // }
}
