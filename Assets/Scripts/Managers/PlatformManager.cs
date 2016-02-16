using UnityEngine;
using System.Collections;

public class PlatformManager : MonoBehaviour {

    //Public variables
    public Transform leftPointCircle = null;
    public Transform rightPointCircle = null;
    public float lenght = 5.0f;
    public float offSetZ = 5.0f;
    public PlayerManager.STATE_VERTICAL position;

    //Private Variables
    Renderer rend;

    //------------------------------------------------START--------------------------------------
	void Start () 
    {
        rend = GetComponent<Renderer>();
        StartPosition();
	}

    //------------------------------------------------UPDATE-------------------------------------
	void Update () {
        TextureMovement();
	}

    //------------------------------------------------METHODS------------------------------------
    /// <summary>   StartPosition()
    /// 
    /// </summary>
    void StartPosition()
    {
        switch(position)
        {
            case PlayerManager.STATE_VERTICAL.UP:
                //Width of the platform
                transform.localScale = new Vector3(leftPointCircle.position.x - rightPointCircle.position.x, transform.localScale.y, lenght);
                float distance = (leftPointCircle.position - rightPointCircle.position).magnitude;
                //position of the platform
                Vector3 posUp;
                    posUp.x = 0.0f;
                    posUp.y = PlayerManager.getTransformPlayer().position.y + distance *2/3 + 1.5f;//PlayerManager.getTransformPlayer().localScale.y;
                    posUp.z = transform.localScale.z / 2 - offSetZ;
                transform.position = posUp;
                break;

            case PlayerManager.STATE_VERTICAL.DOWN:
                //Width of the platform
                transform.localScale = new Vector3(leftPointCircle.position.x - rightPointCircle.position.x, transform.localScale.y, lenght);
                //position of the platform
                Vector3 posDown;
                    posDown.x = 0.0f;
                    posDown.y = PlayerManager.getTransformPlayer().position.y - 1.5f;//PlayerManager.getTransformPlayer().localScale.y;
                    posDown.z = transform.localScale.z / 2 - offSetZ;
                transform.position = posDown;
                break;
        }
    }//StartPosition

    /// <summary>
    /// 
    /// </summary>
    void TextureMovement()
    {
        float offSet = Time.time * PlayerManager.getSpeed() /100;
        switch (position)
        {
            case PlayerManager.STATE_VERTICAL.UP:
                rend.material.SetTextureOffset("_MainTex", new Vector2(0, offSet));
                break;
            case PlayerManager.STATE_VERTICAL.DOWN:
                rend.material.SetTextureOffset("_MainTex", new Vector2(0, -offSet));
                break;
        }
        
    }//TextureMovement
}
