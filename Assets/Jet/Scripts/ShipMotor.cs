/*  .-------.     .--.                   
    |_     _|--.--|  |.----..---.
      |   | |  |  |  |  ^__|   _|
      |___| |__   |__|_____|__| 
             __|  |     
            |_____|
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class ShipMotor : MonoBehaviour
{

    /////*******************************************/////
    /////                   VARS                    /////  
    /////*******************************************/////

    private Vector2 velocity = Vector2.zero;
    private float rotation = 0f;
    private Rigidbody2D rb;
    public int Health = 100;
    public float rotation_speed_mod = 0.25f;
    Transform target;


    /////*******************************************/////
    /////                 UPDATE                    /////  
    /////*******************************************/////

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Gets a movement vector
    public void Move(Vector2 _velocity)
    {
        velocity = _velocity;
    }
    public void Rot(float _rotation)
    {
        rotation = _rotation;
    }

    // Run every physics iteration
    void FixedUpdate()
    {
        PerformMovement();
    }

    // Perform Movement based on velocity variable
    void PerformMovement()
    {
        if(velocity != Vector2.zero)
        {
            // rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            if(velocity.magnitude>0){
                if(rb.velocity.magnitude<5){
                    rb.AddForce(velocity);
                }
            }
            if(velocity.magnitude<0){
                if(rb.velocity.magnitude>-5){
                    rb.AddForce(velocity);
                }
            }
            
        }
        if(rotation != 0f)
        {
            if(rotation > 0 & rb.angularVelocity<360)
            {
                rb.AddTorque(rotation*Time.deltaTime*rotation_speed_mod);
            }
            if(rotation < 0 & rb.angularVelocity>-360)
            {
                rb.AddTorque(rotation*Time.deltaTime*rotation_speed_mod);
            }
            // gameObject.transform.Rotate(new Vector3(0f, 0f, rotation));
        }
    }

    public void FollowTarget (Interactable newTarget)
    {
        target = newTarget.interactionTransform;
    }
    public void StopFollowingTarget()
    {
        target = null;
    }

    void Update ()
    {
        if (target != null)
        {
            //Do PID controller to follow target... doesn't need to happen right now
            //Also maybe do that in a coroutine
        }
    }

    public void TakeDamage(int damage) 
    {
        Health -= damage;
        if(Health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }

}