using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; //incrementado para acessar o text mesh pro
public class inventario : MonoBehaviour
{
    private     _GameController     _GameController;
    public      Button[]            slot;
    public      Image[]             iconItem;
    public      GameObject          objetoSlot; 
    public      TextMeshProUGUI     qtdHealthPotion, qtdManaPotion, qtdArrowA, qtdArrowB, qtdArrowC; 
    public      int                 qHealthPotion, qManaPotion, qArrowA, qArrowB, qArrowC; //usar para atualizar as variaveis
    public      List<GameObject> itemInventario; //estou criando uma lista pois um array contem erros ao apagar elementos a lista não
    public      List<GameObject> itensCarregados; // para poder utilizar o script pois o item inventario esta na pasta
    void Start()
    {
        _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
        _GameController.Objsafira.SetActive(false);
        _GameController.ObjEsmeralda.SetActive(false);
        _GameController.ObjRuby.SetActive(false);
        _GameController.arrowA.SetActive(false);
        _GameController.arrowB.SetActive(false);
        _GameController.arrowC.SetActive(false);

       
    }

    public void usarItem()
    {
        objetoSlot.SendMessage("usarItem", SendMessageOptions.DontRequireReceiver); //toda vez que usar o item vai chamar essa funcao
    }

    public void carregarInventario()
    {

        string gold = _GameController.gold.ToString("N0");
        _GameController.txtQtdGold.text = gold.Replace(",",".");

        string diamante = _GameController.diamante.ToString("N0");
        _GameController.txtQtdDiamond.text = diamante.Replace(",",".");
        
///=================== VAI EXIBIR O ITEM DE ACORDO COM A PROFISSAO ==========//

    switch(_GameController.idClasse[_GameController.idPersonagem])
    {
        case 0:
            string ruby = _GameController.ruby.ToString("N0");
            _GameController.txtQtdRuby.text = ruby.Replace(",",".");
            _GameController.ObjRuby.SetActive(true);
            _GameController.ObjEsmeralda.SetActive(false);
            _GameController.Objsafira.SetActive(false);

        break;


        case 1:
            string esmeralda = _GameController.esmeralda.ToString("N0");
            _GameController.txtQtdEsmeralda.text = esmeralda.Replace(",",".");
            _GameController.ObjEsmeralda.SetActive(true);
            _GameController.arrowA.SetActive(true);
            _GameController.arrowB.SetActive(true);
            _GameController.arrowC.SetActive(true);

            _GameController.ObjRuby.SetActive(false);
            _GameController.Objsafira.SetActive(false);
        break;

        case 2:
            string safira = _GameController.safira.ToString("N0");
            _GameController.txtQtdSafira.text = safira.Replace(",",".");
            _GameController.Objsafira.SetActive(true);
            _GameController.ObjEsmeralda.SetActive(false);
            _GameController.ObjRuby.SetActive(false);
        break;


    }
    
        

///=================== VAI EXIBIR O ITEM DE ACORDO COM A PROFISSAO ==========//



        limparItensCarregados();
        foreach(Button b in slot)
        {
            b.interactable = false; // poem todos os botoes falso entro do slot
        }
        foreach(Image i in iconItem)
        {
            i.sprite = null; // vai começar sem imagens
            i.gameObject.SetActive(false); // o icone vai começar sem item
        }
        qtdHealthPotion.text = "x " + _GameController.qtdPocoes[0].ToString();
        qtdManaPotion.text = "x "   + _GameController.qtdPocoes[1].ToString();
        qtdArrowA.text = "x "       + _GameController.qtdFlechas[0].ToString();
        qtdArrowB.text = "x "       + _GameController.qtdFlechas[1].ToString();
        qtdArrowC.text = "x "       + _GameController.qtdFlechas[2].ToString();

        int s = 0; //ID DO SLOT
        foreach(GameObject i in itemInventario)
        {
            GameObject  temp     = Instantiate(i); //variavel temporaria
            item        itemInfo = temp.GetComponent<item>(); //agora tenho acesso ao script dentro do objeto 
            itensCarregados.Add(temp);
            slot[s].GetComponent<slotInventario>().objetoSlot = temp;
            slot[s].interactable = true; // assim que ele carrega fica interativo, ou seja pode ser clicavel
            iconItem[s].sprite = _GameController.imgInventario[itemInfo.idItem]; // pondo a imagem do item no inventario
            iconItem[s].gameObject.SetActive(true); // pondo a imagem do item no inventario
            s++;
        }
    }

    public void limparItensCarregados()
    {
         foreach(GameObject ic in itensCarregados)
        {
            Destroy(ic); //vai destruir os item para ficar somente os nomes
        }
        itensCarregados.Clear(); //limpar a lista depois que for carregado
    }
}
