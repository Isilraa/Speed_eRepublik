using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ProjectileManager : MonoBehaviour {

    //Public Variables
    public float lifeTime = 2.0f;
    public float speed = 20.0f;

    //Private Variables
    Rigidbody m_rb;
    float m_timePassed = 0;

    //------------------------------------------------START--------------------------------------
	void Start () {
        m_rb = GetComponent<Rigidbody>();
        m_rb.velocity = Vector3.forward * speed;
	}

    //------------------------------------------------UPDATE-------------------------------------
    void Update()
    {
        m_timePassed += Time.deltaTime;
        if (m_timePassed >= lifeTime)
            Destroy(this.gameObject);
    }

    //-------------------------------------------ON_COLLISION_ENTER------------------------------
    void OnCollisionEnter(Collision coll)
    {
        //@TODO que sea un explosión y destruya todo lo que toca
        Destroy(coll.gameObject);
        Destroy(this.gameObject);
    }
}
