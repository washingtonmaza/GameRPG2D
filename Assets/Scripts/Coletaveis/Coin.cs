using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private         _GameController         _GameController;
    public         int         valor;

    void Start()
    {
      _GameController = FindObjectOfType(typeof(_GameController)) as _GameController; // instanciei para ter acesso ao game controle  
    }
    public void coletar()
    {
        _GameController.gold += valor;
        Destroy(this.gameObject);
     }
   


}
