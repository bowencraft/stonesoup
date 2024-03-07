using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class apt283Mushroom : Tile
{
    public AudioClip pickupSound;
    private AudioSource audioSource;
    public Material invertColorsMaterial; // Material with the shader to invert colors

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            // Play the pickup sound
            if (pickupSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(pickupSound);
            }

            // Disable the sprite renderer and collider so the mushroom "disappears"
            if (GetComponent<SpriteRenderer>() != null)
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
            if (GetComponent<Collider2D>() != null)
            {
                GetComponent<Collider2D>().enabled = false;
            }

            StartCoroutine(InvertColorsEffect());

            // Destroy the mushroom object after the sound has finished playing
            Destroy(gameObject, pickupSound.length);
        }
    }

    IEnumerator InvertColorsEffect()
    {
        var originalMaterials = new Dictionary<Renderer, Material>();
        var allRenderers = FindObjectsOfType<Renderer>();

        // Change the materials to inverted color
        foreach (var renderer in allRenderers)
        {
            if (renderer != null)
            {
                originalMaterials[renderer] = renderer.material;
                renderer.material = invertColorsMaterial;
            }
        }

        yield return new WaitForSeconds(7);

        // Revert the materials back
        foreach (var kvp in originalMaterials)
        {
            Renderer renderer = kvp.Key;
            Material originalMaterial = kvp.Value;
            if (renderer != null)
            {
                renderer.material = originalMaterial;
            }
        }
    }
}
