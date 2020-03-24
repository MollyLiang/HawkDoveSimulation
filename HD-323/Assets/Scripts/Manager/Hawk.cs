﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hawk : MonoBehaviour
{
    public TMP_InputField d, r;
    public int energy;
    // Start is called before the first frame update
    void Start()
    {
        energy = 100;
    }
    // Update is called once per frame
    void Update()

    {
        d = GameObject.Find("DeathInput").GetComponent<TMP_InputField>();
        r = GameObject.Find("ReproductionInput").GetComponent<TMP_InputField>();
        int m = System.Convert.ToInt32(d.text);
        int n = System.Convert.ToInt32(r.text);
        if (energy <= m)
        {
            GameObject.DestroyImmediate(this.gameObject);
        }
        if(energy >= n)
        {
            GameObject.Find("Canvas").GetComponent<Controller>().createoffspring("Hawk", energy/2);
            energy =energy / 2;
        }
            
    }

}