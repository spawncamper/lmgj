using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotator : MonoBehaviour
{
    [SerializeField] float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(Vector3.up, rotationSpeed * Time.deltaTime);

        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
