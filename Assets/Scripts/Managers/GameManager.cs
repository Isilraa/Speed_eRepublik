using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static bool m_playing = false;

    //Serialized Variables
    [SerializeField]
    GameObject hudGame;
    [SerializeField]
    GameObject gamePad;
    [SerializeField]
    GameObject hudMenu;
    [SerializeField]
    GameObject turboButton;

    //Private Variables
    static GameManager m_instance;

    //------------------------------------------------AWAKE--------------------------------------
    void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else
            Destroy(gameObject);
    }

    //------------------------------------------------START--------------------------------------
	void Start () {
        stopGame();
	}

    //------------------------------------------------UPDATE-------------------------------------
    void Update()
    {
        if(m_playing)
        {
            if (TimeManager.m_time >= 10)
                turboButton.SetActive(false);
        }
    }

    //------------------------------------------------METHODS------------------------------------
    /// <summary>   StartGame()
    ///     Starts the game.
    ///     It restarts all the managers and activates the hud game.
    /// </summary>
    public void StartGame()
    {
        m_playing = true;
        GeneratorManager.restartGenerator();
        TimeManager.restartTimer();
        PointsManager.restartPoints();
        PlayerManager.restartPlayer();
        hudGame.SetActive(true);
        gamePad.SetActive(true);
        hudMenu.SetActive(false);
        turboButton.SetActive(true);

    }//StartGame

    //--------------------------------------------STATIC_METHODS---------------------------------
    /// <summary>   stopGame()
    ///     Stops the game. Activates the Menu Hud.
    /// </summary>
    public static void stopGame()
    {
        m_playing = false;
        m_instance.hudGame.SetActive(false);
        m_instance.gamePad.SetActive(false);
        m_instance.hudMenu.SetActive(true);

    }//stopGame
}
