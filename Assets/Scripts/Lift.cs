using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    [Header("Lift Settings")]
    public Transform objectToMove;
    public List<Transform> points;
    public float speed = 2f;

    private int currentPointIndex = 0;
    private bool isMoving = false;

    private RCC_CarControllerV3 playerCar;
    private Rigidbody platformRb;

    void Start()
    {
        // 🔥 Get Rigidbody of platform
        platformRb = objectToMove.GetComponent<Rigidbody>();

        if (platformRb == null)
        {
            Debug.LogError("Platform needs a Rigidbody (set it to Kinematic)");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        RCC_CarControllerV3 car = other.GetComponentInParent<RCC_CarControllerV3>();

        if (car != null && !isMoving)
        {
            playerCar = car;

            // 🧲 Parent player to platform
            playerCar.transform.SetParent(objectToMove);

            // 🛑 Disable controls
            playerCar.canControl = false;
            playerCar.gasInput = 0f;
            playerCar.brakeInput = 1f;

            // 🧊 Stop unwanted motion
            Rigidbody carRb = playerCar.GetComponent<Rigidbody>();
            carRb.velocity = Vector3.zero;
            carRb.angularVelocity = Vector3.zero;

            StartCoroutine(MoveAlongPoints());

            Debug.Log("Lift started → player attached + controls disabled");
        }
    }

    IEnumerator MoveAlongPoints()
    {
        isMoving = true;

       
        while (currentPointIndex < points.Count)
        {
            Transform target = points[currentPointIndex];

            while (Vector3.Distance(platformRb.position, target.position) > 0.05f)
            {
                // 🔥 Move using Rigidbody (NO JITTER)
                platformRb.MovePosition(Vector3.MoveTowards(
                    platformRb.position,
                    target.position,
                    speed * Time.deltaTime
                ));

                yield return new WaitForFixedUpdate(); // 🔥 IMPORTANT
            }

            currentPointIndex++;
        }

        Debug.Log("Lift movement completed");

        // 🎮 Restore control + detach
        if (playerCar != null)
        {
            playerCar.transform.SetParent(null);

            Rigidbody carRb = playerCar.GetComponent<Rigidbody>();
            carRb.velocity = Vector3.zero;

            playerCar.canControl = true;
        }

        // Reset for reuse
        Destroy(gameObject);

        isMoving = false;
        currentPointIndex = 0;
    }
}