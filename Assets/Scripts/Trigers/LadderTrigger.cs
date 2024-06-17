using System;
using UnityEngine;

public class LadderTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;

    public event Action OnDetect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((targetLayer.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            //Debug.Log("Hit with Layermask");
        }
        else
        {
            //Debug.Log("Not in Layermask");
        }
    }

}
