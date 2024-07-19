using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager i;
    public GameSession currentSession;
    public GameState State;

    [SerializeField]
    public int LivesInASession = 4;
    private void Awake()
    {
        if (i != null)
            Destroy(i.gameObject);

        i = this;
        DontDestroyOnLoad(this);
        ChangeState(GameState.Start);
    }

    private void Update()
    {
        currentSession.secondsElapsed += Time.deltaTime;
    }
    public enum GameState
    {
        Menu, Start, Ongoing, Finishing, Over
    }

    private void ChangeState(GameState state)
    {
        LeaveState(State);
        State = state;
        EnterState(State);
    }

    private void EnterState(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                break;
            case GameState.Start:
                currentSession = new();
                break;
            case GameState.Ongoing:
                break;
            case GameState.Finishing:
                break;
            case GameState.Over:
                break;
            default:
                break;
        }
    }

    private void LeaveState(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                break;
            case GameState.Start:
                break;
            case GameState.Ongoing:
                break;
            case GameState.Finishing:
                break;
            case GameState.Over:
                break;
            default:
                break;
        }
    }
}
