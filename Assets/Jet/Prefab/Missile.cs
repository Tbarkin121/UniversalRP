﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    public Rigidbody2D rb;
    private float first_stage_counter = 1.0f;
    private float second_stage_counter = 1.0f;
    private bool stage_one_done = false;
    private bool stage_two_start = false;
    private bool missile_fule_empty = false;
    private bool armed = false;
    public ParticleSystem smokePrefab;
    // Start is called before the first frame update
    void Start()
    {
        // rb.AddForce(transform.right*10);
        Destroy(gameObject,5f);
        first_stage_counter = Random.Range(0.5f, first_stage_counter);
    }

    // Update is called once per frame
    void Update()
    {
        if(!stage_one_done)
        {
            StartCoroutine(StageOne());
        }
        if(stage_one_done & !stage_two_start)
        {
            rb.velocity = rb.velocity*0.1f;
            stage_two_start = true;
            armed = true;
            StartCoroutine(StageTwo());
        }
        
    }
    public IEnumerator StageOne()
    {
        float counter = 0f;
        while(counter<first_stage_counter)
        {
            counter += Time.deltaTime;
            yield return null;
        }
        stage_one_done = true;
        
    }
    public IEnumerator StageTwo()
    {
        float counter = 0f;
        while(counter<second_stage_counter)
        {
            counter += Time.deltaTime;
            rb.AddForce(transform.up*speed);
            yield return null;
        }
        missile_fule_empty = true;
        
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Debug.Log(hitInfo.name);
        if(armed)
        {
            ShipMotor ship = hitInfo.GetComponent<ShipMotor>();
            if(ship != null)
            {
                ship.TakeDamage(damage);
                Instantiate(smokePrefab, transform.position, transform.rotation);
                Destroy(gameObject);
                
            }else{
                Instantiate(smokePrefab, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }        
    }
}
