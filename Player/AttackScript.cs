using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    public Animator animator;
    public Player player;

    public float attackRange = 1.5f;
    public LayerMask enemyLayers;

    private Transform attackPoint;

    void Update()
    {
        ItemScriptableObject item = player?.ItemPrefab;
        if (Input.GetMouseButtonDown(0) && item?.itemType == ItemType.WEAPON)
        {
            animator.SetTrigger("Attack");
            UpdateAttackPoint(item);
        }
    }

    void UpdateAttackPoint(ItemScriptableObject item)
    {
        if (player.currentItem != null)
        {
            Transform found = player.currentItem.transform.Find("AttackPoint");
            if (found != null)
            {
                attackPoint = found;
                DealDamage(item.damage);
            }
            else
            {
                Debug.LogWarning("AttackPoint not found!");
            }
        }
        else
        {
            attackPoint = null;
        }
    }

    public void DealDamage(float damage)
    {
        if (attackPoint == null) return;

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>()?.TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
