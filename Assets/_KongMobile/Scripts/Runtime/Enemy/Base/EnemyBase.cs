using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamageable, IBleedable
{
    [SerializeField]
    private const float hp = 500;
    [SerializeField]
    private const float atk = 10f;

    protected Animator animator = default;
    protected Rigidbody rigidBody = default;
    public float Hp { get; protected set; }
    public float MaxHP { get; protected set; }
    public float Atk { get; protected set; }

    public abstract void OnDamage(float damage);

    public abstract void GetBleed();

    protected virtual void Init()
    {
        Hp = hp;
        Atk = atk;
        MaxHP = Hp;
        animator = this.GetComponentInChildren<Animator>();
        rigidBody = this.GetComponent<Rigidbody>();
    }


}
