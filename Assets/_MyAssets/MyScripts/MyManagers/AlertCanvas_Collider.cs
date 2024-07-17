using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertCanvas_Collider : MonoBehaviour
{
    [SerializeField]
    private AlertCanvas_Network_Game gameMan;

    private void OnTriggerExit2D(Collider2D collision)
    {
        gameMan.isOnTarget = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameMan.isOnTarget = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        gameMan.isOnTarget = true;
        Debug.Log(collision.gameObject.name);
    }
}
