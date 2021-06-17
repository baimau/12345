using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{

    public AnimationCurve m_Curve = null;
    public Vector3 m_StartPos;
    public Vector3 m_EndPos;
    public bool m_Moving = false;
    public float m_Time = 0;

    public bool CheckMoving()
    {
        return m_Moving;
    }


    public void SetEndPos(Vector3 pos)
    {
        m_StartPos = transform.localPosition;
        m_EndPos = pos;
        m_Time = 0;
        m_Moving = true;
    }

    private void update()
    {
        if (m_Moving)
        {
            m_Time += (Time.deltaTime*2);
            float posX = Mathf.Lerp(m_StartPos.x, m_EndPos.x, m_Time);
            float value = m_Curve.Evaluate(m_Time);
            float math1 = m_EndPos.y - m_StartPos.y;
            float math2 = value * math1;
            float posY = m_StartPos.y + math2;
            transform.localPosition = new Vector3(posX, posY, 0);
            if(m_Time > 1)
            {
                transform.localPosition = m_EndPos;
                m_Moving = false;
            }
        }
    }


}
