using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertCanvas_Collider : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}
