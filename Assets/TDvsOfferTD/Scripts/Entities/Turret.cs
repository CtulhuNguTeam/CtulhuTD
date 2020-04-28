using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float range = 15f;
    public string enemyTag = "Enemy";
    public float fireRate = 1f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public int bulletsAmount = 1;
    [SerializeField]
    private bool needRotation = false;
    [SerializeField]
    private bool animation = false;
    [SerializeField]
    private Sprite spriteRight;
    [SerializeField]
    private Sprite spriteRightDown;
    [SerializeField]
    private Sprite spriteDown;
    [SerializeField]
    private Sprite spriteRightUp;
    [SerializeField]
    private Sprite spriteUp;
    private Transform[] targets;
    private float fireCountdown = 0f;
    private EnemyDistanceComparer enemyDistanceComparer;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private string direction = "Right";
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyDistanceComparer = new EnemyDistanceComparer(transform.position);
        targets = new Transform[bulletsAmount];
        if (animation)
        {
            animator = GetComponent<Animator>();
        }
        else
        {
            InvokeRepeating("UpdateTargets", 0f, 0.5f);
        }
        if (needRotation && !animation)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (needRotation && !animation)
        {
            UpdateSprite();
        }
        if (!animation)
        {
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private class EnemyDistanceComparer : IComparer<GameObject>
    {
        private Vector3 origin;

        public EnemyDistanceComparer (Vector3 origin)
        {
            this.origin = origin;
        }

        public int Compare (GameObject enemy1, GameObject enemy2)
        {
            float dist1 = Vector3.Distance(enemy1.transform.position, origin);
            float dist2 = Vector3.Distance(enemy2.transform.position, origin);
            return (int)Mathf.Round(dist1 - dist2);
        }

        public void SetOrigin (Vector3 origin)
        {
            this.origin = origin;
        }
    }

    void UpdateTargets ()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag).Where(enemy => Vector3.Distance(transform.position, enemy.transform.position) <= range).ToArray();
        enemyDistanceComparer.SetOrigin(transform.position);
        Array.Sort(enemies, enemyDistanceComparer);
        for (int i = 0; i < bulletsAmount; i++)
        {
            if (i >= enemies.Length)
            {
                targets[i] = null;
            }
            else
            {
                targets[i] = enemies[i].transform;
            }
        }
        if (animation)
        {
            UpdateAnimation();
        }
    }

    private void UpdateSprite ()
    {
        Transform target = targets[0];
        if (target == null) return;
        Vector3 direction = target.position - transform.position;
        direction = direction.normalized;
        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
            direction.x = -direction.x;
        }
        else
        {
            spriteRenderer.flipX = false;
        }       
        float tan = direction.y / direction.x;
        if (tan > Mathf.Sqrt(3))
        {
            spriteRenderer.sprite = spriteUp;
        }
        else if (tan > 1 / Mathf.Sqrt(3))
        {
            spriteRenderer.sprite = spriteRightUp;
        }
        else if (tan > -1 / Mathf.Sqrt(3))
        {
            spriteRenderer.sprite = spriteRight;
        }
        else if (tan > -Mathf.Sqrt(3))
        {
            spriteRenderer.sprite = spriteRightDown;
        }
        else
        {
            spriteRenderer.sprite = spriteDown;
        }
    }

    public void UpdateAnimation ()
    {
        Transform target = targets[0];
        if (!needRotation)
        {
            if (target == null)
            {
                animator.Play("Idle");
            }
            else
            {
                animator.Play("Attack");
            }
            return;
        }
        if (target == null)
        {
            if (isAttacking)
            {
                isAttacking = false;
                animator.Play(String.Format("{0}_idle", this.direction));
            }
            return;
        }
        Vector3 direction = target.position - transform.position;
        direction = direction.normalized;
        string newDirection;
        if (direction.x > 0)
        {
            float tan = direction.y / direction.x;
            if (tan > Mathf.Sqrt(3))
            {
                newDirection = "Back";
            }
            else if (tan > 1 / Mathf.Sqrt(3))
            {
                newDirection = "Back_right";
            }
            else if (tan > -1 / Mathf.Sqrt(3))
            {
                newDirection = "Right";
            }
            else if (tan > -Mathf.Sqrt(3))
            {
                newDirection = "Forward_right";
            }
            else
            {
                newDirection = "Forward";
            }
        }
        else
        {
            float tan = -direction.y / direction.x;
            if (tan > Mathf.Sqrt(3))
            {
                newDirection = "Back";
            }
            else if (tan > 1 / Mathf.Sqrt(3))
            {
                newDirection = "Back_left";
            }
            else if (tan > -1 / Mathf.Sqrt(3))
            {
                newDirection = "Left";
            }
            else if (tan > -Mathf.Sqrt(3))
            {
                newDirection = "Forward_left";
            }
            else
            {
                newDirection = "Forward";
            }
        }
        if (!isAttacking || newDirection != this.direction)
        {
            this.direction = newDirection;
            animator.Play(String.Format("{0}_attack", this.direction));
        }
        isAttacking = true;
    }

    public void Shoot()
    {
        foreach (Transform target in targets)
        {
            if (target == null) break;
            GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Seek(target);
            }
        }
    }

}
