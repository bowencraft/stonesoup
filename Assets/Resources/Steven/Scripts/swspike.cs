using UnityEngine;

public class swspike : Tile
{
    public LayerMask playerLayer;

    public int damageAmount = 1;
    public float damageForce = 1000;

    void OnTriggerEnter2D(Collider2D other)
    {
            Tile otherTile = other.gameObject.GetComponent<Tile>();
            if (otherTile != null)
            {
                otherTile.takeDamage(this, damageAmount);
                Vector2 toOtherTile = (Vector2)otherTile.transform.position - (Vector2)transform.position;
                toOtherTile.Normalize();
                otherTile.addForce(damageForce * toOtherTile);
            }
    }
}