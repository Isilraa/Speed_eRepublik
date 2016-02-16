using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	//Serialized Variables
    [SerializeField]
    private float offSetYUp = 2;
    [SerializeField]
    private float offSetYDown = -2;
    [SerializeField]
    private float initialOffSetZ = -8;
    [SerializeField]
    private float speed = 10.0f;

    //Private Variables
    private static CameraManager m_instance;
    private float m_offSetY;
    private float m_offSetZ;
    PlayerManager.STATE_VERTICAL m_previousState;

    //------------------------------------------------AWAKE--------------------------------------
    void Awake()
    {
        if(m_instance == null)
            m_instance = this;
        else
            Destroy(gameObject);
    }

    //------------------------------------------------START--------------------------------------
    void Start()
    {
        m_previousState = PlayerManager.STATE_VERTICAL.MIDDLE;
        m_offSetY = offSetYUp;
        m_offSetZ = initialOffSetZ;
    }

    //------------------------------------------------UPDATE-------------------------------------
	void Update () {
        CheckPlayerState();
        ReloadPosition();
	}

    //------------------------------------------------METHODS------------------------------------
    /// <summary>   ReloadPosition()
    ///     The camera is always moving to the position of the player with an offset.
    /// </summary>
    void ReloadPosition()
    {
        Vector3 position;
        Vector3 playerPos = PlayerManager.getTransformPlayer().position;
        position.x = playerPos.x;
        position.y = playerPos.y + m_offSetY;//Above the player
        position.z = playerPos.z + m_offSetZ; //Behind the player
        transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
    }//ReloadPosition

    /// <summary>   CheckPlayerState()
    ///     If the player its up or down the offset in Y changes, so that the
    ///     camera does not go out of the limits.
    /// </summary>
    void CheckPlayerState()
    {
        if(PlayerManager.getStateVertical() != m_previousState)
        {
            if(PlayerManager.getStateVertical() == PlayerManager.STATE_VERTICAL.UP)
            {
                m_offSetY = offSetYDown;
            }
            else if(PlayerManager.getStateVertical() == PlayerManager.STATE_VERTICAL.DOWN)
            {
                m_offSetY = offSetYUp;
            }
        }
    }//CheckPlayerState

    /// <summary>   TurboSpeed(float multiplier)
    ///     The camera zooms out when the player enters turbo mode.
    /// </summary>
    /// <param name="multiplier"> Multiplies the offset in Z </param>
    public void TurboSpeed(float multiplier)
    {
        m_offSetZ *= multiplier;
        StartCoroutine(RestartPosition());
    }//TurboSpeed

    /// <summary>   RestartPosition()
    ///     Restarts the offset in z to the initial offset.
    /// </summary>
    /// <returns>
    ///     Time of 0.3f seconds
    /// </returns>
    IEnumerator RestartPosition()
    {
        yield return new WaitForSeconds(0.3f);
        m_offSetZ = initialOffSetZ;
    }
}
