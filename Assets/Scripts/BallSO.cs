using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ball Data", menuName = "Game/Ball Data")]
public class BallSO : ScriptableObject
{
    public string ballName;
    public GameObject mainPrefab;
    public Color ballColor;

    public GameObject InitBall(Vector3 position,Quaternion quaternion)
    {
        mainPrefab.GetComponent<BallController>().ballSO = this;
        return Instantiate(mainPrefab, position, quaternion);
    }
}
