using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    public TurretAI turretAI;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = turretAI.speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

    }

    void HitTarget()
    {
        Destroy(gameObject);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.curHealth -= turretAI.damage;
            enemy.healthBar.fillAmount = enemy.curHealth / enemy.maxHealth;
            
            if (enemy.curHealth <= 0)
            {
                enemy.isDead = true;
            }
            Destroy(gameObject);
        }
    }
}