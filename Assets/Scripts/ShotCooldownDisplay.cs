using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotCooldownDisplay : MonoBehaviour
{
    PlayerController player;
    Image image;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = 1f - player.shotCooldownRemaining;
    }
}
