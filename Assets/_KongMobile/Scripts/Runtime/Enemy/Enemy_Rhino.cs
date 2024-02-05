using System.Collections;
using UnityEngine;


public class Rhino : EnemyBase
{
    private Detector detector = default;
    private PlayerController player = default;
    
    private bool IsDamageable = default;

    private float distToTarget = default;
    private Vector3 dirToTarget = default;
    private Vector3 targetPos = default;
    

    private float dashSpeed = 2f;
    private EnemySkill dashSkill = default;
    private Collider dashCollider = default;

    private WaitForSeconds wait = new WaitForSeconds(2f);

    #region bleed Parameter
    private bool IsBleeding = default;
    private float bleedParameter = default;
    private float bleedlimit = default;
    #endregion

    #region ChangeAttackState Paramaeter
    private readonly float CloseAttackDist = 15f;
    private readonly float MeteorDist = 25f;
#endregion 

    #region Enum State
    enum STATE
    {
        NONE = -1, IDLE, SHOUT, WAIT, ClOSEATTACk, ROTATE, DASH, METEOR, DIE
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
        detector = this.GetComponentInChildren<Detector>();
        player = GFunc.GetRootObj("Player").GetComponent<PlayerController>();        
        bleedParameter = 0f;
        bleedlimit = 4f;
        dashSkill = this.GetComponentInChildren<EnemySkill>();
        dashCollider = dashSkill.GetComponent<Collider>();
        dashSkill.GetDamage(Atk * 3f);
        dashCollider.enabled = false;
    }       // Init()

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(STATE.IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        CheckTarget();
        StateProcese();
    }

    // ! 플레이어의 위치를 캐싱하는 함수
    private void CheckTarget()
    {
        if (player.IsValid() == false) { return; }
        targetPos = player.transform.position;
        distToTarget = (targetPos - this.transform.position).magnitude;
        dirToTarget = (targetPos - this.transform.position).normalized;
    }       // CheckTarget()


    // ! 거리에 따른 공격 패턴을 반환 하는 함수
    private STATE ChooseAttackState()
    {
        STATE state = default;

        if( 0 < distToTarget && distToTarget < CloseAttackDist)
        {
            state = STATE.ClOSEATTACk;
        }    
        else if(CloseAttackDist < distToTarget && distToTarget < MeteorDist)
        {
            state = STATE.METEOR;
        }
        else
        {
            state = STATE.DASH;
        }

        return state;
    }       // ChooseAttackState()

    #region IEnumerator STATE
    IEnumerator Rotate()
    {

        animator.SetBool("Walk",true);
        Vector3 dir = targetPos - this.transform.position;
        dir.y = 0;
        float time = 0f;

        while (time < 3f)
        {
            time += Time.deltaTime;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime);
            yield return null;
        }
        animator.SetBool("Walk", false);

        yield return wait;
        if (state == STATE.ROTATE)
            ChangeState(ChooseAttackState());
    }       // Rotate()


    IEnumerator CloseAttack()
    {
        GameObject indicator = PoolManager.Instance.GetFromPool(Define.CLOSE_ATTACK_INDICATOR, this.transform.position, Quaternion.identity);
        indicator.transform.localScale = Vector3.one * 2f;
        yield return wait;
        PoolManager.Instance.ReturnToPool(indicator);
        GameObject closeAttack = PoolManager.Instance.GetFromPool(Define.CLOSE_ATTACK, this.transform.position , Quaternion.identity);
        closeAttack.transform.localScale = new Vector3(8f, 8f, 8f);
        closeAttack.GetComponent<EnemySkill>().damage = Atk * 4f;
        yield return new WaitForSeconds(4f);
        if (state == STATE.ClOSEATTACk)
            ChangeState(STATE.ROTATE);
    }       // CloseAttack()

    IEnumerator Dash()
    {
        Vector3 currentTarget = this.transform.position;
        Vector3 dir = dirToTarget;
        dir.y = 0;
        float dist = distToTarget * 1.5f;
        animator.SetBool("Dash", true);
        dashCollider.enabled = true;
        while (dist > 0.1f)
        {
            float delta = 15f * Time.deltaTime;
            if (dist < delta)
            {
                delta = dist;
            }
            this.transform.forward = dir;
            currentTarget += dir * delta;
            this.transform.position = Vector3.Lerp(this.transform.position, currentTarget, Time.deltaTime * dashSpeed);

            dist -= delta;
            yield return null;
        }
        animator.SetBool("Dash", false);
        yield return wait;
        dashCollider.enabled = false;
        if (state == STATE.DASH)
            ChangeState(STATE.ROTATE);
    }       // DashAttack()

    IEnumerator Meteor()
    {
        Vector3 tempPos = targetPos;

        GameObject indicator = PoolManager.Instance.GetFromPool(Define.METEOR_INDICATOR, tempPos, Quaternion.identity);
        yield return wait;
        PoolManager.Instance.ReturnToPool(indicator);
        GameObject meteor = PoolManager.Instance.GetFromPool(Define.METEOR, tempPos, Quaternion.identity);
        meteor.GetComponent<EnemySkill>().damage = Atk * 2f;
        yield return new WaitForSeconds(5f);
        if (state == STATE.METEOR)
            ChangeState(STATE.ROTATE);
    }       // Meteor()
    private void Die()
    {
        animator.SetTrigger("Die");
        GameManager.Instance.GameWin();
    }
    #endregion

    #region State Change and Process
    void ChangeState(STATE state_)
    {
        if (state == state_) { return; }
        state = state_;
        switch (state)
        {
            case STATE.IDLE:
                break;
            case STATE.SHOUT:
                IsDamageable = false;
                animator.SetTrigger("Shout");
                break;
            case STATE.ROTATE:
                StartCoroutine(Rotate());
                break;
            case STATE.ClOSEATTACk:
                StartCoroutine(CloseAttack());
                break;
            case STATE.METEOR:
                animator.SetTrigger("Meteor");
                StartCoroutine(Meteor());
                break;
            case STATE.DASH:
                StartCoroutine(Dash());
                break;
            case STATE.DIE:
                Die();
                break;
        }
    }       // ChangeState()

    void StateProcese()
    {
        switch (state)
        {
            case STATE.IDLE:
                if (detector.IsEnter == true)
                {
                    ChangeState(STATE.SHOUT);
                }
                break;
            case STATE.SHOUT:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Shout") &&
    animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
                {
                    IsDamageable = true;
                    ChangeState(STATE.ROTATE);

                }
                break;
            case STATE.ROTATE:
                break;
            case STATE.ClOSEATTACk:
                break;
            case STATE.DASH:
                    break;
            case STATE.METEOR:

                break;
            case STATE.DIE:
                
                break;
        }
    }       // StateProcese()

    #endregion



    public override void OnDamage(float damage)
    {
        if(state == STATE.DIE) { return; }
        if(IsDamageable == true)
        {
            if (Hp > damage)
            {
                animator.SetTrigger("Hit");
                Hp -= damage;
            }
            else
            {
                IsDamageable = false;
                Hp = 0f;
                StopCoroutine(Bleeding());
                ChangeState(STATE.DIE);
            }
        }        
    } // OnDamage()


    public override void GetBleed()
    {
        if(IsBleeding == true) { return; }
        
        if(bleedParameter < bleedlimit)
        {
            bleedParameter++;
        }
        else
        {
            StartCoroutine(Bleeding());
        }                
    } // GetBleed()

    IEnumerator Bleeding()
    {
        bleedParameter = 0f;
        IsBleeding = true;
        float timer = 0f;
        float dealTimer = 1f;
        GameObject effect = PoolManager.Instance.GetFromPool(Define.BLEEDING_EFFECT, this.transform.position, Quaternion.identity);
        while(timer <= 10f)
        {
            timer += Time.deltaTime;
            if(timer > dealTimer)
            {
                dealTimer += 1;
                OnDamage(10f);
            }

            yield return null;
        }
        PoolManager.Instance.ReturnToPool(effect);
        IsBleeding = false;
    }       // Bleeding()
}
