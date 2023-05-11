using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    public int maxStar = 5;
    public float potentialToSpawn = 0.5f;
    public bool forceSpawnAll = false; 

    private GameObject[] stars;

    private void Awake()
    {
        stars = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        
            stars[i] = transform.GetChild(i).gameObject;

        OnDisable();
        
    }

    private void OnEnable()
    {
        if (Random.Range(0.0f, 1.0f) > potentialToSpawn)
            return;

        if (forceSpawnAll)

            for (int i = 0; i < maxStar; i++) 
                stars[i].SetActive(true);
        else
        {
            int r = Random.Range(0, maxStar);
            for (int i = 0; i < r; i++)
            {
                stars[i].SetActive(true);
            }

        }

        
    }

    private void OnDisable()
    {
        foreach (GameObject go in stars)
            go.SetActive(false);
    }
}
