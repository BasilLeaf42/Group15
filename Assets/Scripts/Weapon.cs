using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    BulletPool bPool;
	public Transform camera;
	public Transform firePoint;
	
	// Weapon stat variables
	public float bulletSpeed = 10;
	public bool isFiring;
	public float fireDelay;
	public float fireTimer;
	public int currentAmmo;
	public int totalAmmo = 5;
	public float reloadTimer = 1f;
	private bool isReloading = false;
	
	// Ammo counter variables
	public InputField currentAmmoField;
	public InputField totalAmmoField;
	
	// Start is called before the first frame update
    private void Start()
    {
        currentAmmo = totalAmmo;
		bPool = BulletPool.main;
    }
	
	// Spawns a bullet from the BulletPool
	public void Fire()
	{
		// Initialize bullet velocity
		Vector3 bulletVelocity = camera.forward * bulletSpeed;
		
		// Spawn a bullet
		bPool.ChooseFromPool(firePoint.position, bulletVelocity);
		
		// Remove a bullet from the magazine
		currentAmmo = currentAmmo - 1;
	}
	
	// Player presses fire button
	public void FirePress()
	{
		Fire();
		Debug.Log("Fire button pressed.");
	}
	
	// Player releases fire button
	public void FireRelease()
	{
		fireTimer = 0;
		isFiring = false;
		Debug.Log("Fire button released.");
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
		
		// Checks if the player is firing
		if (isFiring == true)
		{
			// Checks if the weapon cooldown is active
			if (fireTimer > 0)
			{
				fireTimer = fireTimer - Time.deltaTime;
				Debug.Log("Cannot fire while cooldown is active.");
			}
			
			// Fire!
			else
			{
				fireTimer = fireDelay;
				Fire();
				Debug.Log("Fired a bullet.");
			}
		}
    }
	
	// Reload function
	IEnumerator Reload()
	{
		// Reload start
		isReloading = true;
		Debug.Log("Reloading...");
		yield return new WaitForSeconds(reloadTimer);
		
		// Reload complete
		currentAmmo = totalAmmo;
		isReloading = false;
		Debug.Log("Reloading complete.");
	}
}
