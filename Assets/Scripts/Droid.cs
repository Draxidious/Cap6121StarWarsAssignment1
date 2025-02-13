using Oculus.Interaction.Body.Input;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Droid : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public float health;
	float fullHealth;
	public PeriodicLaserShooter shooter;
	public float damageDealt;
	public Player player;
	public TrainingDroid trainingDroid;

	List<Vector3> futurePositions = new List<Vector3>();
	public bool inFuture;
	public int futureIndex;
	public int futureSteps;
	public GameObject ghost;
	public Transform pointB;
	public float speed;
	public float worldWidth;
	public bool training;
	[HideInInspector]
	public Transform stayPut;
	public float distance;
	public float angle;
	Gradient healthGradient;
	public GameObject healthBar;
	Image healthBarImage;
	RectTransform healthBarRect;
	float healthBarHeight;
	float healthBarWidth;
	public PeriodicLaserShooter laserShooter;

	//World width needed to be global
	float upperX;
	float lowerX;
	float upperZ;
	float lowerZ;

	private BoxCollider damageCollider;
	public string laserTag = "Laser";
	public int laserDamage = 10;
	public Transform playerLocation;




	void Start()
	{
		upperX = worldWidth / 2f;
		lowerX = -1 * upperX;
		upperZ = worldWidth / 2f;
		lowerZ = -1 * upperZ;
		healthBarRect = healthBar.GetComponent<RectTransform>();
		damageCollider = GetComponent<BoxCollider>();
		healthGradient = new Gradient();
		GradientSetup(healthGradient);
		trainingDroid.player = player.gameObject;
		fullHealth = health;
		// Set initial position of the ball at the start point

		for (int i = 0; i < 10; i++)
		{
			addFuture(true);

		}


		ghost.SetActive(false);
		healthBarImage = healthBar.GetComponent<Image>();
		healthBarHeight = healthBarRect.sizeDelta.y;
		healthBarWidth = healthBarRect.sizeDelta.x;
		playerLocation = player.gameObject.transform;
	}

	// Update is called once per frame
	void Update()
	{
		playerLocation = player.gameObject.transform;
		Vector3 direction = transform.position - player.gameObject.transform.position;
		Vector3 horizontalDirection = new Vector3(direction.x, 0f, direction.z);
		distance = Mathf.Abs((direction).magnitude);
		angle = Vector3.Angle(player.transform.forward, horizontalDirection);

		float checkDistance = Mathf.Sqrt(Mathf.Pow((transform.position.x - pointB.position.x), 2) + Mathf.Pow((transform.position.z - pointB.position.z), 2));
		float checkDistanceGhost = Mathf.Sqrt(Mathf.Pow((ghost.transform.position.x - pointB.position.x), 2) + Mathf.Pow((ghost.transform.position.z - pointB.position.z), 2));


		if ((Mathf.Abs(checkDistance) < 0.6f) || (inFuture && (Mathf.Abs(checkDistanceGhost) < 0.6f)))
		{
			reachedPoint();

		}
		healthBarUI();


	}
	public void seeFuture()
	{
		inFuture = true;
		ghost.SetActive(true);
		healthBar.SetActive(false);
		futureIndex = 0;
		stayPut = gameObject.transform;
		shooter.inFuture = true;

	}

	public void addFuture(bool init = false)
	{

		float x = UnityEngine.Random.Range(lowerX, upperX);
		float z = UnityEngine.Random.Range(lowerZ, upperZ);
		float currentDistance = Mathf.Sqrt(Mathf.Pow((transform.localPosition.x - playerLocation.localPosition.x), 2) + Mathf.Pow((transform.localPosition.z - playerLocation.localPosition.z), 2));
		float newDistance = Mathf.Sqrt(Mathf.Pow((playerLocation.localPosition.x - x), 2) + Mathf.Pow((playerLocation.localPosition.z - z), 2));

		while (newDistance <= currentDistance && !training)
		{
			x = UnityEngine.Random.Range(lowerX, upperX);
			z = UnityEngine.Random.Range(lowerZ, upperZ);
			newDistance = Mathf.Sqrt(Mathf.Pow((transform.position.x - x), 2) + Mathf.Pow((transform.position.z - z), 2));
		}
		if (!training)
		{
			//Move range of randomly generated points 50% closer to the player
			if (playerLocation.localPosition.x > x)
			{
				upperX = (playerLocation.localPosition.x + x) / 2;
				lowerX = upperX - worldWidth / 2;
			}
			else
			{
				lowerX = (playerLocation.localPosition.x + x) / 2;
				upperX = lowerX +worldWidth / 2;
			}
			if (playerLocation.localPosition.z > z)
			{
				upperZ = (playerLocation.localPosition.z +z) / 2;
				lowerZ = upperZ - worldWidth / 2;
			}
			else
			{
				lowerZ = (playerLocation.localPosition.z +z) / 2;
				upperZ = lowerZ - +worldWidth / 2;
			}
		}
		//Debug.LogWarning(newDistance >= currentDistance);
		Vector3 pos = new Vector3(x, 0.5f, z);

		futurePositions.Add(pos);

		if (!init)
		{
			futurePositions.RemoveAt(0);
		}
	}

	public void reachedPoint()
	{

		pointB.localPosition = futurePositions[futureIndex];

		if (inFuture)
		{
			futureIndex++;
			if ((futureIndex == futureSteps))
			{
				inFuture = false;
				//shader.enabled = false;
				futureIndex = 0;
				ghost.SetActive(false);
				pointB.localPosition = futurePositions[0];
				shooter.inFuture = false;
				//rb.constraints = RigidbodyConstraints.None;
				healthBar.SetActive(true);
			}
		}
		else
		{
			addFuture();

			trainingDroid.reachedPoint();
		}
	}

	public void healthBarUI()
	{
		// Calculate the fill amount (0 to 1)
		float fillAmount = health / fullHealth;

		// Set the fill amount of the image
		healthBarImage.rectTransform.sizeDelta = new Vector2(fillAmount * healthBarWidth, healthBarHeight);
		healthBarRect.sizeDelta = new Vector2(healthBarWidth * fillAmount, healthBarHeight);

		// Set the color based on the health percentage
		healthBarImage.color = healthGradient.Evaluate(fillAmount);
	}

	public void takeDamage(float Damage)
	{
		health -= Damage;
		if (health <= 0)
		{
			player.deadDroids.Add(this);

		}
	}

	public void GradientSetup(Gradient g)
	{
		var colors = new GradientColorKey[3];
		colors[0] = new GradientColorKey(Color.red, 0f);
		colors[1] = new GradientColorKey(Color.yellow, 0.5f);
		colors[2] = new GradientColorKey(Color.green, 1f);
		var alphas = new GradientAlphaKey[3];
		alphas[0] = new GradientAlphaKey(1.0f, 0.0f);
		alphas[1] = new GradientAlphaKey(1.0f, 0.5f);
		alphas[2] = new GradientAlphaKey(1.0f, 1.0f);
		g.SetKeys(colors, alphas);
	}

	private void OnDrawGizmosSelected()
	{
		// Draw the firing range in the editor
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, worldWidth);
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(laserTag))
		{
			takeDamage(laserDamage);
			Destroy(other.gameObject);
			player.killDroids();
		}
	}


}
