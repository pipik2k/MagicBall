using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxCollider : MonoBehaviour
{
    [SerializeField]
    private int hitPoints = 100;
    [SerializeField]
    private Text hitpointText;

    private void Start()
    {
        UpdateHitPointTxt();
    }

    public void TakeDamage(int damage)
    {
        if (hitPoints > 0)
        {
            hitPoints -= damage;
            UpdateHitPointTxt();
        }
        else
            Destroy(gameObject);
    }

    private void UpdateHitPointTxt()
    {
        hitpointText.text = hitPoints.ToString();
    }
}
