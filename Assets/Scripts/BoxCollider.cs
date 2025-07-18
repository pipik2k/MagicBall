using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollider : MonoBehaviour
{
    [SerializeField]
    private int hitPoints = 100;

    // Call this function to apply damage to the box
    public void TakeDamage()
    {
        var damage = 1;
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
