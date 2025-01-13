using Oculus.Platform;
using System.Net;
using UnityEngine;
using UnityEngine.PlayerLoop;
using System.Collections;
using System.Linq;
using Oculus.Interaction;
using System.Collections.Generic;
using System;

public class TrainingDroid: MonoBehaviour {
    public GameObject head;
     public GameObject eye;
    public GameObject body;
	public float bodyRadius;
	public GameObject player;
	public Transform pointA;
	public Transform pointB;
	public float speed;
	private Rigidbody rb;
	private bool reachedA;
	private float angularVelocity;
	private float headZ;
	public Transform headProxy;
	private List<Vector3> futurePositions = new List<Vector3>();


	private Vector3 spinDirection;
	void Start()
	{

		headZ = head.transform.rotation.z;
		rb = GetComponent<Rigidbody>();
		// Set initial position of the ball at the start point
		spinDirection = pointB.position - transform.position;
		for (int i = 0; i < 15; i++) {
			addFuture(true);
		}
		
	}

	void Update()
    {
		headZ = head.transform.rotation.z;
		

		Vector3 target = new Vector3(head.transform.position.x,head.transform.position.y, player.transform.position.z);
		headProxy.LookAt(player.transform);
		if (body.transform.position == pointB.position)
		{
			pointB.position = futurePositions[0];
			addFuture();
			spinDirection = (pointB.position-transform.position);
			Debug.Log(spinDirection.magnitude);
			spinDirection.x /= spinDirection.magnitude;
			spinDirection.y /= spinDirection.magnitude;
			spinDirection.z /= spinDirection.magnitude;
		}
		
		var step = Time.deltaTime*speed;
		Vector3 direction = Vector3.MoveTowards(body.transform.position, pointB.position, step);
		transform.position = direction;
		float degX = ((spinDirection.x * step)) / bodyRadius * Mathf.Rad2Deg;
		float degZ = ((spinDirection.z * step)) / bodyRadius * Mathf.Rad2Deg;
		float degY = ((spinDirection.y * step)) / bodyRadius * Mathf.Rad2Deg;
		body.transform.Rotate(degX, degY, degZ);


	}
	void addFuture(bool init = false)
	{
		Vector3 pos = new Vector3(Random.Range(-3.5f, 3.5f), 0.5f, Random.Range(-3.5f, 3.5f));
		futurePositions.Add(pos);
		if(!init)
		{
			futurePositions.RemoveAt(0);
		}
	}




}
