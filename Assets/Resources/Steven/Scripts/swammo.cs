using UnityEngine;

public class swammo : MonoBehaviour
{
    public LayerMask playerLayer;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            swgun gun = other.gameObject.GetComponentInChildren<swgun>();
            if (gun != null)
            {
                gun.addAmmo(1);
                Destroy(gameObject);
            }
        }
    }
}