using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkill : MonoBehaviour
{
    public float damage = default;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer(Define.LAYER_PLAYER))
        {
            other.gameObject.GetComponent<PlayerBase>().OnDamage(damage);
        }
    }

    public void GetDamage(float damage_)
    {
        if (damage != default) { return; }
        damage = damage_;
    }
}
