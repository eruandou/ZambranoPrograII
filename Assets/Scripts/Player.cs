using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;


    private Animator anim;




    private void Start()
    {
        anim = GetComponent<Animator>();
    }



    private void Update()
    {
        CheckMovement();
    }


    private void CheckMovement()
    {
        float moveX = Mathf.Abs(Input.GetAxis("Horizontal")) * speed * Time.deltaTime ;
        float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        if (Input.GetAxisRaw("Horizontal") < 0) transform.rotation = Quaternion.LookRotation(-Vector3.forward);
        else if (Input.GetAxisRaw("Horizontal") > 0) transform.rotation = Quaternion.LookRotation(Vector3.forward);

        transform.position += transform.right * speed * Time.deltaTime * moveX + Vector3.up * moveY * speed * Time.deltaTime;

        if (moveX != 0 || moveY != 0) anim.SetBool("Walking", true);
        else anim.SetBool("Walking", false);




    }



}
