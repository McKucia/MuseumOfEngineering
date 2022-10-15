using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string interactionPrompt { get; }
    public Transform handTarget { get; }
    
    public bool Interact();
}
