using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xpMonstro : MonoBehaviour
{
    private _GameController     _GameController;
    private audioController     audioController;



    void Start()
    {
    _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
    audioController = FindObjectOfType(typeof(audioController)) as audioController;
    
    }

    // Update is called once per frame
    void Update()
    {
    
        

     
    }

    
     
    
}
