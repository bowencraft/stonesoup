using UnityEngine;

public class swmedkit : Tile
{
    public LayerMask playerLayer;

    public int damageAmount = 1;
    public float damageForce = 1000;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            Tile otherTile = other.gameObject.GetComponent<Tile>();
            if (otherTile != null)
            {
                otherTile.takeDamage(this, -damageAmount);
                Destroy(gameObject);
            }
        }
    }
}