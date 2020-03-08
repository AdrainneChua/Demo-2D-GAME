using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Transform pos1, pos2;
    public float speed;
    public Transform startPos;

    Vector3 nextpos;

    // Start is called before the first frame update
    void Start()
    {
        nextpos = startPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position == pos1.position){
            nextpos = pos2.position;
        }
        if(transform.position == pos2.position){
            nextpos = pos1.position;
        }
        transform.position = Vector3.MoveTowards(transform.position, nextpos, speed * Time.deltaTime);
    }

    private void OnDrawGizmos(){
        Gizmos.DrawLine(pos1.position, pos2.position);
    }

     private void OnTriggerEnter2D (Collider2D collision) {
          
        if (collision.gameObject.tag == "Player") {
           Debug.Log ("test1");
           collision.transform.SetParent(transform);

        }

    }

    private void OnTriggerExit2D (Collider2D collision) {
       
        if (collision.gameObject.tag == "Player") {
            Debug.Log ("test2");
           collision.transform.SetParent(null);

        }

    }
    
}
