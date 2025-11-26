using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BrickGenerator : MonoBehaviour
{
    [SerializeField]
    private List<BaseBrick> brickPrefabs;

    [SerializeField]
    private List<float> brickProbabilities;

    [SerializeField]
    private int initialRows = 4;

    [SerializeField]
    private int columns = 8;

    [SerializeField]
    private float ScrollSpeed = 1f;
    

    private int maxYGenerated = 0;
    private float yThreshold = 0;
    
    
    private void Start()
    {
        yThreshold = transform.position.y;
        
        for (int i = 0; i < initialRows; i++)
        {
            GenerateRow(-i);
        }
        maxYGenerated = 0;
    }

    private void FixedUpdate()
    {
        transform.position += Vector3.down * (ScrollSpeed * Time.fixedDeltaTime);

        if (transform.position.y + maxYGenerated < yThreshold)
        {
            GenerateRow(maxYGenerated + 1);
            maxYGenerated += 1;
        }
    }

    private void GenerateRow(int y)
    {
        var columnsForRow = columns;
        float offset = 0;
        if (Mod(y, 2) == 1)
        {
            columnsForRow--;
            offset = 1;
        }
        
        for (int i = 0; i < columnsForRow; i++)
        {
            var prefab = ChooseBrick();
            if (prefab is not null)
            {
                var brick = Instantiate(prefab, transform);
                brick.transform.localPosition = new Vector3(i * 2 + offset, y, 0);
            }
        }
    }

    private BaseBrick ChooseBrick()
    {
        var p = Random.value;
        for (int i = 0; i < brickProbabilities.Count; i++)
        {
            p -= brickProbabilities[i];
            if (p < 0)
            {
                return brickPrefabs[i];
            }
        }

        return null;
    }

    private static int Mod(int x, int m)
    {
        int r = x%m;
        return r < 0 ? r + m : r;
    }
}
