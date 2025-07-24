using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallParticleEffector : MonoBehaviour
{
    BallController ballController;
    Color color;
    private void Start()
    {
        ballController = GetComponent<BallController>();
        color = ballController.ballSO.ballColor;
    }
    public void PlayParticle(Vector2 pos)
    {
        ShareParticleSystemPool.Instance.PlayAt(pos, color);
    }
}
