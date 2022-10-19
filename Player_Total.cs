using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Total : MonoBehaviour
{
    private static Player_Total instance = null;
        
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static Player_Total Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    public Player_Status m_ps_Status;
    public Player_Move m_pm_Move;
    public Player_Itemslot m_pi_Itemslot;
    public Player_Equipment m_pe_Equipment;
    public Player_Effect m_pe_Effect;
    public Player_Quest m_pq_Quest;
    public int hInput; // 수평이동
    public int vInput; // 수직이동
    public int m_nPosValue;

    // 공격 히트박스
    Vector2 m_vSize;
    int nLayer1;
    // 특수기능 히트박스
    int nLayer2;
    // NPC
    int nLayer3;
    // 아이템
    int nLayer4;
    
    public void InitialSet_Player_Total()
    {
        m_ps_Status = this.GetComponent<Player_Status>();
        m_pm_Move = this.GetComponent<Player_Move>();
        m_pi_Itemslot = this.GetComponent<Player_Itemslot>();
        m_pe_Equipment = this.GetComponent<Player_Equipment>();
        m_pe_Effect = this.GetComponent<Player_Effect>();
        m_pq_Quest = this.GetComponent<Player_Quest>();

        m_vSize = new Vector2(0.25f, 0.35f);
        nLayer1 = 1 << LayerMask.NameToLayer("Monster") | 1 << LayerMask.NameToLayer("RuinableObject");
        nLayer2 = 1 << LayerMask.NameToLayer("Monster");
        nLayer3 = 1 << LayerMask.NameToLayer("NPC");
        nLayer4 = 1 << LayerMask.NameToLayer("Item");
    }

    void Update()
    {
        UpdateState();
        Controller();
        //UpdateEqiip(); //
        //AnimationTest();
    }

    public void Controller()
    {
        if (GUIManager_Total.Instance.m_GUI_Interaction.m_g_ChatBox.activeSelf == false)
        {
            InputKey_Move();
            InputKey_Attack();
            InputKey_Goaway();
            InputKey_Roll();
            InputKey_Interaction();
            InputKey_GUI_Quest();
            InputKey_GUI_Loop();
            InputKey_GetItem();
            InputKey_GUI_Itemslot();
            InputKey_GUI_Equipslot();
            InputKey_GUI_Status();
            //InputKey_Equip(); //

            if (m_pm_Move.m_ePlayerMoveState == Player_Move.E_PLAYER_MOVE_STATE.DEATH)
            {
                InputKey_F1(); // 리스폰
            }
        }
        else
        {
            hInput = 0; vInput = 0;
            Move(false);
            m_pm_Move.Cancel_Goaway();

            InputKey_ESC();
        }
    }

    // 공격속도관련, 퀘스트 업데이트 관련
    void UpdateState()
    {
        m_pq_Quest.QuestUpdate_Collect();
        SetAttackspeed();
    }

    public void SetAttackspeed()
    {
        m_pm_Move.m_fAttackDelayTime = m_ps_Status.m_sStatus.GetSTATUS_AttackSpeed() / 2;
    }

    // Idle, Run
    public void InputKey_Move()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            hInput = 1;
            m_nPosValue = 1;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
            hInput = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            hInput = -1;
            m_nPosValue = -1;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
            hInput = 0;
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
            hInput = 0;

        if (Input.GetKey(KeyCode.UpArrow))
            vInput = 1;
        if (Input.GetKeyUp(KeyCode.UpArrow))
            vInput = 0;
        if (Input.GetKey(KeyCode.DownArrow))
            vInput = -1;
        if (Input.GetKeyUp(KeyCode.DownArrow))
            vInput = 0;
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow))
            vInput = 0;

        Move(true);
    }
    private void Move(bool move)
    {
        if (move == true)
            m_pm_Move.Move(hInput, vInput, m_ps_Status.m_sStatus.GetSTATUS_Speed());
        else
            m_pm_Move.Move(0, 0, m_ps_Status.m_sStatus.GetSTATUS_Speed());
    }

    // Attack
    public void InputKey_Attack()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            Attack();
        }
    }
    int m_nAtk;
    private void Attack()
    {
        //AttackCheck();
        m_nAtk = m_pm_Move.Attack();

        if (m_nAtk != 0)
        {
            switch (m_nAtk)
            {
                case 1:
                    {
                        AttackCheck(1f);
                    }
                    break;
                case 2:
                    {
                        AttackCheck(1f);
                    }
                    break;
                case 3:
                    {
                        AttackCheck(2f);
                    }
                    break;
            }
        }
    }
    Collider2D[] co2_1;
    Vector3 m_vAttackPos;
    void AttackCheck(float percent)
    {
        if (m_nPosValue == 1)
        {
            co2_1 = Physics2D.OverlapBoxAll(transform.position + new Vector3(0.125f, 0.15f, 0), m_vSize, 0, nLayer1);
        }
        else
        {
            co2_1 = Physics2D.OverlapBoxAll(transform.position + new Vector3(-0.125f, 0.15f, 0), m_vSize, 0, nLayer1);
        }

        if (co2_1.Length > 0)
        {
            for (int i = 0; i < co2_1.Length; i++)
            {
                if (co2_1[i].gameObject.layer == LayerMask.NameToLayer("Monster"))
                    m_vAttackPos = co2_1[i].gameObject.transform.position;
                else if (co2_1[i].gameObject.layer == LayerMask.NameToLayer("RuinableObject"))
                {
                    m_vAttackPos = co2_1[i].gameObject.transform.position;
                    m_vAttackPos += new Vector3(0, 0.1f, 0);
                }

                if (co2_1[i].GetComponent<Monster_Total>().Attacked((int)(m_ps_Status.m_sStatus.GetSTATUS_Damage_Total() * percent), this.gameObject) == true)
                {
                    //Debug.Log("Damage: " + (int)(m_ps_Status.m_sStatus.GetSTATUS_Damage_Total() * percent));
                    m_pe_Effect.Effect1(co2_1[i].transform.position);
                }
            }
        }
    }
    public void MobDeath(Monster_Total mt)
    {
        m_pq_Quest.QuestUpdate_Kill(mt.m_ms_Status.m_eMonster_Kind, mt.m_ms_Status.m_nMonsterCode);
        m_ps_Status.MobDeath(mt.m_ms_Status.m_sSoc_Death, mt.m_ms_Status.m_sStatus_Death);
    }

    // Attacked
    public void Attacked(int damage)
    {
        if (m_pm_Move.Attacked() == true)
        {
            m_ps_Status.Attacked(damage);
            //Debug.Log("Player attacked by monster");
            Death();
        }
    }

    // Goaway
    public void InputKey_Goaway()
    {
        if (Input.GetKeyUp(KeyCode.D))
        {
            Goaway();
        }
        if (m_pm_Move.m_bGoaway_Success == true)
        {
            GoawayCheck();
        }
    }
    private void Goaway()
    {
        m_pm_Move.Goaway();
    }
    Collider2D[] co2_2;
    SOC soc2_2;
    void GoawayCheck()
    {
        co2_2 = Physics2D.OverlapCircleAll(transform.position, 0.5f, nLayer2);

        if (co2_2.Length > 0)
        {
            for (int i = 0; i < co2_2.Length; i++)
            {
                if (co2_2[i].gameObject.tag != "Boss")
                {
                    soc2_2.SetSOC(co2_2[i].gameObject.GetComponent<Monster_Total>().Goaway());
                    // 평판은 33%확률로 상승
                    int nrandom = Random.Range(0, 101);
                    if (nrandom <= 50)
                    {
                        m_ps_Status.Goaway(soc2_2);
                    }
                    //m_pe_Effect.Effect2(co2_2[i].transform.position);
                    m_vEffectPos = co2_2[i].gameObject.transform.position;
                    m_pq_Quest.QuestUpdate_Goaway(co2_2[i].gameObject.GetComponent<Monster_Total>().m_ms_Status.m_eMonster_Kind, co2_2[i].gameObject.GetComponent<Monster_Total>().m_ms_Status.m_nMonsterCode);
                }
            }
        }
    }
    Vector3 m_vEffectPos;
    IEnumerator ProcessGoawayEffect(Vector3 pos)
    {
        Vector3 vpos = pos;
        m_pe_Effect.Effect4(vpos);
        yield return new WaitForSeconds(0.3f);
        m_pe_Effect.Effect4(vpos);
        yield return new WaitForSeconds(0.3f);
        m_pe_Effect.Effect4(vpos);
    }

    // Roll
    public void InputKey_Roll()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            Roll();
        }
    }
    public void Roll()
    {
        m_pm_Move.Roll();
    }

    // Death
    public void Death()
    {
        if (m_ps_Status.m_sStatus.GetSTATUS_HP_Current() <= 0)
        {
            m_pm_Move.Death();
        }
    }

    // NPC와 상호작용, 채집 등
    public void InputKey_Interaction()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Interaction();
        }
    }
    Collider2D[] co2_3;
    public void Interaction()
    {
        if (m_nPosValue == 1)
        {
            co2_3 = Physics2D.OverlapBoxAll(transform.position + new Vector3(0.125f, 0.15f, 0), m_vSize, 0, nLayer3);
        }
        else
        {
            co2_3 = Physics2D.OverlapBoxAll(transform.position + new Vector3(-0.125f, 0.15f, 0), m_vSize, 0, nLayer3);
        }

        if (co2_3.Length > 0)
        {
            for (int i = 0; i < co2_3.Length; i++)
            {
                GUIManager_Total.Instance.Interaction(co2_3[i].gameObject.GetComponent<NPC_Total>());
                break;
            }
        }
    }

    // 아이템 줍기
    public void InputKey_GetItem()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            GetItem();
        }
    }
    Collider2D[] co2_4;
    Vector3 co2_4_Offset = new Vector3(-0.007f, 0.035f, 0);
    Vector3 co2_4_BoxSize = new Vector3(0.13f, 0.05f, 0);
    public void GetItem()
    {
        co2_4 = Physics2D.OverlapBoxAll(this.transform.position + co2_4_Offset, co2_4_BoxSize, 0, nLayer4);
        
        for (int i = 0; i < co2_4.Length; i++)
        {
            GUIManager_Total.Instance.UpdateLog(co2_4[i].gameObject.GetComponent<Item>().m_sItemName + " 을(를) 획득 하였습니다.");
            //Debug.Log(co2_4[i].gameObject.GetComponent<Item>().m_sItemName + " 획득");
            m_pi_Itemslot.GetItem(co2_4[i].gameObject);
            if (co2_4[i].gameObject.GetComponent<Item>().m_eItemtype == ItemType.ETC || co2_4[i].gameObject.GetComponent<Item>().m_eItemtype == ItemType.USE)
                Destroy(co2_4[i].gameObject);
            else
                co2_4[i].gameObject.SetActive(false);
        }
    }

    // Quest창 오픈
    public void InputKey_GUI_Quest()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            GUIManager_Total.Instance.Display_GUI_Quest();
        }
    }

    // Loop창 오픈
    public void InputKey_GUI_Loop()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            GUIManager_Total.Instance.Display_GUI_Loop();
        }
    }

    // Itemslot 오픈
    public void InputKey_GUI_Itemslot()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            GUIManager_Total.Instance.Display_GUI_Itemslot();
        }
    }

    // Equipslot 오픈
    public void InputKey_GUI_Equipslot()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            GUIManager_Total.Instance.Display_GUI_Equipslot();
        }
    }

    // Status 오픈
    public void InputKey_GUI_Status()
    {
        if (Input.GetKeyUp(KeyCode.O))
        {
            GUIManager_Total.Instance.Display_GUI_Status();
        }
    }

    // 퀘스트 처리 관련 함수
    public void AddQuest(Quest quest)
    {
        m_pq_Quest.AddQuest(quest);
    }
    public void RemoveQuest(Quest quest)
    {
        m_pq_Quest.RemoveQuest(quest);
    }
    public void GetQuestReward(Quest quest)
    {
        m_pq_Quest.GetQuestReward(quest);
        m_ps_Status.GetQuestReward(quest);
        m_pi_Itemslot.GetQuestReward(quest);
        m_pi_Itemslot.DeleteCollectItem(quest);
    }


    public void InputKey_ESC()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //GUIManager_Total.Instance.ControlGUI_Interaction_Exit();
        }
    }

    // Test
    public void InputKey_F1()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            m_ps_Status.Respone();
            m_pm_Move.Respone();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (m_nPosValue == 1)
            Gizmos.DrawWireCube(this.transform.position + new Vector3(0.125f, 0.15f, 0), m_vSize);
        else
            Gizmos.DrawWireCube(this.transform.position + new Vector3(-0.125f, 0.15f, 0), m_vSize);

        Gizmos.DrawWireSphere(this.transform.position, 0.5f);
    }

    // Animation Test
    void AnimationTest()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            Debug.Log("Test: Player: IDLE");
            m_pm_Move.SetAnimatorParameters("Idle");
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            Debug.Log("Test: Player: RUN");
            m_pm_Move.SetAnimatorParameters("Run");
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            Debug.Log("Test: Player: ATTACK1_1");
            m_pm_Move.SetAnimatorParameters("Attack1_1");
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            Debug.Log("Test: Player: ATTACK1_2");
            m_pm_Move.SetAnimatorParameters("Attack1_2");
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            Debug.Log("Test: Player: ATTACK1_3");
            m_pm_Move.SetAnimatorParameters("Attack1_3");
        }
        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            Debug.Log("Test: Player: ATTACKED");
            m_pm_Move.SetAnimatorParameters("Attacked");
        }
        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            Debug.Log("Test: Player: DEATH");
            m_pm_Move.SetAnimatorParameters("Death");
        }
        if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            Debug.Log("Test: Player: ROLL");
            m_pm_Move.SetAnimatorParameters("Roll");
        }
        if (Input.GetKeyUp(KeyCode.Alpha9))
        {
            Debug.Log("Test: Player: GOAWAY");
            m_pm_Move.SetAnimatorParameters("Goaway");
        }
    }
}