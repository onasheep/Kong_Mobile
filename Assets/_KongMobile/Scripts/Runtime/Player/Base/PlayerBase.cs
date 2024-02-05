using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour, IDamageable
{
    [SerializeField]
    private const float hp = 200f;
    [SerializeField]
    private const float moveSpeed = 4f;
    [SerializeField]
    private const float atk = 10f;
    [SerializeField]
    private const float stamina = 100f;
    [SerializeField]
    private const float staminaRegen = 4f;

    protected Rigidbody rigidBody = default;
    protected Animator animator = default;

    public float MaxHp { get; protected set; }
    public float Hp { get; protected set; }
    public float MoveSpeed { get; protected set; }
    public float Atk { get; protected set; }
    public float Stamina { get; protected set; }
    public float MaxStamina { get; protected set; }
    public float StaminaRegen { get; protected set; }
    
    protected virtual void Init()
    {
        Hp = hp;
        MaxHp = hp;
        Stamina = stamina;
        MaxStamina = stamina;
        StaminaRegen = staminaRegen;
        MoveSpeed = moveSpeed;
        Atk = atk;
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();    
    }

    public abstract void OnDamage(float damage);
  
}
