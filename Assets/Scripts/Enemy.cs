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
    public GameObject end, start;
    public GameObject gun;

    public float fireRate = 0.2f;  
    public float aimRate = 0.2f;
    private float nextFire = 0f; 

    public float health = 100;
    public bool isDead;
    
    int index = 0;
    bool playerInRange = false;
    bool followPlayer = false;
    bool Keep_follow_target = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 ){
            isDead = true;
            deadEffects();
        } else {

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
                Debug.Log("Not following");
                FollowTargets();
            }

            if(followPlayer) {
                Debug.Log("Following player");
                GetComponent<Animator>().SetBool("run", true);
                FollowPlayer();

                if (playerInRange) {
                    GetComponent<Animator>().SetBool("run", false);
                    Debug.Log("Player in range");
                    AttackPlayer();
                } 
                // else {
                //     GetComponent<Animator>().SetBool("run", true);
                //     Debug.Log("Player not in range, following player");
                // }
            }   
        }

    }

    public void Being_shot(float damage) // getting hit from player
    {
        Debug.Log("Enemy being shot");
        // use component to get enemy's health
        // GetComponent<CharacterMovement>().isDead = true;
        // GetComponent<CharacterController>().enabled = false;
        health -= damage;
        Debug.Log(health);
        // chande isDead to true if it's dead
        // change dead to true in the anamator
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
                if(GetComponent<GunVR>().health <= 0){
                    Debug.Log("player supposed o be dead");
                    GetComponent<CharacterMovement>().isDead = true;
                    GetComponent<GunVR>().isDead = true;
                }
            }
            // else {
            //     Instantiate(bulletHole, rayHit.point+rayHit.transform.up*0.01f, rayHit.transform.rotation);
            // }
        }
    }

    void addEffects() 
    {
        Destroy(Instantiate(shotSound, transform.position, transform.rotation), 2.0f);
        GameObject tempMuzzle = Instantiate(mazzlePrefab, end.transform.position, end.transform.rotation);
        tempMuzzle.GetComponent<ParticleSystem>().Play();
        Destroy(tempMuzzle, 2.0f);
    }

    void deadEffects(){
        GetComponent<CharacterController>().enabled = false;
        GetComponent<Animator>().SetBool("die", true);
        gun.transform.parent = null; // to make it an independent object
        gun.GetComponent<Collider>().enabled = true;
        gun.AddComponent<Rigidbody>().isKinematic = false;
    }
}