using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectDetector : MonoBehaviour
{
    [SerializeField] private Transform intersectionPoint;
    [SerializeField] private float sizeX, sizeY, sizeZ;
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
        intersectedObjects = Physics.OverlapBox(intersectionPoint.position, new Vector3(sizeX, sizeY, sizeZ), 
            Quaternion.identity, intersectionLayerMask);

        numFound = intersectedObjects.Length;

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
    //     Gizmos.DrawCube(intersectionPoint.position, new Vector3(sizeX, sizeY, sizeZ));
    // }
}
