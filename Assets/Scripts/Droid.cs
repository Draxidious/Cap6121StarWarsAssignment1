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

    private BoxCollider damageCollider;
    public string laserTag = "Laser";
    public int laserDamage = 10;




	void Start()
	{
		healthBarRect = healthBar.GetComponent<RectTransform>();
        damageCollider = GetComponent<BoxCollider>();
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
		angle = Vector3.Angle(player.transform.forward, horizontalDirection);

		float checkDistance = Mathf.Sqrt( Mathf.Pow((transform.position.x - pointB.position.x), 2) + Mathf.Pow((transform.position.z - pointB.position.z), 2));
		float checkDistanceGhost = Mathf.Sqrt(Mathf.Pow((ghost.transform.position.x - pointB.position.x), 2) + Mathf.Pow((ghost.transform.position.z - pointB.position.z), 2));


		if ((Mathf.Abs(checkDistance) <0.6f) || (inFuture && (Mathf.Abs(checkDistanceGhost ) < 0.6f)))
		{
			reachedPoint();

		}
		healthBarUI();


	}
	public void seeFuture()
	{
		inFuture = true;
		ghost.SetActive(true);
		gameObject.GetComponent<BoxCollider>().enabled = false;
		//rb.constraints = RigidbodyConstraints.FreezeAll;
		healthBar.SetActive(false);
		futureIndex = 0;
		stayPut = gameObject.transform;
		shooter.inFuture = true;

	}

	public void addFuture(bool init = false)
	{
		float upper = worldWidth / 2f;
		float lower = -1 * upper;
		float x = UnityEngine.Random.Range(lower, upper);
		float z = UnityEngine.Random.Range(lower, upper);
		float playerDistance = Mathf.Sqrt(Mathf.Pow((transform.position.x - player.transform.position.x), 2) + Mathf.Pow((transform.position.z - player.transform.position.z), 2));
		float newDistance = Mathf.Sqrt(Mathf.Pow((transform.position.x - x), 2) + Mathf.Pow((transform.position.z -z), 2));
		//while (newDistance >= playerDistance && !training)
		//{
		//	x = UnityEngine.Random.Range(lower, upper);
		//	z = UnityEngine.Random.Range(lower, upper);
		//	newDistance = Mathf.Sqrt(Mathf.Pow((transform.position.x - x), 2) + Mathf.Pow((transform.position.z - z), 2));
		//}
		Vector3 pos = new Vector3(UnityEngine.Random.Range(lower, upper), 0.5f, UnityEngine.Random.Range(lower, upper));
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
				pointB.position = futurePositions[0];
				shooter.inFuture = false;
				gameObject.GetComponent<BoxCollider>().enabled = true;
				//rb.constraints = RigidbodyConstraints.None;
				healthBar.SetActive(true);
			}
		}
		else
		{
			addFuture();

		}
			trainingDroid.reachedPoint();
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
		if(health <= 0)
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
        }
    }


}
