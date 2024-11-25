using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusyScript : MonoBehaviour
{
    [SerializeField] private float busyMetter = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int index1 = 0; index1 < busyMetter; index1++)
        {
            for (int index2 = 0; index2 < busyMetter; index2++)
            {
                Vector3 v3 = new Vector3(index1, index2, 0);
            }
        }
    }
}
