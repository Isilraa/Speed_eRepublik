using UnityEngine;
using System.Collections;

public class AudioElement : MonoBehaviour {

    //Public Variables
    public bool CheckActivity { get; set; }


    //------------------------------------------------UPDATE-------------------------------------
	void Update () {
	    if(CheckActivity)
        {
            if(!this.gameObject.GetComponent<AudioSource>().isPlaying)
            {
                Destroy(this.gameObject);
            }
        }
	}
}
