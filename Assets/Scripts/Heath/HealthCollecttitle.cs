using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollecttitle : MonoBehaviour
{
    [SerializeField] private float healthValue;

    private void OnTriggerEnter2D(Collider2D col)
    {
        col.GetComponent<Health>().AddHealth(healthValue);
        gameObject.SetActive(false);
    }

}
