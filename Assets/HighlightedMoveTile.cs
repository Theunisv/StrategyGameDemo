using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightedMoveTile : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Sprite _highlightedSprite;
    private Sprite _baseSprite;
    private GizmoManager _gizmoManager;

    private void Start()
    {
        _gizmoManager = GameObject.Find("BattleMap").GetComponent<GizmoManager>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _baseSprite = _spriteRenderer.sprite;
    }

    private void OnMouseEnter()
    {
        _spriteRenderer.sprite = _highlightedSprite;
    }

    private void OnMouseExit()
    {
        _spriteRenderer.sprite = _baseSprite;
    }

    private void OnMouseDown()
    {
        _gizmoManager.MoveToTileClicked(gameObject);
    }
}
