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
    public float aimRate = 0.2f;
    private float nextFire = 0f; 

    public float health = 100;
    public bool isDead;

    // float gunShotTime = 0.2f;
    // float gunReloadTime = 1.0f;
    
    int index = 0;
    bool playerInRange = false;
    bool followPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Detect the player
        Vector3 toPlayer = player.transform.position - transform.position;
        float dist2Player = Vector3.Distance(player.transform.position, transform.position);
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
            GetComponent<Animator>().SetBool("run", true);
            FollowPlayer();

            if (playerInRange) {
                // Debug.Log("Player in range");
                AttackPlayer();
            } 
            // else {
            //     Debug.Log("Player not in range anymore, but still following player");
            // }
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

        if (nextFire > fireRate)
        {
            GetComponent<Animator>().SetBool("run", false);
            GetComponent<Animator>().SetTrigger("fire");
            shotDetection(); 
            addEffects();
            nextFire = 0;
        }
        nextFire += Time.deltaTime;
        
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
        Vector3 rand = end.transform.up * Random.Range(-0.1f, 0.1f) + end.transform.right * Random.Range(-0.1f, 0.1f);
        Vector3 end_rand = end.transform.position + rand;
        var forward = end_rand - start.transform.position;
        
        if(Physics.Raycast(end.transform.position, forward, out rayHit, 100.0f)){
            if(rayHit.transform.tag == "Player"){
                //Player take damage
                GetComponent<GunVR>().Being_shot(20);
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

    // void isDead(){
        // AddComponent<Rigidbody>(); 
        // disable is kinematic to add phsics
        // gun.transform.parent = null; // to make it an independent object
        // make sure thre's a box collider on your gun when it's independent
    // }

}


// https://free3d.com/
// For ammo models (as objects)