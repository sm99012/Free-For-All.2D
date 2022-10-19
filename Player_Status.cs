using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Status : MonoBehaviour
{
    public SOC m_sSoc;
    public SOC m_sSoc_Origin;
    public SOC m_sSoc_Extra_Equip_Hat;
    public SOC m_sSoc_Extra_Equip_Top;
    public SOC m_sSoc_Extra_Equip_Bottoms;
    public SOC m_sSoc_Extra_Equip_Shose;
    public SOC m_sSoc_Extra_Equip_Gloves;
    public SOC m_sSoc_Extra_Equip_Mainweapon;
    public SOC m_sSoc_Extra_Equip_Subweapon;

    public STATUS m_sStatus; // Total Status / m_sStatus_Origin + m_sStatus_Extra
    public STATUS m_sStatus_Origin;
    public STATUS m_sStatus_Extra_Equip_Hat;
    public STATUS m_sStatus_Extra_Equip_Top;
    public STATUS m_sStatus_Extra_Equip_Bottoms;
    public STATUS m_sStatus_Extra_Equip_Shose;
    public STATUS m_sStatus_Extra_Equip_Gloves;
    public STATUS m_sStatus_Extra_Equip_Mainweapon;
    public STATUS m_sStatus_Extra_Equip_Subweapon;

    private void Start()
    {
        InitialSet_Status();
        InitialSet_SOC();
    }

    public void InitialSet_Status()
    {
        // 기본 스탯(공속 0.1)
        // 공속의 경우 ATTACK1_1(0.5f), ATTACK1_2(0.5f), ATTACK1_3(1.0f) 의 연계 조건시간이 필요.
        // 현 공속이 만약 1이라면 ATTACK1_1 에서 ATTACK1_2로의 공격 연계는 불가능.
        m_sStatus_Origin = new STATUS(1, 10, 0, 10, 10, 0, 0, 1, 1, 1, 0, 100, 100, 0, 0, 0, 0.1f);
        m_sStatus = new STATUS();
        m_sStatus.SetSTATUS(m_sStatus_Origin);

    }
    public void InitialSet_SOC()
    {
        m_sSoc = new SOC();
        m_sSoc_Origin = new SOC();
        m_sSoc_Origin.SetSOC(m_sSoc);
    }

    int m_nTotalDamage;
    public void Attacked(int damage)
    {
        m_nTotalDamage = damage - m_sStatus.GetSTATUS_Defence_Physical();
        if (m_nTotalDamage <= 0)
            m_nTotalDamage = 1;

        if (m_sStatus.GetSTATUS_HP_Current() - m_nTotalDamage > 0)
            m_sStatus.P_OperatorSTATUS_HP_Current(-m_nTotalDamage);
        else
            m_sStatus.SetSTATUS_HP_Current(0);

        //GUIManager_Total.Instance.UpdateLog("HP: " + (-damage).ToString());
    }

    public void Goaway(SOC soc)
    {
        m_sSoc_Origin.P_OperatorSOC(soc);
        UpdateSOC();
    }

    public void MobDeath(SOC soc, STATUS status)
    {
        m_sSoc_Origin.P_OperatorSOC(soc);
        UpdateSOC();

        CarculateEXP(status);
        //GUIManager_Total.Instance.UpdateLog("+EXP: " + status.GetSTATUS_EXP_Current());
    }
    int stexp;
    void CarculateEXP(STATUS status)
    {
        // 획득 경험치
        stexp = status.GetSTATUS_EXP_Current();
        while (m_sStatus.GetSTATUS_EXP_Current() + stexp >= m_sStatus.GetSTATUS_EXP_Max())
        {
            stexp = m_sStatus.GetSTATUS_EXP_Current() + stexp - m_sStatus.GetSTATUS_EXP_Max();
            CarculateLV();
        }
        if (stexp > 0)
        {
            m_sStatus.P_OperatorSTATUS_EXP_Current(stexp);
        }
    }
    int m_nLV = 0;
    // LV 업
    void CarculateLV()
    {
        m_nLV++;
        m_sStatus_Origin.P_OperatorSTATUS_LV(1);
        if (m_nLV % 3 == 0)
        {
            m_sStatus_Origin.P_OperatorSTATUS_Damage_Total(1);
            m_sStatus_Origin.P_OperatorSTATUS_Damage_Physical(1);
            m_sStatus_Origin.P_OperatorSTATUS_Damage_Magical(1);
            m_sStatus_Origin.P_OperatorSTATUS_Defence_Physical(1);
            m_sStatus_Origin.P_OperatorSTATUS_Defence_Magical(1);
        }
        // 경험치통은 2배로 늘어난다???
        m_sStatus_Origin.M_OperatorSTATUS_EXP_Max(1.3f);
        m_sStatus_Origin.SetSTATUS_EXP_Current(0);

        m_sStatus_Origin.P_OperatorSTATUS_HP_Max(1);
        //m_sStatus_Origin.SetSTATUS_HP_Current(m_sStatus.GetSTATUS_HP_Max());
        m_sStatus_Origin.P_OperatorSTATUS_MP_Max(1);
        //m_sStatus_Origin.SetSTATUS_MP_Current(m_sStatus.GetSTATUS_MP_Max());

        UpdateStatus_LVup();

        m_sStatus.SetSTATUS_HP_Current(m_sStatus.GetSTATUS_HP_Max());
        m_sStatus.SetSTATUS_MP_Current(m_sStatus.GetSTATUS_MP_Max());
    }
    STATUS m_sStatusBefore;
    int m_nEXP_Current;
    int m_nHP_Current;
    int m_nMP_Current;

    // 장비 착용으로인한 스탯변경
    public void UpdateStatus_Equip()
    {
        // Status
        m_nEXP_Current = m_sStatus.GetSTATUS_EXP_Current();
        m_nHP_Current = m_sStatus.GetSTATUS_HP_Current();
        m_nMP_Current = m_sStatus.GetSTATUS_MP_Current();

        m_sStatus.SetSTATUS_Zero();
        m_sStatus.P_OperatorSTATUS(m_sStatus_Origin);
        m_sStatus.P_OperatorSTATUS(m_sStatus_Extra_Equip_Hat);
        m_sStatus.P_OperatorSTATUS(m_sStatus_Extra_Equip_Top);
        m_sStatus.P_OperatorSTATUS(m_sStatus_Extra_Equip_Bottoms);
        m_sStatus.P_OperatorSTATUS(m_sStatus_Extra_Equip_Shose);
        m_sStatus.P_OperatorSTATUS(m_sStatus_Extra_Equip_Gloves);
        m_sStatus.P_OperatorSTATUS(m_sStatus_Extra_Equip_Mainweapon);
        m_sStatus.P_OperatorSTATUS(m_sStatus_Extra_Equip_Subweapon);

        m_sStatus.SetSTATUS_EXP_Current(m_nEXP_Current);
        m_sStatus.SetSTATUS_HP_Current(m_nHP_Current);
        m_sStatus.SetSTATUS_MP_Current(m_nMP_Current);

        CheckLogic();

        // Soc
        m_sSoc.SetSOC_Zero();
    }
    
    // 레벨업 으로인한 스탯변경
    public void UpdateStatus_LVup()
    {
        m_sStatus.SetSTATUS_Zero();
        m_sStatus.P_OperatorSTATUS(m_sStatus_Origin);
        m_sStatus.P_OperatorSTATUS(m_sStatus_Extra_Equip_Hat);
        m_sStatus.P_OperatorSTATUS(m_sStatus_Extra_Equip_Top);
        m_sStatus.P_OperatorSTATUS(m_sStatus_Extra_Equip_Bottoms);
        m_sStatus.P_OperatorSTATUS(m_sStatus_Extra_Equip_Shose);
        m_sStatus.P_OperatorSTATUS(m_sStatus_Extra_Equip_Gloves);
        m_sStatus.P_OperatorSTATUS(m_sStatus_Extra_Equip_Mainweapon);
        m_sStatus.P_OperatorSTATUS(m_sStatus_Extra_Equip_Subweapon);

        m_sStatus.SetSTATUS_EXP_Current(m_nEXP_Current);
        m_sStatus.SetSTATUS_HP_Current(m_nHP_Current);
        m_sStatus.SetSTATUS_MP_Current(m_nMP_Current);

        CheckLogic();
    }

    // Monster Kill, Goaway 로 인한 SOC(평판) 변경
    public void UpdateSOC()
    {
        m_sSoc.SetSOC_Zero();
        m_sSoc.P_OperatorSOC(m_sSoc_Origin);
        m_sSoc.P_OperatorSOC(m_sSoc_Extra_Equip_Hat);
        m_sSoc.P_OperatorSOC(m_sSoc_Extra_Equip_Top);
        m_sSoc.P_OperatorSOC(m_sSoc_Extra_Equip_Bottoms);
        m_sSoc.P_OperatorSOC(m_sSoc_Extra_Equip_Shose);
        m_sSoc.P_OperatorSOC(m_sSoc_Extra_Equip_Gloves);
        m_sSoc.P_OperatorSOC(m_sSoc_Extra_Equip_Mainweapon);
        m_sSoc.P_OperatorSOC(m_sSoc_Extra_Equip_Subweapon);
    }

    public void GetQuestReward(Quest quest)
    {
        CarculateEXP(quest.m_sRewardSTATUS);
        m_sSoc_Origin.P_OperatorSOC(quest.m_sRewardSOC);
        UpdateSOC();
    }

    // 논리(조건) 체크
    void CheckLogic()
    {
        if (m_sStatus.GetSTATUS_HP_Current() > m_sStatus.GetSTATUS_HP_Max())
        {
            m_sStatus.SetSTATUS_HP_Current(m_sStatus.GetSTATUS_HP_Max());
        }

        if (m_sStatus.GetSTATUS_MP_Current() > m_sStatus.GetSTATUS_MP_Max())
        {
            m_sStatus.SetSTATUS_MP_Current(m_sStatus.GetSTATUS_MP_Max());
        }
    }

    
    public void Respone()
    {
        m_sStatus.SetSTATUS_HP_Current(m_sStatus.GetSTATUS_HP_Max());
    }
}

