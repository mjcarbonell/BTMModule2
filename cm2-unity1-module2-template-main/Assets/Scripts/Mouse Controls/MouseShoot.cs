using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MouseShoot : MonoBehaviour
{
    public float range = 20f;
    public float damage = 10f;
    public AudioClip fireSoundEffects;

    public GameObject muzzleFlashPrefab; // Muzzle flash effect
    public GameObject hitEffectPrefab;   // Bullet impact effect
    public Transform muzzlePoint;        // Where the bullet starts
    [SerializeField]
    private Camera fpsCam;
    private AudioSource source;
    private LineRenderer bulletTrail;

    public float trailOffset = 0.3f; // Offset to the right

    void Start()
    {
        // fpsCam = Camera.main;
        source = GetComponent<AudioSource>();

        // Create and configure LineRenderer
        bulletTrail = gameObject.AddComponent<LineRenderer>();
        bulletTrail.startWidth = 0.1f;
        bulletTrail.endWidth = 0.05f;
        bulletTrail.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        bulletTrail.startColor = Color.white;
        bulletTrail.endColor = Color.cyan;
        bulletTrail.enabled = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        source.PlayOneShot(fireSoundEffects);

        // Get raycast start position (center of the screen)
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        // Offset bullet trail slightly to the right
        Vector3 offset = fpsCam.transform.right * trailOffset;
        Vector3 bulletStartPos = rayOrigin + offset;

        // Show muzzle flash
        if (muzzleFlashPrefab != null && muzzlePoint != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, muzzlePoint.position, muzzlePoint.rotation);
            Destroy(flash, 0.1f);
        }

        RaycastHit hit;
        if (Physics.Raycast(bulletStartPos, fpsCam.transform.forward, out hit, range))
        {
            IDamagable damageable = hit.collider.GetComponent<IDamagable>();
            DoorButton doorButton = hit.collider.GetComponent<DoorButton>();
            if (damageable != null){
                damageable.TakeDamage(damage);
            }
            if (doorButton != null){
                doorButton.OpenDoor();
            }

            // Show bullet impact effect
            if (hitEffectPrefab != null)
            {
                GameObject impact = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 1f);
            }

            // Show bullet trail to hit point
            StartCoroutine(ShowBulletTrail(bulletStartPos, hit.point));
        }
        else
        {
            // Show bullet trail to max range
            StartCoroutine(ShowBulletTrail(bulletStartPos, bulletStartPos + fpsCam.transform.forward * range));
        }
    }

    IEnumerator ShowBulletTrail(Vector3 start, Vector3 end)
    {
        bulletTrail.SetPosition(0, start);
        bulletTrail.SetPosition(1, end);
        bulletTrail.enabled = true;

        yield return new WaitForSeconds(0.15f);

        bulletTrail.enabled = false;
    }
}
