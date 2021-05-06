using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ButtonInteraction : MonoBehaviour
{
    private Animator anim;
    public float resetTime;   
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    private void OnMouseDown()
    {
        StartCoroutine(FireTrigger());
    }

    private IEnumerator FireTrigger()
    {
        anim.Play("ClickAnim");
        yield return new WaitForSeconds(0.4f); 
        DestroyObject();
        //anim.ResetTrigger("Active");
    }
    
    public void DestroyObject()
    {
        GameObject frame = gameObject.transform.parent.gameObject;
        Destroy(frame);
    }
}
