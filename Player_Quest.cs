using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Player_Quest : MonoBehaviour
{
    // 진행중인 퀘스트
    public List <Quest> m_lQuestList_Progress;
    // 완료한 퀘스트
    public List <Quest> m_lQuestList_Complete;

    private void Start()
    {
        m_lQuestList_Progress = new List<Quest>();
        m_lQuestList_Complete = new List<Quest>();
    }

    // 토벌 퀘스트
    public void QuestUpdate_Kill(Monster_Status.E_MONSTER_KIND mk, int code)
    {
        //Stopwatch watch = new Stopwatch();
        //watch.Start();
        for (int i = 0; i < m_lQuestList_Progress.Count; i++)
        {
            // 몬스터 토벌 퀘스트_타입
            if (m_lQuestList_Progress[i].m_eQuestType == Quest.E_QUEST_TYPE.KILL_TYPE)
            {
                if (m_lQuestList_Progress[i].m_bCondition == false)
                {
                    if (m_lQuestList_Progress[i].Check_KILL_TYPE(mk) == true)
                    {
                        //GUIManager_Total.Instance.UpdateLog("[" + m_lQuestList_Progress[i].m_sQuest_Title + "] " + m_lQuestList_Progress[i].m_nCount_Current + " / " + m_lQuestList_Progress[i].m_nCount_Max + "\n");
                    }
                    if (m_lQuestList_Progress[i].m_bCondition == true)
                    {
                        //GUIManager_Total.Instance.UpdateLog("[" + m_lQuestList_Progress[i].m_sQuest_Title + "] 완료\n");

                    }
                }
            }
            // 몬스터 토벌 퀘스트_몬스터
            else if (m_lQuestList_Progress[i].m_eQuestType == Quest.E_QUEST_TYPE.KILL_MONSTER)
            {
                if (m_lQuestList_Progress[i].m_bCondition == false)
                {
                    if (m_lQuestList_Progress[i].Check_KILL_MONSTER(code) == true)
                    {
                        //GUIManager_Total.Instance.UpdateLog("[" + m_lQuestList_Progress[i].m_sQuest_Title + "] " + m_lQuestList_Progress[i].m_nCount_Current + " / " + m_lQuestList_Progress[i].m_nCount_Max + "\n");
                    }
                    if (m_lQuestList_Progress[i].m_bCondition == true)
                    {
                        //GUIManager_Total.Instance.UpdateLog("[" + m_lQuestList_Progress[i].m_sQuest_Title + "] 완료\n");

                    }
                }
            }
        }
        //watch.Stop();
        //GUIManager_Total.Instance.UpdateLog(watch.ElapsedMilliseconds + " ms");
    }

    // GOAWAY 퀘스트
    public void QuestUpdate_Goaway(Monster_Status.E_MONSTER_KIND mk, int code)
    {
        for (int i = 0; i < m_lQuestList_Progress.Count; i++)
        {
            // Goaway 퀘스트_타입
            if (m_lQuestList_Progress[i].m_eQuestType == Quest.E_QUEST_TYPE.GOAWAY_TYPE)
            {
                if (m_lQuestList_Progress[i].m_bCondition == false)
                {
                    if (m_lQuestList_Progress[i].Check_GOAWAY_TYPE(mk) == true)
                    {
                        //GUIManager_Total.Instance.UpdateLog("[" + m_lQuestList_Progress[i].m_sQuest_Title + "] " + m_lQuestList_Progress[i].m_nCount_Current + " / " + m_lQuestList_Progress[i].m_nCount_Max + "\n");
                    }
                    if (m_lQuestList_Progress[i].m_bCondition == true)
                    {
                        //GUIManager_Total.Instance.UpdateLog("[" + m_lQuestList_Progress[i].m_sQuest_Title + "] 완료\n");

                    }
                }
            }
            // Goaway 퀘스트_몬스터
            else if (m_lQuestList_Progress[i].m_eQuestType == Quest.E_QUEST_TYPE.GOAWAY_MONSTER)
            {
                if (m_lQuestList_Progress[i].m_bCondition == false)
                {
                    if (m_lQuestList_Progress[i].Check_GOAWAY_MONSTER(code) == true)
                    {
                        //GUIManager_Total.Instance.UpdateLog("[" + m_lQuestList_Progress[i].m_sQuest_Title + "] " + m_lQuestList_Progress[i].m_nCount_Current + " / " + m_lQuestList_Progress[i].m_nCount_Max + "\n");
                    }
                    if (m_lQuestList_Progress[i].m_bCondition == true)
                    {
                        //GUIManager_Total.Instance.UpdateLog("[" + m_lQuestList_Progress[i].m_sQuest_Title + "] 완료\n");

                    }
                }
            }
        }
    }

    // COLLECT 퀘스트
    public void QuestUpdate_Collect()
    {
        for (int i = 0; i < m_lQuestList_Progress.Count; i++)
        {
            // Collect 퀘스트_타입
            if (m_lQuestList_Progress[i].m_eQuestType == Quest.E_QUEST_TYPE.COLLECT)
            {
                if (m_lQuestList_Progress[i].m_bCondition == false)
                {
                    if (m_lQuestList_Progress[i].Check_COLLECT() == true)
                    {
                        //GUIManager_Total.Instance.UpdateLog("[" + m_lQuestList_Progress[i].m_sQuest_Title + "] " + m_lQuestList_Progress[i].m_nCount_Current + " / " + m_lQuestList_Progress[i].m_nCount_Max + "\n");
                    }
                }
            }
        }
    }


    public void AddQuest(Quest quest)
    {
        m_lQuestList_Progress.Add(quest);
        quest.m_bProcess = true;
        quest.m_bClear = false;
    }
    public void RemoveQuest(Quest quest)
    {
        for (int i = 0; i < m_lQuestList_Progress.Count; i++)
        {
            if (m_lQuestList_Progress[i] == quest)
            {
                m_lQuestList_Progress.RemoveAt(i);
                quest.m_bProcess = false;
                quest.m_bClear = true;
                break;
            }
        }
    }
    public void InitQuestAll()
    {
        m_lQuestList_Progress.Clear();
        m_lQuestList_Complete.Clear();
    }
    public void GetQuestReward(Quest quest)
    {
        for (int i = 0; i < m_lQuestList_Progress.Count; i++)
        {
            if (m_lQuestList_Progress[i].m_nQuest_Code == quest.m_nQuest_Code)
            {
                m_lQuestList_Progress[i].m_nClearDay = GUIManager_Total.Instance.GetDay();  
                m_lQuestList_Complete.Add(quest);
                m_lQuestList_Progress.RemoveAt(i);
                GUIManager_Total.Instance.UpdateLog(quest.m_sQuest_Title + "클리어");
                break;
            }
        }
    }
}
