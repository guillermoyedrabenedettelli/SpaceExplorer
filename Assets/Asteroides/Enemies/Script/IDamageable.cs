using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDamageable
{
    void NotifyHit(float damage);
    UnityEvent GetDeathEvent();
}