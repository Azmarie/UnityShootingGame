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

    public float timeBetweenAttacks = 0.2f;
    // public int attackDamage = 10;
    public float aimRate = 0.2f;

    float gunShotTime = 0.1f;
    float gunReloadTime = 1.0f;
    
    int index = 0;
    bool playerInRange = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(!playerInRange) {
            FollowTargets();
        }

        // Detect the player
        Vector3 foward = transform.TransformDirection(transform.forward);
        Vector3 toPlayer = player.transform.position - transform.position;
        float dist2Player = Vector3.Distance(player.transform.position, transform.position);

        float dot = Vector3.Dot(foward, toPlayer)/(foward.magnitude*toPlayer.magnitude);
        var acos = Mathf.Acos(dot);
        var angle = acos*180/Mathf.PI;

        if (angle <= 20 && angle >= -20 && dist2Player <= 10){
            if(!playerInRange)
                Debug.Log("Player within the Angle/Distance Range, follow/ start shooting");
            playerInRange = true;
        }

        if (playerInRange) {
            FollowPlayer();
            AttachPlayer();
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

    void AttachPlayer(){
        // Cool down times
        if (gunShotTime >= 0.0f)
        {
            gunShotTime -= Time.deltaTime;
        }
        // if (gunReloadTime >= 0.0f)
        // {
        //     gunReloadTime -= Time.deltaTime;
        // }

        shotDetection(); 
        addEffects();
        // animator.SetBool("fire", true);
        gunShotTime = 0.5f;
        
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
        if(Physics.Raycast(end.transform.position, (end.transform.position - start.transform.position), out rayHit, 100.0f)){
            if(rayHit.transform.tag == "player"){
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
