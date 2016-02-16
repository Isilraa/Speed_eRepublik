using UnityEngine;
using System.Collections;

public class PlaneMovement : MonoBehaviour {

    //Public Variables
    public float multiplier = 1.0f;

    //Private Variables
    Renderer rend;

    //------------------------------------------------START-------------------------------------
	void Start () {
        rend = GetComponent<Renderer>();
	}

    //------------------------------------------------UPDATE-------------------------------------
	void Update () {
        TextureMovement();
	}

    //------------------------------------------------METHODS------------------------------------
    /// <summary>   TextureMovement()
    ///     Movement of the texture given the player velocity.
    /// </summary>
    void TextureMovement()
    {
        float offSet = Time.time * PlayerManager.getSpeed() * multiplier / 100;
        rend.material.SetTextureOffset("_MainTex", new Vector2(0, offSet));

    }//TextureMovement
}
