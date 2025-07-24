using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallSelectionUI : MonoBehaviour
{
    [Header("Ball Data")]
    public List<BallSO> ballList;

    [Header("Ball 1 UI")]
    public Image ball1Image;
    public Button ball1LeftButton;
    public Button ball1RightButton;
    public Text ball1Text;

    [Header("Ball 2 UI")]
    public Image ball2Image;
    public Button ball2LeftButton;
    public Button ball2RightButton;
    public Text ball2Text;

    [Header("Start")]
    public Button startButton;

    private int ball1Index = 0;
    private int ball2Index = 1;

    void Start()
    {
        UpdateUI();

        ball1LeftButton.onClick.AddListener(() => ChangeBall(ref ball1Index, -1, ball1Image));
        ball1RightButton.onClick.AddListener(() => ChangeBall(ref ball1Index, 1, ball1Image));

        ball2LeftButton.onClick.AddListener(() => ChangeBall(ref ball2Index, -1, ball2Image));
        ball2RightButton.onClick.AddListener(() => ChangeBall(ref ball2Index, 1, ball2Image));

        startButton.onClick.AddListener(OnStart);
    }

    void ChangeBall(ref int index, int direction, Image targetImage)
    {
        int count = ballList.Count;
        index = (index + direction + count) % count;
        targetImage.color = ballList[index].ballColor;
        UpdateUI();
    }

    void UpdateUI()
    {
        ball1Image.color = ballList[ball1Index].ballColor;
        ball1Text.color = ballList[ball1Index].ballColor;
        ball1Text.text = ballList[ball1Index].ballName;
        ball2Image.color = ballList[ball2Index].ballColor;
        ball2Text.color = ballList[ball2Index].ballColor;
        ball2Text.text = ballList[ball2Index].ballName;
    }

    void OnStart()
    {
        BallSelectionHolder.selectedBall1 = ballList[ball1Index];
        BallSelectionHolder.selectedBall2 = ballList[ball2Index];
        this.gameObject.SetActive(false);
        if (GameManager.Instance)
            GameManager.Instance.InitBall();
    }
}
