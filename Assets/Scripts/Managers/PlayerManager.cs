using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    //Public Variables
    public float moveSpeedAxis = 2.0f;
    public float moveSpeedForward = 5.0f;
    public float rotationSpeed = 5.0f;
    public float increaseSpeed = 0.01f;
    public float maxRot = 45;//degrees
    public Transform leftPointCircle = null;
    public Transform rightPointCircle = null;
    public GameObject projectile = null;

    public enum STATE_HORIZONTAL
    {
        LEFT = 0,
        RIGHT,
        MIDDLE,
    }

    public enum STATE_VERTICAL
    {
        DOWN = 0,
        MIDDLE,
        UP
    }

    //Private Variables
    static PlayerManager m_instance = null;
    bool m_rotated = false;
    bool m_canShoot = true;
    float m_distance;
    float m_initialForwardSpeed;
    //Positions for the X axis
    float m_leftPos, m_middleHPos, m_rightPos;
    //Positions for the Y axis
    float m_upPos, m_middleVPos, m_downPos;

    enum STATEMOVE
    {
        MOVE_LEFT,
        MOVE_RIGHT,
        MOVE_UP,
        MOVE_DOWN,
        NONE
    }

    enum STATEROTATE
    {
        ROTATE_LEFT,
        ROTATE_RIGHT,
        ROTATE_UP,
        ROTATE_DOWN,
        NONE
    }

    STATE_HORIZONTAL m_stateH;
    STATE_VERTICAL m_stateV;
    STATEMOVE m_stateMove;
    STATEROTATE m_stateRotate;


    //------------------------------------------------AWAKE--------------------------------------
    void Awake()
    {
        //Use for singleton
        if (m_instance == null)
            m_instance = this;
        else
            Destroy(gameObject);

        //Set Ship to Start Position
        StartPosition();
        m_initialForwardSpeed = moveSpeedForward;
    }

    //------------------------------------------------UPDATE-------------------------------------
    void Update()
    {
        if (GameManager.m_playing)
        {
            MovementWithAxis();
            MoveForward();
            RotationWithMovement();
            //@TODO Tener en cuenta el reseteo
            //Reset position if it goes far away
            //if (transform.position.z >= 100)
            //    
        }
        
    }

    //-------------------------------------------ON_COLLISION_ENTER------------------------------
    void OnCollisionEnter(Collision coll)
    {
        //gameObject.SetActive(false);
        GameManager.stopGame();
        transform.parent.position = Vector3.zero;
    }

    //------------------------------------------------METHODS------------------------------------
    /// <summary>   StartPosition()
    ///     We get the distance between the reference of the leftPoint and rightPoint. Then we made
    ///     a 3 by 3 matrix.
    ///     We initialize the start position to de middle position of x and y axis.
    ///     And also we say the spaceship is not moving.
    /// </summary>
    void StartPosition()
    {
        m_distance = (leftPointCircle.position - rightPointCircle.position).magnitude;

        m_middleHPos = 0.0f;
        m_leftPos = - m_distance / 3;
        m_rightPos = m_distance / 3;

        m_middleVPos = 0.0f;
        m_downPos = -m_distance / 3;
        m_upPos = m_distance / 3;

        this.transform.position = new Vector3(m_middleHPos, m_middleVPos);

        //States inicialization
        m_stateH = STATE_HORIZONTAL.MIDDLE;
        m_stateV = STATE_VERTICAL.MIDDLE;
        m_stateMove = STATEMOVE.NONE;
        m_stateRotate = STATEROTATE.NONE;
    }//StartPosition

    /// <summary>   MoveForward()
    ///     This function is called in the Update. In ecery call the speed increases.
    /// </summary>
    void MoveForward()
    {
        transform.parent.position += new Vector3(0.0f, 0.0f, 1.0f) * moveSpeedForward * Time.deltaTime;

        moveSpeedForward += increaseSpeed;

    }//MoveForward

    /// <summary>   MovementWithAxis()
    ///     Movement of the Ship whith the gamepad
    /// </summary>
    void MovementWithAxis()
    {
        //STATES MACHINE FOR MOVEMENT
        switch(m_stateMove)
        {
            case STATEMOVE.MOVE_LEFT: //Moving Left
                //Used position instead of Translate because i wanted to use world space coordinates
                transform.position += Vector3.right * -moveSpeedAxis * Time.deltaTime;
                switch (m_stateH)
                {
                    case STATE_HORIZONTAL.MIDDLE:
                        if (transform.position.x <= m_leftPos)
                        {
                            m_stateMove = STATEMOVE.NONE;
                            m_stateH = STATE_HORIZONTAL.LEFT;
                        }
                        break;
                    case STATE_HORIZONTAL.RIGHT:
                        if (transform.position.x <= m_middleHPos)
                        {
                            m_stateMove = STATEMOVE.NONE;
                            m_stateH = STATE_HORIZONTAL.MIDDLE;
                        }
                        break;
                }
                break;

            case STATEMOVE.MOVE_RIGHT: //Moving Right
                transform.position += Vector3.right * moveSpeedAxis * Time.deltaTime;
                switch (m_stateH)
                {
                    case STATE_HORIZONTAL.MIDDLE:
                        if (transform.position.x >= m_rightPos)
                        {
                            m_stateMove = STATEMOVE.NONE;
                            m_stateH = STATE_HORIZONTAL.RIGHT;
                        }
                        break;
                    case STATE_HORIZONTAL.LEFT:
                        if (transform.position.x >= m_middleHPos)
                        {
                            m_stateMove = STATEMOVE.NONE;
                            m_stateH = STATE_HORIZONTAL.MIDDLE;
                        }
                        break;
                }
                break;

            case STATEMOVE.MOVE_UP: //Moving Up
                transform.position += Vector3.up * moveSpeedAxis * Time.deltaTime;
                switch (m_stateV)
                {
                    case STATE_VERTICAL.DOWN:
                        if (transform.position.y >= m_middleVPos)
                        {
                            m_stateMove = STATEMOVE.NONE;
                            m_stateV = STATE_VERTICAL.MIDDLE;
                        }
                        break;
                    case STATE_VERTICAL.MIDDLE:
                        if (transform.position.y >= m_upPos)
                        {
                            m_stateMove = STATEMOVE.NONE;
                            m_stateV = STATE_VERTICAL.UP;
                        }
                        break;
                }
                break;

            case STATEMOVE.MOVE_DOWN: //Moving Down
                transform.position += Vector3.up * -moveSpeedAxis * Time.deltaTime;
                switch (m_stateV)
                {
                    case STATE_VERTICAL.UP:
                        if (transform.position.y <= m_middleVPos)
                        {
                            m_stateMove = STATEMOVE.NONE;
                            m_stateV = STATE_VERTICAL.MIDDLE;
                        }
                        break;
                    case STATE_VERTICAL.MIDDLE:
                        if (transform.position.y <= m_downPos)
                        {
                            m_stateMove = STATEMOVE.NONE;
                            m_stateV = STATE_VERTICAL.DOWN;
                        }
                        break;
                }
                break;
        }
    }//MovementWithAxis

    /// <summary>   RotationWithMovement()
    ///     Rotation of the ship
    /// </summary>
    void RotationWithMovement()
    {
        //STATES MACHINE FOR ROTATION
        switch(m_stateRotate)
        {
            case STATEROTATE.ROTATE_UP:
                if(RotateAndUndo(-maxRot, 0.0f, 0.0f))
                    m_stateRotate = STATEROTATE.NONE;
                break;

            case STATEROTATE.ROTATE_DOWN:
                if(RotateAndUndo(maxRot, 0.0f, 0.0f))
                    m_stateRotate = STATEROTATE.NONE;
                break;

            case STATEROTATE.ROTATE_LEFT:
                if(RotateAndUndo(0.0f, 0.0f, maxRot))
                    m_stateRotate = STATEROTATE.NONE;
                break;

            case STATEROTATE.ROTATE_RIGHT:
                if(RotateAndUndo(0.0f, 0.0f, -maxRot))
                    m_stateRotate = STATEROTATE.NONE;
                break;
        }
    }//RotationWithMovement

    /// <summary>   RotateAndUndo(float anglesX, float anglesY, float anglesZ)
    ///     This method is a coroutine. Every time is called it will rotate the object
    ///     and when a time passes it will undo the rotation.
    /// </summary>
    /// <param name="anglesX"> Rotation around the X axis </param>
    /// <param name="anglesY"> Rotation around the Y axis </param>
    /// <param name="anglesZ"> Rotation around the Z axis </param>
    /// <returns>
    ///     true --> when the rotation is done
    ///     false--> the rotation is being done, and not finished
    /// </returns>
    bool RotateAndUndo(float anglesX, float anglesY, float anglesZ)
    {
        if (!m_rotated)
        {
            Quaternion firstRotation = Quaternion.Euler(anglesX, anglesY, anglesZ);
            transform.rotation = Quaternion.Slerp(transform.rotation, firstRotation, rotationSpeed * Time.deltaTime);
            if(transform.rotation == firstRotation)
                m_rotated = true;
            return false;
        }
        else
        {
            Quaternion secondRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, secondRotation, rotationSpeed * Time.deltaTime);
            if(transform.rotation == secondRotation)
            {
                m_rotated = false;
                return true;
            }
            return false;
        }
    }//RotateAndUndo

    /// <summary>   TurboSpeed(float multiplier)
    ///     When the TURBO button is pressed the forward speed will increase.
    /// </summary>
    /// <param name="multiplier"> The value that will multiplie the forward velocity </param>
    public void TurboSpeed(float multiplier){
        moveSpeedForward *= multiplier;
        AudioManager.Instance.Play("Turbo");
    }//TurboSpeed

    //--------------------------------------------STATIC_METHODS---------------------------------

    /// <summary>   getTransformPlayer()
    ///     Gets the Transform of the player
    /// </summary>
    /// <returns>
    ///     Transform
    /// </returns>
    public static Transform getTransformPlayer()
    {
        return m_instance.transform;
    }//getTransformPlayer

    /// <summary>   getPlayer()
    ///     Gets the GameObject of the Player
    /// </summary>
    /// <returns>
    ///     GameObject
    /// </returns>
    public static GameObject getPlayer()
    {
        return m_instance.transform.gameObject;
    }//getPLayer

    /// <summary>   getStateVertical()
    ///     Gets the state of the position in the y axis or vertical.
    /// </summary>
    /// <returns>
    ///     STATE_VERTICAL
    /// </returns>
    public static STATE_VERTICAL getStateVertical()
    {
        return m_instance.m_stateV;
    }//getStateVertical

    /// <summary>   moveRight()
    ///     Changes the state of movement to, is moving right if it
    ///     really can move right, that is if he is in the left side or
    ///     in the middle and is not doing already a movement.
    /// </summary>
    public static void moveRight()
    {
        if (m_instance.m_stateMove == STATEMOVE.NONE)
        {
            //We make sure we are not in the middle of a move
            if (m_instance.m_stateH == STATE_HORIZONTAL.LEFT || 
                m_instance.m_stateH == STATE_HORIZONTAL.MIDDLE)
            {
                m_instance.m_stateMove = STATEMOVE.MOVE_RIGHT;
                m_instance.m_stateRotate = STATEROTATE.ROTATE_RIGHT;
                AudioManager.Instance.PlayHalfVolumen("Move");
            }
        }  
    }//moveRight

    /// <summary>   moveLeft()
    ///     Changes the state of movement to "is moving left" if it
    ///     really can move left, that is if he is in the right side or
    ///     in the middle and is not doing already a movement.
    /// </summary>
    public static void moveLeft()
    {
        //We make sure we are not in the middle of a move
        if (m_instance.m_stateMove == STATEMOVE.NONE)
        {
            if (m_instance.m_stateH == STATE_HORIZONTAL.RIGHT ||
                m_instance.m_stateH == STATE_HORIZONTAL.MIDDLE)
            {
                m_instance.m_stateMove = STATEMOVE.MOVE_LEFT;
                m_instance.m_stateRotate = STATEROTATE.ROTATE_LEFT;
                AudioManager.Instance.PlayHalfVolumen("Move");
            }
        }
    }//moveLeft

    /// <summary>   moveUp()
    ///     Changes the state of movement to "is moving up" if it
    ///     really can move up, that is if he is in the down or
    ///     in the middle and is not doing already a movement.
    /// </summary>
    public static void moveUp()
    {
        if(m_instance.m_stateMove == STATEMOVE.NONE)
        {
            if(m_instance.m_stateV == STATE_VERTICAL.DOWN ||
               m_instance.m_stateV == STATE_VERTICAL.MIDDLE)
            {
                m_instance.m_stateMove = STATEMOVE.MOVE_UP;
                m_instance.m_stateRotate = STATEROTATE.ROTATE_UP;
                AudioManager.Instance.PlayHalfVolumen("Move");
            }
        }
    }//moveUp

    /// <summary>   moveDown()
    ///     Changes the state of movement to "is moving down" if it
    ///     really can move down, that is if he is above or
    ///     in the middle and is not doing already a movement.
    /// </summary>
    public static void moveDown()
    {
        if (m_instance.m_stateMove == STATEMOVE.NONE)
        {
            if (m_instance.m_stateV == STATE_VERTICAL.UP ||
                m_instance.m_stateV == STATE_VERTICAL.MIDDLE)
            {
                m_instance.m_stateMove = STATEMOVE.MOVE_DOWN;
                m_instance.m_stateRotate = STATEROTATE.ROTATE_DOWN;
                AudioManager.Instance.PlayHalfVolumen("Move");
            }
        }
    }//moveDown

    /// <summary>   getSpeed()
    ///     Gets the speed of the Player
    /// </summary>
    /// <returns>
    ///     Forward speed
    /// </returns>
    public static float getSpeed()
    {
        return m_instance.moveSpeedForward;
    }//getSpeed

    /// <summary>   canShoot()
    ///     Enables the ability to shoot
    /// </summary>
    public static void canShoot()
    {
        m_instance.m_canShoot = true;
    }//canShoot

    /// <summary>   shoot()
    ///     Shoots a laser bullet. For now it can only shoot one
    ///     @TODO Shoot more than one bullet recharging with time.
    /// </summary>
    public static void shoot()
    {
        if (m_instance.m_canShoot)
        {
            if (m_instance.projectile == null)
                Debug.LogError("Prefab of projectile in GameManager is NULL");

            GameObject projectileIns = GameObject.Instantiate(m_instance.projectile);
            Vector3 pos;
            pos.x = m_instance.transform.position.x;
            pos.y = m_instance.transform.position.y;
            pos.z = m_instance.transform.position.z + 3;
            projectileIns.transform.position = pos;
            projectileIns.transform.parent = m_instance.transform.parent;
            m_instance.m_canShoot = false;
            AudioManager.Instance.Play("Shoot");
        }
    }//shoot

    /// <summary>   restartPlayer()
    ///     Restarts the position, velocity, angularVelocity, moveSpeed, and rotation
    ///     of the Player.
    /// </summary>
    public static void restartPlayer()
    {
        m_instance.StartPosition();
        m_instance.transform.rotation = Quaternion.identity;
        Rigidbody rb = m_instance.gameObject.GetComponent<Rigidbody>();
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        m_instance.moveSpeedForward = m_instance.m_initialForwardSpeed;
    }//restartPlayer
}
