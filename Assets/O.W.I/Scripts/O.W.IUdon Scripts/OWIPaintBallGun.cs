
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static VRC.SDKBase.VRC_Pickup;

public class OWIPaintBallGun : UdonSharpBehaviour
{
    private VRC_Pickup pickup;
    private Transform paintGun;
    private BoxCollider gunCollider;
    private OWIPaintBall[] paintBalls = null;
    private OWIPaintBallNetworked[] paintBallsNetworked = null;
    private AudioSource audioSource;
    private int currentPaintBallIndex = 0;
    private bool magEmpty;
    private float currentTimer;
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
    private AudioClip ballShot;
    [SerializeField]
    private bool isNetworked = false;
    [SerializeField]
    private float lagCorrection = 0.25f;
    [HideInInspector]
    public bool isRightHandHolding;

    private void Start()
    {
        pickup = GetComponent<VRC_Pickup>();
        gunCollider = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();

        int count = 0;
        paintGun = transform.Find("PaintGun_Body");
        // First pass to count the number of "PaintBall" objects
        foreach (Transform child in paintGun)
        {
            if (child.name == (isNetworked ? "O.W.I_Paint_Ball_Networked" : "O.W.I_Paint_Ball"))
            {
                count++;
            }
        }
        // Initialize the paintBalls array with the counted size
        if (isNetworked)
        {
            paintBallsNetworked = new OWIPaintBallNetworked[count];
        }
        else
        {
            paintBalls = new OWIPaintBall[count];
        }

        // Second pass to populate the array
        int index = 0;
        foreach (Transform child in paintGun)
        {
            if (child.name == (isNetworked ? "O.W.I_Paint_Ball_Networked" : "O.W.I_Paint_Ball"))
            {
                if (isNetworked)
                {
                    OWIPaintBallNetworked owoPaintBallNetworked = child.GetComponent<OWIPaintBallNetworked>();
                    if (owoPaintBallNetworked != null)
                    {
                        paintBallsNetworked[index] = owoPaintBallNetworked;
                        index++;
                    }
                }
                else
                {
                    OWIPaintBall owoPaintBall = child.GetComponent<OWIPaintBall>();
                    if (owoPaintBall != null)
                    {
                        paintBalls[index] = owoPaintBall;
                        index++;
                    }
                }
            }
        }

        if (isNetworked)
        {
            foreach (OWIPaintBallNetworked oWOPaintBall in paintBallsNetworked)
            {
                oWOPaintBall.intensity = shotIntensity;
                oWOPaintBall.paintballSpeed = shotSpeed;
                oWOPaintBall.lagcorrection = lagCorrection;
                oWOPaintBall.sensationPriority = sensationPriority + 1;
            }
        }
        else
        {
            foreach (OWIPaintBall oWOPaintBall in paintBalls)
            {
                oWOPaintBall.intensity = shotIntensity;
                oWOPaintBall.paintballSpeed = shotSpeed;
                oWOPaintBall.lagcorrection = lagCorrection;
                oWOPaintBall.sensationPriority= sensationPriority + 1;
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
                if (isNetworked)
                {
                    foreach (OWIPaintBallNetworked oWOPaintBall in paintBallsNetworked)
                    {
                        oWOPaintBall.ReloadPaintBall();
                    }
                }
                else
                {
                    foreach (OWIPaintBall oWOPaintBall in paintBalls)
                    {
                        oWOPaintBall.ReloadPaintBall();
                    }
                }
                // Reset the index so that next interaction starts over from the first paintball
                currentPaintBallIndex = 0;
                currentTimer = 0f;
                magEmpty = false;
            }
        }
    }


    public override void OnPickupUseDown()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "NetworkedShooting");
        if ((isNetworked && currentPaintBallIndex < paintBallsNetworked.Length) ||
            (!isNetworked && currentPaintBallIndex < paintBalls.Length))
        {
            Debug.Log($"VRC_OWO_WorldIntegration: [{{ \"priority\": {sensationPriority},\"sensation\": \"Recoil\",\"frequency\": 1,\"duration\": 1,\"intensity\": {recoilIntensity},\"rampup\":0,\"rampdown\":1.5,\"exitdelay\":0,\"Muscles\": {{ \"arm_{(isRightHandHolding ? "R" : "L")}\": 100,\"pectoral_{(isRightHandHolding ? "R" : "L")}\": 100}}}}]");
        }

    }
    public void NetworkedShooting()
    {
        if (isNetworked)
        {
            if (currentPaintBallIndex < paintBallsNetworked.Length)
            {
                // Call your method on the current OWOPaintBall
                paintBallsNetworked[currentPaintBallIndex].FirePaintBall();

                audioSource.clip = ballShot;
                audioSource.Play();
                // Increment the index for the next interaction
                currentPaintBallIndex++;
            }
            else
            {
                audioSource.clip = emptyMag;
                audioSource.Play();
                magEmpty = true;
            }
        }
        else
        {
            if (currentPaintBallIndex < paintBalls.Length)
            {
                // Call your method on the current OWOPaintBall
                paintBalls[currentPaintBallIndex].FirePaintBall();

                audioSource.clip = ballShot;
                audioSource.Play();
                // Increment the index for the next interaction
                currentPaintBallIndex++;
            }
            else
            {
                audioSource.clip = emptyMag;
                audioSource.Play();
                magEmpty = true;
            }
        }



    }

}
