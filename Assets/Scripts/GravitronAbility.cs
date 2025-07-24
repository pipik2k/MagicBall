using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitronAbility : BallController
{
    public float baseJumpForce = 8f;
    public float gravityScaleStart = 1f;
    public float gravityIncreasePerBounce = 0.5f;
    private float currentGravityScale;
    private float timer;
    public float jumpHeight = 3;
    public AnimationCurve animationCurve;
    protected override void Start()
    {
        base.Start();
        currentGravityScale = gravityScaleStart;
        rb.gravityScale = currentGravityScale;
    }

    private void GravityLogic(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        if (Mathf.Abs(normal.x) > .9f)
        {
            var xspeed = directionX * moveSpeed;
            var velo = new Vector2(xspeed, rb.velocity.y);
            ChangeVelocity(velo); 
        }
        if (normal.y > 0.9f)
        {
            currentGravityScale += gravityIncreasePerBounce;
            rb.gravityScale = currentGravityScale;
            var jumpHeightRate = 0.1f * animationCurve.Evaluate(timer);
            var smallestJumpHeight = 0.4f;
            jumpHeight -= jumpHeightRate;
            jumpHeight = Mathf.Max(jumpHeight, smallestJumpHeight);
            timer += Time.deltaTime;
            float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
            float requiredJumpSpeed = Mathf.Sqrt(2 * gravity * jumpHeight);
            var xspeed = directionX * moveSpeed;
            var velo = new Vector2(xspeed, requiredJumpSpeed);
            ChangeVelocity(velo);
        }
    }


    protected override void DoAbilityOnCollisionStay(Collision2D collision)
    {
        if (rb.velocity.y < 0.1f)
        {
            GravityLogic(collision);
            Damageable(collision);
        }
    }

    protected override void DoAbilityOnCollisionEnter(Collision2D collision)
    {
        base.DoAbilityOnCollisionEnter(collision);
        GravityLogic(collision);
    }

}
