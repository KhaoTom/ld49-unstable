using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public GameObject shotHitPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(shotHitPrefab, collision.transform.position, Quaternion.identity);
    }
}
