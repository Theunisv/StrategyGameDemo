using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimate : MonoBehaviour
{
    private bool _isSpinning = false;
    private bool _isFloating = false;
    public float spinTime = 2.5f;
    private float _yPeakPos; 
    private float rotationYDegrees = 180f;
    private float _yStartingPos;
    private BattleManager activeManager;

    private void Start()
    {
        activeManager = GameObject.Find("TurnManager").GetComponent<BattleManager>();
        _yStartingPos = gameObject.transform.position.y;
        _yPeakPos = _yStartingPos + 0.15f;
    }
    private void Update()
    {
        if (_isSpinning == false)
        {
            _isSpinning = true;
           // StartCoroutine(Spin());
        }

        if (_isFloating == false)
        {
            _isFloating = true;
            StartCoroutine(Float());
        }

        if (gameObject.CompareTag("ActiveMageIcon"))
        {
            Vector3 magePos = activeManager.CurrentMageController.gameObject.transform.position;
            if (magePos.y !=  _yStartingPos - 0.9f && magePos.x != gameObject.transform.position.x)
            {
                _yStartingPos = magePos.y + 0.9f;
                gameObject.transform.position = new Vector3(magePos.x,_yStartingPos, 0);
                _yPeakPos = _yStartingPos + 0.15f;
            }
        }
    }
    private IEnumerator Spin()
    {
        LeanTween.rotate(gameObject, new Vector3(0f,rotationYDegrees,0f), spinTime);
        yield return new WaitForSeconds(spinTime);
        _isSpinning = false;
        if (rotationYDegrees == 180f) rotationYDegrees = 0;
        else rotationYDegrees = 180f;
    }
    private IEnumerator Float()
    {
        LeanTween.moveY(gameObject, _yPeakPos, spinTime);
        yield return new WaitForSeconds(spinTime);
        LeanTween.moveY(gameObject, _yStartingPos, spinTime);
        yield return new WaitForSeconds(spinTime);
        _isFloating = false;
    }
}
