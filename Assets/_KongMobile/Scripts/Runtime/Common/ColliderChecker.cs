using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderChecker : MonoBehaviour
{
    public float damage = default;



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(Define.LAYER_ENEMY))
        {
            if (other.gameObject.GetComponent<IDamageable>() == null) { return; }

            other.gameObject.GetComponent<EnemyBase>().OnDamage(damage);
            PoolManager.Instance.GetFromPool(Define.HIT_EFFECT, other.bounds.ClosestPoint(this.transform.position), Quaternion.identity);            //coroutine = StartCoroutine(Delay(other));          
        }
    }




    public void GetDamage(float damage_)
    {
        if(damage != default) { return; }
        damage = damage_;
    }
}
