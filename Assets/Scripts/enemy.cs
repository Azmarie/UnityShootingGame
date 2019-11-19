using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject[] targets;
    public GameObject player;
    public GameObject mazzlePrefab;
    public GameObject shotSound;
    public GameObject bulletHole;
    public GameObject end, start; // The gun start and end point
    public GameObject gun;

    public float fireRate = 0.2f;  
    // public int attackDamage = 10;
    public float aimRate = 0.2f;
    private float nextFire; 
    // private float minAngle;
    // private float maxAngle;

    // float gunShotTime = 0.2f;
    // float gunReloadTime = 1.0f;
    
    int index = 0;
    bool playerInRange = false;
    bool followPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        // GetComponent<Animator>().SetBool("run", true);
    }

    // Update is called once per frame
    void Update()
    {
        // Detect the player
        // Vector3 foward = transform.TransformDirection(transform.forward);
        Vector3 toPlayer = player.transform.position - transform.position;
        float dist2Player = Vector3.Distance(player.transform.position, transform.position);

        // float dot = Vector3.Dot(foward, toPlayer)/(foward.magnitude*toPlayer.magnitude);
        // var acos = Mathf.Acos(dot);
        // var angle = acos*180/Mathf.PI;
        float angle = Vector3.Angle(toPlayer, transform.forward);

        if (angle <= 40 && angle >= -40 && dist2Player <= 10){
            followPlayer = true;

            if(dist2Player <= 5) {
                playerInRange = true;
            } else {
                playerInRange = false;
            }
        }

        if(!followPlayer) {
            // Debug.Log("Not following");
            FollowTargets();
        }

        if(followPlayer) {
            // Debug.Log("Following player");
            GetComponent<Animator>().SetBool("fire", false);
            GetComponent<Animator>().SetBool("run", true);
            FollowPlayer();

            if (playerInRange) {
                // Debug.Log("Player in range");
                GetComponent<Animator>().SetBool("run", false);
                // GetComponent<Animator>().SetBool("fire", true);
                AttackPlayer();
            } 
            else {
                // Debug.Log("Player not in range anymore, but still following player");
                GetComponent<Animator>().SetBool("fire", false);
                GetComponent<Animator>().SetBool("run", true);
            }
        }

    }

    void FollowTargets(){
        // Follow the path defined by the targets
        int numtargets = targets.Length;
        Vector3 temPos = new Vector3(targets[index].transform.position.x, transform.position.y, targets[index].transform.position.z);
        float dist = Vector3.Distance(temPos, transform.position);
        transform.LookAt(temPos);
        Quaternion desiredRotation = Quaternion.LookRotation(temPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime);

        if (dist<1.0) {
            index = (index + 1) %numtargets;
            print(index);
        }
    }

    void FollowPlayer(){
        Vector3 playerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(playerPos);
        Quaternion desiredRotation = Quaternion.LookRotation(playerPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime);
    }

    void AttackPlayer(){
        if(Time.time > nextFire){
            GetComponent<Animator>().SetBool("fire", true);
            nextFire = Time.time + fireRate;
            shotDetection(); 
            addEffects();
        }
        // animator.SetBool("fire", true);
        
        // // Instantiating the muzzle prefab and shot sound
        
        // magBulletsVal = magBulletsVal - 1;
        // if (magBulletsVal <= 0 && remainingBulletsVal > 0)
        // {
        //     animator.SetBool("reloadAfterFire", true);
        //     gunReloadTime = 2.5f;
        //     Invoke("reloaded", 2.5f);
        // }
    }

    void shotDetection() 
    {   
        RaycastHit rayHit;
        // float randomAngle = new System.Random(minAngle, maxAngle);
        // Vector3 axis = new Vector3(1, 0, 1);
        // var rotation = Quaternion.AngleAxis(20, axis);
        var forward = end.transform.position - start.transform.position;
        
        // if(Physics.Raycast(end.transform.position, rotation*forward, out rayHit, 100.0f)){
        if(Physics.Raycast(end.transform.position, forward, out rayHit, 100.0f)){
            if(rayHit.transform.tag == "Player"){
                //Player take damage

            } else {
                Instantiate(bulletHole, rayHit.point+rayHit.transform.up*0.01f, rayHit.transform.rotation);
            }
        }
    }

    void addEffects() 
    {
        Destroy(Instantiate(shotSound, transform.position, transform.rotation), 2.0f);
        GameObject tempMuzzle = Instantiate(mazzlePrefab, end.transform.position, end.transform.rotation);
        tempMuzzle.GetComponent<ParticleSystem>().Play();
        Destroy(tempMuzzle, 2.0f);
    }

}
