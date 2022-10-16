using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainPerformer : MonoBehaviour, IPerformer
{
    [SerializeField] private float trainSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private Transform teleport;

    private ParticleSystem smokeParticleSystem;
    private Animator animator;
    private bool isRiding = false;
    private int velocityHash;

    private void Start()
    {
        animator = GetComponent<Animator>();
        smokeParticleSystem = transform.Find("Smoke").GetComponent<ParticleSystem>();

        velocityHash = Animator.StringToHash("trainVelocity");
    }

    public void LateUpdate()
    {
        if (isRiding)
        {
            Move();
        }
    }

    public void Perform()
    {
        isRiding = !isRiding;
        animator.SetBool("isRiding", isRiding);

        if(isRiding) smokeParticleSystem.Play();
        else smokeParticleSystem.Stop();

        ParticleSystem.EmissionModule emisionModule = smokeParticleSystem.emission;
        emisionModule.enabled = isRiding;
    }

    private void Move()
    {
        if(trainSpeed < maxSpeed) trainSpeed += acceleration * Time.deltaTime;

        transform.Translate(Vector3.forward * trainSpeed * Time.deltaTime);
        animator.SetFloat(velocityHash, trainSpeed);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TrainTeleport")
            StartCoroutine( Teleport() );
    }

    private IEnumerator Teleport()
    {
        isRiding = false;
        transform.position = new Vector3(teleport.position.x, transform.position.y, transform.position.z);
        yield return new WaitForSeconds(5);
        isRiding = true;
    }
}
