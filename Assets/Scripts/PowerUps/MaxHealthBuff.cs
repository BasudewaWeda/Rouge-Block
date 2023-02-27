using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PowerUps/MaxHealthBuff")]
public class MaxHealthBuff : PowerupEffect
{
    public float amount;

    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerStats>().playerMaxHealth += amount; // Adds max health to player
        target.GetComponent<PlayerStats>().playerHealth += amount;
    }
}
