using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Itemslot : MonoBehaviour
{
    // 아이템 슬롯
    public GameObject[] m_gary_Itemslot_Equip;
    public int[] m_nary_Itemslot_Equip_Count;
    public GameObject[] m_gary_Itemslot_Use;
    public int[] m_nary_Itemslot_Use_Count;
    public GameObject[] m_gary_Itemslot_Etc;
    public int[] m_nary_Itemslot_Etc_Count;
    // 아이템 보유 최대치
    int m_nMaxCount = 10;

    private void Awake()
    {
        m_gary_Itemslot_Equip = new GameObject[60];
        m_nary_Itemslot_Equip_Count = new int[60];
        m_gary_Itemslot_Use = new GameObject[60];
        m_nary_Itemslot_Use_Count = new int[60];
        m_gary_Itemslot_Etc = new GameObject[60];
        m_nary_Itemslot_Etc_Count = new int[60];
        for (int i = 0; i < 60; i++)
        {
            m_nary_Itemslot_Etc_Count[i] = 0;
        }
    }

    public void GetItem(GameObject item)
    {
        switch (item.GetComponent<Item>().m_eItemtype)
        {
            case ItemType.EQUIP:
                {
                    for (int i = 0; i < 60; i++)
                    {
                        if (m_gary_Itemslot_Equip[i] == null)
                        {
                            m_gary_Itemslot_Equip[i] = item;//GetCloneItem(item);
                            m_nary_Itemslot_Equip_Count[i] = 1;
                            break;
                        }
                    }
                }
                break;
            case ItemType.USE:
                {
                    bool Have = false;
                    int arynum_have = -1;
                    int arynum_null = -1;
                    for (int i = 0; i < 60; i++)
                    {
                        if (m_gary_Itemslot_Use[i] == null)
                        {
                            if (arynum_null == -1)
                            {
                                arynum_null = i;
                            }
                        }
                        else
                        {
                            if (m_gary_Itemslot_Use[i].name == item.name && m_nary_Itemslot_Use_Count[i] < m_nMaxCount)
                            {
                                Have = true;
                                arynum_have = i;
                                break;
                            }
                            else
                                continue;
                        }
                    }
                    if (Have == false)
                    {
                        m_gary_Itemslot_Use[arynum_null] = GetCloneItem(item);
                        m_nary_Itemslot_Use_Count[arynum_null] += 1;
                    }
                    else
                    {
                        m_nary_Itemslot_Use_Count[arynum_have] += 1;
                    }
                }
                break;
            case ItemType.ETC:
                {
                    bool Have = false;
                    int arynum_have = -1;
                    int arynum_null = -1;
                    for (int i = 0; i < 60; i++)
                    {
                        if (m_gary_Itemslot_Etc[i] == null)
                        {
                            if (arynum_null == -1)
                            {
                                arynum_null = i;
                            }
                        }
                        else
                        {
                            if (m_gary_Itemslot_Etc[i].name == item.name && m_nary_Itemslot_Etc_Count[i] < m_nMaxCount)
                            {
                                Have = true;
                                arynum_have = i;
                                break;
                            }
                            else
                                continue;
                        }
                    }
                    if (Have == false)
                    {
                        m_gary_Itemslot_Etc[arynum_null] = GetCloneItem(item);
                        m_nary_Itemslot_Etc_Count[arynum_null] += 1;
                    }
                    else
                    {
                        m_nary_Itemslot_Etc_Count[arynum_have] += 1;
                    }
                }
                break;
        }
    }

    GameObject copyobj;
    public void GetQuestReward(Quest quest)
    {
        for (int i = 0; i < quest.m_lRewardList.Count; i++)
        {
            for (int a = 0; a < 60; a++)
            {
                if (quest.m_lRewardList[i].GetComponent<Item>().m_eItemtype == ItemType.EQUIP)
                {
                    if (m_gary_Itemslot_Equip[a] == null)
                    {
                        copyobj = Instantiate(quest.m_lRewardList[i]);
                        copyobj.transform.position = QuestManager.Instance.transform.position;
                        m_gary_Itemslot_Equip[a] = copyobj;
                        m_nary_Itemslot_Equip_Count[a] = 1;
                        break;
                    }
                }
                else if (quest.m_lRewardList[i].GetComponent<Item>().m_eItemtype == ItemType.USE)
                {
                    if (m_gary_Itemslot_Use[a] == null)
                    {
                        if (m_gary_Itemslot_Use[a].GetComponent<Item>().m_nItemCode == quest.m_nl_ItemCode[i])
                        {
                            if (m_nary_Itemslot_Use_Count[a] > quest.m_nl_ItemCount_Max[i])
                                m_nary_Itemslot_Use_Count[a] -= quest.m_nl_ItemCount_Max[i];
                            else
                                m_nary_Itemslot_Use_Count[a] = 0;
                        }
                        break;
                    }
                }
                else
                {
                    if (m_gary_Itemslot_Etc[a] == null && m_nary_Itemslot_Etc_Count[a] < m_nMaxCount)
                    {
                        m_gary_Itemslot_Etc[a] = quest.m_lRewardList[i];
                        m_nary_Itemslot_Etc_Count[a]++;
                        break;
                    }
                }
            }
        }
    }

    public void DeleteCollectItem(Quest quest)
    {
        if (quest.m_eQuestType == Quest.E_QUEST_TYPE.COLLECT)
        {
            for (int i = 0; i < quest.m_nl_ItemCode.Count; i++)
            {
                for (int a = 0; a < 60; a++)
                {
                    if (m_gary_Itemslot_Equip[a] != null)
                    {
                        if (m_gary_Itemslot_Equip[a].GetComponent<Item>().m_nItemCode == quest.m_nl_ItemCode[i])
                        {
                            m_nary_Itemslot_Equip_Count[a] -= quest.m_nl_ItemCount_Max[i];
                            break;
                        }
                    }
                    if (m_gary_Itemslot_Use[a] != null)
                    {
                        if (m_gary_Itemslot_Use[a].GetComponent<Item>().m_nItemCode == quest.m_nl_ItemCode[i])
                        {
                            m_nary_Itemslot_Use_Count[a] -= quest.m_nl_ItemCount_Max[i];
                            break;
                        }
                    }
                    if (m_gary_Itemslot_Etc[a] != null && m_nary_Itemslot_Etc_Count[a] >= quest.m_nl_ItemCount_Max[i])
                    {
                        if (m_gary_Itemslot_Etc[a].GetComponent<Item>().m_nItemCode == quest.m_nl_ItemCode[i])
                        {
                            m_nary_Itemslot_Etc_Count[a] -= quest.m_nl_ItemCount_Max[i];
                            break;
                        }
                    }
                }
            }
        }
    }

    public GameObject GetCloneItem(GameObject item)
    {
        GameObject ReturnITem;
        if (item.GetComponent<Item>().m_eItemtype == ItemType.ETC)
        {
            ReturnITem = Resources.Load("Prefab/Item/Item_Etc/" + item.name) as GameObject;
        }
        else if (item.GetComponent<Item>().m_eItemtype == ItemType.USE)
        {
            ReturnITem = Resources.Load("Prefab/Item/Item_Use/" + item.name) as GameObject;
        }
        else
        {
            ReturnITem = Resources.Load("Prefab/Item/Item_Equip/" + item.name) as GameObject;
        }

        return ReturnITem;
    }
}
