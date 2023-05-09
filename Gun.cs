
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] public float damage = 10f;
    [SerializeField] public float range = 100f;
	public float fireRate = 0.45f;
	private float lastShootTime = 0f;
    //public KeyCode firingKey = KeyCode.Mouse2;
	public ParticleSystem particle;
	public GameObject impactEffect;

	[SerializeField] public float impactForce = 300f;
	private bool isFiring;
	private float shotCounter = 0f;

	public Camera fpsCam;
	Reloading ammoScript;


	void Start(){
		ammoScript = GetComponent<Reloading>();
	}

    void Update()
    {
        if (Input.GetButton("Fire1") && ammoScript.currentAmmo > 0)
        {
            Shoot();
			isFiring = true;
        }
		else
			isFiring = false;
    }

	void FixedUpdate(){
		if(isFiring && !ammoScript.needReload){
			shotCounter -= Time.deltaTime;
			if(shotCounter <= 0){
				shotCounter = fireRate;
				ammoScript.DecreaseAmmo();
			}
			else
				shotCounter -= Time.deltaTime;
			}
	}



	void RaycastShoot(){

		particle.Play();
		
		RaycastHit hit;
		if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)){
			Debug.Log(hit.transform.name);

			Target target = hit.transform.GetComponent<Target>();
			if (target != null){
				target.TakeDamage(damage);
			}

			if (hit.rigidbody != null){
				hit.rigidbody.AddForce(-hit.normal * impactForce);
			}

			GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
			Destroy(impactGO,2f);
		}

	}

	void Shoot(){
		if (Time.time > lastShootTime + fireRate){
			Debug.Log("Shoot");
			lastShootTime = Time.time;

			RaycastShoot();
		}
	}

}
