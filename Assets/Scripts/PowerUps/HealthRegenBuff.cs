using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PowerUps/HealthRegenBuff")]
public class HealthRegenBuff : PowerupEffect
{
    public float amount;
    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerStats>().playerHealthRegen += amount; // Adds max health to player
    }
}
