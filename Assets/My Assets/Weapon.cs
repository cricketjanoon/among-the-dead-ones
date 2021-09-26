using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace shahbaz
{
	public enum ShootingMode { Semi, Auto }

	public class Weapon : MonoBehaviour
	{
		private Vector3 originalPosition;
		public Vector3 aimPosition;

		public Camera fpsCamera = null;

		public float aimSpeed = 2.0f;

		private Animator _animator = null;

		public ParticleSystem muzzleFlash;

		public ShootingMode shootingMode;
		private bool shootInput = false;

		public float range = 100.0f;

		[Header("Impact and Particle System")]
		public GameObject hitParticle;

		public GameObject bulletImpact;

		public Transform shootPoint;

		public int bulletsPerMag = 30;
		public int bulletsLeft = 200;
		public int currentBullets;  //bullets left in current magzine

		public float fireRate = 0.2f;
		public float fireTimer;

		public AudioClip gunSound;
		public AudioClip reloadSound;

		public bool isReloading;

		private void Start()
		{
			currentBullets = bulletsPerMag;
			_animator = GetComponent<Animator>();

			originalPosition = transform.localPosition;
		}

		private void Update()
		{
			switch (shootingMode)
			{
				case ShootingMode.Auto:
					shootInput = Input.GetMouseButton(0);
					break;

				case ShootingMode.Semi:
					shootInput = Input.GetMouseButtonDown(0);
					break;
			}

			if (shootInput)
			{
				if (currentBullets > 0 && !isReloading)
					Fire();
				else if (bulletsLeft > 0)
					Reload();
			}

			if (Input.GetKeyDown(KeyCode.R))
			{
				if (currentBullets < bulletsPerMag && bulletsLeft > 0)
					Reload();
			}

			if (fireTimer < fireRate)
			{
				fireTimer += Time.deltaTime;
			}

			AimDownSight();
		}

		private void AimDownSight()
		{
			if (Input.GetMouseButton(1) && !isReloading)
			{
				transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * aimSpeed);

				fpsCamera.fieldOfView = Mathf.Lerp(fpsCamera.fieldOfView, 30, 3 * Time.deltaTime);
			}
			else
			{
				transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * aimSpeed);
				fpsCamera.fieldOfView = Mathf.Lerp(fpsCamera.fieldOfView, 60, 3 * Time.deltaTime);
			}
		}

		private void FixedUpdate()
		{
			AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);

		//	Debug.Log("Animator State info: " + info.fullPathHash);
		}

		private void Fire()
		{
			if (fireTimer < fireRate || currentBullets <= 0) return;

			//RaycastHit hit;
			//if (Physics.Raycast(fpsCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)), out hit, range))
			//{
			//    Debug.Log("Hit Object:" + hit.transform.name);

			//    GameObject hitParticleEffect = Instantiate(hitParticle, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
			//    GameObject bulletHole = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

			//    Destroy(hitParticleEffect, 1f);
			//    Destroy(bulletHole, 2f);
			//}

			_animator.SetTrigger("Fire");
			AudioSource.PlayClipAtPoint(gunSound, shootPoint.transform.position);
			muzzleFlash.Play();

			currentBullets--;
			fireTimer = 0.0f; //Reset the timer
		}

		public void Reload()
		{
			if (isReloading) return;

			isReloading = true;

			if (bulletsLeft <= 0)
				return;

			int bulletsToLoad = bulletsPerMag - currentBullets;

			int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

			bulletsLeft -= bulletsToDeduct;

			currentBullets += bulletsToDeduct;

			DoReloadAnimation();
		}

		private void DoReloadAnimation()
		{
			//AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
			//isReloading = info.IsName("Reload");
			//if (isReloading)
			//    return;

		//	Debug.Log("Reloding");
			_animator.SetTrigger("Reload");
			AudioSource.PlayClipAtPoint(reloadSound, this.transform.position);
		}
	}
}