using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using UnityEngine.Audio;
public class Player : MonoBehaviour
{
	public float health;
	float fullHealth;

	public DateTime lastFuture;
	public float futureCooldown;
	public GameObject futureBar;
	Image futureBarImg;
	public ParticleSystem futureParticles;
	public ParticleSystem futureParticlesL;

	public DateTime lastHeal;
	public float healCooldown;
	public int healAmount;
	public GameObject healBar;
	Image healBarImg;
	public ParticleSystem healParticles;
	public ParticleSystem healParticlesL;


	public DateTime lastForce;
	public float forceCooldown;
	public int forceForce;
	public float forceRadius;
	public GameObject forceBar;
	Image forceBarImg;
	public ParticleSystem forceParticles;
	public ParticleSystem forceParticlesL;
	public float forceDamage;

	public DateTime lastLightning;
	public float lightningCooldown;
	public float lightningDamage;
	public float lightningAngle;
	public float lightningRadius;

	public GameObject lightningBar;
	Image lightningBarImg;
	public ParticleSystem lightningParticles;
	public ParticleSystem lightningParticlesL;

	public GameManager gameManager;

	public AudioSource SpecialMoveAudio;
	public LightSaberSpawner SaberSpawner;

	Vector2 minBar;
	Vector2 maxBar;
	float barX;

	bool L;
	bool specialUsed;
	public int droidsKilled;
	public List<Droid> droids = new List<Droid>();
	public List<Droid> deadDroids = new List<Droid>();


	public GameObject healthBar;
	Image healthBarImage;
	RectTransform healthBarRect;
	float healthBarHeight;
	float healthBarWidth;
	Gradient healthGradient;

	public GameState state;
	public List<TrainingMode> levels = new List<TrainingMode>();
	//private void Awake()
	//{
	//	GameManager.OnGameStateChange += GameManagerOnGameStateChanged;
	//}

	void Start()
	{
		fullHealth = health;
		healthBarRect = healthBar.GetComponent<RectTransform>();
		healthGradient = new Gradient();
		healthBarImage = healthBar.GetComponent<Image>();
		healthBarWidth = healthBarRect.sizeDelta.x;
		healthBarHeight = healthBarRect.sizeDelta.y;
		GradientSetup(healthGradient);
		fullHealth = health;
		lastFuture = DateTime.Now.Subtract(TimeSpan.FromSeconds(futureCooldown));
		lastHeal = DateTime.Now.Subtract(TimeSpan.FromSeconds(healCooldown));
		lastForce = DateTime.Now.Subtract(TimeSpan.FromSeconds(forceCooldown));
		RectTransform rT = futureBar.GetComponent<RectTransform>();
		maxBar = new Vector2(rT.rect.width, rT.rect.height);
		minBar = new Vector2(maxBar.x * 0.05f, rT.rect.height);
		rT.pivot = new Vector2(0f, 0.5f);
		barX = rT.position.x;
		healBarImg = healBar.GetComponent<Image>();
		futureBarImg = futureBar.GetComponent<Image>();
		forceBarImg = forceBar.GetComponent<Image>();
		lightningBarImg = lightningBar.GetComponent<Image>();


	}

	// Update is called once per frame
	void Update()
	{
		cooldownColor(healBarImg, healCooldown, lastHeal);
		cooldownColor(futureBarImg, futureCooldown, lastFuture);
		cooldownColor(forceBarImg, forceCooldown, lastForce);
		cooldownColor(lightningBarImg, lightningCooldown, lastLightning);
		healthBarUI();


	}
	public void cooldownColor(Image image, float cooldown, DateTime lastUse)
	{

		float transitionDuration = cooldown; // Duration of the transition in seconds
		Color startColor = Color.red;
		Color middleColor = Color.yellow;
		Color endColor = Color.green;
		float timer = (float)(DateTime.Now.Subtract(lastUse).TotalSeconds);

		float t = timer / cooldown;
		RectTransform rt = image.gameObject.GetComponent<RectTransform>();
		Rect rect = rt.rect;
		rect.xMin = barX;

		if (rect.width < maxBar.x)
		{
			rt.pivot = new Vector2(0f, 0.5f);
			rt.sizeDelta = Vector2.Lerp(minBar, maxBar, t);
		}

		if (timer < transitionDuration)
		{


			// Transition from red to yellow to green
			if (t < 0.5f)
			{
				// Transition from red to yellow
				image.color = Color.Lerp(startColor, middleColor, t * 2f);
			}
			else
			{
				// Transition from yellow to green
				image.color = Color.Lerp(middleColor, endColor, (t - 0.5f) * 2f);
			}

		}
		else
		{
			// Ensure the final color is green
			image.color = endColor;
		}
	}
	public void leftHand()
	{
		L = true;
	}

	public void heal()
	{
		bool inFuture = false;
		if (droids.Count > 0)
		{
			foreach (var d in droids)
			{
				if (d.inFuture)
				{
					inFuture = true;
				}
			}

		}
		if (((float)(DateTime.Now.Subtract(lastHeal).TotalSeconds) >= healCooldown) && !inFuture)
		{
			PlayParticleOnEvent play = healParticles.gameObject.GetComponent<PlayParticleOnEvent>();
			if (L)
			{
				play = healParticlesL.gameObject.GetComponent<PlayParticleOnEvent>();
				L = false;
			}
			play.PlayOnce();
            health += healAmount;
            health = Mathf.Clamp(health, 0f, fullHealth);
			lastHeal = DateTime.Now;
			RectTransform rt = healBar.GetComponent<RectTransform>();
			rt.sizeDelta = new Vector2(0, maxBar.y);
		}
	}

	public void seeFuture()
	{
		bool inFuture = false;
		if (droids.Count > 0) {

			foreach (var d in droids)
			{
				if (d.inFuture)
				{
					inFuture = true;
				}
			}
		}
		if ((float)(DateTime.Now.Subtract(lastFuture).TotalSeconds) >= futureCooldown && !inFuture)
		{
			lastFuture = DateTime.Now;
			PlayParticleOnEvent play = futureParticles.gameObject.GetComponent<PlayParticleOnEvent>();
			if (L)
			{
				play = futureParticlesL.gameObject.GetComponent<PlayParticleOnEvent>();
				L = false;
			}
			play.PlayOnce();
			RectTransform rt = futureBar.GetComponent<RectTransform>();
			rt.sizeDelta = new Vector2(0, maxBar.y);
			foreach (Droid d in droids)
			{
				d.seeFuture();

			}
		}
	}

	public void useForce()
	{
		bool inFuture = false;
		if (droids.Count > 0)
		{
			foreach (var d in droids)
			{
				if (d.inFuture)
				{
					inFuture = true;
				}
			}

		}
		if (((float)(DateTime.Now.Subtract(lastForce).TotalSeconds) >= forceCooldown)&&!inFuture)
		{
			lastForce = DateTime.Now;
			PlayParticleOnEvent play = forceParticles.gameObject.GetComponent<PlayParticleOnEvent>();
			if (L)
			{
				play = forceParticlesL.gameObject.GetComponent<PlayParticleOnEvent>();
				L = false;
			}
			play.PlayOnce();

			RectTransform rt = forceBar.GetComponent<RectTransform>();
			rt.sizeDelta = new Vector2(0, maxBar.y);
			Force();
		}
	}

	public void Force(float forceMultiplier = 1f, float radiusMultiplier = 1f)
	{
		float radius = radiusMultiplier * forceRadius;
		foreach (Droid d in droids)
		{
			float impact = (1 - (d.distance / radius));
			float force = forceForce * impact * forceMultiplier;
			Rigidbody rb = d.gameObject.GetComponent<Rigidbody>();
			if ((rb != null) && (d.distance <= radius) && (d.gameObject.activeInHierarchy))
			{
				Vector3 forceDirection = d.gameObject.transform.position - gameObject.transform.position;
				forceDirection = forceDirection / forceDirection.magnitude;
				rb.AddForce(forceDirection.x * force/(2 * forceMultiplier), force * 0.5f/(2 * forceMultiplier), forceDirection.z * force / (2 * forceMultiplier), ForceMode.Impulse);
				float damage = (1 - (d.distance / radius)) * forceDamage;
				d.takeDamage(forceDamage * impact);
			}

		}

		killDroids();

	}

	public void Lightning(float damgeMultiplier = 1f, float radiusMultiplier = 1f, float anglemultiplier = 1f)
	{
		float radius = radiusMultiplier * lightningRadius;
		float angle = anglemultiplier * lightningAngle;
		foreach (Droid d in droids)
		{
			float angleFactor = Mathf.Clamp01(d.angle / angle);
			float distanceFactor = Mathf.Clamp01(d.distance / radius);
			float impact = 1f - ((distanceFactor + angleFactor) / 2);
			float damage = lightningDamage * impact * damgeMultiplier;
			Rigidbody rb = d.gameObject.GetComponent<Rigidbody>();
			if ((rb != null) && (d.distance <= radius) && (d.angle <= angle))
			{

				Vector3 forceDirection = d.gameObject.transform.position - gameObject.transform.position;
				forceDirection = forceDirection / forceDirection.magnitude;
				//rb.AddForce(forceDirection.x * damage/(2*damgeMultiplier), damage * 0.5f/ (2 * damgeMultiplier), forceDirection.z * damage/ (2 * damgeMultiplier), ForceMode.Impulse);

				d.takeDamage(damage);
			}

		}
		killDroids();


	}

	public void useLightning()
	{
		bool inFuture = false;
		if (droids.Count > 0)
		{
			foreach (var d in droids)
			{
				if (d.inFuture)
				{
					inFuture = true;
				}
			}

		}
		if (((float)(DateTime.Now.Subtract(lastLightning).TotalSeconds) >= lightningCooldown) && !inFuture)
		{
			lastLightning = DateTime.Now;
			PlayParticleOnEvent play = lightningParticles.gameObject.GetComponent<PlayParticleOnEvent>();
			if (L)
			{
				play = lightningParticlesL.gameObject.GetComponent<PlayParticleOnEvent>();
				L = false;
			}
			play.PlayOnce();
			RectTransform rt = lightningBar.GetComponent<RectTransform>();
			rt.sizeDelta = new Vector2(0, maxBar.y);
			Lightning();
		}
	}

	public void useSpecialMove()
	{
		bool inFuture = false;
		if (droids.Count > 0)
		{
			foreach (var d in droids) { 
				if (d.inFuture)
				{
					inFuture = true;
				}
			}

		}
		if (((float)(DateTime.Now.Subtract(lastForce).TotalSeconds) >= forceCooldown) && ((float)(DateTime.Now.Subtract(lastLightning).TotalSeconds) >= lightningCooldown) && ((float)(DateTime.Now.Subtract(lastHeal).TotalSeconds) >= healCooldown) && ((float)(DateTime.Now.Subtract(lastFuture).TotalSeconds) >= futureCooldown) && !inFuture && !specialUsed)
		{
			lastForce = DateTime.Now;
			lastLightning = DateTime.Now;
			lastFuture = DateTime.Now;
			lastHeal = DateTime.Now;
			PlayParticleOnEvent playLR = lightningParticles.gameObject.GetComponent<PlayParticleOnEvent>();
			PlayParticleOnEvent playLL = lightningParticlesL.gameObject.GetComponent<PlayParticleOnEvent>();
			PlayParticleOnEvent playFR = forceParticles.gameObject.GetComponent<PlayParticleOnEvent>();
			PlayParticleOnEvent playFL = forceParticlesL.gameObject.GetComponent<PlayParticleOnEvent>();

			SaberSpawner.SpawnSabers();
			playLR.PlayOnce();
			playLL.PlayOnce();
			playFL.PlayOnce();
			playFR.PlayOnce();
			SpecialMoveAudio.Play();
			Lightning(2f, 2, 2);
			Force(2, 2);
			specialUsed = true;
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.name == "Laser")
		{
			health -= UnityEngine.Random.Range(5, 15);
			if(health < 0)
			{
				gameManager.SetDeathState();
			}
			Debug.LogWarning("playerHit");
		}
	}

	public void healthBarUI()
	{
		// Calculate the fill amount (0 to 1)
		float fillAmount = health / fullHealth;
		// Set the fill amount of the image
		//healthBarImage.rectTransform.sizeDelta = new Vector2(fillAmount * healthBarWidth, healthBarHeight);
		healthBarRect.sizeDelta = new Vector2(healthBarWidth * fillAmount, healthBarHeight);

		// Set the color based on the health percentage
		healthBarImage.color = healthGradient.Evaluate(fillAmount);
	}



	public void GradientSetup(Gradient g)
	{
		var colors = new GradientColorKey[3];
		colors[0] = new GradientColorKey(Color.red, 0f);
		colors[1] = new GradientColorKey(Color.yellow, 0.5f);
		colors[2] = new GradientColorKey(Color.green, 1f);
		var alphas = new GradientAlphaKey[2];
		alphas[0] = new GradientAlphaKey(1.0f, 0.0f);
		alphas[1] = new GradientAlphaKey(1.0f, 0.5f);
		alphas[1] = new GradientAlphaKey(1.0f, 1.0f);
		g.SetKeys(colors, alphas);
	}

	public void killDroids()
	{
		foreach (Droid d in deadDroids)
		{
			droids.Remove(d);
			d.gameObject.SetActive(false);
			Destroy(d.laserShooter);
			Destroy(d.trainingDroid);
			Destroy(d.gameObject.transform.parent.gameObject);
			droidsKilled++;

		}

		deadDroids.Clear();
	}
	public void Reset()
	{
		specialUsed = false;
		health = fullHealth;
		lastFuture = DateTime.Now.Subtract(TimeSpan.FromSeconds(futureCooldown));
		lastHeal = DateTime.Now.Subtract(TimeSpan.FromSeconds(healCooldown));
		lastForce = DateTime.Now.Subtract(TimeSpan.FromSeconds(forceCooldown));
		droidsKilled = 0;
		SaberSpawner.DespawnSabers();
	}



}