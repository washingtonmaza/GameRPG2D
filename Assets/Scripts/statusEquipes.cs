using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // para usar as interfaces da unity

public class statusEquipes : MonoBehaviour
{

    
    [Header("STATUS EQUIPES")]
   public   Image           helmet;
   public   Image           amulet;
   public   Image           weapon;
   public   Image           armor;
   public   Image           shield;
   public   Image           Legs;
   public   Image           ring;
   public   Image           boots;
   public   int             idItem;
   public   item            item;
   private  _GameController _GameController;
   // public   GameObject      obWeapon;
    void Start()
    {
            _GameController = FindObjectOfType(typeof (_GameController)) as _GameController;
            item = FindObjectOfType(typeof(item)) as item;
            idItem = item.idItem;
            //GameObject  temp     = Instantiate(obWeapon);
            //item        itemInfo = temp.GetComponent<item>(); //agora tenho acesso ao script dentro do objeto 
            
            weapon.sprite = weapon.sprite; // pondo a imagem do item no inventario
            weapon.gameObject.SetActive(true); // pondo a imagem do item no inventario
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
