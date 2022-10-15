using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInteractor : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject signCamera;
    [SerializeField] private PlayerController playerController;

    private bool isTrigger = false;
    public Transform handTarget { get; }
    public string interactionPrompt => prompt;

    public bool Interact()
    {
        playerController.canMove = isTrigger;
        isTrigger = !isTrigger;
        
		playerCamera.SetActive(!isTrigger);
        signCamera.SetActive(isTrigger);
        
        return true;
    }
}
