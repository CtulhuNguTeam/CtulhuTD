using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Enemy), typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    [HideInInspector]
    public Waypoints path;
    private Transform target;
    private int wavePointIndex = -1;
    private Enemy enemy;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        if (path != null)
        {
            GetNextWayPoint();
        }
    }

    void Update()
    {
        if (target == null) return;
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);
        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWayPoint();
        }
    }

    void GetNextWayPoint()
    {
        if (wavePointIndex >= path.points.Length - 1)
        {
            EndPath();
            return;
        }
        wavePointIndex++;
        target = path.points[wavePointIndex];
        UpdateAnimation();
    }

    void EndPath()
    {
        PlayerStats.lives--;
        Destroy(gameObject);
    }

    private void UpdateAnimation()
    {
        Vector3 direction = target.position - transform.position;
        direction = direction.normalized;
        if (direction.magnitude == 0) return;
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            direction.x = -direction.x;
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }       
        float tan = direction.y / direction.x;
        if (tan > Mathf.Sqrt(3))
        {
            animator.Play("Back");
        }
        else if (tan > 1 / Mathf.Sqrt(3))
        {
            animator.Play("Back-Right");
        }
        else if (tan > -1 / Mathf.Sqrt(3))
        {
            animator.Play("Right");
        }
        else if (tan > -Mathf.Sqrt(3))
        {
            animator.Play("Forward-Right");
        }
        else
        {
            animator.Play("Forward");
        }
    }
}
