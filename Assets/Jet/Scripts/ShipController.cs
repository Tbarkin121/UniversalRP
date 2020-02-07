using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ShipMotor))]
public class ShipController : MonoBehaviour
{
    Camera cam;
    public Interactable focus;
    public LayerMask movementMask;
    public Transform firePointLeft;
    public Transform firePointRight;
    public Transform machinegunLeft;
    public Transform machinegunRight;
    public GameObject missilePrefab;
    public GameObject impactEffect;
    public LineRenderer lineRendererL;
    public LineRenderer lineRendererR;
    [SerializeField]
    private float speed = 5f;
    private float turnForce = 5f;
    public int damage = 5;
    private ShipMotor motor;
    private bool whichBay = false;

    void Start()
    {
        motor = GetComponent<ShipMotor>();
        cam = Camera.main;
        if(cam == null)
        {
            Debug.Log("No Camera Found"); 
        }
    }

    void Update()
    {
        // If you want to disable input while you are using menues... no thanks
        // if (EventSystem.current.IsPointerOverGameObject())
        //     return;
        //Calculate Total Engine Force (Break down into engines later)
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector2 _movVerticle = transform.up * _zMov;
        Vector2 _velocity = (_movVerticle).normalized * speed;
        // Debug.Log("Velocity : [" + _velocity[0] + ", " + _velocity[1] + "]");

        //Apply movment
        motor.Move(_velocity);
        motor.Rot(-turnForce*_xMov);
        // gameObject.GetComponent<Rigidbody2D>().AddTorque(1.0f*Time.deltaTime);

        if(Input.GetMouseButtonDown(0))
        {
            ShootMissile();

            Vector3 mousePos = GetWorldPositionOnPlane(Input.mousePosition, 0f);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            // Debug.Log("MousePosition" + Input.mousePosition);
            // Debug.Log("MousePositionWorldPoint" + mousePos);
            if(hit.collider != null)
            {
                Debug.Log("We Hit" + hit.collider.name + " " + hit.point);
                
            }
            RemoveFocus();
        }
        if(Input.GetMouseButtonDown(1))
        {
            StartCoroutine(ShootMachineGun());
            Vector3 mousePos = GetWorldPositionOnPlane(Input.mousePosition, 0f);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            // Debug.Log("MousePosition" + Input.mousePosition);
            // Debug.Log("MousePositionWorldPoint" + mousePos);
            if(hit.collider != null)
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if(interactable != null)
                {
                    SetFocus(interactable);
                }
            }
        }

        // if(Input.GetButtonDown("Fire1"))
        // {
        //     ShootMissile();
        // }
        // if(Input.GetButtonDown("Fire2"))
        // {
        //     StartCoroutine(ShootMachineGun());
        // }
        void ShootMissile()
        {
            if(whichBay)
            {
                whichBay = false;
                GameObject _missile = Instantiate(missilePrefab, firePointRight.position, firePointRight.rotation);
                _missile.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
                _missile.GetComponent<Rigidbody2D>().angularVelocity = gameObject.GetComponent<Rigidbody2D>().angularVelocity;
                _missile.GetComponent<Rigidbody2D>().AddForce(transform.right*20f);
                
            }else{
                whichBay = true;
                GameObject _missile = Instantiate(missilePrefab, firePointLeft.position, firePointLeft.rotation);
                _missile.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
                _missile.GetComponent<Rigidbody2D>().angularVelocity = gameObject.GetComponent<Rigidbody2D>().angularVelocity;
                _missile.GetComponent<Rigidbody2D>().AddForce(-1.0f*transform.right*20f);
            }
            
        }
        IEnumerator ShootMachineGun()
        {
            
            RaycastHit2D hitInfoL = Physics2D.Raycast(machinegunLeft.position+machinegunLeft.up, machinegunLeft.up);
            RaycastHit2D hitInfoR = Physics2D.Raycast(machinegunRight.position+machinegunRight.up   , machinegunRight.up);

            if(hitInfoL)
            {
                ShipMotor ship = hitInfoL.transform.GetComponent<ShipMotor>();
                if (ship != null)
                {
                    ship.TakeDamage(damage);
                    Instantiate(impactEffect, hitInfoL.point, Quaternion.identity);
                    lineRendererL.SetPosition(0, machinegunLeft.position);
                    lineRendererL.SetPosition(1, hitInfoL.point);
                }
            }else
            {
                lineRendererL.SetPosition(0, machinegunLeft.position);
                lineRendererL.SetPosition(1, machinegunLeft.position + machinegunLeft.up*100);
            }
            if(hitInfoR)
            {
                ShipMotor ship = hitInfoR.transform.GetComponent<ShipMotor>();
                if (ship != null)
                {
                    ship.TakeDamage(damage);
                    Instantiate(impactEffect, hitInfoR.point, Quaternion.identity);
                    lineRendererR.SetPosition(0, machinegunRight.position);
                    lineRendererR.SetPosition(1, hitInfoR.point);
                }
            }else
            {
                lineRendererR.SetPosition(0, machinegunRight.position);
                lineRendererR.SetPosition(1, machinegunRight.position + machinegunRight.up*100);
            }
            lineRendererL.enabled = true;
            lineRendererR.enabled = true;
            yield return 0;
            lineRendererL.enabled = false;
            lineRendererR.enabled = false;
        }
    }

    void SetFocus (Interactable newFocus)
    {
        if (newFocus != focus) 
        {
            if (focus != null)
                focus.OnDefocused();
            focus = newFocus;
            motor.FollowTarget(newFocus);
        }
        newFocus.OnFocused(transform);
    }
    void RemoveFocus ()
    {
        if (focus != null)
            focus.OnDefocused();

        focus = null;
        motor.StopFollowingTarget();
    }
    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z) 
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0,0,z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
}
