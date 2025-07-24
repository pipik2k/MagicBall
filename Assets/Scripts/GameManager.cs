using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public event Action onGamePlay;
    public event Action onGameEnd;
    [SerializeField]
    private Transform ball1Pos, ball2Pos;
    public enum GameState {Play,End}
    private GameState _gameState;

    private GameState gameState
    {
        get => _gameState;
        set
        {
            _gameState = value;

            switch (_gameState)
            {
                case GameState.Play:
                    onGamePlay?.Invoke();
                    Time.timeScale = 1;
                    break;
                case GameState.End:
                    Debug.Log("InVokeeeeeeeeeee");
                    onGameEnd?.Invoke();
                    Time.timeScale = 0;
                    break;
            }
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void ChangeGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public void InitBall()
    {
        ChangeGameState(GameState.Play);
        BallSelectionHolder.selectedBall1.InitBall(ball1Pos.position, Quaternion.identity);
        BallSelectionHolder.selectedBall2.InitBall(ball2Pos.position, Quaternion.identity);
    }
}
