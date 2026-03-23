using UnityEngine;



public class RotationOnCollision : MonoBehaviour
{
    public float rotationAmount = 10f;   // degrees
    public float rotationSpeed = 3f;     // speed of rotation

    private bool shouldRotate = false;
    private Quaternion targetRotation;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Set target rotation (add 10 degrees on X)
            targetRotation = transform.rotation * Quaternion.Euler(rotationAmount, 0f, 0f);

            shouldRotate = true;

            Debug.Log("Start smooth rotation");
        }
    }

    void Update()
    {
        if (shouldRotate)
        {
            // Smoothly rotate
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Stop when close enough
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                shouldRotate = false;
            }
        }
    }
}