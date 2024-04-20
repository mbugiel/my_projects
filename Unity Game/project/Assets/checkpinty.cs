using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpinty : MonoBehaviour
{
    public static int ileCheckpointów = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ileCheckpointów = ileCheckpointów + 1;
        Destroy(this);
    }
}
