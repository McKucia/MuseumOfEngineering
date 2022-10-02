using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPullTheLever : MonoBehaviour
{
    [SerializeField] private Transform handIKTarget;
    [SerializeField] private Transform handTarget;

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
            animator.SetTrigger("pullTheLever");
        }
    }

    public void Pull()
    {
        isPulling = true;
    }
}
