using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAbility : BallController
{
    private float speedMultiply=1;
    [SerializeField]
    private float speedRate=0.0001f;
    Vector3 lastVelocity;

    
    private void IncreaseSpeed(float speedMul)
    {
        speedMultiply += speedMul;
    }

    protected override void DoAbilityOnUpdate()
    {
        lastVelocity = rb.velocity;
        IncreaseSpeed(Time.deltaTime*speedRate);
    }

    protected override void DoAbilityOnCollisionEnter(Collision2D collision)
    {
        base.DoAbilityOnCollisionEnter(collision);
        Vector2 normal = collision.contacts[0].normal;
        var direction = Vector3.Reflect(lastVelocity.normalized, normal);
        var maxVelocity = 450;
        var velo = direction*Mathf.Min(moveSpeed*speedMultiply,maxVelocity);
        rb.AddForce(velo);
    }
}
