using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeverInteractor : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject leverCamera;
    [SerializeField] private GameObject performerObject;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerPullTheLever playerPullTheLever;

    private Animator animator;
    private bool isTrigger = false;
    public string interactionPrompt => prompt;
    public Transform handTarget => target;
    public Sprite interactionSprite => sprite;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetHover(bool isHover)
    {
    }

    public void Interact()
    {
        playerController.canMove = false;
        isTrigger = !isTrigger;

        playerCamera.SetActive(false);
        leverCamera.SetActive(true);

        playerPullTheLever.Pull();
        
        StartCoroutine(DelayedAnimation());
    }

    public void Reset()
    {
    }
    
    IEnumerator DelayedAnimation()
    {
        yield return new WaitForSeconds(0.8f);
        animator.SetBool("isTrigger", isTrigger);

        performerObject.GetComponent<IPerformer>().Perform();
    }
}
