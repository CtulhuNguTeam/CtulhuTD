using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float startSpeed = 10f;
    public float startHealth = 100;
    public int units = 1;
    public float armory = 0;
    public int rating = 0;
    [HideInInspector]
    public float speed;
    public Image healthBar;
    private float health;

    void Start()
    {
        speed = startSpeed * 0.5f;
        health = startHealth;
    }

    public void TakeDamage(float amount)
    {
        health -= amount * (1 - armory);
        healthBar.fillAmount = health / startHealth;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        PlayerStats.IncreaseElement(1f / units);
        Destroy(gameObject);
    }

    public void Slow(float pct)
    {
        speed = startSpeed / pct;
    }
}
