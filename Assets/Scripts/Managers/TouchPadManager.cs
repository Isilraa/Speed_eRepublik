using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchPadManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //Serialized Variables
    [SerializeField]
    float m_maxDist = 50.0f;

    //Private Variables
    Vector2 m_startPosition;
    Vector2 m_finishPosition;
    


    //--------------------------------------ON_POINTER_DOWN----------------------------
    public void OnPointerDown(PointerEventData data)
    {
        m_startPosition = data.position;
    }

    //--------------------------------------ON_POINTER_UP-----------------------------
    public void OnPointerUp(PointerEventData data)
    {
        m_finishPosition = data.position;
        chooseState();
    }

    //---------------------------------------METHODS----------------------------------
    /// <summary>
    ///     Controls the state it will be sendo to the player depending in
    ///     the direction it has been done using the pad.
    /// </summary>
    void chooseState()
    {
        //Direction
        Vector2 m_direction = m_finishPosition - m_startPosition;

        if (m_direction.magnitude >= m_maxDist)
        {
            m_direction.Normalize();
            //State send depending of the direction
            if (m_direction.x < -0.8f) //Left
                PlayerManager.moveLeft();
            else if (m_direction.x > 0.8f) //Right
                PlayerManager.moveRight();
            else if (m_direction.y > 0.8f) //Up
                PlayerManager.moveUp();
            else if (m_direction.y < 0.8f) //Down
                PlayerManager.moveDown();
        }
        else
            PlayerManager.shoot(); //Shoot
    }

    
}
