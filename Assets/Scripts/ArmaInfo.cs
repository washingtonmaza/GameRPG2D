using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaInfo : MonoBehaviour
{
private _GameController     _GameController;
 public float danoMax;
 public float danoMin;
 public int tipoDeDano;
void Start()
    {
        _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
  
        if(_GameController.idClasse[_GameController.idPersonagem] == 1) // ELE VAI ATRIBUIR O ATAK DA FLECHA + ATAK DO BOW 
        {
       
            danoMax += _GameController.danoMaxArma[_GameController.idArma]; 
            
        }
    }

}
  
