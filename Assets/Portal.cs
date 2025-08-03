using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
   void OnTriggerEnter2D(Collider2D other)
{
    Debug.Log("Entered portal");

    if (other.CompareTag("Player"))
    {
        other.transform.position = new Vector2(-8f, -2f);
    }
}

}
