using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPullTheLever : MonoBehaviour
{
    [SerializeField] private Transform handIKTarget;
    [SerializeField] private Transform handTarget;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject leverCamera;
    
    private Animator animator;
    private bool isPulling = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(isPulling)
        {
            handIKTarget.position = handTarget.position;
            handIKTarget.rotation = handTarget.rotation;
        }
    }

    public void Pull()
    {
        animator.CrossFade("PullTheLever", 0.2f);
        isPulling = true;
    }

    public void StopPulling()
    {
        isPulling = false;
        GetComponent<PlayerController>().canMove = true;

        leverCamera.SetActive(false);
        playerCamera.SetActive(true);
    }
}
