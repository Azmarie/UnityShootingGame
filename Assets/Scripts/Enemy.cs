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
    public GameObject cover;

    public float fireRate = 0.2f;  
    public float aimRate = 0.2f;
    private float nextFire = 0f; 

    public float health = 100;
    public bool isDead;
    
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
        if(player.GetComponent<GunVR>().health > 0){

            if (health <= 0){
                isDead = true;
                deadEffects();
            } else {

                // Detect the player
                Vector3 toPlayer = player.transform.position - transform.position;
                float dist2Player = Vector3.Distance(player.transform.position, transform.position);
                float angle = Vector3.Angle(toPlayer, transform.forward);

                if (angle <= 40 && angle >= -40 && dist2Player <= 15){
                    followPlayer = true;

                    if(dist2Player <= 10) {
                        playerInRange = true;
                    } else {
                        playerInRange = false;
                    }
                }

                if(!followPlayer) {
                    FollowTargets();
                }

                if(followPlayer) {
                    float dist2Cover = Vector3.Distance(cover.transform.position, transform.position); 

                    GetComponent<Animator>().SetBool("run", true);
                    // For enemy 2 and 3, who don't take cover
                    if(dist2Cover>20.0f) {
                        FollowPlayer();
                        if(playerInRange) {
                            AttackPlayer();
                        }
                    }

                    // For enemy 1, who takes cover only in room 1 :)
                    if(playerInRange) {
                        if (dist2Cover<10.0f) SeekCover();
                        if (dist2Cover<1.6f) {
                            GetComponent<Animator>().SetBool("run", false);
                            AttackPlayer();
                        }
                    }
                }   
            }
        }
    }


    public void Being_shot(float damage) // getting hit from player
    {
        health -= damage;
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
        }
    }

    void FollowPlayer(){
        Vector3 playerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(playerPos);
        Quaternion desiredRotation = Quaternion.LookRotation(playerPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime);
    }

    void SeekCover(){
        GetComponent<Animator>().SetBool("run", true);
        Debug.Log("Seeking Cover");
        float dist2Cover = Vector3.Distance(cover.transform.position, transform.position); 
        Vector3 coverPos = new Vector3(cover.transform.position.x, transform.position.y, cover.transform.position.z);
        transform.LookAt(coverPos);

     }

    void AttackPlayer(){
        FollowPlayer();
        if (nextFire > fireRate)
        {
            shotDetection(); 
            addEffects();
            nextFire = 0;
        }
        nextFire += Time.deltaTime;
    }

    void shotDetection() 
    {   
        GetComponent<Animator>().SetTrigger("fire");
        RaycastHit rayHit;
        Vector3 rand = end.transform.up * Random.Range(-0.1f, 0.1f) + end.transform.right * Random.Range(-0.1f, 0.1f);
        Vector3 end_rand = end.transform.position + rand;
        var forward = end_rand - start.transform.position;
        
        if(Physics.Raycast(end.transform.position, forward, out rayHit, 100.0f)){
            if(rayHit.transform.tag == "Player") rayHit.transform.GetComponent<GunVR>().Being_shot(20);
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
        gun.transform.parent = null;
        gun.GetComponent<Collider>().enabled = true;
        if(gun.GetComponent<Rigidbody>() == null)
            gun.AddComponent<Rigidbody>().isKinematic = false;
    }
}