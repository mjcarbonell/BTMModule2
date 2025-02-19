using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingDoor : MonoBehaviour
{
    public float openAngle = 90f; // Angle the door rotates to
    public float speed = 2f;      // Speed of rotation
    public bool openClockwise = true; // Rotate clockwise or counterclockwise
    public bool autoClose = false; // Should the door close automatically?
    public float closeDelay = 3f; // Time before closing (if autoClose is enabled)

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen = false;

    void Start()
    {
        closedRotation = transform.rotation;
        float rotationDirection = openClockwise ? 1f : -1f;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle * rotationDirection, 0);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleDoor();
        }
    }
    public void ToggleDoor()
    {
        Debug.Log("HERE"); 
        StopAllCoroutines();
        StartCoroutine(isOpen ? CloseDoorRoutine() : OpenDoorRoutine());
    }

    IEnumerator OpenDoorRoutine()
    {
        isOpen = true;
        float time = 0;
        while (time < 1f)
        {
            transform.rotation = Quaternion.Slerp(closedRotation, openRotation, time);
            time += Time.deltaTime * speed;
            yield return null;
        }
        transform.rotation = openRotation;

        if (autoClose)
        {
            yield return new WaitForSeconds(closeDelay);
            StartCoroutine(CloseDoorRoutine());
        }
    }

    IEnumerator CloseDoorRoutine()
    {
        isOpen = false;
        float time = 0;
        while (time < 1f)
        {
            transform.rotation = Quaternion.Slerp(openRotation, closedRotation, time);
            time += Time.deltaTime * speed;
            yield return null;
        }
        transform.rotation = closedRotation;
    }
}
