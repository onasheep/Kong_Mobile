using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleObject : MonoBehaviour
{
    private void Awake()
    {
        var _system = GetComponent<ParticleSystem>().main;
        _system.stopAction = ParticleSystemStopAction.Callback;
    }
    private void OnParticleSystemStopped()
    {
        PoolManager.Instance.ReturnToPool(this.gameObject);
    }
}
