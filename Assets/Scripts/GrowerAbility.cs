using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowerAbility : BallController
{
    [SerializeField]
    private float ScaleRate=0.2f;
    private Vector3 startScale;
    protected override void Start()
    {
        base.Start();
        startScale = transform.localScale;
    }

    private void Grower()
    {
        transform.localScale += (startScale * ScaleRate);
    }
    protected override void DoAbilityOnCollisionEnter(Collision2D collision)
    {
        base.DoAbilityOnCollisionEnter(collision);
        Vector2 normal = collision.contacts[0].normal;
        if(normal.y>0.9f)
        Grower();
    }

    protected override void DoAbilityOnCollisionStay(Collision2D collision)
    {
        if (rb.velocity.y < 0.1f)
        Damageable(collision);
    }

    protected override void DoAbilityOnUpdate()
    {
        base.DoAbilityOnUpdate();
    }

}
