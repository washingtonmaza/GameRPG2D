using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class imgTrocaFlechaPersonagem : MonoBehaviour
{
   private      _GameController             _GameController;
   private      SpriteRenderer              sRenderer;

    void Start()
    {
        _GameController = FindObjectOfType(typeof(_GameController)) as _GameController; //ligando acesso ao game controller
        sRenderer       = GetComponent<SpriteRenderer>(); 

        
    }

    // Update is called once per frame
    void Update()
    {
        sRenderer.sprite = _GameController.imgFlecha[_GameController.idFlechaEquipada]; //troquei o icone da flecha no personagem
        
    }
}
