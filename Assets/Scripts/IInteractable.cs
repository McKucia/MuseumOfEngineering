using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IInteractable
{
    public string interactionPrompt { get; }
    public Sprite interactionSprite { get; }
    public Transform handTarget { get; }
    
    public void SetHover(bool isHover);
    public void Interact();
    public void Reset();
}
