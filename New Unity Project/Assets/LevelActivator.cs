using UnityEngine;
using System.Collections;

public class LevelActivator : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        LevelManager.instance.RaiseLadder();
    }
}
