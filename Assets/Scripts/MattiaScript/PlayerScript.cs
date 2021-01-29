using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GravityAffectedScript gravityForBoolean;

    // Start is called before the first frame update
    void Start()
    {
        gravityForBoolean = transform.GetComponent<GravityAffectedScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.position.y < transform.position.y)
        {
            transform.SetParent(other.transform);
        }
        else if (other.transform.position.y > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, other.transform.localPosition.z - (other.transform.localScale.z));
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.position.y > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, other.transform.position.y - (other.transform.localScale.y), transform.position.z);
        }
    }
    private void OnTriggerExit(Collider other)
    {

    }
}
