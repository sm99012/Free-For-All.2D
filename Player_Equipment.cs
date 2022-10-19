using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Equipment : MonoBehaviour
{
    public GameObject m_gEquipment_Hat;
    public GameObject m_gEquipment_Top;
    public GameObject m_gEquipment_Bottoms;
    public GameObject m_gEquipment_Shose;
    public GameObject m_gEquipment_Gloves;
    public GameObject m_gEquipment_Mainweapon;
    public GameObject m_gEquipment_Subweapon;

    STATUS m_sStatus_Effect;
    SOC m_sSoc_Effect;

    private void Start()
    {
        m_gEquipment_Hat = null;
        m_gEquipment_Top = null;
        m_gEquipment_Bottoms = null;
        m_gEquipment_Shose = null;
        m_gEquipment_Gloves = null;
        m_gEquipment_Mainweapon = null;
        m_gEquipment_Subweapon = null;

        m_sStatus_Effect = new STATUS();
        m_sSoc_Effect = new SOC();
    }

    public STATUS UpdateEquipmentStatus()
    {
        InitStatus();
        if (m_gEquipment_Hat != null)
        {
            m_sStatus_Effect.P_OperatorSTATUS(m_gEquipment_Hat.GetComponent<Item_Equip>().m_sStatus_Effect);
        }
        if (m_gEquipment_Top != null)
        {
            m_sStatus_Effect.P_OperatorSTATUS(m_gEquipment_Top.GetComponent<Item_Equip>().m_sStatus_Effect);
        }
        if (m_gEquipment_Bottoms != null)
        {
            m_sStatus_Effect.P_OperatorSTATUS(m_gEquipment_Bottoms.GetComponent<Item_Equip>().m_sStatus_Effect);
        }
        if (m_gEquipment_Shose != null)
        {
            m_sStatus_Effect.P_OperatorSTATUS(m_gEquipment_Shose.GetComponent<Item_Equip>().m_sStatus_Effect);
        }
        if (m_gEquipment_Gloves != null)
        {
            m_sStatus_Effect.P_OperatorSTATUS(m_gEquipment_Gloves.GetComponent<Item_Equip>().m_sStatus_Effect);
        }
        if (m_gEquipment_Mainweapon != null)
        {
            m_sStatus_Effect.P_OperatorSTATUS(m_gEquipment_Mainweapon.GetComponent<Item_Equip>().m_sStatus_Effect);
        }
        if (m_gEquipment_Subweapon != null)
        {
            m_sStatus_Effect.P_OperatorSTATUS(m_gEquipment_Subweapon.GetComponent<Item_Equip>().m_sStatus_Effect);
        }

        return m_sStatus_Effect;
    }
    void InitStatus()
    {
        m_sStatus_Effect.SetSTATUS_Zero();
    }

    public SOC UpdateEquipmentSoc()
    {
        InitSoc();
        if (m_gEquipment_Hat != null)
        {
            m_sSoc_Effect.P_OperatorSOC(m_gEquipment_Hat.GetComponent<Item_Equip>().m_sSoc_Effect);
        }
        if (m_gEquipment_Top != null)
        {
            m_sSoc_Effect.P_OperatorSOC(m_gEquipment_Top.GetComponent<Item_Equip>().m_sSoc_Effect);
        }
        if (m_gEquipment_Bottoms != null)
        {
            m_sSoc_Effect.P_OperatorSOC(m_gEquipment_Bottoms.GetComponent<Item_Equip>().m_sSoc_Effect);
        }
        if (m_gEquipment_Shose != null)
        {
            m_sSoc_Effect.P_OperatorSOC(m_gEquipment_Shose.GetComponent<Item_Equip>().m_sSoc_Effect);
        }
        if (m_gEquipment_Gloves != null)
        {
            m_sSoc_Effect.P_OperatorSOC(m_gEquipment_Gloves.GetComponent<Item_Equip>().m_sSoc_Effect);
        }
        if (m_gEquipment_Mainweapon != null)
        {
            m_sSoc_Effect.P_OperatorSOC(m_gEquipment_Mainweapon.GetComponent<Item_Equip>().m_sSoc_Effect);
        }
        if (m_gEquipment_Subweapon != null)
        {
            m_sSoc_Effect.P_OperatorSOC(m_gEquipment_Subweapon.GetComponent<Item_Equip>().m_sSoc_Effect);
        }

        return m_sSoc_Effect;
    }
    void InitSoc()
    {
        m_sSoc_Effect.SetSOC_Zero();
    }

    // 장비 착용
    public bool Equip(GameObject item, STATUS playerstatus, SOC playersoc)
    {
        if (CheckCondition_Equip(item.GetComponent<Item_Equip>(), playerstatus, playersoc) == true)
        {
            switch (item.GetComponent<Item_Equip>().m_eItemEquipType)
            {
                case E_ITEM_EQUIP_TYPE.HAT:
                    {
                        m_gEquipment_Hat = item;
                        Player_Total.Instance.m_ps_Status.m_sStatus_Extra_Equip_Hat = m_gEquipment_Hat.gameObject.GetComponent<Item_Equip>().m_sStatus_Effect;
                        Player_Total.Instance.m_ps_Status.m_sSoc_Extra_Equip_Hat = m_gEquipment_Hat.gameObject.GetComponent<Item_Equip>().m_sSoc_Effect;
                        Player_Total.Instance.m_ps_Status.UpdateStatus_Equip();
                        Player_Total.Instance.m_ps_Status.UpdateSOC();
                        Debug.Log("Hat");
                    }
                    break;
                case E_ITEM_EQUIP_TYPE.TOP:
                    {
                        m_gEquipment_Top = item;
                        Player_Total.Instance.m_ps_Status.m_sStatus_Extra_Equip_Top = m_gEquipment_Top.gameObject.GetComponent<Item_Equip>().m_sStatus_Effect;
                        Player_Total.Instance.m_ps_Status.m_sSoc_Extra_Equip_Top = m_gEquipment_Top.gameObject.GetComponent<Item_Equip>().m_sSoc_Effect;
                        Player_Total.Instance.m_ps_Status.UpdateStatus_Equip();
                        Player_Total.Instance.m_ps_Status.UpdateSOC();
                        Debug.Log("Top");
                    }
                    break;
                case E_ITEM_EQUIP_TYPE.BOTTOMS:
                    {
                        m_gEquipment_Bottoms = item;
                        Player_Total.Instance.m_ps_Status.m_sStatus_Extra_Equip_Bottoms = m_gEquipment_Bottoms.gameObject.GetComponent<Item_Equip>().m_sStatus_Effect;
                        Player_Total.Instance.m_ps_Status.m_sSoc_Extra_Equip_Bottoms = m_gEquipment_Bottoms.gameObject.GetComponent<Item_Equip>().m_sSoc_Effect;
                        Player_Total.Instance.m_ps_Status.UpdateStatus_Equip();
                        Player_Total.Instance.m_ps_Status.UpdateSOC();
                        Debug.Log("Bottoms");
                    }
                    break;
                case E_ITEM_EQUIP_TYPE.SHOSE:
                    {
                        m_gEquipment_Shose = item;
                        Player_Total.Instance.m_ps_Status.m_sStatus_Extra_Equip_Shose = m_gEquipment_Shose.gameObject.GetComponent<Item_Equip>().m_sStatus_Effect;
                        Player_Total.Instance.m_ps_Status.m_sSoc_Extra_Equip_Shose = m_gEquipment_Shose.gameObject.GetComponent<Item_Equip>().m_sSoc_Effect;
                        Player_Total.Instance.m_ps_Status.UpdateStatus_Equip();
                        Player_Total.Instance.m_ps_Status.UpdateSOC();
                        Debug.Log("Shose");
                    }
                    break;
                case E_ITEM_EQUIP_TYPE.GLOVES:
                    {
                        m_gEquipment_Gloves = item;
                        Player_Total.Instance.m_ps_Status.m_sStatus_Extra_Equip_Gloves = m_gEquipment_Gloves.gameObject.GetComponent<Item_Equip>().m_sStatus_Effect;
                        Player_Total.Instance.m_ps_Status.m_sSoc_Extra_Equip_Gloves = m_gEquipment_Gloves.gameObject.GetComponent<Item_Equip>().m_sSoc_Effect;
                        Player_Total.Instance.m_ps_Status.UpdateStatus_Equip();
                        Player_Total.Instance.m_ps_Status.UpdateSOC();
                        Debug.Log("Gloves");
                    }
                    break;
                case E_ITEM_EQUIP_TYPE.MAINWEAPON:
                    {
                        m_gEquipment_Mainweapon = item;
                        Player_Total.Instance.m_ps_Status.m_sStatus_Extra_Equip_Mainweapon = m_gEquipment_Mainweapon.gameObject.GetComponent<Item_Equip>().m_sStatus_Effect;
                        Player_Total.Instance.m_ps_Status.m_sSoc_Extra_Equip_Mainweapon = m_gEquipment_Mainweapon.gameObject.GetComponent<Item_Equip>().m_sSoc_Effect;
                        Player_Total.Instance.m_ps_Status.UpdateStatus_Equip();
                        Player_Total.Instance.m_ps_Status.UpdateSOC();
                        Debug.Log("Mainweapon");
                    }
                    break;
                case E_ITEM_EQUIP_TYPE.SUBWEAPON:
                    {
                        m_gEquipment_Subweapon = item;
                        Player_Total.Instance.m_ps_Status.m_sStatus_Extra_Equip_Subweapon = m_gEquipment_Subweapon.gameObject.GetComponent<Item_Equip>().m_sStatus_Effect;
                        Player_Total.Instance.m_ps_Status.m_sSoc_Extra_Equip_Subweapon = m_gEquipment_Subweapon.gameObject.GetComponent<Item_Equip>().m_sSoc_Effect;
                        Player_Total.Instance.m_ps_Status.UpdateStatus_Equip();
                        Player_Total.Instance.m_ps_Status.UpdateSOC();
                        Debug.Log("Subweapon");
                    }
                    break;
            }
            return true;
        }
        else
        {
            Debug.Log("NonEquip");
            return false;
        }
    }

    // 장비착용 해제
    public void NonEquip(E_ITEM_EQUIP_TYPE miet)
    {
        switch(miet)
        {
            case E_ITEM_EQUIP_TYPE.HAT:
                {
                    m_gEquipment_Hat = null;
                    Player_Total.Instance.m_ps_Status.m_sStatus_Extra_Equip_Hat.SetSTATUS_Zero();
                    Player_Total.Instance.m_ps_Status.m_sSoc_Extra_Equip_Hat.SetSOC_Zero();
                    Player_Total.Instance.m_ps_Status.UpdateStatus_Equip();
                    Player_Total.Instance.m_ps_Status.UpdateSOC();
                } break;
            case E_ITEM_EQUIP_TYPE.TOP:
                {
                    m_gEquipment_Top = null;
                    Player_Total.Instance.m_ps_Status.m_sStatus_Extra_Equip_Top.SetSTATUS_Zero();
                    Player_Total.Instance.m_ps_Status.m_sSoc_Extra_Equip_Top.SetSOC_Zero();
                    Player_Total.Instance.m_ps_Status.UpdateStatus_Equip();
                    Player_Total.Instance.m_ps_Status.UpdateSOC();
                }
                break;
            case E_ITEM_EQUIP_TYPE.BOTTOMS:
                {
                    m_gEquipment_Bottoms = null;
                    Player_Total.Instance.m_ps_Status.m_sStatus_Extra_Equip_Bottoms.SetSTATUS_Zero();
                    Player_Total.Instance.m_ps_Status.m_sSoc_Extra_Equip_Bottoms.SetSOC_Zero();
                    Player_Total.Instance.m_ps_Status.UpdateStatus_Equip();
                    Player_Total.Instance.m_ps_Status.UpdateSOC();
                }
                break;
            case E_ITEM_EQUIP_TYPE.SHOSE:
                {
                    m_gEquipment_Shose = null;
                    Player_Total.Instance.m_ps_Status.m_sStatus_Extra_Equip_Shose.SetSTATUS_Zero();
                    Player_Total.Instance.m_ps_Status.m_sSoc_Extra_Equip_Shose.SetSOC_Zero();
                    Player_Total.Instance.m_ps_Status.UpdateStatus_Equip();
                    Player_Total.Instance.m_ps_Status.UpdateSOC();
                }
                break;
            case E_ITEM_EQUIP_TYPE.GLOVES:
                {
                    m_gEquipment_Gloves = null;
                    Player_Total.Instance.m_ps_Status.m_sStatus_Extra_Equip_Gloves.SetSTATUS_Zero();
                    Player_Total.Instance.m_ps_Status.m_sSoc_Extra_Equip_Gloves.SetSOC_Zero();
                    Player_Total.Instance.m_ps_Status.UpdateStatus_Equip();
                    Player_Total.Instance.m_ps_Status.UpdateSOC();
                }
                break;
            case E_ITEM_EQUIP_TYPE.MAINWEAPON:
                {
                    m_gEquipment_Mainweapon = null;
                    Player_Total.Instance.m_ps_Status.m_sStatus_Extra_Equip_Mainweapon.SetSTATUS_Zero();
                    Player_Total.Instance.m_ps_Status.m_sSoc_Extra_Equip_Mainweapon.SetSOC_Zero();
                    Player_Total.Instance.m_ps_Status.UpdateStatus_Equip();
                    Player_Total.Instance.m_ps_Status.UpdateSOC();
                }
                break;
            case E_ITEM_EQUIP_TYPE.SUBWEAPON:
                {
                    m_gEquipment_Subweapon = null;
                    Player_Total.Instance.m_ps_Status.m_sStatus_Extra_Equip_Subweapon.SetSTATUS_Zero();
                    Player_Total.Instance.m_ps_Status.m_sSoc_Extra_Equip_Subweapon.SetSOC_Zero();
                    Player_Total.Instance.m_ps_Status.UpdateStatus_Equip();
                    Player_Total.Instance.m_ps_Status.UpdateSOC();
                }
                break;
        }
    }

    // 장비 착용 조건 체크
    public bool CheckCondition_Equip(Item_Equip item, STATUS playerstatus, SOC playersoc)
    {
        if (playerstatus.CheckCondition_Max(item.m_sStatus_Limit_Max) == false)
        {
            Debug.Log("Status_Max");
            return false;
        }
        if (playerstatus.CheckCondition_Min(item.m_sStatus_Limit_Min) == false)
        {
            Debug.Log("Status_Min");
            return false;
        }
        if (playersoc.CheckCondition_Max(item.m_sSoc_Limit_Max) == false)
        {
            Debug.Log("Soc_Max");
            return false;
        }
        if (playersoc.CheckCondition_Min(item.m_sSoc_Limit_Min) == false)
        {
            Debug.Log("Soc_Min");
            return false;
        }

        return true;
    }
}
