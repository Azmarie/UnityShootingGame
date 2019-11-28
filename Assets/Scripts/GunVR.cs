using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GunVR : MonoBehaviour {

    public GameObject end, start; // The gun start and end point
    public GameObject gun;
    public GameObject AmmoBox;

    public Animator animator;
    
    public GameObject spine;
    public GameObject handMag;
    public GameObject gunMag;
    public GameObject mazzlePrefab;
    public GameObject shotSound;
    public GameObject bulletHole;

    float gunShotTime = 0.1f;
    float gunReloadTime = 1.0f;
    Quaternion previousRotation;
    public float health = 100;
    public bool isDead;

    public Text magBullets;
    public Text remainingBullets;
    public Text health_text;

    int magBulletsVal = 30;
    int remainingBulletsVal = 90;
    int magSize = 30;
    public GameObject headMesh;
    public static bool leftHanded { get; private set; }

    // Use this for initialization
    void Start() {
        headMesh.GetComponent<SkinnedMeshRenderer>().enabled = true; // Hiding player character head to avoid bugs :)
    }

    // Update is called once per frame
    void Update() {

        float dist2AmmoBox = Vector3.Distance(AmmoBox.transform.position, transform.position);

        // Cool down times
        if (gunShotTime >= 0.0f)
        {
            gunShotTime -= Time.deltaTime;
        }
        if (gunReloadTime >= 0.0f)
        {
            gunReloadTime -= Time.deltaTime;
        }


        OVRInput.Update();
        
        // if ((OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButtonDown(0))
        //     && gunShotTime <= 0 && gunReloadTime <= 0.0f && magBulletsVal > 0 && !isDead)
        if( (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButtonDown(0)) && gunShotTime <= 0 && gunReloadTime <= 0.0f && magBulletsVal > 0 && !isDead)
        {
            shotDetection(); 

            addEffects();

            animator.SetBool("fire", true);
            gunShotTime = 0.5f;
            
            // Instantiating the muzzle prefab and shot sound
            
            magBulletsVal = magBulletsVal - 1;
            if (magBulletsVal <= 0 && remainingBulletsVal > 0)
            {
                animator.SetBool("reloadAfterFire", true);
                gunReloadTime = 2.5f;
                Invoke("reloaded", 2.5f);
            }
        }
        else
        {
            animator.SetBool("fire", false);
        }
        
        // Bonus: if the ammo bos is within 3 meters, refill
        if(dist2AmmoBox <= 1 && remainingBulletsVal <120 && !isDead)
            remainingBulletsVal += 30;

        if ((OVRInput.GetDown(OVRInput.Button.Back) || OVRInput.Get(OVRInput.Button.Back) || OVRInput.GetDown(OVRInput.RawButton.Back) || OVRInput.Get(OVRInput.RawButton.Back))
            && gunReloadTime <= 0.0f && gunShotTime <= 0.1f && remainingBulletsVal > 0 && magBulletsVal < magSize && !isDead )
        {
            // Debug.Log("here");
            animator.SetBool("reload", true);
            gunReloadTime = 2.5f;
            Invoke("reloaded", 2.0f);
        }
        else
        {
            animator.SetBool("reload", false);
        }
        updateText();

    }

    public void Being_shot(float damage) // getting hit from enemy
    {
        Debug.Log("Player being shot, called from enemy script");
        health -= damage;

        if(health <= 0){
            isDead = true;
            GetComponent<Animator>().SetBool("dead", true);
            GetComponent<CharacterMovement>().isDead = true;
            GetComponent<CharacterController>().enabled = false;
            
            Invoke("reloadGame", 10f);

        }
    }

    void reloadGame(){
        Debug.Log("Scene loading: 0");
        SceneManager.LoadScene("SampleScene");
    }

    public void ReloadEvent(int eventNumber) // appearing and disappearing the handMag and gunMag
    {
        if(eventNumber == 1){
            handMag.GetComponent<SkinnedMeshRenderer>().enabled = true;
            gunMag.GetComponent<SkinnedMeshRenderer>().enabled = false;
        }
        if(eventNumber == 2){
            handMag.GetComponent<SkinnedMeshRenderer>().enabled = false;
            gunMag.GetComponent<SkinnedMeshRenderer>().enabled = true;
        }
    }

    void reloaded()
    {
        int newMagBulletsVal = Mathf.Min(remainingBulletsVal + magBulletsVal, magSize);
        int addedBullets = newMagBulletsVal - magBulletsVal;
        magBulletsVal = newMagBulletsVal;
        remainingBulletsVal = Mathf.Max(0, remainingBulletsVal - addedBullets);
        animator.SetBool("reloadAfterFire", false);
    }

    void updateText()
    {
        magBullets.text = magBulletsVal.ToString() ;
        remainingBullets.text = remainingBulletsVal.ToString();
        health_text.text = health.ToString();
    }

    void shotDetection() // Detecting the object which player shot 
    {   
        RaycastHit rayHit;
        if(Physics.Raycast(end.transform.position, (end.transform.position - start.transform.position), out rayHit, 100.0f)){
            Debug.Log(rayHit.transform.tag);

            if(rayHit.transform.tag == "Enemy"){
                Debug.Log("Enemy being shotttt");
                rayHit.transform.GetComponent<Enemy>().Being_shot(20);
            } else {
                Instantiate(bulletHole, rayHit.point+rayHit.transform.up*0.01f, rayHit.transform.rotation);
            }
        }
    }

    void addEffects() // Adding muzzle flash, shoot sound and bullet hole on the wall
    {
        Destroy(Instantiate(shotSound, transform.position, transform.rotation), 2.0f);
        GameObject tempMuzzle = Instantiate(mazzlePrefab, end.transform.position, end.transform.rotation);
        tempMuzzle.GetComponent<ParticleSystem>().Play();
        Destroy(tempMuzzle, 2.0f);
    }

}

