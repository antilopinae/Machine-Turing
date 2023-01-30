using System;
using UnityEngine;

public class OnAnimationOver : MonoBehaviour
{
    public static Action<States> AnimationOver;
    public void RenameOver()
    {
        AnimationOver?.Invoke(States.cell_rename);
    }
    public void StateOver()
    {
        AnimationOver?.Invoke(States.cell_state);
    }
    public void InstantOver()
    {
        AnimationOver?.Invoke(States.cell_instant);
    }
}
