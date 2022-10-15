using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainPerformer : MonoBehaviour, IPerformer
{
    private Animator animator;
    private bool isRiding = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Perform()
    {
        isRiding = !isRiding;
        animator.SetBool("isRiding", isRiding);
    }
}
