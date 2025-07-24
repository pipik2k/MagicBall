using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicatorAbility : BallController
{
    [SerializeField]
    DuplicatorAbility duplicatorObj;
    [SerializeField]
    float timetoDuplicate = 10;
    float timeCounter = 0;
    public float randomAngle = 1;
    protected override void Start()
    {
        base.Start();
        LaunchAtAngle(moveSpeed, angle*randomAngle);
    }
    protected override void DoAbilityOnCollisionEnter(Collision2D collision)
    {
        base.DoAbilityOnCollisionEnter(collision);
    }


    protected override void DoAbilityOnUpdate()
    {
        Duplicate();
    }

    private void Duplicate()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter >= timetoDuplicate)
        {
            var duplicator = Instantiate(duplicatorObj, transform.position, Quaternion.identity);
            float randomAngle = Random.Range(0f, 360f);
            duplicator.randomAngle = randomAngle;
            timeCounter = 0;
        }
    }
}
