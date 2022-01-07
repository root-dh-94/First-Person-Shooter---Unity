using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy2 : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    public GameObject end, start;
    public GameObject gun;
    public bool isDead;
    public GameObject[] targets;
    private float dist_targ1;
    private float dist_targ2;
    private float dist_targ3;
    private float dist_targ4;
    private bool start_cond;
    private bool target2;
    private bool target3;
    private bool target4;
    public float dist_player;
    public float angle_player;
    public GameObject player;
    public float fire_rate;
    private float next_bullet;
    public float health = 100;
    public GameObject muzzleFlash;
    public GameObject shotSound;
    private Gun_script control;
    private Animator anim;
    public GameObject cover;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("detect_player", false);
        animator.SetBool("standby_dist", false);
        start_cond = true;
        target2 = false;
        target3 = false;
        target4 = false;
        fire_rate = .2f;

        //Get access to player script
        control = player.GetComponent<Gun_script>();
        next_bullet = 0;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == true && animator.GetBool("dead") == false)
        {
            Death();

            //Remove gun as child and add rigid body and collider
            Invoke("sep_gun", 2f);

        }
        if (isDead == false && control.isDead == false)
        {
            Walk();
            Detect();

            //Fire if enemy within 10 distance and if next bullet can be fired (5 per second)
            if ((animator.GetBool("standby_dist") == true) && (Time.time >= next_bullet) && isDead == false)
            {
                Firing();
                animator.SetBool("firing", true);
                next_bullet = Time.time + fire_rate;
            }
        }
    }

    //Remove gun as child and add rigid body and collider
    void sep_gun()
    {
        gun.transform.parent = null;
        gun.AddComponent<Rigidbody>();
        gun.AddComponent<BoxCollider>();
    }

    //Death animation
    void Death()
    {
        animator.SetBool("dead", true);
    }

    void Detect()
    {
        //Calculate distance and angel between enemy and player
        dist_player = Vector3.Distance(transform.position, player.transform.position);
        angle_player = Mathf.Abs(Vector3.Angle((player.transform.position - transform.position), transform.forward));

        if (((dist_player <= 20) && (angle_player <= 45)) || (health < 100))
        {
            animator.SetBool("detect_player", true);
        }

        if (animator.GetBool("detect_player") == true)
        {
            //Turn to player
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((player.transform.position - transform.position)), Time.deltaTime*3);

        }

        if (dist_player <= 10 && animator.GetBool("detect_player") == true)
        {
            // enters standby animation-ready to shoot
            animator.SetBool("standby_dist", true);
            animator.SetFloat("running_dist", dist_player);
        }

        if ((animator.GetBool("standby_dist") == true) && (dist_player > 10) )
        {
            //Follow player  and maintain distance of 10
            animator.SetFloat("running_dist", dist_player);
            animator.SetBool("standby_dist", false);
            transform.position = (transform.position - player.transform.position).normalized * 10 + player.transform.position;
        }
    }

    void Firing()
    {
        RaycastHit rayHit;

        //Add additional offset since raycast points to left of player
        Vector3 direction = (Quaternion.AngleAxis(-4, Vector3.up) * (end.transform.position -start.transform.position));

        //Add bullet spread
        direction.x += Random.Range(-.2f, .2f);
        direction.y += Random.Range(-.25f, .25f);

        if(Physics.Raycast(end.transform.position, direction.normalized, out rayHit, 30.0f))
        {
            //Control player health depending on what part is hit
            if (rayHit.collider.tag == "Player")
            {
                control.health = control.health - 30;
                AddEffects();
            }

            if (rayHit.collider.tag == "head")
            {
                control.health = control.health - 100;
            }

            if (rayHit.collider.tag == "arm")
            {
                control.health = control.health - 10;
            }

            if (rayHit.collider.tag == "leg")
            {
                control.health = control.health - 20;
            }

            if (control.health <= 0)
            {
                control.isDead = true;
            }
        }
    }

    void AddEffects()
    {
        GameObject muzzleFlashObject = Instantiate(muzzleFlash, end.transform.position, end.transform.rotation);
        muzzleFlashObject.GetComponent<ParticleSystem>().Play();
        Destroy(muzzleFlashObject, 1f);

        Destroy((GameObject) Instantiate(shotSound, transform.position, transform.rotation), 1f);
    }

    //Enemy walks along walkpath
    void Walk()
    {
        if (animator.GetBool("detect_player") == false)
        {

            if (start_cond == true)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targets[0].transform.position - transform.position), Time.deltaTime);
            }

            if (target2 == true)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targets[1].transform.position - transform.position), Time.deltaTime);
            }

            if (target3 == true)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targets[2].transform.position - transform.position), Time.deltaTime);
            }

            if (target4 == true)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targets[3].transform.position - transform.position), Time.deltaTime);
            }

            dist_targ1 = Vector3.Distance(transform.position, targets[0].transform.position);
            dist_targ2 = Vector3.Distance(transform.position, targets[1].transform.position);
            dist_targ3 = Vector3.Distance(transform.position, targets[2].transform.position);
            dist_targ4 = Vector3.Distance(transform.position, targets[3].transform.position);

            //Move to next target
            if (dist_targ1 < .5f)
            {
                start_cond = false;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targets[1].transform.position - transform.position), Time.deltaTime);
                target2 = true;
            }

            if (dist_targ2 < .5f)
            {
                target2 = false;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targets[2].transform.position - transform.position), Time.deltaTime);
                target3 = true;
            }

            if (dist_targ3 < .5f)
            {
                target3 = false;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targets[3].transform.position - transform.position), Time.deltaTime);
                target4 = true;
            }


            if (dist_targ4 < .5f)
            {
                target4 = false;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targets[0].transform.position - transform.position), Time.deltaTime);
                start_cond = true;
            }
        }

    }
}
