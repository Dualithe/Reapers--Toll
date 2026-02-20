using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] public Transform target;
    private NavMeshAgent agent;

    [Header("Stats")]
    public float health = 100f;
    public float maxHealth = 100f;
    public float speed = 3.5f;
    public int attackPower = 10;
    public float attackSpeed = 1.5f;
    public float attackRange = 0.5f;

    [Header("UI")]
    public Slider healthBar;

    [Header("Death Spawn")]
    [SerializeField] private GameObject spawnOnDeathPrefab;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] [Range(0.5f, 10f)] private float spawnRange = 2f;

    private float lastAttackTime = 0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
        UpdateHealthBar();
    }

    private void Update()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= attackRange)
            { 
                agent.ResetPath();

                if (Time.time >= lastAttackTime + 1f / attackSpeed)
                {
                    Attack();
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                agent.SetDestination(target.position);
            }
        }
    }

    void Attack()
    {
        PlayerHandler.Instance.DamagePlayer(attackPower);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthBar();
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        if (spawnOnDeathPrefab != null)
        {
            Vector2 randomOffset = Random.insideUnitCircle * spawnRange;
            Vector3 spawnPos = new Vector3(
                transform.position.x + randomOffset.x,
                transform.position.y,
                transform.position.z + randomOffset.y
            );
            Instantiate(spawnOnDeathPrefab, spawnPos, Quaternion.identity);
        }
        
        Instantiate(particles, transform.position, quaternion.identity);
        Destroy(gameObject);
    }

    public void SetHealthBar(Slider slider)
    {
        healthBar = slider;
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = health / maxHealth;
        }
    }
}
