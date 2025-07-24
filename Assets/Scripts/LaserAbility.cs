using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))] 
public class LaserAbility : BallController
{
    private LineRenderer lineRenderer;
    [SerializeField]
    private Transform laserPoint;
    [SerializeField]
    private LayerMask ground;
    [SerializeField]
    private float interval = 1;
    private float timeCounter = 1;
    [SerializeField] private AnimationCurve intervalCurve = AnimationCurve.EaseInOut(0, 1f, 1, 0.1f);
    [SerializeField] private float totalDecreaseTime = 10f;
    private float elapsedTime = 0f;
    protected override void Start()
    {
        base.Start();
        lineRenderer = GetComponent<LineRenderer>();
    }

    protected override void DoAbilityOnUpdate()
    {
        base.DoAbilityOnUpdate();
        FireLaser();
    }

    public void Damageable(Collider2D collider2D,Vector2 point)
    {
        var collider = collider2D.GetComponent<BoxCollider>();
        if (collider)
        {
            collider.TakeDamage(damage);
            ballSound.PlaySoundEffect();
            if (ballParticleEffector)
                ballParticleEffector.PlayParticle(point);
        }
    }
    private void FireLaser()
    {
        transform.Rotate(new Vector3(0,0,90) * Time.deltaTime);
        var hit = Physics2D.Raycast(laserPoint.position,transform.right, 10,ground);
        timeCounter += Time.deltaTime;
        if (hit)
        {
            lineRenderer.SetPosition(0,laserPoint.position);
            lineRenderer.SetPosition(1,hit.point);
            if (timeCounter >= interval)
            {
                Damageable(hit.collider, hit.point);
                timeCounter = 0;
            }
        }
    }
    private void ReduceInterval()
    {
        elapsedTime += 0.2f;
        float t = Mathf.Clamp01(elapsedTime / totalDecreaseTime);
        interval = intervalCurve.Evaluate(t);
    }
    protected override void DoAbilityOnCollisionEnter(Collision2D collision)
    {
        ReduceInterval();
    }
}
