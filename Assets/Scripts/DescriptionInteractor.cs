using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class DescriptionInteractor : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;
    [SerializeField] private GameObject canvas;

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
        Debug.Log("interakt");
        isDisplayed = !isDisplayed;
        canvas.SetActive(isDisplayed);
    }

    public void Reset()
    {
        isDisplayed = false;
        canvas.SetActive(isDisplayed);
    }
}
