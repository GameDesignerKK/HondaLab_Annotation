using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.ARSubsystems;

public class FollowHeadToolbar : MonoBehaviour
{
    public Transform head; // assign your HMD camera transform here

    [Header("Offset (meters)")]
    public float distance = 0.6f;
    public float sideOffset = 0.25f;   // +right / -left
    public float heightOffset = -0.15f; // slightly below eye level

    [Header("Smoothing")]
    public float positionLerp = 12f;
    public float rotationLerp = 12f;

    private HandheldARInputDevice device;

    private void Awake()
    {
        device = InputSystem.devices
            .OfType<HandheldARInputDevice>()
            .FirstOrDefault();
    }

    void LateUpdate()
    {
        /*
        if (head == null)
        {
            if (Camera.main != null) head = Camera.main.transform;
            else return;
        }
        */

        Vector3 headPos = device.devicePosition.ReadValue();
        Quaternion headRot = device.deviceRotation.ReadValue();

        Vector3 targetPos =
            headPos +
            headRot * Vector3.forward * distance +
            headRot * Vector3.right * sideOffset +
            headRot * Vector3.up * heightOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            1f - Mathf.Exp(-positionLerp * Time.deltaTime)
        );

        // face user, keep upright
        Vector3 lookDir = transform.position - headPos;
        lookDir.y = 0f;
        if (lookDir.sqrMagnitude < 1e-6f) lookDir = transform.forward;

        Quaternion targetRot = Quaternion.LookRotation(lookDir.normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            1f - Mathf.Exp(-rotationLerp * Time.deltaTime)
        );
    }
}
