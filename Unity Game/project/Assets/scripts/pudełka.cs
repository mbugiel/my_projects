using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pudełka : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform dopudełka;

    // Update is called once per frame
    void Update()
    {
        this.transform.SetPositionAndRotation(dopudełka.position, Quaternion.identity);
    }
}
