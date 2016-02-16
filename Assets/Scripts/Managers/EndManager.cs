using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class EndManager : MonoBehaviour {

    //Public Variables
    public float offsetZ = 5.0f;
    public Transform leftPointCircle = null;
    public Transform rightPointCircle = null;

    //------------------------------------------------START--------------------------------------
	void Start () 
    {
        StartPosition();
        ResizeCollider();
	}

    //-------------------------------------------ON_COLLISION_ENTER--------------------------------
    void OnCollisionEnter(Collision coll)
    {
        Destroy(coll.transform.parent.parent.gameObject);
    }
	

    //------------------------------------------------METHODS--------------------------------------
    /// <summary>   StartPosition()
    ///     Sets the position considering a reference.
    /// </summary>
    void StartPosition()
    {
        Vector3 position;
        position.x = transform.position.x;
        position.y = transform.position.y;
        position.z = PlayerManager.getTransformPlayer().position.z - offsetZ;

        transform.position = position;
    }//StartPosition

    /// <summary>   ResizeCollider()
    ///     Resize the size of the collider given a reference.
    /// </summary>
    void ResizeCollider()
    {
        BoxCollider boxColl = GetComponent<BoxCollider>();

        float distance = (leftPointCircle.position - rightPointCircle.position).magnitude;

        Vector3 size;
        size.x = distance;
        size.y = distance;
        size.z = 1.0f;

        boxColl.size = size;
    }//ResizeCollider
}
