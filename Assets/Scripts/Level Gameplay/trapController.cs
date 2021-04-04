using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapController : MonoBehaviour
{
    // Start is called before the first frame update
    bool armed;
    Light redLight;

    GameObject inCoin;

    void Start()
    {
        armed = false;
        redLight = gameObject.GetComponent<Light>(); 
        redLight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "gold")
        {
            Debug.Log("trap armed!");
            inCoin = other.gameObject;
            armed = true;
            redLight.enabled = true;
            redLight.range = 2f;
            redLight.intensity = 1f;
            other.gameObject.transform.position = transform.position + new Vector3(0, 0.5f, 0);

        }
        if(other.gameObject.tag == "enemy" && armed)
        {
            Debug.Log("trap explodes!");
            //Destroy(other.gameObject, 0.5f);
            armed = false;
            redLight.enabled = false;
            Destroy(inCoin.gameObject);
            other.gameObject.GetComponent<EnemyController>().dead();
            

            //добавить анимацию убийства
            //добавить скрипт передачи монет

        }
    }

}
