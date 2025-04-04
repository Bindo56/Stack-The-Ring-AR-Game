using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject[] spawnObjects;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spwanDonuts());
    }

    IEnumerator spwanDonuts()
    {
        yield return new WaitForSeconds(2);

        for (int i = 0; i < 3; i++)
        {
            Instantiate(spawnObjects[i], spawnPoints[i].position, Quaternion.identity);
        }

      //  StartCoroutine(spwanDonuts());

    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
