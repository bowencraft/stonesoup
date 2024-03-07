using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class apt283meat : Tile
{

    public AudioClip pickupSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Ensure there's an AudioSource component attached
        if (audioSource == null)
        {
            // Add an AudioSource component dynamically if not found
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.health += 1;  // Increase player's health

            // Play the pickup sound if available
            if (pickupSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(pickupSound);
                // Disable the sprite renderer and collider so the meat "disappears" but the sound can still play
                if (GetComponent<SpriteRenderer>() != null)
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                }
                if (GetComponent<Collider2D>() != null)
                {
                    GetComponent<Collider2D>().enabled = false;
                }
                // Destroy the game object after the sound has finished playing
                Destroy(gameObject, pickupSound.length);
            }
            else
            {
                // If no sound, destroy the object immediately
                Destroy(gameObject);
            }
        }
    }
}
