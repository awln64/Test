using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 50;
    public Animator animator;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        animator.SetTrigger("Death");
        GetComponent<Collider>().enabled = false;
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
    }

    public void OnDeathAnimationEnd()
    {
        Destroy(gameObject);
    }
}
