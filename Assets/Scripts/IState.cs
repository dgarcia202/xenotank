using UnityEngine;
using System.Collections;

public interface IState<T> where T : class 
{
    void Enter(T agent);

    void UpdateState(T agent);

    void Exit(T agent);
}
