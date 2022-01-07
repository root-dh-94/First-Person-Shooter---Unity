using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class Gun_script : MonoBehaviour {

    public GameObject end, start; // The gun start and end point
    public GameObject gun;
    public Animator animator;

    public GameObject spine;
    public GameObject handMag;
    public GameObject gunMag;

    float gunShotTime = 0.1f;
    float gunReloadTime = 1.0f;
    Quaternion previousRotation;
    public float health = 100;
    public bool isDead;


    public Text magBullets;
    public Text remainingBullets;
    public Text health_disp;


    int magBulletsVal = 30;
    int remainingBulletsVal = 90;
    int magSize = 30;
    public GameObject headMesh;
    public static bool leftHanded { get; private set; }
    public GameObject bulletHole;
    public GameObject muzzleFlash;
    public GameObject shotSound;
    public GameObject enemy1;
    private enemy control;
    private Animator anim;
    public GameObject enemy2;
    private enemy2 control2;
    public GameObject enemy3;
    private enemy3 control3;
    public GameObject ammo_crate;
    // Use this for initialization
    void Start() {
        headMesh.GetComponent<SkinnedMeshRenderer>().enabled = false; // Hiding player character head to avoid bugs :)

        //Access scripts for different enemies
        control = enemy1.GetComponent<enemy>();
        control2 = enemy2.GetComponent<enemy2>();
        control3 = enemy3.GetComponent<enemy3>();


    }

    // Update is called once per frame
    void Update() {

        //Carry out death animation of health = 0 and isDead is true
        if (isDead == true)
        {
            GetComponent<CharacterMovement>().isDead = true;
            Death();
        }

        //Add health to UI
        if (health > 0)
        {
            health_disp.text = health.ToString();
        }
        else{
            health_disp.text = 0.ToString();
        }

        // Ammo crate Bonus
        if ( Vector3.Distance(transform.position, ammo_crate.transform.position) < 2f)
        {
            magBulletsVal = 30;
        }




        // Cool down times
        if (gunShotTime >= 0.0f)
        {
            gunShotTime -= Time.deltaTime;
        }
        if (gunReloadTime >= 0.0f)
        {
            gunReloadTime -= Time.deltaTime;
        }


        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && gunShotTime <= 0 && gunReloadTime <= 0.0f && magBulletsVal > 0 && !isDead)
        {
            //Shoots at enemies and controls their health accordingly
            shotDetection();

            //Adds muzzle flash and shot sound
            addEffects();
            animator.SetBool("fire", true);
            gunShotTime = 0.5f;

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


        if ((Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.R)) && gunReloadTime <= 0.0f && gunShotTime <= 0.1f && remainingBulletsVal > 0 && magBulletsVal < magSize && !isDead )
        {
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

    //runs death animation
    void Death()
    {
        animator.SetBool("dead", true);
    }


    public void ReloadEvent(int eventNumber) // appearing and disappearing the handMag and gunMag
    {
        if(eventNumber == 1)
        {
            handMag.GetComponent<SkinnedMeshRenderer>().enabled = true;
            gunMag.GetComponent<SkinnedMeshRenderer>().enabled = false;
        }

        if(eventNumber == 2)
        {
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
    }

    //Implements player shooting enemy and reducing enemy health
    void shotDetection()
    {
        RaycastHit rayHit;
        int layerMask = 1<<8;
        layerMask = ~layerMask;
        if(Physics.Raycast(end.transform.position, (end.transform.position -start.transform.position).normalized, out rayHit, 30f, layerMask))
        {
            if(rayHit.collider.tag == "enemy")
            {
                //reducing enemy health if shot
                control.health = control.health - 20;
            }

            if (control.health <= 0)
            {
                //if enemy health is 0 set isDead for enemy as true
                control.isDead = true;
            }

            if(rayHit.collider.tag == "enemy2")
            {
                control2.health = control2.health - 20;
            }

            if (control2.health <= 0)
            {

                control2.isDead = true;
            }
            if(rayHit.collider.tag == "enemy3")
            {
                control3.health = control3.health - 20;
            }

            if (control3.health <= 0)
            {

                control3.isDead = true;
            }
        }
    }

    void addEffects() // Adding muzzle flash, shoot sound and bullet hole on the wall
    {
        RaycastHit rayHit;
        int layerMask = 1<<8;
        layerMask = ~layerMask;
        if(Physics.Raycast(end.transform.position, (end.transform.position -start.transform.position).normalized, out rayHit, 100.0f, layerMask))
        {
            GameObject bulletHoleObject = Instantiate(bulletHole, rayHit.point + rayHit.collider.transform.up*0.01f, rayHit.collider.transform.rotation);
            Destroy(bulletHoleObject, 2.0f);
        }

        GameObject muzzleFlashObject = Instantiate(muzzleFlash, end.transform.position, end.transform.rotation);
        muzzleFlashObject.GetComponent<ParticleSystem>().Play();
        Destroy(muzzleFlashObject, 1.0f);

        Destroy((GameObject) Instantiate(shotSound, transform.position, transform.rotation), 1.0f);

    }

}
