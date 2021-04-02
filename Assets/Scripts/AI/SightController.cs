using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightController : MonoBehaviour
{
    BoxCollider SightField;
    private void Start()
    {

        SightField = gameObject.GetComponent<BoxCollider>();

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "gold")
        {
            Debug.Log("See a coin!");
            gameObject.GetComponentInParent<EnemyController>().CoinSpotted(other);
            //gameObject.GetComponentInParent<EnemyController>.
        }

        if(other.gameObject.tag == "Player")
        {
            Debug.Log("See a Player!");
            gameObject.GetComponentInParent<EnemyController>().PlayerSpotted(other);
        }

    }

    public void SetMode(string SightMode)
    {
        if(SightMode == "straight")
        {
            SightField.enabled = true;
            SightField.size = new Vector3(1.5f, 1.56f, 6f);
            Debug.Log("Looking forward!");
        }
        if(SightMode == "wide")
        {
            SightField.enabled = true;
            SightField.size = new Vector3(8f, 1.56f, 6f);
          //смотрим врепед и по бокам для поиска монеток
            Debug.Log("Looking around!");
        }
        if(SightMode == "around")
        {
            //тут надо подумать, возможно сместить центр назад, пока заглушка как wide
            SightField.enabled = true;
           //смотрим врепед и по бокам для поиска монеток
        }
        if(SightMode == "disabled")
        {
            SightField.enabled = false; 
            Debug.Log("See nothing!");
        }
    }
}
