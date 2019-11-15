using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public GameObject[] targets;
    int index = 0;
    int numtargets = 4; //targets.Length;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        // transform.forward;
        // new Vector3(player.transform.position - transform.position);
        // Vector3.Dot

        // Angle: 20/-20 "the cos value of the angle"
        // Distance: between the two, move towards the player
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temPos = new Vector3(targets[index].transform.position.x, transform.position.y, targets[index].transform.position.z);
        float dist = Vector3.Distance(temPos, transform.position);
        transform.LookAt(temPos);
        Quaternion desiredRotation = Quaternion.LookRotation(temPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime);

        if (dist<1.0) {
            index = (index + 1)%numtargets;
        }
    }
}
