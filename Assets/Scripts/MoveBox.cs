using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBox : MonoBehaviour
{
    private float rotateDegree = 90f;
    //private Vector3 newPos = new Vector3(-10, 5, 0); // Vector3(-10, -5, 0) ~ Vector3(-10, 5, 0);

    // Start is called before the first frame update
    void Start()
    {

        //transform.position = Random.Range(0, 100.0f);

        transform.rotation = Random.rotationUniform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime, Time.deltaTime, 0);
        transform.Rotate(new Vector3(Time.deltaTime * rotateDegree, Time.deltaTime * rotateDegree, Time.deltaTime * rotateDegree));

        if (transform.position.x > 10f || transform.position.x < -10f
            || transform.position.y > 10f || transform.position.y < -10f
            || transform.position.z > 3f || transform.position.z < -3f)
        {
            transform.position = new Vector3(0, 0, 0);
            transform.rotation = Random.rotationUniform;
        }

    }
}
