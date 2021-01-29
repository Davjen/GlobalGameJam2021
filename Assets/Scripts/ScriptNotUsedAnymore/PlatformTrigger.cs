using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        InputWithRB player;
        other.TryGetComponent<InputWithRB>(out player);

        if (player != null)
        {
            player.floating = false;
            player.platformRot = Quaternion.LookRotation(other.transform.forward);

        }

    }

    private void OnTriggerExit(Collider other)
    {
        InputWithRB player;
        other.TryGetComponent<InputWithRB>(out player);
        if (player != null)
            player.floating = true;

    }
}
