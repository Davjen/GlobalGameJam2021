using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[SerializeField]
public class SoundsEvents : UnityEvent<string> { }
[SerializeField]
public static class EventManagerStatic 
{
    public static SoundsEvents PlaySound;
}
