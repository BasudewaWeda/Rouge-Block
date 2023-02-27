using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PowerUps/FireRateBuff")]
public class FireRateBuff : PowerupEffect
{
    public float amount;
    public override void Apply(GameObject target)
    {
        target.transform.GetChild(0).gameObject.GetComponent<ShootScript>().fireRate += amount; // Increase firerate
    }
}
