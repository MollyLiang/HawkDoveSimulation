using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Food : MonoBehaviour
{

    public TMP_InputField expiration;

    public int eTime;
    // Start is called before the first frame update
    void Start()
    {
       eTime = System.Convert.ToInt32(GameObject.Find("ExpirationInput").GetComponent<TMP_InputField>().text);
    }

    // Update is called once per frame
    void Update()
    {
        if(eTime == 0)
        {
            GameObject.DestroyImmediate(this.gameObject);
        }
          
        
    }

    public void Expiring()
    {
        eTime = eTime - 1;
    }
}
