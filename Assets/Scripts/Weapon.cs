using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    BulletPool bPool;
	public Transform camera;
	public Transform firePoint;
	public Animator animator;
	
	// Weapon stat variables
	public float bulletSpeed = 10;
	public bool isFiring;
	public float fireDelay = 1.2f;
	public float fireTimer;
	public int currentAmmo = 5;
	public int totalAmmo = 5;
	public float reloadTimer = 3.3f;
	
	// Animation things
	private bool isReloading = false;
	private bool isRechambering = false;
	private bool isScoped = false;
	public GameObject scopeReticle;
	public GameObject weaponCamera;
	public Camera PlayerCamera;
	public float scopeFOV = 15f;
	private float previousFOV;
	
	// Ammo counter variables
	public Text ammoCounter;
	
	// Start is called before the first frame update
    private void Start()
    {
        currentAmmo = totalAmmo;
		bPool = BulletPool.main;
		scopeReticle.SetActive(false);
		weaponCamera.SetActive(true);
		ammoCounter.text = currentAmmo.ToString() + " | " + totalAmmo.ToString() + " (+1)";
    }
	
	// Update is called once per frame
    void Update()
    {
		// Return if the player is reloading
		if (isReloading == true)
		{
			return;
		}
		
		// Automatically reloads if the magazine is empty and the player is not reloading
		else if ((currentAmmo <= 0) && (isReloading == false))
		{
			StartCoroutine(Reload());
			return;
		}
		
		// Return if the player is reloading
		if (isRechambering == true)
		{
			return;
		}
		
		// Checks if the player is firing
		if (isFiring == true)
		{
			// Checks if the weapon cooldown is active
			if (fireTimer > 0 && isRechambering == true)
			{
				Debug.Log("Cannot fire while cooldown is active.");
				fireTimer = fireTimer - Time.deltaTime;
			}
			
			// Fire!
			else
			{
				fireTimer = 11f;
				Fire();
				Debug.Log("Fired a bullet.");
			}
		}
    }
	
	// Spawns a bullet from the BulletPool
	public void Fire()
	{
		// Refuse to fire if reloading
		if (isReloading == true)
		{
			Debug.Log("Cannot fire while reloading.");
		}
		
		// Refuse to fire if rechamberiqng
		else if (isRechambering == true)
		{
			Debug.Log("Cannot fire while rechambering.");
		}
		
		else
		{
			// Initialize bullet velocity
			Vector3 bulletVelocity = camera.forward * bulletSpeed;
			
			// Fire!
			bPool.ChooseFromPool(firePoint.position, bulletVelocity);
			Debug.Log("Fired a bullet.");
			
			// Remove a bullet from the magazine and update ammo counter
			currentAmmo = currentAmmo - 1;
			ammoCounter.text = currentAmmo.ToString() + " | " + totalAmmo.ToString() + " (+1)";
			
			// Rechamber
			isRechambering = true;
			StartCoroutine(RateOfFire());
		}
	}
	
	// Player presses fire button
	public void FirePress()
	{
		Debug.Log("Fire button pressed.");
		Fire();
	}
	
	// Player releases fire button
	public void FireRelease()
	{
		Debug.Log("Fire button released.");
		fireTimer = 0;
		isFiring = false;
	}
	
	// (Un)scope function
	IEnumerator Scope()
	{
		// Refuse to scope in while reloading
		if (isReloading == true)
		{
			Debug.Log("Cannot scope in while reloading.");
		}
		
		// Scope in
		else if (isScoped == false)
		{
			Debug.Log("Scoping in...");
			animator.SetBool("Scoped", true);
			yield return new WaitForSeconds(0.25f);
			scopeReticle.SetActive(true);
			weaponCamera.SetActive(false);
			previousFOV = PlayerCamera.fieldOfView;
			PlayerCamera.fieldOfView = scopeFOV;
			isScoped = true;
		}
		
		// Scope out
		else
		{
			Debug.Log("Scoping out...");
			animator.SetBool("Scoped", false);
			scopeReticle.SetActive(false);
			weaponCamera.SetActive(true);
			PlayerCamera.fieldOfView = previousFOV;
			isScoped = false;
		}
	}
	
	// Player presses fire button
	public void ScopePress()
	{
		Debug.Log("Scope button pressed.");
		StartCoroutine(Scope());
	}
	
	// Player releases fire button
	public void ScopeRelease()
	{
		Debug.Log("Scope button released.");
	}
	
	// Reload function
	IEnumerator Reload()
	{
		// Unscope if scoped in
		if (isScoped == true)
		{
			Debug.Log("Scoping out to reload.");
			animator.SetBool("Scoped", false);
			scopeReticle.SetActive(false);
			weaponCamera.SetActive(true);
			PlayerCamera.fieldOfView = previousFOV;
			isScoped = false;
		}
		
		// Prevent reload if magazine and chamber are already full
		if (currentAmmo == totalAmmo + 1)
		{
			Debug.Log("Magazine and chamber already full; cannot reload.");
		}
		
		
		// If the rifle can't hold an extra round in the chamber, use this code instead
		// Prevent reload if the magazine is already full
		/* if (currentAmmo == totalAmmo)
		{
			Debug.Log("Magazine already full; cannot reload.");
		} 
		
		// Reload!
		else
		{
			// Reload start
			isReloading = true;
			Debug.Log("Reloading...");
			yield return new WaitForSeconds(reloadTimer);
			
			// Reload complete
			currentAmmo = totalAmmo;
			isReloading = false;
			Debug.Log("Reloading complete.");
		}*/
		
		// Reload from empty
		else if (currentAmmo == 0)
		{
			// Reload start
			isReloading = true;
			Debug.Log("Reloading from empty...");
			animator.SetTrigger("Reload");
			yield return new WaitForSeconds(3.3f); // reloadTimer
			
			// Reload complete
			currentAmmo = totalAmmo;
			ammoCounter.text = currentAmmo.ToString() + " | " + totalAmmo.ToString() + " (+1)";
			isReloading = false;
			Debug.Log("Reloading complete.");
		}
		
		// Reloading closed-bolt rifle with a round already chambered
		else
		{
			// Reload start
			isReloading = true;
			Debug.Log("Reloading with a round in the chamber...");
			animator.SetTrigger("Reload");
			yield return new WaitForSeconds(3.3f); // reloadTimer
			
			// Reload complete
			currentAmmo = totalAmmo + 1;
			ammoCounter.text = currentAmmo.ToString() + " | " + totalAmmo.ToString() + " (+1)";
			isReloading = false;
			Debug.Log("Reloading complete. Extra round!");
		}
	}
	
	// Player presses reload button
	public void ReloadPress()
	{
		Debug.Log("Reload button pressed.");
		
		// Prevent reload if magazine and chamber are already full
		if (currentAmmo == totalAmmo + 1)
		{
			Debug.Log("Magazine and chamber already full; cannot reload.");
		}
		
		// If the rifle can't hold an extra round in the chamber, use this code instead
		/* if (currentAmmo == totalAmmo)
		{
			Debug.Log("Magazine already full; cannot reload.");
		} */
		
		else
		{
			// Begin reloading
			StartCoroutine(Reload());
		}
	}
	
	// Player releases reload button
	public void ReloadRelease()
	{
		Debug.Log("Reload button released.");
	}

	// Rate of Fire function
	IEnumerator RateOfFire()
	{
		if (isRechambering == true)
		{
			Debug.Log("Rechambering...");
			yield return new WaitForSeconds(1.2f); // fireDelay
			Debug.Log("Ready to fire.");
			isRechambering = false;
		}
	}
}
	