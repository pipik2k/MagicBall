using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyAbility : BallController
{
    private int StickRate = 3;
    private int stickCounter;
    Vector3 lastVelocity;
    protected override void Start()
    {
        base.Start();
    }
    void StickGround()
    {
        if (stickCounter >= StickRate)
        {
            stickCounter = 0;
            var JumpForce = 5f;
            var velo = new Vector2(moveSpeed*directionX, JumpForce);
            ChangeVelocity(velo);
        }
        stickCounter++;
    }

    void IncreaseStickRate()=>StickRate++;

    protected override void DoAbilityOnUpdate()
    {
        base.DoAbilityOnUpdate();
        lastVelocity = rb.velocity;
    }
    protected override void DoAbilityOnCollisionStay(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        if (normal.y > 0.9f)
        {
            StickGround();
        }
        Damageable(collision);
    }

    protected override void DoAbilityOnCollisionEnter(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        if (normal.y > 0.9f)
        {
            ChangeVelocity(Vector2.zero);
            IncreaseStickRate();
        }
        if (Mathf.Abs(normal.x) > 0.9f)
        {
            var direction = Vector3.Reflect(lastVelocity.normalized, normal);
            var velo = direction * moveSpeed;
            velo.y *= 1.5f;
            ChangeVelocity(velo);
        }
    }
}
