using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BallParticleEffector))]
public abstract class BallController : MonoBehaviour
{
    protected BallSound ballSound;
    protected BallParticleEffector ballParticleEffector;
    [SerializeField]
    protected Rigidbody2D rb;
    protected int directionX = 1;
    protected int damage = 1;
    [SerializeField]
    protected float moveSpeed = 5;
    [SerializeField]
    protected float angle = 45;
    public BallSO ballSO;
    protected virtual void Start()
    {
        if(!rb)
        rb = GetComponent<Rigidbody2D>();
        if(!ballSound)
        ballSound = GetComponent<BallSound>();
        if (!ballParticleEffector)
            ballParticleEffector = GetComponent<BallParticleEffector>();
        LaunchAtAngle(moveSpeed, angle);
    }

    private void Update()
    {
        DoAbilityOnUpdate();
    }

    public void LaunchAtAngle(float speed, float angleDegrees)
    {
        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        Vector2 launchVelocity = new Vector2(
            speed * Mathf.Cos(angleRadians),
            speed * Mathf.Sin(angleRadians)
        );
        rb.velocity = launchVelocity;
    }

    protected virtual void DoAbilityOnUpdate()
    {

    }

    protected virtual void DoAbilityOnCollisionEnter(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        if (Mathf.Abs(normal.x) > 0.9f)
            ChangeDirection();
        Damageable(collision);
    }

    protected virtual void DoAbilityOnCollisionStay(Collision2D collision)
    {

    }
    public void ChangeVelocity(Vector2 velo)
    {
        rb.velocity = velo;
    }
    public void ChangeDirection()
    {
        directionX *= -1;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            GameManager.Instance.ChangeGameState(GameManager.GameState.End);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        DoAbilityOnCollisionEnter(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        DoAbilityOnCollisionStay(collision);
    }

    public void AddDamage(int damage)
    {
        this.damage += damage;
    }

    public void Damageable(Collision2D collision)
    {
        BoxCollider boxCollider = collision.gameObject.GetComponent<BoxCollider>();
        ContactPoint2D contact = collision.contacts[0];
        Vector3 hitPoint = contact.point;
        if (boxCollider)
        {
            boxCollider.TakeDamage(damage);
            ballSound.PlaySoundEffect();
            if(ballParticleEffector)
            ballParticleEffector.PlayParticle(hitPoint);
        }
    }
}
