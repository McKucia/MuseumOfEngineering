using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IInteractable
{
    public string interactionPrompt { get; }
    public Transform handTarget { get; }
    public Sprite interactionSprite { get; }
    
    public bool Interact();
}
