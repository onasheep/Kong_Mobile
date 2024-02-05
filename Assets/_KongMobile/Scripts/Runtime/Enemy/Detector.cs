using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private bool isEnter = default;
    public bool IsEnter { get { return isEnter; } }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer(Define.LAYER_PLAYER))
        {
            isEnter = true;
        }
    }


}
