using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class PointsManager : MonoBehaviour {

    //Private Variables
    static PointsManager m_instance;
    Text m_pointsText;
    Text m_bestPointsText;
    int m_points = 0;
    int m_bestPoints = 0;
    int m_multiplier = 1;

    //------------------------------------------------AWAKE--------------------------------------
    void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else
            Destroy(gameObject);
    }

    //------------------------------------------------START--------------------------------------
    void Start()
    {
        m_pointsText = GetComponent<Text>();
        m_bestPointsText = GameObject.FindGameObjectWithTag("BestPoints").GetComponent<Text>();
        UpdatePoints();
        UpdateBestPoints();
    }

    //------------------------------------------------UPDATE-------------------------------------
    void Update()
    {
        if (GameManager.m_playing)
            AddPoints((int)TimeManager.m_time/ 10);
        m_instance.UpdateBestPoints();
        
    }

    //------------------------------------------------METHODS------------------------------------
    /// <summary>   UpdatePoints()
    ///     Changes the output of the text with the new points.
    /// </summary>
    void UpdatePoints()
    {
        m_pointsText.text = "x" + m_multiplier.ToString() + " Points: " + m_points.ToString();
    }//UpdatePoints

    /// <summary>   UpdateBestPoints()
    ///     Changes the output of the max points earned.
    /// </summary>
    void UpdateBestPoints()
    {
        m_bestPointsText.text = "Best: " + m_bestPoints.ToString();
    }//UpdateBestPoints

    /// <summary>   TurboSpeed(int multiplier)
    ///     Earns more points being in turbo speed.
    /// </summary>
    /// <param name="multiplier"> Mulitplier of the number of points earned over time </param>
    public void TurboSpeed(int multiplier)
    {
        m_multiplier = multiplier;
    }//TurboSpeed

    //--------------------------------------------STATIC_METHODS---------------------------------
    /// <summary>   AddPoints(int amount)
    ///     Increases the points earn over time.
    /// </summary>
    /// <param name="amount"> Amount of points earned </param>
	static void AddPoints(int amount)
    {
        m_instance.m_points += amount * m_instance.m_multiplier;
        m_instance.UpdatePoints();
    }//AddPoints

    /// <summary>   restartPoints()
    ///     Multiplier set to 1.
    ///     Compares the maximum points achieved so far with the ones you got now.
    /// </summary>
    public static void restartPoints()
    {
        m_instance.m_multiplier = 1;
        if (m_instance.m_points > m_instance.m_bestPoints)
            m_instance.m_bestPoints = m_instance.m_points;
        m_instance.m_points = 0;
        
    }
}
