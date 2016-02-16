using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TimeManager : MonoBehaviour {

    //Private variables
    static TimeManager m_instance;
    Text timeText;
    public static float m_time = 0;

    //------------------------------------------------AWAKE--------------------------------------
    void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else
            Destroy(gameObject);
    }

    //------------------------------------------------START--------------------------------------
	void Start () 
    {
        timeText = GetComponent<Text>();
	}

    //------------------------------------------------UPDATE-------------------------------------
	void Update () {
        if (GameManager.m_playing)
            UpdateTime();
	}

    //------------------------------------------------METHODS------------------------------------
    /// <summary>   UpdateTime()
    ///     Increases the time the game has been running
    /// </summary>
    void UpdateTime()
    {
        m_time += Time.deltaTime;
        timeText.text = "Time: " + ((int)m_time).ToString();
    }//UpdateTime

    /// <summary>   restartTimer()
    ///     Sets the time to cero.
    /// </summary>
    public static void restartTimer()
    {
        m_time = 0;
    }//restartTimer

}
