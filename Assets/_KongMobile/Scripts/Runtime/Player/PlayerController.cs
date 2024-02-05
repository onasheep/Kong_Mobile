using System.Collections;
using UnityEngine;

public class PlayerController : PlayerBase, IDamageable
{
    [SerializeField]
    private VirtualDPad virtualDPad = default;
    [SerializeField]
    private Button[] skillBtns = default;
   
    private bool isDamageable = default;

    #region WaitforSeconds 
    private WaitForSeconds undamgedTime = new WaitForSeconds(1.5f);
    private WaitForSeconds regenerateDelay = new WaitForSeconds(1f);
    #endregion

    #region Need Stamina Parameter
    private readonly float SpinStamina = 15f;
    private readonly float bloodStamina = 20f;
    private readonly float heal = 20f;
    #endregion

    #region State
    enum STATE
     {
        NONE = -1, MOVE, ATTACK, SPIN, BLOODSTONE, HEAL, DIE
     }
    [SerializeField]
    STATE state = STATE.NONE;
    #endregion

    private void Awake()
    {
        Init();

    }
    protected override void Init()
    {
        base.Init();
        isDamageable = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        ChangeState(STATE.MOVE);
        
        StartCoroutine(RegenerateStamina());
    }

    // Update is called once per frame
    void Update()
    {
        StateProcese();
    }




    #region IEnumerator State
    private void Move()
    {
        rigidBody.velocity = new Vector3(virtualDPad.Horizontal * MoveSpeed, rigidBody.velocity.y, virtualDPad.Vertical * MoveSpeed);

        Vector3 VirtualPos = new Vector3(virtualDPad.Horizontal, 0f, virtualDPad.Vertical);

        animator.SetFloat("MoveSpeed", rigidBody.velocity.magnitude);

        this.transform.LookAt((VirtualPos + this.transform.position));
    }       // Move()

    IEnumerator Attack(float time)
    {
        
        animator.SetBool("Attack", true);
        GameObject attackEffect = PoolManager.Instance.GetFromPool(Define.BASIC_ATTACK,
            this.transform.position, Quaternion.identity);
        attackEffect.GetComponent<ColliderChecker>().GetDamage(Atk);
        attackEffect.transform.forward = this.transform.forward;

        yield return new WaitForSeconds(time);
        ChangeState(STATE.MOVE);
    }       // Attack()


    IEnumerator Spin()
    {
        float dist = 10f;
        float dodgeSpeed = 15.0f;
        Vector3 dest = this.transform.position + this.transform.forward * dist;
        Vector3 dir = (dest - this.transform.position).normalized;
        animator.SetBool("Dash", true);
        GameObject effect = PoolManager.Instance.GetFromPool(Define.SPIN_ATTACK, this.transform.position, Quaternion.identity);
        effect.GetComponent<ColliderChecker>().GetDamage(Atk * 3f);        
        
        while (dist > Mathf.Epsilon)
        {
            effect.transform.position = this.transform.position + new Vector3(0f,1f,0f);
            float delta = dodgeSpeed * Time.deltaTime;
            if (dist < delta)
            {
                delta = dist;
            }
            else
            {
                this.transform.position += dir * delta;
            }
            dist -= delta;
            yield return null;
        }
        animator.SetBool("Dash", false);
        ChangeState(STATE.MOVE);

    }// Spin()

    IEnumerator SpawnBloodStone(float time)
    {
        Vector3 offset = this.transform.position + this.transform.forward * 2f + new Vector3(0f, 1f, 0f);
        PoolManager.Instance.GetFromPool(Define.BLOODSTONE_GATE, offset, this.transform.rotation);
        yield return new WaitForSeconds(time);
        GameObject bloodStone = PoolManager.Instance.GetFromPool(Define.BLOODSTONE_PROJECTILE, offset, this.transform.rotation);
        bloodStone.GetComponent<BloodStone>().Fire(this.transform.forward);
        ChangeState(STATE.MOVE);
    }       // SpawnBloodStone()

    IEnumerator Heal(float time)
    {
        animator.SetTrigger("Heal");
        GameObject effect = PoolManager.Instance.GetFromPool(Define.HEALING_EFFECT, this.transform.position + new Vector3(0f,1f,0f), Quaternion.identity);
        Hp += 20f;
        yield return new WaitForSeconds(time);
        ChangeState(STATE.MOVE);
    }       // Heal()

    private void Die()
    {
        animator.SetTrigger("Die");
        rigidBody.velocity = Vector3.zero;
        GameManager.Instance.GameOver();
    }

    #endregion


    #region State Change and Process
    void ChangeState(STATE state_)
    {
        if (state == state_) { return; }
        state = state_;
        switch (state)
        {
            case STATE.MOVE:
                break;
            case STATE.ATTACK:

                StartCoroutine(Attack(1f));
                break;
            case STATE.SPIN:
                if (CanUseSkill(SpinStamina))
                {
                    StartCoroutine(Spin());
                }
                else
                {
                    ChangeState(STATE.MOVE);
                }
                break;
            case STATE.BLOODSTONE:
                if (CanUseSkill(bloodStamina))
                {
                    StartCoroutine(SpawnBloodStone(1f));
                }
                else
                {
                    ChangeState(STATE.MOVE);
                }
                break;
            case STATE.HEAL:
                if (CanUseSkill(heal))
                {
                    StartCoroutine(Heal(1f));
                }
                else
                {
                    ChangeState(STATE.MOVE);
                }
                break;
            case STATE.DIE:
                break;
        }
    }       // ChangeState()


    void StateProcese()
    {

        switch (state)
        {
            case STATE.MOVE:
                if (skillBtns[0].IsPressed == true)
                {
                    ChangeState(STATE.ATTACK);
                }
                else if (skillBtns[1].IsPressed == true)
                {
                    ChangeState(STATE.SPIN);
                }
                else if (skillBtns[2].IsPressed == true)
                {
                    ChangeState(STATE.BLOODSTONE);
                }
                else if (skillBtns[3].IsPressed == true)
                {
                    ChangeState(STATE.HEAL);
                }
                Move();
                break;
            case STATE.ATTACK:
                break;
            case STATE.SPIN:
                break;
            case STATE.BLOODSTONE:
                break;
            case STATE.DIE:
                Die();
                break;
        }
    }       // StateProcese()

    #endregion

    // ! 데미지를 입으면 일정 시간동안 무적으로 만드는 함수
    IEnumerator UnDamagebale()
    {
        isDamageable = false;
        yield return undamgedTime;
        isDamageable = true;
    }

    // ! 스태미나를 회복시키는 함수
    IEnumerator RegenerateStamina()
    {
        while (state != STATE.DIE)
        {
            if (Stamina < MaxStamina)
            {
                Stamina += StaminaRegen;
            }
            yield return regenerateDelay;

        }
    }       // RegenerateStamina()

    public override void OnDamage(float damage)
    {
        if(isDamageable)
        {
            if (damage < Hp)
            {
                Hp -= damage;
                animator.SetTrigger("Hit");
                StartCoroutine(UnDamagebale());
            }
            else
            {
                Hp = 0f;
                ChangeState(STATE.DIE);
            }
        }

    }       // OnDamage()

    private bool CanUseSkill(float useStamina)
    {
        if (Stamina >= useStamina)
        {
            Stamina -= useStamina;
            return true;

        }
        else
        {
            return false;
        }

    }       // CanUseSkill()
}
