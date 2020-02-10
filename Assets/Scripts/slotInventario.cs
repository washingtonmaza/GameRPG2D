using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slotInventario : MonoBehaviour
{
    public          GameObject          objetoSlot;
    private         _GameController     _GameController;
    private         PainelItemInfo      PainelItemInfo; 
    public          int                 idSlot;
    
    void Start()
    {
        _GameController = FindObjectOfType(typeof(_GameController)) as _GameController; //para acessar o script do game controler
        PainelItemInfo  = FindObjectOfType(typeof(PainelItemInfo))  as PainelItemInfo; //para acessar o script do painel
    }

   
    void Update()
    {
        
    }
    public void usarItem() 
    {
        if (objetoSlot != null)
        {
            objetoSlot.SendMessage("usarItem", SendMessageOptions.DontRequireReceiver); //dont require caso n tenha receptador n vai da erros
            PainelItemInfo.objetoSlot = objetoSlot;
            PainelItemInfo.idSlot     = idSlot;
            PainelItemInfo.carregarInfoItem(); //chamei a funcao para carregar os dados do item
           _GameController.openItemInfo();
        }
    }

}
