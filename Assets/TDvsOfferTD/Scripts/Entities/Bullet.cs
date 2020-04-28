using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float speed = 70f;
    public float explosionRadius = 0f;
    public int damage = 50;
    public float slow;
    public int slowContinuation;
    public int damageContinuation;
    public bool instantExplode = false;
    private Transform target;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (instantExplode)
        {
            Explode();
            Destroy(gameObject);
            return;
        }
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }
        Destroy(gameObject);
    }

    private IEnumerator ContinueSlow(Enemy enemy)
    {
        for (int i = 0; i < slowContinuation - 1; i++)
        {
            yield return new WaitForSeconds(1f);
            enemy.Slow(slow);
        }
        enemy.Slow(1);
        yield return 0;
    }

    private IEnumerator ContinueDamage(Enemy enemy)
    {
        for (int i = 0; i < damageContinuation - 1; i++)
        {
            yield return new WaitForSeconds(1f);
            enemy.TakeDamage(damage);
        }
        yield return 0;
    }

    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        if (e != null)
        {
            e.TakeDamage(damage);
            if (slowContinuation != 0)
            {
                e.Slow(slow);
                StartCoroutine(ContinueSlow(e));
            }
            if (damageContinuation != 0)
            {
                StartCoroutine(ContinueDamage(e));
            }
        }
    }

    void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
