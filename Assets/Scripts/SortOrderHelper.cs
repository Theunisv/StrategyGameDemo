using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class SortOrderHelper : MonoBehaviour
{
    public void OnUpdate()
    {
        SpriteRenderer[] spriteRenderers = FindObjectsOfType<SpriteRenderer>();

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer.sortingLayerName == "Units&Objects")
            {
                spriteRenderer.sortingOrder = (int) (spriteRenderer.transform.position.y * -100);
                if (spriteRenderer.gameObject.CompareTag("SelectionCircles"))
                {
                    spriteRenderer.sortingOrder -= 100;
                }
            }
        }
    }
}
