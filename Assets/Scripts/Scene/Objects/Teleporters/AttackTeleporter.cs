using UnityEngine;

public class AttackTeleporter : MonoBehaviour
{

    #region Attributes

    public Vector2 startPosition;
    public Vector2 targetPosition;

    #endregion

    #region Common

    private void TeleportAttack(AttackController attack)
    {
        attack.caster.CastLocalAttack(startPosition, targetPosition);
        Destroy(attack.gameObject, .1f);
    }

    #endregion

    #region Events

    private void OnTriggerEnter2D(Collider2D other)
    {

        AttackController attack = other.GetComponent<AttackController>();

        if (attack)
        {
            TeleportAttack(attack);
        }
    }

    #endregion

}