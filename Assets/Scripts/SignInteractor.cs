using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignInteractor : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject signCamera;
    [SerializeField] private PlayerController playerController;

    private bool isTrigger = false;
    public Transform handTarget { get; }
    public string interactionPrompt => prompt;
    public Sprite interactionSprite => sprite;

    public void SetHover(bool isHover)
    {
    }

    public void Interact()
    {
        playerController.canMove = isTrigger;
        isTrigger = !isTrigger;
        
		playerCamera.SetActive(!isTrigger);
        signCamera.SetActive(isTrigger);
    }

    public void Reset()
    {
    }
}
