using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionInteractor : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject canvas;

    public Transform handTarget { get; }
    public string interactionPrompt => prompt;
    public Sprite interactionSprite => sprite;
    private bool isDisplayed = false;

    public bool Interact()
    {
        isDisplayed = !isDisplayed;
        canvas.SetActive(isDisplayed);
        return true;
    }
}
