using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloodStone : MonoBehaviour
{
    private Rigidbody rigidBody = default;
    private float speed = default;
    private float damage = default;
    private float lifeTime = default;


    // Start is called before the first frame update
    private void Awake()
    {
        Init();
    }
    void Init()
    {
        rigidBody = GetComponent<Rigidbody>();
        speed = 15;
        damage = 20f;
        lifeTime = 4f;
    }

    public void Fire(Vector3 dir)
    {
        rigidBody.AddForce(dir * speed, ForceMode.Impulse);
        Invoke("DeActive", lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer(Define.LAYER_ENEMY))
        {
            other.GetComponent<EnemyBase>().OnDamage(damage);
            other.GetComponent<EnemyBase>().GetBleed();
            PoolManager.Instance.GetFromPool(Define.BLOODSTONE_EFFECT, this.transform.position - new Vector3(0f,this.transform.position.y,0f), Quaternion.identity);
            PoolManager.Instance.ReturnToPool(this.gameObject);            
        }
    }

    private void OnDisable()
    {
        rigidBody.velocity = Vector3.zero;             
    }

}
