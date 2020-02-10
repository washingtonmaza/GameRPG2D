using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaDrops : MonoBehaviour
{
    private         _GameController         _GameController;
    public          GameObject[]               itemColetar;
    private         bool                    coletado;
    void Start()
    {
      _GameController = FindObjectOfType(typeof(_GameController)) as _GameController; // instanciei para ter acesso ao game controle  
    }
    public void coletar()
    {
        if(coletado == false) // para garantir que o personagem so vá colidir uma unica vez, para n pegar dois itens
        {
        coletado = true;
        _GameController.ColetarItens(itemColetar[Random.Range(0, itemColetar.Length)]); //para escolher randomicamente qual dos item pegar
        Destroy(this.gameObject);
        }
     }
   
}
