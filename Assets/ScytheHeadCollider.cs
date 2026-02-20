using NUnit.Framework.Constraints;
using UnityEngine;

public class ScytheHeadCollider : MonoBehaviour
{
    [SerializeField]
    private Transform collectionTransform;
    [SerializeField] private Animator anim;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("enemy"))
        {
            var enemy = other.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(PlayerHandler.Instance.scytheDamage);
            }

            anim.Play("hit");
            GameplayHandler.Instance.sound.Play();
        }else if (other.transform.CompareTag("point") || other.transform.CompareTag("bonusPoint"))
        {
            other.transform.GetComponent<Point>().GetCollected(collectionTransform);
        }
    }
}
