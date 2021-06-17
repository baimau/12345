using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwigNodeUnit : MonoBehaviour
{
    bool m_Start = false;
    int m_speed = 0;

    public void SetStart()
    {
        m_Start = true;
    }

    public void SetSpeed(int rank)
    {
        m_speed = rank;
    }

    private void Update()
    {
        if (m_Start)
        {
            transform.position += new Vector3(0, -2f, 0)*Time.deltaTime *(1+ m_speed*0.25f);
        }
    }

}
