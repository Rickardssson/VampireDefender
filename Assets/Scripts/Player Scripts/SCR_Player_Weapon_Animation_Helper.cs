using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SCR_Player_Weapon_Animation_Helper : MonoBehaviour
{
    public UnityEvent OnAnimationEventTriggered, OnAttackPerformed;

    public void TriggerAnimationEvent()
    {
        OnAnimationEventTriggered?.Invoke();
    }

    public void TriggerAttack()
    {
        OnAttackPerformed?.Invoke();
    }
}
