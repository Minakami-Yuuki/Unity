using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterJoyStick : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        print(Input.GetAxis("LRT"));
    }
}
