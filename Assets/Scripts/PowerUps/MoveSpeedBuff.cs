using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PowerUps/MoveSpeedBuff")]
public class MoveSpeedBuff : PowerupEffect
{
    public float amount;

    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerMovement>().moveSpeed += amount; // Adds move speed to player
    }
}
