using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafBlower : MonoBehaviour
{
    [SerializeField] bool isCollidingWithLeaf;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && isCollidingWithLeaf)
        {
            Blow();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("colliding");
        if (collision.gameObject.CompareTag("Leaf"))
        {
            isCollidingWithLeaf = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Leaf"))
        {
            isCollidingWithLeaf = false;
        }
    }
    void Blow()
    {
        Debug.Log("blow");
    }
}
