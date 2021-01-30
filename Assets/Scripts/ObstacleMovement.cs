using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType { Circular, Bounce, FromTop}
public class ObstacleMovement : MonoBehaviour
{
    public float speed = 5;
    public Transform player;
    public MovementType movement;
    public Transform spawnCenter;
    private Rigidbody rb;
    private delegate void UpdateMethod();
    private delegate void CollisionMethod<T>(T collision);
    private UpdateMethod update;
    private CollisionMethod<Collision> collisionMethod;
    private Vector3 rotationAxis;
    private Vector3 bounceModeDirection;
    private float angle =0;
    private Vector3 dirFromTop;
    private bool canUpdateFromTop;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<Rigidbody>(out rb);
        switch (movement)
        {
            case MovementType.Circular:
                update = UpdateCircular;
                rotationAxis = transform.position - spawnCenter.position;
                break;
            case MovementType.Bounce:
                update = UpdateBounce;
                collisionMethod = CollisionBounce;
                StartBounceMod();
                break;
            case MovementType.FromTop:
                update = UpdateFromTop;
                collisionMethod = CollisionFromTop;
                dirFromTop = player.position - transform.position;
                canUpdateFromTop = true;
                break;
            default:
                break;
        }
    }
    void StartBounceMod()
    {
        transform.Rotate(transform.up, Random.Range(0, 360));
        //rb.velocity = transform.right * speed;
        rb.useGravity = false;
        //float x = Random.Range(0, 10f);
        //float z = Random.Range(0, 10f);
        //Vector3 startDir = new Vector3(x, 0, z).normalized;
        Vector3 centerPos = spawnCenter.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        Vector3 startDir = (transform.position - spawnCenter.position).normalized;
        Debug.Log(startDir);
        bounceModeDirection = startDir;
    }
    void Autorotate()
    {
        angle += 500 * Time.deltaTime;

        transform.Rotate(new Vector3(0, 0, angle));
    }
    void LookUp()
    {
        transform.forward = Vector3.up;

    }
    void UpdateCircular()
    {
        spawnCenter.Rotate(Vector3.up,speed * Time.deltaTime);
        Vector3 dir = spawnCenter.position - transform.position;

        LookUp();
        Autorotate();
    }
    void UpdateBounce()
    {
        transform.position = transform.position + bounceModeDirection * 30 * Time.deltaTime;
        LookUp();
        Autorotate();
    }
    void CollisionBounce(Collision collision)
    {
        Vector3 newDir = Vector3.Reflect(bounceModeDirection, collision.contacts[0].normal);
        Debug.Log(collision.contactCount);
        newDir.Normalize();
        bounceModeDirection = new Vector3(newDir.x, 0, newDir.z);
    }
    void CollisionFromTop(Collision collision)
    {
        canUpdateFromTop = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(rb);
        collisionMethod(collision);
    }

    void UpdateFromTop()
    {
        if (canUpdateFromTop)
        {
            transform.position += dirFromTop * 50 * Time.deltaTime;
            LookUp();
            Autorotate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Autorotate();
        update();
    }
}
