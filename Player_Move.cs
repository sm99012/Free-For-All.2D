using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    public SpriteRenderer m_sSpriteRenderer;
    public Transform m_tTransform;
    public Animator m_aAnimator;
    public Rigidbody2D m_rRigdbody;

    Vector3 m_vScale;
    Vector3 m_vRightPos;
    Vector3 m_vLeftPos;
    Vector3 m_vInputDir;

    public bool m_bMove;

    public enum E_PLAYER_MOVE_STATE { IDLE, RUN, ATTACK1_1, ATTACK1_2, ATTACK1_3, ATTACKED, DEATH, ROLL, GOAWAY }
    public E_PLAYER_MOVE_STATE m_ePlayerMoveState = E_PLAYER_MOVE_STATE.IDLE;

    public bool m_bAttack;
    // 연계 공격
    public bool m_bAttack1_1;
    public bool m_bAttack1_2;
    public bool m_bAttack1_3;

    // 구르기: 무적판정. 이동은 가능
    public bool m_bRoll;

    // 무적
    public bool m_bPower;

    // Goaway
    public bool m_bGoaway;
    public bool m_bGoaway_Success;
    public float m_fGoaway_Cooltime;
    public float m_fGoaway_Durationtime;


    private void Start()
    {
        m_sSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        m_tTransform = this.gameObject.GetComponent<Transform>();
        m_aAnimator = this.gameObject.GetComponent<Animator>();
        m_rRigdbody = this.gameObject.GetComponent<Rigidbody2D>();

        m_vRightPos = new Vector3(1, 1, 1);
        m_vLeftPos = new Vector3(-1, 1, 1);

        m_vScale = m_vRightPos;

        m_bAttack = true;

        m_bAttack1_1 = true;
        m_bAttack1_2 = false;
        m_bAttack1_3 = false;

        m_bMove = true;

        m_bRoll = true;
        m_fCooltime_Roll = 3f; // 3

        m_bPower = false;

        m_bGoaway = true;
        m_fGoaway_Cooltime = 10f; // 10
        m_fGoaway_Durationtime = 3f; // 3
        m_bGoaway_Success = false;

        m_fAttackedToIdleTime = 0.3f;
    }

    // 플레이어 움직임
    public void Move(int h, int v, int fspeed)
    {
        if (m_bMove == true)
        {
            if (m_ePlayerMoveState == E_PLAYER_MOVE_STATE.IDLE || m_ePlayerMoveState == E_PLAYER_MOVE_STATE.RUN ||
               m_ePlayerMoveState == E_PLAYER_MOVE_STATE.ROLL || m_ePlayerMoveState == E_PLAYER_MOVE_STATE.GOAWAY)
            {
                SetScale(h, v);

                if (h == 0 && v == 0)
                    m_vInputDir = Vector3.zero;
                else if (h != 0 && v != 0)
                    m_vInputDir = new Vector3(h / 1.4f, v / 1.4f);
                else
                    m_vInputDir = new Vector3(h, v);

                if (m_ePlayerMoveState == E_PLAYER_MOVE_STATE.IDLE || m_ePlayerMoveState == E_PLAYER_MOVE_STATE.RUN)
                    this.gameObject.transform.position += m_vInputDir * fspeed * Time.deltaTime * 0.005f;
                if (m_ePlayerMoveState == E_PLAYER_MOVE_STATE.ROLL)
                    this.gameObject.transform.position += m_vInputDir * fspeed * Time.deltaTime * 0.005f * 1.5f;
            }
        }
    }
    // 플레이어 방향 설정, UML 적용
    void SetScale(int h, int v)
    {
        if (m_ePlayerMoveState == E_PLAYER_MOVE_STATE.IDLE || m_ePlayerMoveState == E_PLAYER_MOVE_STATE.RUN)
        {
            if (h == 1)
            {
                m_vScale = m_vRightPos;
                m_ePlayerMoveState = SetPlayerMoveState(E_PLAYER_MOVE_STATE.RUN);
            }
            else if (h == -1)
            {
                m_vScale = m_vLeftPos;
                m_ePlayerMoveState = SetPlayerMoveState(E_PLAYER_MOVE_STATE.RUN);
            }

            if (v == 1)
            {
                m_ePlayerMoveState = SetPlayerMoveState(E_PLAYER_MOVE_STATE.RUN);
            }
            else if (v == -1)
            {
                m_ePlayerMoveState = SetPlayerMoveState(E_PLAYER_MOVE_STATE.RUN);
            }

            if (h == 0 && v == 0)
                m_ePlayerMoveState = SetPlayerMoveState(E_PLAYER_MOVE_STATE.IDLE);
        }
        if (m_ePlayerMoveState == E_PLAYER_MOVE_STATE.ROLL)
        {
            if (h == 1)
            {
                m_vScale = m_vRightPos;
            }
            else if (h == -1)
            {
                m_vScale = m_vLeftPos;
            }
        }

        m_tTransform.localScale = m_vScale;
    }

    public int Attack()
    {
        if (m_ePlayerMoveState == E_PLAYER_MOVE_STATE.IDLE || m_ePlayerMoveState == E_PLAYER_MOVE_STATE.RUN)
        {
            if (m_bAttack == true)
            {
                if (m_bAttack1_1 == true)
                {
                    m_ePlayerMoveState = SetPlayerMoveState(E_PLAYER_MOVE_STATE.ATTACK1_1);
                    //Debug.Log("Player Attack1_1");
                    return 1;
                }
                else if (m_bAttack1_2 == true)
                {
                    m_ePlayerMoveState = SetPlayerMoveState(E_PLAYER_MOVE_STATE.ATTACK1_2);
                    //Debug.Log("Player Attack1_2");
                    return 2;
                }
                else if (m_bAttack1_3 == true)
                {
                    m_ePlayerMoveState = SetPlayerMoveState(E_PLAYER_MOVE_STATE.ATTACK1_3);
                    //Debug.Log("Player Attack1_3");
                    return 3;
                }
            }
        }

        return 0;
    }
    // 연계공격 허용 지속시간
    Coroutine m_cProcess_Attack_Duration = null;
    Coroutine m_cProcess_AttackToIdle_Duration = null;
    Coroutine m_cProcess_AttackDelay_Duration = null;
    float m_fAttack_DurationTime;
    float m_fAttackDelay_DurationTime;
    public float m_fAttack1_1DurationTime = 0.5f; // 0.4f
    public float m_fAttack1_2DurationTime = 0.5f; // 0.4f
    public float m_fAttack1_3DurationTime = 1.0f; // 1.0f
    // 플레이어 공격속도(딜레이)
    public float m_fAttackDelayTime;

    IEnumerator ProcessAttackDelay()
    {
        m_fAttackDelay_DurationTime = m_fAttackDelayTime;
        m_bAttack = false;
        while (m_fAttackDelay_DurationTime > 0)
        {
            m_fAttackDelay_DurationTime -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        m_bAttack = true;
        if (m_cProcess_AttackDelay_Duration != null)
            m_cProcess_AttackDelay_Duration = null;
    }
    IEnumerator ProcessAttack1_1()
    {
        m_fAttack_DurationTime = m_fAttack1_1DurationTime;
        m_bAttack1_1 = false;
        m_bAttack1_2 = true;
        m_bAttack1_3 = false;
        while (m_fAttack_DurationTime > 0)
        {
            m_fAttack_DurationTime -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        m_cProcess_Attack_Duration = null;
        m_bAttack1_1 = true;
        m_bAttack1_2 = false;
        m_bAttack1_3 = false;
    }
    IEnumerator ProcessAttack1_2()
    {
        m_fAttack_DurationTime = m_fAttack1_2DurationTime;
        m_bAttack1_1 = false;
        m_bAttack1_2 = false;
        m_bAttack1_3 = true;
        while (m_fAttack_DurationTime > 0)
        {
            m_fAttack_DurationTime -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        m_cProcess_Attack_Duration = null;
        m_bAttack1_1 = true;
        m_bAttack1_2 = false;
        m_bAttack1_3 = false;
    }
    IEnumerator ProcessAttack1_3()
    {
        m_fAttack_DurationTime = m_fAttack1_3DurationTime;
        m_bAttack1_1 = true;
        m_bAttack1_2 = false;
        m_bAttack1_3 = false;
        while (m_fAttack_DurationTime > 0)
        {
            m_fAttack_DurationTime -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        m_cProcess_Attack_Duration = null;
        m_bAttack1_1 = true;
        m_bAttack1_2 = false;
        m_bAttack1_3 = false;
    }
    IEnumerator ProcessAttackToIdle(float ftime)
    {
        m_bMove = false;
        yield return new WaitForSeconds(ftime);
        m_ePlayerMoveState = SetPlayerMoveState(E_PLAYER_MOVE_STATE.IDLE);
        m_bMove = true;
        m_cProcess_AttackToIdle_Duration = null;
    }
    void Attack1_1()
    {
        m_cProcess_Attack_Duration = null;
        m_cProcess_Attack_Duration = StartCoroutine(ProcessAttack1_1());
        m_cProcess_AttackToIdle_Duration = StartCoroutine(ProcessAttackToIdle(0.3f));//0.3f
    }
    void Attack1_2()
    {
        m_cProcess_Attack_Duration = null;
        m_cProcess_Attack_Duration = StartCoroutine(ProcessAttack1_2());
        m_cProcess_AttackToIdle_Duration = StartCoroutine(ProcessAttackToIdle(0.3f));//0.3f
    }
    void Attack1_3()
    {
        m_cProcess_Attack_Duration = null;
        m_cProcess_Attack_Duration = StartCoroutine(ProcessAttack1_3());
        m_cProcess_AttackToIdle_Duration = StartCoroutine(ProcessAttackToIdle(0.7f));//0.6f
    }

    Coroutine m_cProcess_Attacked = null;
    Coroutine m_cProcess_Power = null;
    public bool Attacked()
    {
        if (m_bPower == false)
        {
            // GOAWAY 기능 취소
            if (m_cProcess_Goaway_Duration != null)
            {
                StopCoroutine(m_cProcess_Goaway_Duration);
                m_bMove = true;
            }
            m_cProcess_Power = StartCoroutine(ProcessPower());
            if (m_ePlayerMoveState == E_PLAYER_MOVE_STATE.IDLE || m_ePlayerMoveState == E_PLAYER_MOVE_STATE.RUN ||
                m_ePlayerMoveState == E_PLAYER_MOVE_STATE.GOAWAY)
            {
                m_cProcess_Attacked = StartCoroutine(ProcessAttackedToIdle());
                m_ePlayerMoveState = SetPlayerMoveState(E_PLAYER_MOVE_STATE.ATTACKED);
            }
            return true;
        }
        return false;
    }
    float m_fAttackedToIdleTime;
    IEnumerator ProcessAttackedToIdle()
    {
        m_bMove = false;
        yield return new WaitForSeconds(0.3f);
        m_ePlayerMoveState = SetPlayerMoveState(E_PLAYER_MOVE_STATE.IDLE);
        m_bMove = true;
        m_cProcess_Attacked = null;
    }
    IEnumerator ProcessPower()
    {
        m_bPower = true;
        yield return new WaitForSeconds(1f);
        m_bPower = false;
        m_cProcess_Power = null;
    }


    public void Death()
    {
        m_bMove = false;
        m_bPower = true;
        m_ePlayerMoveState = SetPlayerMoveState(E_PLAYER_MOVE_STATE.DEATH);
        StopAllCoroutines();
    }

    public void Respone()
    {
        m_bMove = true;
        m_bPower = false;
        m_ePlayerMoveState = SetPlayerMoveState(E_PLAYER_MOVE_STATE.IDLE);
    }

    public void Roll()
    {
        if (m_ePlayerMoveState == E_PLAYER_MOVE_STATE.IDLE || m_ePlayerMoveState == E_PLAYER_MOVE_STATE.RUN || 
            m_ePlayerMoveState == E_PLAYER_MOVE_STATE.ATTACK1_1 || m_ePlayerMoveState == E_PLAYER_MOVE_STATE.ATTACK1_2 ||
            m_ePlayerMoveState == E_PLAYER_MOVE_STATE.ATTACK1_3 || m_ePlayerMoveState == E_PLAYER_MOVE_STATE.GOAWAY)
        {
            if (m_bRoll == true)
            {
                // 공격 모션(후딜) 캔슬
                if (m_cProcess_AttackToIdle_Duration != null)
                    StopCoroutine(m_cProcess_AttackToIdle_Duration);
                // GOAWAY 기능 취소
                if (m_cProcess_Goaway_Duration != null)
                {
                    StopCoroutine(m_cProcess_Goaway_Duration);
                    m_bMove = true;
                }
                // ATTACKED 캔슬
                if (m_cProcess_Attacked != null)
                    StopCoroutine(m_cProcess_Attacked);

                m_ePlayerMoveState = SetPlayerMoveState(E_PLAYER_MOVE_STATE.ROLL);
                if (m_cProcess_Power != null)
                    StopCoroutine(m_cProcess_Power);

                StartCoroutine(ProcessRoll_Cooltime());
                StartCoroutine(ProcessRollToIdle());
            }
        }
    }
    IEnumerator ProcessRoll_Cooltime()
    {
        m_bMove = true;
        m_bRoll = false;
        yield return new WaitForSeconds(m_fCooltime_Roll);
        m_bRoll = true;
    }
    IEnumerator ProcessRollToIdle()
    {
        m_bPower = true;
        yield return new WaitForSeconds(0.5f);
        m_bPower = false;
        m_ePlayerMoveState = SetPlayerMoveState(E_PLAYER_MOVE_STATE.IDLE);
    }
    public float m_fCooltime_Roll;


    public void Goaway()
    {
        if (m_ePlayerMoveState == E_PLAYER_MOVE_STATE.IDLE || m_ePlayerMoveState == E_PLAYER_MOVE_STATE.RUN)
        {
            if (m_bGoaway == true)
            {
                m_bGoaway_Success = false;
                m_ePlayerMoveState = SetPlayerMoveState(E_PLAYER_MOVE_STATE.GOAWAY);
                StartCoroutine(ProcessGoaway_Cooltime());
                m_cProcess_Goaway_Duration = StartCoroutine(ProcessGoawayToIdle());
            }
        }
    }
    IEnumerator ProcessGoaway_Cooltime()
    {
        m_bGoaway = false;
        yield return new WaitForSeconds(m_fGoaway_Cooltime);
        m_bGoaway = true;
    }
    IEnumerator ProcessGoawayToIdle()
    {
        m_bMove = false;
        yield return new WaitForSeconds(m_fGoaway_Durationtime);
        m_bMove = true;
        m_bGoaway_Success = true;
        if (m_cProcess_Goaway_Duration != null)
            m_cProcess_Goaway_Duration = null;
        m_ePlayerMoveState = SetPlayerMoveState(E_PLAYER_MOVE_STATE.IDLE);
        yield return new WaitForSeconds(Time.deltaTime);
        m_bGoaway_Success = false;
    }
    // Goaway 키다운 지속시간
    Coroutine m_cProcess_Goaway_Duration = null;
    public void Cancel_Goaway()
    {
        // GOAWAY 기능 취소
        if (m_cProcess_Goaway_Duration != null)
        {
            StopCoroutine(m_cProcess_Goaway_Duration);
            m_ePlayerMoveState = SetPlayerMoveState(E_PLAYER_MOVE_STATE.IDLE);
            m_bMove = true;
        }
    }

    // FSM 관리
    public E_PLAYER_MOVE_STATE SetPlayerMoveState(E_PLAYER_MOVE_STATE pms)
    {
        switch (pms)
        {
            case E_PLAYER_MOVE_STATE.IDLE:
                {
                    if (m_ePlayerMoveState != pms)
                    {
                        SetAnimatorParameters("Idle");
                    }
                }
                break;
            case E_PLAYER_MOVE_STATE.RUN:
                {
                    if (m_ePlayerMoveState != pms)
                    {
                        SetAnimatorParameters("Run");
                    }
                }
                break;
            case E_PLAYER_MOVE_STATE.ATTACK1_1:
                {
                    if (m_ePlayerMoveState != pms)
                    {
                        if (m_cProcess_AttackDelay_Duration != null)
                        {
                            StopCoroutine(m_cProcess_AttackDelay_Duration);
                        }
                        m_cProcess_AttackDelay_Duration = StartCoroutine(ProcessAttackDelay());
                        if (m_cProcess_Attack_Duration != null)
                            StopCoroutine(m_cProcess_Attack_Duration);
                        SetAnimatorParameters("Attack1_1");
                        Attack1_1();
                    }
                }
                break;
            case E_PLAYER_MOVE_STATE.ATTACK1_2:
                {
                    if (m_ePlayerMoveState != pms)
                    {
                        if (m_cProcess_AttackDelay_Duration != null)
                        {
                            StopCoroutine(m_cProcess_AttackDelay_Duration);
                        }
                        m_cProcess_AttackDelay_Duration = StartCoroutine(ProcessAttackDelay());
                        if (m_cProcess_Attack_Duration != null)
                            StopCoroutine(m_cProcess_Attack_Duration);
                        SetAnimatorParameters("Attack1_2");
                        Attack1_2();
                    }
                }
                break;
            case E_PLAYER_MOVE_STATE.ATTACK1_3:
                {
                    if (m_ePlayerMoveState != pms)
                    {
                        if (m_cProcess_AttackDelay_Duration != null)
                        {
                            StopCoroutine(m_cProcess_AttackDelay_Duration);
                        }
                        m_cProcess_AttackDelay_Duration = StartCoroutine(ProcessAttackDelay());
                        if (m_cProcess_Attack_Duration != null)
                            StopCoroutine(m_cProcess_Attack_Duration);
                        SetAnimatorParameters("Attack1_3");
                        Attack1_3();
                        }
                }
                break;
            case E_PLAYER_MOVE_STATE.ATTACKED:
                {
                    if (m_ePlayerMoveState != pms)
                    {
                        if (m_cProcess_Attack_Duration != null)
                            StopCoroutine(m_cProcess_Attack_Duration);
                        SetAnimatorParameters("Attacked");
                    }
                }
                break;
            case E_PLAYER_MOVE_STATE.DEATH:
                {
                    if (m_ePlayerMoveState != pms)
                    {
                        SetAnimatorParameters("Death");
                        m_bMove = false;
                    }
                }
                break;
            case E_PLAYER_MOVE_STATE.ROLL:
                {
                    if (m_ePlayerMoveState != pms)
                    {
                        SetAnimatorParameters("Roll");
                    }
                }
                break;
            case E_PLAYER_MOVE_STATE.GOAWAY:
                {
                    if (m_ePlayerMoveState != pms)
                    {
                        SetAnimatorParameters("Goaway");
                    }
                }
                break;
        }

        return pms;
    }
    // Animation 관리
    public void SetAnimatorParameters(string str)
    {
        switch (str)
        {
            case "Idle":
                {
                    m_aAnimator.SetBool("Idle", true);
                    m_aAnimator.SetBool("Run", false);
                    m_aAnimator.SetBool("Attack1_1", false);
                    m_aAnimator.SetBool("Attack1_2", false);
                    m_aAnimator.SetBool("Attack1_3", false);
                    m_aAnimator.SetBool("Attacked", false);
                    m_aAnimator.SetBool("Death", false);
                    m_aAnimator.SetBool("Roll", false);
                    m_aAnimator.SetBool("Goaway", false);
                }
                break;
            case "Run":
                {
                    m_aAnimator.SetBool("Idle", false);
                    m_aAnimator.SetBool("Run", true);
                    m_aAnimator.SetBool("Attack1_1", false);
                    m_aAnimator.SetBool("Attack1_2", false);
                    m_aAnimator.SetBool("Attack1_3", false);
                    m_aAnimator.SetBool("Attacked", false);
                    m_aAnimator.SetBool("Death", false);
                    m_aAnimator.SetBool("Roll", false);
                    m_aAnimator.SetBool("Goaway", false);
                }
                break;
            case "Attack1_1":
                {
                    m_aAnimator.SetBool("Idle", false);
                    m_aAnimator.SetBool("Run", false);
                    m_aAnimator.SetBool("Attack1_1", true);
                    m_aAnimator.SetBool("Attack1_2", false);
                    m_aAnimator.SetBool("Attack1_3", false);
                    m_aAnimator.SetBool("Attacked", false);
                    m_aAnimator.SetBool("Death", false);
                    m_aAnimator.SetBool("Roll", false);
                    m_aAnimator.SetBool("Goaway", false);
                }
                break;
            case "Attack1_2":
                {
                    m_aAnimator.SetBool("Idle", false);
                    m_aAnimator.SetBool("Run", false);
                    m_aAnimator.SetBool("Attack1_1", false);
                    m_aAnimator.SetBool("Attack1_2", true);
                    m_aAnimator.SetBool("Attack1_3", false);
                    m_aAnimator.SetBool("Attacked", false);
                    m_aAnimator.SetBool("Death", false);
                    m_aAnimator.SetBool("Roll", false);
                    m_aAnimator.SetBool("Goaway", false);
                }
                break;
            case "Attack1_3":
                {
                    m_aAnimator.SetBool("Idle", false);
                    m_aAnimator.SetBool("Run", false);
                    m_aAnimator.SetBool("Attack1_1", false);
                    m_aAnimator.SetBool("Attack1_2", false);
                    m_aAnimator.SetBool("Attack1_3", true);
                    m_aAnimator.SetBool("Attacked", false);
                    m_aAnimator.SetBool("Death", false);
                    m_aAnimator.SetBool("Roll", false);
                    m_aAnimator.SetBool("Goaway", false);
                }
                break;
            case "Attacked":
                {
                    m_aAnimator.SetBool("Idle", false);
                    m_aAnimator.SetBool("Run", false);
                    m_aAnimator.SetBool("Attack1_1", false);
                    m_aAnimator.SetBool("Attack1_2", false);
                    m_aAnimator.SetBool("Attack1_3", false);
                    m_aAnimator.SetBool("Attacked", true);
                    m_aAnimator.SetBool("Death", false);
                    m_aAnimator.SetBool("Roll", false);
                    m_aAnimator.SetBool("Goaway", false);
                }
                break;

            case "Death":
                {
                    m_aAnimator.SetBool("Idle", false);
                    m_aAnimator.SetBool("Run", false);
                    m_aAnimator.SetBool("Attack1_1", false);
                    m_aAnimator.SetBool("Attack1_2", false);
                    m_aAnimator.SetBool("Attack1_3", false);
                    m_aAnimator.SetBool("Attacked", false);
                    m_aAnimator.SetBool("Death", true);
                    m_aAnimator.SetBool("Roll", false);
                    m_aAnimator.SetBool("Goaway", false);
                }
                break;

            case "Roll":
                {
                    m_aAnimator.SetBool("Idle", false);
                    m_aAnimator.SetBool("Run", false);
                    m_aAnimator.SetBool("Attack1_1", false);
                    m_aAnimator.SetBool("Attack1_2", false);
                    m_aAnimator.SetBool("Attack1_3", false);
                    m_aAnimator.SetBool("Attacked", false);
                    m_aAnimator.SetBool("Death", false);
                    m_aAnimator.SetBool("Roll", true);
                    m_aAnimator.SetBool("Goaway", false);
                }
                break;

            case "Goaway":
                {
                    m_aAnimator.SetBool("Idle", false);
                    m_aAnimator.SetBool("Run", false);
                    m_aAnimator.SetBool("Attack1_1", false);
                    m_aAnimator.SetBool("Attack1_2", false);
                    m_aAnimator.SetBool("Attack1_3", false);
                    m_aAnimator.SetBool("Attacked", false);
                    m_aAnimator.SetBool("Death", false);
                    m_aAnimator.SetBool("Roll", false);
                    m_aAnimator.SetBool("Goaway", true);
                }
                break;

        }
    }
}
