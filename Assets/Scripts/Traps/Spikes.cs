using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : EnemyDamage
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
