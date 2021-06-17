using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HurtObj : MonoBehaviour
{
    public Image m_Effect = null;

    public void OnHurt()
    {
        gameObject.SetActive(true);
        m_Effect.color = new Color(m_Effect.color.r, m_Effect.color.g, m_Effect.color.b, 0.3f);
    }
 
    // Update is called once per frame
    void Update()
    {
        if (m_Effect.color.a > 0)
        {
            m_Effect.color -= new Color(0, 0, 0, 1 * Time.deltaTime);
            if(m_Effect.color.a<=0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
