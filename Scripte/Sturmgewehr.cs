using UnityEngine;
using TMPro;
using UnityEditor;

public class PickupableWeapon : MonoBehaviour
{
    [Header("Waffen-Einstellungen")]
    public float fireRate = 0.1f;
    public int maxAmmo = 30;
    public float reloadTime = 2f;

    [Header("Zielen-Einstellungen")]
    public float aimFOV = 40f;
    public float normalFOV = 60f;
    public float aimSpeed = 10f;
    public Transform aimPosition;
    public Transform defaultPosition;
    public Transform aimTarget; // Das Empty, auf das die Waffe beim Zielen schaut
    public Camera playerCamera;
    public GameObject SniperCam;
    public Vector3 aimPositionOffset;
    public Vector3 defaultPositionOffset;
    public Vector3 defaultRotationOffset;

    [Header("Prefabs und Referenzen")]
    public GameObject bulletPrefab;
    public GameObject shellPrefab;
    public Transform firePoint;
    public Transform ejectPoint;
    public Animator weaponAnimator;

    [Header("Pickup-Einstellungen")]
    public Transform playerHand;
    public float pickupRange = 2f;
    public LayerMask pickupLayer;

    [Header("Carry Offset")]
    public Vector3 carryPositionOffset;
    public Vector3 carryRotationOffset;

    [Header("Patronenauswurf")]
    public float ejectionForce = 2f;
    public float shellLifetime = 5f;
    public float bulletSpeed = 50f;

    public int currentAmmo;
    private float nextFireTime = 0f;
    private bool isReloading = false;
    private bool isAiming = false;
    private bool isPickedUp = false;

    private WeaponManager player;

    private Rigidbody weaponRigidbody;
    private Collider weaponCollider;

    [Header("Effekte")]
    public ParticleSystem muzzleFlash; // Muzzle Flash Particle System
    public Light muzzleLight;
    public AudioSource gunAudio;

    [Header("UI-Elemente")]
    public TextMeshProUGUI ammoText;

    void Start()
    {
        currentAmmo = maxAmmo;
        weaponRigidbody = GetComponent<Rigidbody>();
        weaponCollider = GetComponent<Collider>();
        player = FindFirstObjectByType<WeaponManager>(); // Sucht den WeaponManager im Spiel

        if (playerCamera != null)
            playerCamera.fieldOfView = normalFOV;
    }

    void Update()
    {
        if (isPickedUp)
        {
            HandleAiming();
            HandleShooting();
            HandleReloading();

            if (Input.GetKeyDown(KeyCode.Q))
            {
                DropWeapon();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TryPickupWeapon();
            }
        }
    }

    void HandleShooting()
    {
        if (isReloading || !isPickedUp) return;

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
            else
            {
                StartCoroutine(Reload());
            }
        }
    }

    void HandleReloading()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    void HandleAiming()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            isAiming = true;

            if (weaponAnimator != null)
                weaponAnimator.SetBool("Aim", true);
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            isAiming = false;

            if (weaponAnimator != null)
                weaponAnimator.SetBool("Aim", false);

            // Waffe zur�ck zur Standardposition
            transform.localPosition = carryPositionOffset;
            transform.localRotation = Quaternion.Euler(carryRotationOffset);
        }

        if (playerCamera != null)
        {
            float targetFOV = isAiming ? aimFOV : normalFOV;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * aimSpeed);
        }

        if (isAiming && aimTarget != null)
        {
            // Waffe schaut immer auf das Ziel (aimTarget)
            transform.position = Vector3.Lerp(transform.position, aimPosition.position + aimPositionOffset, Time.deltaTime * aimSpeed);
            transform.LookAt(aimTarget);
            Zielen();
        }
        else
        {
            // Waffe geht zur�ck zur normalen Haltung
            transform.position = Vector3.Lerp(transform.position, defaultPosition.position + defaultPositionOffset, Time.deltaTime * aimSpeed);
            transform.localRotation = Quaternion.Euler(carryRotationOffset);
            transform.LookAt(aimTarget);
            NichtZielen();
        }
    }

    void TryPickupWeapon()
    {


        Collider[] colliders = Physics.OverlapSphere(playerCamera.transform.position, pickupRange, pickupLayer);

        foreach (var collider in colliders)
        {
            if (collider.gameObject == gameObject)
            {
                PickupWeapon();
                break;
            }
        }
    }

    public void SetEquipped(bool equipped)
    {
        isPickedUp = equipped;
        gameObject.SetActive(equipped);
    }

    void PickupWeapon()
    {


        player.AddWeapon(this);



        isPickedUp = true;
        weaponRigidbody.isKinematic = true;
        weaponCollider.enabled = false;

        Debug.Log("Waffe aufgenommen!");
        SetEquipped(true);
        transform.SetParent(playerHand);
        transform.localPosition = carryPositionOffset;
        transform.localRotation = Quaternion.Euler(carryRotationOffset);

        if (ammoText != null)
        {
            ammoText.text = $"Ammo: {currentAmmo}/{maxAmmo}";
        }

        Debug.Log("Waffe aufgenommen!");
    }


    public void DropWeapon()
    {
        isPickedUp = false; // Waffe wird nicht mehr gehalten
        transform.SetParent(null); // Waffe aus der Hierarchie des Spielers entfernen
        weaponRigidbody.isKinematic = false;
        weaponCollider.enabled = true;

        player.DropCurrentWeapon();

        weaponRigidbody.AddForce(Vector3.forward * 2f, ForceMode.Impulse);

        Debug.Log("Waffe fallen gelassen!");
    }

    void Shoot()
    {
        if (currentAmmo <= 0 || !isPickedUp) return;

        currentAmmo--;

        if (bulletPrefab != null && firePoint != null)
        {
            RaycastHit hit;
            Vector3 shootDirection;

            // Stelle sicher, dass firePoint.forward genau auf die Kamera ausgerichtet ist
            Vector3 firePointForward = firePoint.forward;

            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 100f))
            {
                shootDirection = (hit.point - firePoint.position).normalized;
            }
            else
            {
                shootDirection = playerCamera.transform.forward;
            }

            // Kugel instanziieren direkt an firePoint mit exakter Rotation
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(shootDirection));
            muzzleFlash.Play();
            StartCoroutine(MuzzleLightEffect());
            gunAudio.Play();


            // Falls nötig, Kugelposition manuell korrigieren, um exakte Übereinstimmung zu erreichen
            bullet.transform.position = firePoint.position;
            bullet.transform.forward = shootDirection;

            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {

                bulletRb.linearVelocity = shootDirection * bulletSpeed;
            }
            Destroy(bullet, shellLifetime);
        }

        // Patronenhülse auswerfen
        if (shellPrefab != null && ejectPoint != null)
        {
            GameObject shell = Instantiate(shellPrefab, ejectPoint.position, ejectPoint.rotation);
            Rigidbody shellRigidbody = shell.GetComponent<Rigidbody>();

            if (shellRigidbody != null)
            {
                Vector3 ejectionDirection = ejectPoint.forward + ejectPoint.right * Random.Range(-0.2f, 0.2f);
                shellRigidbody.AddForce(ejectionDirection.normalized * ejectionForce, ForceMode.Impulse);
            }
            Destroy(shell, shellLifetime);
        }

        Debug.Log("Schuss abgefeuert!");

        // UI aktualisieren
        if (ammoText != null)
        {
            ammoText.text = $"Ammo: {currentAmmo}/{maxAmmo}";
        }



    }




    System.Collections.IEnumerator MuzzleLightEffect()
    {
        muzzleLight.enabled = true;
        yield return new WaitForSeconds(0.05f);
        muzzleLight.enabled = false;
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;

        if (weaponAnimator != null)
        {
            weaponAnimator.SetTrigger("Reload");
        }

        Debug.Log("Nachladen...");
        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;

        Debug.Log("Nachladen abgeschlossen!");

        if (ammoText != null)
        {
            ammoText.text = $"Ammo: {currentAmmo}/{maxAmmo}";
        }
    }

    private void Zielen()
    {
        SniperCam.SetActive(true);
    }
    private void NichtZielen()
    {
        SniperCam.SetActive(false);
    }

}

