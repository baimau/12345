using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwigGameManager : MonoBehaviour
{

    public TwigUnit m_CloneObj = null;
    public Transform m_TwigNode = null;
    public PlayerUnit m_Player = null;
    public TwigNodeUnit m_TwigNodeUnit = null;
    public HurtObj m_HurtObj = null;

    public Text m_HighText = null;
    public Text m_LifeText = null;

    public List<int[]> m_AllTwigPoint = new List<int[]>();
    public List<List<TwigUnit>> m_AllTwigObj = new List<List<TwigUnit>>();
    public List<int> m_Road = new List<int>();

    private Queue<TwigUnit> m_TwigPool = new Queue<TwigUnit>();

    private int m_NeedTwigCount = 15;
    private int m_NowIndex = 0;
    private bool m_Start = false;
    private int m_NowHigh = 0;
    private int m_TempHigh = 0;
    private int m_AddIndex = 0;
    private bool m_SpeedUp = false;
    private int m_LifeValue = 3;



    private void Awake()
    {
        m_NowIndex = 1;
        m_NowHigh = 0;
        m_LifeValue = 3;
    }

    // Start is called before the first frame update
    void Start()
    {

        for(int i = 0; i< m_NeedTwigCount;i++)
        {
            AddLine(i);
        }

        for (int i = 0; i < m_AllTwigPoint.Count; i++)
        {
            int[] line = m_AllTwigPoint[i];
            List<TwigUnit> list = new List<TwigUnit>();
            for (int j = 0; j < line.Length; j++)
            {
                if (line[j] == 1)
                {
                    Vector3 pos = new Vector3(-9+6*j, -6+ i * 2f, 0);
                    TwigUnit unit = GetSwig();
                    unit.transform.localPosition = pos;
                    list.Add(unit);
                    m_AllTwigObj.Add(list);
                }
            }
        }
        m_AddIndex = m_AllTwigPoint.Count;

    }

    private TwigUnit GetSwig()
    {
        TwigUnit unit = null;
        if (m_TwigPool.Count > 0)
        {
            unit = m_TwigPool.Dequeue();
        }
        else
        { 
            unit = Instantiate<TwigUnit>(m_CloneObj, m_TwigNode);
        }
        return unit;
    }

    private void AddSwigLine()
    {
        AddLine(m_AddIndex);

        int[] line = m_AllTwigPoint[m_AddIndex];
        List<TwigUnit> list = new List<TwigUnit>();
        for (int j = 0; j < line.Length; j++)
        {
            if (line[j] == 1)
            {
                 Vector3 pos = new Vector3(-9 + 6 * j, -6 + m_AddIndex * 2f, 0);
                 TwigUnit unit = GetSwig();
                 unit.transform.localPosition = pos;
                 list.Add(unit);
                 m_AllTwigObj.Add(list);
            }
        }
       
        m_AddIndex++;
    }

    private void AddLine(int index)
    {
        int[] line = new int[4];
        int rightIndex = 0;
        if (index == 0)
        {
            rightIndex = Random.Range(0, 4);
        }
        else
        {
            int realIndex = m_Road[index - 1];
            rightIndex = Random.Range(0, 2) == 0 ? realIndex + 1 : realIndex - 1;
            if (rightIndex < 0)
            {
                rightIndex = realIndex + 1;
            }
            else if (rightIndex > 3)
            {
                rightIndex = realIndex - 1;
            }
        }

        line[rightIndex] = 1;
        m_Road.Add(rightIndex);
        m_AllTwigPoint.Add(line);
    }

    private void RemoveSwigLine()
    {
        List<TwigUnit> listUnit = m_AllTwigObj[0];
        for (int i = 0; i < listUnit.Count; i++)
        {
            m_TwigPool.Enqueue(listUnit[i]);
        }
        m_AllTwigObj.RemoveAt(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_TwigNodeUnit.transform.localPosition.y < (-4 + m_TempHigh * -2) && m_TempHigh <= m_NowHigh)
        {
            m_TempHigh++;
            RemoveSwigLine();
            AddSwigLine();
        }

        if (m_NowHigh > m_TempHigh + 9 && m_SpeedUp == false)
        {
            m_SpeedUp = true;
            Time.timeScale = 20;
        }
        else if (m_SpeedUp == true)
        {
            m_SpeedUp = false;
            Time.timeScale = 1;
        }


        if (m_Start == false)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                m_Player.transform.localPosition = new Vector3(-6, -8, -1);
                m_NowIndex = 0;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                m_Player.transform.localPosition = new Vector3(0, -8, -1);
                m_NowIndex = 1;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                m_Player.transform.localPosition = new Vector3(6, -8, -1);
                m_NowIndex = 2;
            }
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            OnJumpLeft();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            OnJumpRight();
        }

    }

    private void OnJumpLeft()
    {
        if (m_NowIndex <= 0 && m_Start == false)
        {
            OnHurt();
            return;
        }

        int nextIndex = m_NowIndex - 1;

        if (m_Start == false)
        {
            nextIndex = m_NowIndex;
        }

        int[] line= m_AllTwigPoint[m_NowHigh];

        if (line[nextIndex] == 1)
        {
            Vector3 pos = new Vector3(-9 + 6 * nextIndex, -4.5f + m_NowHigh * 2f, 0);
            m_Player.transform.localPosition = pos;
            m_NowIndex = nextIndex;
            m_NowHigh++;
            if (m_Start == false)
            {
                m_Start = true;
                m_TwigNodeUnit.SetStart();
            }
        }
        else
        {
            OnHurt();
        }

        m_TwigNodeUnit.SetSpeed(m_NowHigh / 10);
        m_HighText.text = "當前高度:" + m_NowHigh.ToString();

    }

    private void OnJumpRight()
    {
        if (m_NowIndex >= 3)
        {
            OnHurt();
            return;
        }
        int nextIndex = m_NowIndex + 1;

        
        int[] line = m_AllTwigPoint[m_NowHigh];

        if (line[nextIndex] == 1)
        {
            Vector3 pos = new Vector3(-9 + 6 * nextIndex, -4.5f + m_NowHigh * 2f, 0);
            m_Player.transform.localPosition = pos;
            m_NowIndex = nextIndex;
            m_NowHigh++;
            if (m_Start == false)
            {
                m_Start = true;
                m_TwigNodeUnit.SetStart();
            }
        }
        else
        {
            OnHurt();
        }
        m_TwigNodeUnit.SetSpeed(m_NowHigh / 10);
        m_HighText.text = "當前高度:" + m_NowHigh.ToString();

    }

    private void OnHurt()
    {
        m_HurtObj.OnHurt();
        m_LifeValue--;
        if(m_LifeValue <0)
        {
            m_LifeValue = 0;
            Time.timeScale = 0;
        }
        m_LifeText.text = "目前生命:" + m_LifeValue.ToString();

    }
}
