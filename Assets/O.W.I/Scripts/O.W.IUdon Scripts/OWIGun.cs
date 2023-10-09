
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static VRC.SDKBase.VRC_Pickup;

public class OWIGun : UdonSharpBehaviour
{
    private VRC_Pickup pickup;
    private Transform gun;
    private BoxCollider gunCollider;
    private OWIBullet[] bullets = null;
    private AudioSource audioSource;
    private int currentBulletIndex = 0;
    private bool magEmpty;
    private float currentTimer;
    [SerializeField] private bool showBulletTrail;
    [SerializeField, Tooltip("Enable to Override the Bullet intensity values")]
    private bool bulletIntensityOverride;
    [SerializeField, Tooltip("Value Decides if it interrupts the previous sensation")]
    private int sensationPriority = 1;
    [SerializeField]
    [Range(1, 100)]
    private int shotIntensity = 50;
    [SerializeField]
    [Range(1, 100)]
    private int recoilIntensity = 70;
    [SerializeField]
    private float shotSpeed = 10f;
    [SerializeField]
    private float reloadTimer = 5f;
    [SerializeField]
    private AudioClip emptyMag;
    [SerializeField]
    private AudioClip bulletShot;
    [HideInInspector]
    public bool isRightHandHolding;
    private Animator animator;

    private void Start()
    {
        pickup = GetComponent<VRC_Pickup>();
        gunCollider = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();

        int count = 0;
        gun = transform.Find("Gun_Body");
        animator = gun.GetComponent<Animator>();

        // First pass to count the number of "Bullet" objects
        foreach (Transform child in gun)
        {
            OWIBullet bullet = child.GetComponent<OWIBullet>();
            if (bullet != null)
            {
                count++;
            }
        }
        bullets = new OWIBullet[count];

        // Second pass to populate the array
        int index = 0;
        foreach (Transform child in gun)
        {
            OWIBullet oWIBullet = child.GetComponent<OWIBullet>();
            if (oWIBullet != null)
            {
                bullets[index] = oWIBullet;
                index++;
            }
        }


        {
            foreach (OWIBullet oWIBullet in bullets)
            {
                if (bulletIntensityOverride)
                {
                    oWIBullet.SetSensationValues(shotIntensity,shotSpeed,showBulletTrail);
                }
                else
                    oWIBullet.SetSensationValues(0, shotSpeed,showBulletTrail);
            }
        }
    }
    public override void OnPickup()
    {
        gunCollider.enabled = false;
        isRightHandHolding = (pickup.currentHand == PickupHand.Right);
    }

    public override void OnDrop()
    {
        gunCollider.enabled = true;
    }

    private void Update()
    {
        if (magEmpty)
        {
            currentTimer += Time.deltaTime;

            if (currentTimer >= reloadTimer)
            {
                foreach (OWIBullet oWIBullet in bullets)
                {
                    oWIBullet.Reload();
                }
                currentBulletIndex = 0;
                currentTimer = 0f;
                magEmpty = false;
            }
        }
    }


    public override void OnPickupUseDown()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "NetworkedShooting");
        if (currentBulletIndex < bullets.Length)
        {
            Debug.Log($"VRC_OWO_WorldIntegration: [{{ \"priority\": {sensationPriority},\"sensation\": \"Recoil\",\"frequency\": 11,\"duration\": 3,\"intensity\": {recoilIntensity},\"rampup\":0,\"rampdown\":1.5,\"exitdelay\":0,\"Muscles\": {{ \"arm_{(isRightHandHolding ? "R" : "L")}\": 100,\"pectoral_{(isRightHandHolding ? "R" : "L")}\": 100}}}}]");
        }

    }
    public void NetworkedShooting()
    {
        if (currentBulletIndex < bullets.Length)
        {
            bullets[currentBulletIndex].FireBullet();

            audioSource.clip = bulletShot;
            animator.SetTrigger("GunShot");
            audioSource.Play();
            currentBulletIndex++;
        }
        else
        {
            audioSource.clip = emptyMag;
            audioSource.Play();
            magEmpty = true;
        }



    }

}