using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
	
	// Update is called once per frame
	void Update ()
    {/*
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        print(mousePos);
        mousePos = new Vector3(mousePos.x, mousePos.y, transform.position.z);
        print(mousePos);*/

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        if (move != Vector3.zero)
            transform.forward = move.normalized;

        transform.position += move * speed * Time.deltaTime;
    }
}
