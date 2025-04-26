using UnityEngine;

public class EnemyGeneral : MonoBehaviour
{

    [SerializeField] private Animator m_Animator;

    private const string HIT_PARAM = "isHit";
    private const string PLAYER_LIGHT1_HITBOX = "playerHitboxLight1";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == PLAYER_LIGHT1_HITBOX)
        {
            m_Animator.SetTrigger(HIT_PARAM);
        }
    }
}
