using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour
{
    [Header("===== Output signals =====")]
    public float Dup;
    public float Dright;
    public float DJup;
    public float DJright;
    public float Dmag;
    public Vector3 Dvec;
    //public float targetDup;
    //public float targetDright;
    //public float velocityDup;
    //public float velocityDright;

    // once push
    public bool run;
    // twice trigger
    public bool jump;
    // attack trigger
    public bool attack;
    // shield
    public bool defense;


    [Header("===== Others =====")]
    public bool inputEnabled = true;
}
