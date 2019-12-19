using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    public TextMeshPro damageIndicator;
    public float lifetime;
    public GameObject effect;

    private void Start()
    {
        Invoke("Destruction", lifetime);
    }

    public void Setup(int damage)
    {
        damageIndicator.text = damage + "";
    }

    void Destruction()
    {
        Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
