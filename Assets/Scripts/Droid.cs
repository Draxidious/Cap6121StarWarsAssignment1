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

	private List<Vector3> futurePositions = new List<Vector3>();
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
	

	void Start()
	{
		healthBarRect = healthBar.GetComponent<RectTransform>();
		healthGradient = new Gradient();
		GradientSetup(healthGradient);
		fullHealth = health;
		// Set initial position of the ball at the start point
		for (int i = 0; i < 15; i++)
		{
			addFuture(true);
		}
		ghost.SetActive(false);
		healthBarImage = healthBar.GetComponent<Image>();
		healthBarHeight = healthBarRect.sizeDelta.y;
		healthBarWidth = healthBarRect.sizeDelta.x;
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 direction = transform.position - player.gameObject.transform.position;
		Vector3 horizontalDirection = new Vector3(direction.x, 0f, direction.z);
		distance = Mathf.Abs((direction).magnitude);
		angle = Vector3.Angle(player.transform.forward,direction);
		
		if ((Mathf.Abs((transform.position - pointB.position).magnitude) <0.3f) || (inFuture && (Mathf.Abs((ghost.transform.position - pointB.position).magnitude) < 0.3f)))
		{
			reachedPoint();

		}
		healthBarUI();


	}
	public void seeFuture()
	{
		inFuture = true;
		ghost.SetActive(true);
		futureIndex = 0;
		stayPut = gameObject.transform;
		shooter.inFuture = true;
	}

	public void addFuture(bool init = false)
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

	public void reachedPoint()
	{

		pointB.position = futurePositions[futureIndex];
		
		if (inFuture)
		{
			futureIndex++;
			if ((futureIndex == futureSteps))
			{
				inFuture = false;
				//shader.enabled = false;
				futureIndex = 0;
				ghost.SetActive(false);
				pointB.position = futurePositions[0];
				shooter.inFuture = false;
			}
		}
		else
		{
			addFuture();

		}
		if (training)
		{
			trainingDroid.reachedPoint();
		}
	}

	public void healthBarUI()
	{
		// Calculate the fill amount (0 to 1)
		float fillAmount = health / fullHealth;

		// Set the fill amount of the image
		healthBarImage.rectTransform.sizeDelta = new Vector2(fillAmount*healthBarWidth, healthBarHeight);
		healthBarRect.sizeDelta = new Vector2(healthBarWidth * fillAmount, healthBarHeight);

		// Set the color based on the health percentage
		healthBarImage.color = healthGradient.Evaluate(fillAmount);
	}

	public void takeDamage(float Damage)
	{
		health -= Damage;
		Debug.LogWarning("Health: " + health.ToString() + " Angle: " + angle.ToString() + " Distance: " + distance);
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
}
