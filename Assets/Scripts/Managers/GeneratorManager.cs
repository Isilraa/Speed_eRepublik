using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneratorManager : MonoBehaviour {

    //Public Variables
    public float distanceFromPlayer = 20.0f;
    public float timeBetweenSpawns = 3.0f;
    public float minimumTime = 0.5f;
    public float decreaseTime = 0.01f;
    public Transform leftPointCircle = null;
    public Transform rightPointCircle = null;
    public GameObject[] prefabObstacle = null;
    public GameObject prefabPoints = null;

    [System.Serializable]
    public struct difficulty
    {
        public int numPrefabs;
        public int minTime;
        [Range(0, 100)]
        public int percentage;
    }

    public difficulty[] difficulties;
    

    //Private variables
    static GeneratorManager m_instance;
    float m_timePassedForSpawn = 0;
    float m_initialTimeBetweenSpawns;
    bool m_turbo = false;
    Vector2[][] m_positionsSpawn;//Position an obstacle can spawn
    List<GameObject> allContainers = new List<GameObject>();//List of all the obstacles

    //------------------------------------------------AWAKE--------------------------------------
    void Awake()
    {
        //Used for the singleton
        if (m_instance == null)
            m_instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        GetPositionsSpawn();
        m_initialTimeBetweenSpawns = timeBetweenSpawns;
    }

    //------------------------------------------------UPDATE-------------------------------------
	void Update () 
    {
        if (GameManager.m_playing)
        {
            ReloadPosition();

            //Spawn of modules

            m_timePassedForSpawn += Time.deltaTime;
            if (m_timePassedForSpawn >= timeBetweenSpawns)
            {
                StartCoroutine(InstantiateModule());
                m_timePassedForSpawn = 0;
            }

            DecreaseTime();
        }
	}

    //------------------------------------------------METHODS--------------------------------------
    /// <summary>   GetPositionsSpawn()
    ///     Sets the matrix for the posible positions the obstacle can spawn
    /// </summary>
    void GetPositionsSpawn()
    {
        //Initialize
        m_positionsSpawn = new Vector2[3][];
        for (int i = 0; i < m_positionsSpawn.Length; ++i)
        {
            m_positionsSpawn[i] = new Vector2[3];
        }
            float totalDistance = (leftPointCircle.position - rightPointCircle.position).magnitude;
            float distance = totalDistance / 3;

        //Give values
        for (int i = 0; i < m_positionsSpawn.Length; ++i)
        {
            for (int j = 0; j < m_positionsSpawn[i].Length; ++j)
            {
                m_positionsSpawn[i][j] = new Vector2(j - 1, -i + 1) * distance;
            }
        }

    }//GetPositionsSpawn

    /// <summary>   ReloadPosition()
    ///     Correcting of the position related to the player
    /// </summary>
    void ReloadPosition()
    {
        Vector3 pos;
        pos.x = 0.0f;
        pos.y = 0.0f;
        pos.z = PlayerManager.getTransformPlayer().position.z + distanceFromPlayer;
        transform.position = pos;
    }//ReloadPosition

    /// <summary>   InstantiateModule()
    ///     Creates the obstacles
    /// </summary>
    /// <returns>
    ///     Always return cero.
    /// </returns>
    IEnumerator InstantiateModule()
    {
        if(prefabObstacle == null)
        {
            Debug.LogError("you must add the prefab Obstacle to the GeneratorManager");
            yield return new WaitForSeconds(0.0f);
        }

        GameObject container = null;
        while (container == null)
        {
            //random probability
            int randList = Random.Range(0, 100);

            for (int i = 0; i < difficulties.Length; ++i)
            {
                if ((randList -= difficulties[i].percentage) <= 0 && (difficulties[i].minTime <= TimeManager.m_time || m_turbo) && container == null)
                {
                    List<GameObject> obstacles = new List<GameObject>();

                    for (int j = 0; j < difficulties[i].numPrefabs; ++j)
                    {
                        //random position
                        int XPos = Random.Range(0, 300) / 100, YPos = Random.Range(0, 300) / 100;
                        obstacles.Add(GameObject.Instantiate(prefabObstacle[Random.Range(0, prefabObstacle.Length)]));
                        obstacles[j].transform.position = new Vector3(m_positionsSpawn[XPos][YPos].x, m_positionsSpawn[XPos][YPos].y, transform.position.z);
                    }
                    container = new GameObject("Container");
                    container.transform.position = transform.position;

                    for (int j = 0; j < obstacles.Count; ++j)
                    {
                        obstacles[j].transform.parent = container.transform;
                    }
                }
            }
        }
        container.transform.position = transform.position;
        yield return new WaitForSeconds(0.0f);
        allContainers.Add(container);
    }//InstantiateModule

    /// <summary>   DecreaseTime()
    ///     Time between spawns decrease over time to a minimun.
    /// </summary>
    void DecreaseTime()
    {
        timeBetweenSpawns -= decreaseTime;
        if (timeBetweenSpawns <= minimumTime)
            timeBetweenSpawns = minimumTime;
    }//DecreaseTime

    /// <summary>   TurboSpeed()
    ///     Sets the time between spawns to the minimun.
    /// </summary>
    public void TurboSpeed()
    {
        timeBetweenSpawns = minimumTime;
        m_turbo = true;
    }//TurboSpeed

    /// <summary>   restartGenerator()
    ///     Destroys all obstacles in the scene.
    ///     Sets the time between spawns to the initial time.
    ///     An we are not in turbo anymore.
    /// </summary>
    public static void restartGenerator()
    {
        for (int i = 0; i < m_instance.allContainers.Count; ++i )
        {
            if(m_instance.allContainers[i] != null)
                Destroy(m_instance.allContainers[i]);
        }
        m_instance.allContainers.Clear();
        m_instance.timeBetweenSpawns = m_instance.m_initialTimeBetweenSpawns;
        m_instance.m_timePassedForSpawn = 0;
        m_instance.m_turbo = false;
    }//restartGenerator
}
