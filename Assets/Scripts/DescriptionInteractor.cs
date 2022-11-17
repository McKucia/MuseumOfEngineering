using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class DescriptionInteractor : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject descriptionCamera;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject crosshair;

    public Transform handTarget { get; }
    public Sprite interactionSprite { get; }
    public string interactionPrompt => prompt;

    private bool isDisplayed = false;
    private Outline outline;

    void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public void SetHover(bool isHover)
    {
        outline.enabled = isHover;
    }

    public void Interact()
    {
        playerController.canMove = isDisplayed;
        isDisplayed = !isDisplayed;
        canvas.SetActive(isDisplayed);
        SetCursorActive();
        
		playerCamera.SetActive(!isDisplayed);
        descriptionCamera.SetActive(isDisplayed);
    }

    public void Reset()
    {
        isDisplayed = false;
        canvas.SetActive(isDisplayed);
    }

    private void SetCursorActive()
    {
        crosshair.SetActive(!isDisplayed);
        Cursor.visible = isDisplayed;
        Cursor.lockState = isDisplayed ? CursorLockMode.Confined : CursorLockMode.Locked;
    }
}
