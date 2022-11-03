using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateEngine : MonoBehaviour
{
    public enum State
    {
        Play,
        Pause,
        Cinematic,
        GameOver
    };
    private State _state;
    public State state
    {
        get { return _state; }
        set { if (_state == value)
                return;
            State prev = _state;
            _state = value;
            StateChanged(prev);
        }
    }
    
    private void Start()
    {
        state = State.Cinematic;
    }
    private void StateChanged(State previous)
    {
        Debug.Log("game state = " + state.ToString());
        if (state == State.Play)
        {
            
        }
        else if (state == State.Pause)
        {

        }
        else if (state == State.GameOver)
        {

        }
        else if (state == State.Cinematic)
        {

        }
    }
}
