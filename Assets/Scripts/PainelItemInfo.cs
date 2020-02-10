using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //para poder acessar os botoes da interface
using TMPro; // para usar texto do textmash pro

public class PainelItemInfo : MonoBehaviour
{
    private     _GameController     _GameController;
    public      int                 idSlot; //vou usar pra saber qual slot estarei excluindo 
    public      GameObject          objetoSlot;  
    [Header ("configuração HUD")]
    public      Image               imgItem;
    public      TMP_Text            nomeItem;
    public      TMP_Text            danoArma;
    public      TMP_Text            clan;
    public      GameObject[]        aprimoramentos;

    
   
   
     [Header ("Botões")]
     public     Button              btnAprimorar;
    public      Button              btnEquipar;
    public      Button              btnExcluir;

     public   int                  idArma; 
     public   int                  aprimoramento;
     private   int                 idPersonagem;
     

    void Start()
    {
       
        _GameController = FindObjectOfType(typeof(_GameController)) as _GameController; // para convesar com o script game controler Maza
        _GameController.upgradeRuby.SetActive(false);
        _GameController.upgradeEsmeralda.SetActive(false);
        _GameController.upgradeSafira.SetActive(false);
        _GameController.upgradeDiamante.SetActive(true);
        verificaPedraClasse();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void carregarInfoItem()
    {
  
        _GameController.MostrarStatusItens(); // VOU VERIFICAR TODOS OS ATRIBUTOS PARA MOSTRAR PRO CARA O ATAQUE DELE
        int idClassePersonagem    = _GameController.idClasse[_GameController.idPersonagem];

        item        itemInfo        = objetoSlot.GetComponent<item>(); // vou pegar o script do componentem item para ter o id da arma
        idArma                      = itemInfo.idItem; //para pegar o id da arma selecionada


        imgItem.sprite              = _GameController.imgInventario[idArma];
        nomeItem.text               = _GameController.nomeArma[idArma]; //peguei o nome da arma
       
       
        
        string      tipoDano        = _GameController.tiposDano[_GameController.tipoDanoArma[idArma]]; //capturei o tipo de dando da arma para usar futuramente
        int         danoMin         = (int) _GameController.statusDanoMinimo; // AQUI N COLOQUEI O ID DA ARMA COMO EM CIMA PORQUE NO GAME CONTROLE ELE FAZ O CALCULO E JOGA PRA STATUS DANO CRITICO O DE baixo NAO CALCULA
        int         danoMax         = _GameController.danoMaxArma[idArma]; //+ (int) _GameController.statusDanoCritico;
       
        if(_GameController.idClasse[_GameController.idPersonagem] == 1 && _GameController.idArma != 1) // SE FOR PALADIN ELE VAI APARECER STATUS DO BOW 
        {
            danoArma.text               = "Atack " + danoMax.ToString() + " / " + tipoDano; 
        }
         else if(_GameController.idClasse[_GameController.idPersonagem] == 1 && _GameController.idArma == 1) // SE FOR PALADIN ELE VAI APARECER STATUS DO BOW 
        {
            danoArma.text               = "Atack " + danoMax.ToString() ; 
        }
        else
        {
            danoArma.text               = "Dano " + danoMin.ToString() + "-" + danoMax.ToString() + " / " + tipoDano; 
        }
        if(_GameController.idClasseArma[_GameController.idArma] == 0)
        {
            clan.text               = "It can only be wielded properly by Knights"; 
        }
        else if(_GameController.idClasseArma[_GameController.idArma] == 1)
        {
            
            clan.text               = "It can only be wielded properly by Archers"; 
        }
        if(_GameController.idClasseArma[_GameController.idArma] == 2)
        {
            clan.text               = "It can only be wielded properly by Wizards";  
        }


       
        carregarAprimoramento(); // chamei a funcao aqui para atualizar a barrinha azul

        if (idSlot == 0)
        {
            btnEquipar.interactable = false;  //se for o item equipado, não vou poder nem jogar fora nem equipar
            btnExcluir.interactable = false;
        }
        else
        {
            int idClasseArma          = _GameController.idClasseArma[idArma]; // peguei o id da classe da arma
            //print(idArma);
            //print(_GameController.idPersonagem); //peguei o id do personagem para comparar se o id é igual o id do dono da arma para poder usar
            verificaClasse(); // chamei essa função porque por algum motivo o idpersonagem apenas em cima não estava buscando realmente o id, então coloquei ele embaixo e começou a pegar o id do personagem.
            if (idClasseArma == idPersonagem)
            {
                btnEquipar.interactable = true;
                btnExcluir.interactable = true;
            }
            else
            {
                btnExcluir.interactable = true;
                btnEquipar.interactable = false;
                btnAprimorar.interactable = false;
            }
        }
    }

    void verificaPedraClasse(){
            print(_GameController.idClasse[_GameController.idPersonagem]);
     switch(_GameController.idClasse[_GameController.idPersonagem])
 
    {
        case 0:
            //string ruby = _GameController.ruby.ToString("N0");
            //_GameController.txtQtdRuby.text = ruby.Replace(",",".");
            
            _GameController.upgradeRuby.SetActive(true);
            _GameController.upgradeEsmeralda.SetActive(false);
            _GameController.upgradeSafira.SetActive(false);

        break;


        case 1:
            //string esmeralda = _GameController.esmeralda.ToString("N0");
            //_GameController.txtQtdEsmeralda.text = esmeralda.Replace(",",".");
            print("esmeralda archer");
            _GameController.upgradeRuby.SetActive(false);
            _GameController.upgradeEsmeralda.SetActive(true);
            _GameController.upgradeSafira.SetActive(false);
        break;

        case 2:
            //string safira = _GameController.safira.ToString("N0");
            //_GameController.txtQtdSafira.text = safira.Replace(",",".");
            print("safira mage");
            _GameController.upgradeSafira.SetActive(true);
            _GameController.upgradeRuby.SetActive(false);
            _GameController.upgradeEsmeralda.SetActive(false);
            
        break;


    }
}
    public void bAprimorar()
    {
        
        switch(_GameController.idClasse[_GameController.idPersonagem])
        {
            case 0:
                _GameController.ruby -=1;
            break;

             case 1:
                _GameController.esmeralda -=1;
            break;

             case 2:
                _GameController.safira -=1;
            break;
        }


        _GameController.aprimorarArma(idArma);

        
        carregarAprimoramento(); //chamo a funcao pra atualizar a barrinha azul quando clico em aprimorar
    }

     public void bAprimorarDiamante()
    {
        
      _GameController.diamante -=1;
    
     _GameController.aprimorarArma(idArma);
        
        
        carregarAprimoramento(); //chamo a funcao pra atualizar a barrinha azul quando clico em aprimorar
    }
    public void bEquipar()
    {
        objetoSlot.SendMessage("usarItem", SendMessageOptions.DontRequireReceiver);
        _GameController.swapItens(idSlot); // ele vai indicar o slot que esta sendo usado
    }
    public void bExcluir()
    {
        _GameController.excluirItem(idSlot);
    }

    void carregarAprimoramento()
    {
        aprimoramento   = _GameController.aprimoramentoArma[idArma]; //joguei o id da arma em aprimoramento
        if(aprimoramento >= 10)
        {
            btnAprimorar.interactable = false; //se ja tiver em seu limite, o botao fica indisponivel
            _GameController.btnaprimorarDiamond.interactable = false;
        } 
        if(_GameController.diamante <1)
        {
            _GameController.btnaprimorarDiamond.interactable = false;
           _GameController.PedrasTransparentes[0].SetActive(true);
           _GameController.upgradeDiamante.SetActive(false);

        }
        switch(_GameController.idClasse[_GameController.idPersonagem])
        {
            case 0:
            if(_GameController.ruby < 1)
            {
                 btnAprimorar.interactable = false;
                 _GameController.PedrasTransparentes[2].SetActive(true);
                _GameController.upgradeRuby.SetActive(false);
            }
            break;

             case 1:
            if(_GameController.esmeralda < 1)
            {
                btnAprimorar.interactable = false;
                _GameController.PedrasTransparentes[1].SetActive(true);
                _GameController.upgradeEsmeralda.SetActive(false);
            }
            break;

             case 2:
            if(_GameController.safira < 1)
            {
                btnAprimorar.interactable = false;
                _GameController.PedrasTransparentes[3].SetActive(true);
                _GameController.upgradeSafira.SetActive(false);
            }
            break;
        }
      
        
        
        

        foreach(GameObject a in aprimoramentos) // aprimoramentoS é um game object que vai jogar aprimoramento dentro que é o id
        {
            a.SetActive(false); //a barra azul vai ficar toda desativada
            for (int i=0; i<aprimoramento; i++)
            {
                aprimoramentos[i].SetActive(true); //ativo o aprimoramento de acordo com cada nivel de habilidade do player
            }
        }
    }
   public void verificaClasse()
    {
    idPersonagem =  _GameController.idClasse[_GameController.idPersonagem]; // usei essa funcao para poder pegar o id do personagem corretamente
      
    }
}
