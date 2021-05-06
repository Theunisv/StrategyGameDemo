using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class RayCastSpellTriggerable : MonoBehaviour
{
    [HideInInspector] public int splPower = 20;
    [HideInInspector] public float splRange = 5f;
    public Transform _castingMage;
    public Transform _targetMage;
    [HideInInspector] public LineRenderer splLine;

    private WaitForSeconds splTravelDuration = new WaitForSeconds(0.1f);
    private AudioSource mageAudio;

    private TurnManager _turnManager;

    public void InitializeSpl()
    {
        splLine = GetComponent<LineRenderer>();
    }

    public void Cast(Transform castingMage, Transform targetMage)
    {
        _castingMage = castingMage;
        _targetMage = targetMage;
        
        var heading = targetMage.position - castingMage.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        
        //Ray starts at the mage who is casting the spell
        Vector3 rayOrigin = castingMage.position;

        Debug.DrawRay(rayOrigin, direction * distance, Color.white);
    }

}
