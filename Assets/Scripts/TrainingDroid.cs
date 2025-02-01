using Oculus.Platform;
using System.Net;
using UnityEngine;
using UnityEngine.PlayerLoop;
using System.Collections;
using System.Linq;
using Oculus.Interaction;
using System.Collections.Generic;
using System;

public class TrainingDroid : MonoBehaviour
{
	public GameObject head;
	public GameObject eye;
	public GameObject body;
	public float bodyRadius;
	public GameObject player;


	public Transform headProxy;


	public Droid droid;
	public GameObject ghost;
	public GameObject ghostBody;
	public GameObject ghostHead;
	public Transform ghostHeadProxy;

	Rigidbody rb;
	Rigidbody ghostRB;

	//public PostProcess shader;

	private Vector3 spinDirection;
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		ghostRB = ghost.GetComponent<Rigidbody>();
		spinDirection = droid.pointB.position - transform.position;
	}

	void Update()
	{

		headProxy.LookAt(player.transform);
		ghostHeadProxy.LookAt(player.transform);
		float step = Time.deltaTime * droid.speed;
		updateRotation(step);
	}



	public void reachedPoint()
	{
		spinDirection = (droid.pointB.position - transform.position);
		//rb.AddForce(droid.pointB.position - transform.position);
		


		//if(!droid.inFuture){
		//	spinDirection /= spinDirection.magnitude;
		//}

		//spinDirection.x /= spinDirection.magnitude;
		//spinDirection.y /= spinDirection.magnitude;
		//spinDirection.z /= spinDirection.magnitude;
	}

	public void updateRotation(float step)
	{
		float degX = ((spinDirection.x * step)) / bodyRadius * Mathf.Rad2Deg;
		float degZ = ((spinDirection.z * step)) / bodyRadius * Mathf.Rad2Deg;
		float degY = ((spinDirection.y * step)) / bodyRadius * Mathf.Rad2Deg;
		if (!droid.inFuture)
		{
			Vector3 direction = droid.pointB.position - transform.position;
			float distance = direction.magnitude;
				// Calculate the force to apply.  We normalize the direction
				// so the force is consistent regardless of distance.
				Vector3 forceToApply = direction.normalized * droid.speed;

				// Optional: Speed Limiting
				

				rb.AddForce(forceToApply);
			
			//Vector3 direction = Vector3.MoveTowards(body.transform.position, droid.pointB.position, step);
			////Vector3 direction =droid.pointB.position  - body.transform.position;
			////transform.position = direction;
			//direction /= direction.magnitude;
			//direction *= droid.speed;
			//rb.MovePosition(direction);
			//rb.Move(direction, new Quaternion(degX, degY, degZ, body.transform.rotation.w));
			//body.transform.Rotate(degX, degY, degZ);
			//ghost.transform.position = direction;
			ghost.transform.position = transform.position;
			//ghostBody.transform.Rotate(degX, degY, degZ);
		}
		else
		{
			Vector3 ghostDirection = droid.pointB.position - ghost.transform.position;
			float distance = ghostDirection.magnitude;
			// Calculate the force to apply.  We normalize the direction
			// so the force is consistent regardless of distance.
			Vector3 forceToApply = ghostDirection.normalized * droid.speed;

			// Optional: Speed Limiting


			ghostRB.AddForce(forceToApply);
			rb.linearVelocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			
			gameObject.transform.position = droid.stayPut.position;
			//Vector3 ghostDirection = Vector3.MoveTowards(ghostBody.transform.position, droid.pointB.position, step);
			

		}
		
	}




}
