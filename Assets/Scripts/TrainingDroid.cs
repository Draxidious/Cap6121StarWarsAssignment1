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
	public Transform pointB;
	public float speed;
	public Transform headProxy;
	private List<Vector3> futurePositions = new List<Vector3>();
	public float worldWidth;

	public GameObject ghost;
	public GameObject ghostBody;
	public GameObject ghostHead;
	public Transform ghostHeadProxy;

	public bool inFuture;
	public int futureIndex;
	public int futureSteps;
	//public PostProcess shader;

	private Vector3 spinDirection;
	void Start()
	{
		// Set initial position of the ball at the start point
		spinDirection = pointB.position - transform.position;
		for (int i = 0; i < 15; i++)
		{
			addFuture(true);
		}
		ghost.SetActive(false);
		//shader.enabled = false;

	}

	void Update()
	{

		headProxy.LookAt(player.transform);
		ghostHeadProxy.LookAt(player.transform);
		if ((body.transform.position == pointB.position) ||( inFuture &&(ghost.transform.position == pointB.position)))
		{
			reachedPoint();

		}

		var step = Time.deltaTime * speed;
		float degX = ((spinDirection.x * step)) / bodyRadius * Mathf.Rad2Deg;
		float degZ = ((spinDirection.z * step)) / bodyRadius * Mathf.Rad2Deg;
		float degY = ((spinDirection.y * step)) / bodyRadius * Mathf.Rad2Deg;
		if (!inFuture)
		{
			Vector3 direction = Vector3.MoveTowards(body.transform.position, pointB.position, step);
			transform.position = direction;
			body.transform.Rotate(degX, degY, degZ);
		}
		Vector3 ghostDirection = Vector3.MoveTowards(ghostBody.transform.position, pointB.position, step);
		ghost.transform.position = ghostDirection;
		ghostBody.transform.Rotate(degX, degY, degZ);


	}
	void addFuture(bool init = false)
	{
		float upper = worldWidth / 2f;
		float lower = -1 * upper;
		Vector3 pos = new Vector3(UnityEngine.Random.Range(lower, upper), 0.5f, UnityEngine.Random.Range(lower, upper));
		futurePositions.Add(pos);
		if (!init)
		{
			futurePositions.RemoveAt(0);
		}
	}

	public void seeFuture()
	{
		inFuture = true;
		ghost.SetActive(true);
		futureIndex = 0;
		//shader.enabled =true;
	}
	void reachedPoint()
	{
		
		pointB.position = futurePositions[futureIndex];
		Debug.Log(inFuture.ToString());
		if (inFuture)
		{
			Debug.LogWarning( futureIndex.ToString());
			futureIndex++;
			if ((futureIndex == futureSteps))
			{
				inFuture = false;
				//shader.enabled = false;
				futureIndex = 0;
				ghost.SetActive(false);
				pointB.position = futurePositions[0];
			}
		}
		else
		{
			addFuture();
			
		}
		spinDirection = (pointB.position - transform.position);
		spinDirection.x /= spinDirection.magnitude;
		spinDirection.y /= spinDirection.magnitude;
		spinDirection.z /= spinDirection.magnitude;
	}




}
