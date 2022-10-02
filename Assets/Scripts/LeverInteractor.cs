using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverInteractor : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;
    [SerializeField] private Transform target;

    private Animator animator;
    private bool isTrigger = false;
    public string interactionPrompt => prompt;
    public Transform handTarget => target;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool Interact(IntersectDetector interactor)
    {
        isTrigger = !isTrigger;
        StartCoroutine(DelayedAnimation());
        
        return true;
    }
    
    IEnumerator DelayedAnimation()
    {
        yield return new WaitForSeconds(0.8f);
        animator.SetBool("isTrigger", isTrigger);
    }
}
