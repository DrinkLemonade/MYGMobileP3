using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
        i = this;
        ChangeState(GameState.Start);
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

                //TODO do this in a smarter way
                List<WordToggle> buttons = new();
                buttons = FindObjectsOfType<WordToggle>().ToList();
                List<string> words = currentSession.GetWords();

                int i = 0;
                foreach (var b in buttons)
                {
                    b.SetWord(words[i]);
                    i++;
                }
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
