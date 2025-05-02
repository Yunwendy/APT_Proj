using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abc : MonoBehaviour
{

    GameObject boltPivot;
    GameObject bolt;

    // Start is called before the first frame update
    void Start()
    {
        //boltPivot = GameObject.Find("BoltPivot_1");
        //bolt = GameObject.Find("Bolt_1");

        //boltPivot.transform.position = bolt.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 2, Color.blue);    // Z√‡
        Debug.DrawRay(transform.position, transform.up * 2, Color.green);        // Y√‡
        Debug.DrawRay(transform.position, transform.right * 2, Color.red);       // X√‡

        //boltPivot.transform.Rotate(Vector3.forward * 50 * Time.deltaTime, Space.Self);
        transform.Rotate(Vector3.forward * 50 * Time.deltaTime, Space.Self);
        //transform.Rotate(Vector3.forward * 50 * Time.deltaTime, Space.Self);
    }
}
