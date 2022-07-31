using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    private float rotationAmount = 5.0f;

    // Update is called once per frame
    void Update()
    {
        // Very cool camera slow spin
        transform.Rotate(Vector3.up, rotationAmount * Time.deltaTime, Space.World);
    }
}
