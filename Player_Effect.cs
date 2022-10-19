using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Effect : MonoBehaviour
{
    public List<GameObject> m_l_EffectList;

    void Start()
    {
        m_l_EffectList = new List<GameObject>();
        GameObject Effect = Resources.Load("Prefab/Effect/Effect1") as GameObject;
        m_l_EffectList.Add(Effect);
        Effect = Resources.Load("Prefab/Effect/Effect_Yellow_1") as GameObject;
        m_l_EffectList.Add(Effect);
        Effect = Resources.Load("Prefab/Effect/Effect_Yellow_2") as GameObject;
        m_l_EffectList.Add(Effect);
        Effect = Resources.Load("Prefab/Effect/Effect_Yellow_3") as GameObject;
        m_l_EffectList.Add(Effect);
        Effect = Resources.Load("Prefab/Effect/Effect_Yellow_4") as GameObject;
        m_l_EffectList.Add(Effect);
    }
    public void Effect1(Vector3 pos)
    {
        StartCoroutine(ProcessEffect1(pos));
    }
    IEnumerator ProcessEffect1(Vector3 pos)
    {
        yield return new WaitForSeconds(0.1f);
        if (m_l_EffectList[0] != null)
        {
            GameObject efc = Instantiate(m_l_EffectList[0]);
            efc.transform.position = pos;
        }
    }

    public void Effect2(Vector3 pos)
    {
        StartCoroutine(ProcessEffect2(pos));
    }
    IEnumerator ProcessEffect2(Vector3 pos)
    {
        yield return new WaitForSeconds(0.1f);
        if (m_l_EffectList[1] != null)
        {
            GameObject efc = Instantiate(m_l_EffectList[1]);
            efc.transform.position = pos;
        }
    }

    public void Effect3(Vector3 pos)
    {
        StartCoroutine(ProcessEffect3(pos));
    }
    IEnumerator ProcessEffect3(Vector3 pos)
    {
        yield return new WaitForSeconds(0.1f);
        if (m_l_EffectList[1] != null)
        {
            GameObject efc = Instantiate(m_l_EffectList[1]);
            efc.transform.position = pos;
        }
    }

    public void Effect4(Vector3 pos)
    {
        StartCoroutine(ProcessEffect4(pos));
    }
    IEnumerator ProcessEffect4(Vector3 pos)
    {
        yield return new WaitForSeconds(0.1f);
        if (m_l_EffectList[3] != null)
        {
            GameObject efc = Instantiate(m_l_EffectList[3]);
            efc.transform.position = pos;
        }
    }

    public void Effect5(Vector3 pos)
    {
        StartCoroutine(ProcessEffect5(pos));
    }
    IEnumerator ProcessEffect5(Vector3 pos)
    {
        yield return new WaitForSeconds(0.1f);
        if (m_l_EffectList[4] != null)
        {
            GameObject efc = Instantiate(m_l_EffectList[4]);
            efc.transform.position = pos;
        }
    }
}
