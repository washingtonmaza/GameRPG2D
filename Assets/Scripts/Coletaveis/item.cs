using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    private     _GameController     _GameController;
    public      int                 idItem;
    void Start()
    {
        _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public void usarItem()
    {
       
        _GameController.usarItemArma(idItem);
        
    }
}
