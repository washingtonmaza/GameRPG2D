 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //acesso a interface (imagens)
using TMPro; //para usar os textos
using UnityStandardAssets.CrossPlatformInput;

public class Hud : MonoBehaviour
{   
    private     float                   percVida; // VAI CALCULAR O VALOR PRA ENCHER AS BARRINHAS DA HUD
    private     float                   percMana; // FAZ O MESMO QUE O DE CIMA 


    private     audioController         audioController;
    private     _GameController         _GameController;
    private     playerScript            playerScript; //criei uma variavel do tipo playerScript





    public      GameObject              painelFlechas;
    public      Image                   iconFlechas; //para trocar o icone na hud das flechas 
    public      TMP_Text                qtdFlechas;
    public      int                     idPersonagemTemp; //variavel temporaria para pegar o id do personagem
    public      TMP_Text                qtdImgHealthTxt;
    public      GameObject              boxMP;  // E A IMG DO ITEM, ESTOU UTILIZANDO PARA ATIVAR OU DESATIVAR DEPENDENDO DA CLASSE
    public      GameObject              boxHP; // E O ITEM DE HP NA TELA, VOU USAR PARA ATIVAR OU DESATIVAR SE TIVER OU N O ITEM
    public      TMP_Text                qtdImgManaTxt;
    public      RectTransform           BoxA, BoxB; //vou usar pra posicionar na hud os icones de life dinamicamente
    public      Vector2                 posA, posB; //usado para alterar os icones de life e mana na hud caso n tenha um ou outro, posicionar dinamicamente
   
   

[Header("HUD ITENS DAS CLASSES")]

    public  GameObject               staff, sword, bow; // vai aparecer o icone da arma do cara pra atacar





   public   Image                       barraVidaUi; // nova barra de vida 
   public   Image                       barraManaUi;

   public   float                       vidamax;
   public   float                       vidaAtual;


   public   float                       manaAtual;
   public   float                       manaMaxima;
   
   
   
   
   
   
   
   
    void Start()
    {
        //_GameController.idPersonagem        = PlayerPrefs.GetInt("idPersonagem"); //vou pegar do titulo o id do personagem
        boxHP.SetActive(true);
        boxMP.SetActive(true); // inicio falso pois não sei a classe que irá começar
        painelFlechas.SetActive(false);

        audioController         = FindObjectOfType(typeof(audioController)) as audioController;
        _GameController         = FindObjectOfType(typeof(_GameController)) as _GameController;
        playerScript            = FindObjectOfType(typeof(playerScript)) as playerScript; //instanciei o acesso ao script do player
        
     


        posA = BoxA.anchoredPosition; //peguei a posicao do icone de life e joguei na posicao a
        posB = BoxB.anchoredPosition; // peguei a posicao do icone de mana e joguei na posicao b
    }

    // Update is called once per frame
    void Update()
    {   

        // TO PASSANDO PRA HUD OS DADOS DE LIFE E MANA DO PERSONAGEM
        vidaAtual   = _GameController.vidaAtual;
        vidamax     = _GameController.vidaMaxima;

        manaAtual   = _GameController.manaAtual;
        manaMaxima  = _GameController.manaMaxima;
        
       
        //posicaoCaixaPocoes(); // DESATIVADO NO MOMENTO ---------   verifico a posicao dos icones na hud para posicionar de forma dinamicamente 
        ControleBarraVida();
        ControleBarraMana();

        if(painelFlechas.activeSelf == true)
        {
            if(CrossPlatformInputManager.GetButtonDown("btnL"))
            {
                if(_GameController.idFlechaEquipada == 0)
                {
                    _GameController.idFlechaEquipada = _GameController.iconFlecha.Length -1; // vou trocar as flechas pela tecla Q e E
                }
                else
                {
                    _GameController.idFlechaEquipada -=1;
                }
                
            } 
            else if(CrossPlatformInputManager.GetButtonDown("btnR"))
            {
                if(_GameController.idFlechaEquipada == _GameController.iconFlecha.Length -1)
                {
                    _GameController.idFlechaEquipada = 0; // vou trocar as flechas pela tecla Q e E
                }
                else
                {
                    _GameController.idFlechaEquipada +=1;
                }
            } 
            
            iconFlechas.sprite = _GameController.iconFlecha[_GameController.idFlechaEquipada];
            qtdFlechas.text = "x " + _GameController.qtdFlechas[_GameController.idFlechaEquipada].ToString(); // converti para texto a quantidade de flecha que tem no game controler
            
        }

        qtdImgHealthTxt.text = _GameController.qtdPocoes[0].ToString(); // no hud estou adicionando quantas potions eu tenho
        qtdImgManaTxt.text = _GameController.qtdPocoes[1].ToString(); // no hud estou adicionando quantas potions eu tenho
    }

    public void posicaoCaixaPocoes()  // DESATIVADO NO MOMENTO ---------  SERVE PARA POSICIONAR AS POCOES CASO ESTEJA 1 DO LADO DA OUTRA
    {
        if(_GameController.qtdPocoes[0]>0)
        {
            boxHP.GetComponent<RectTransform>().anchoredPosition = posA; //peguei a posicao do icone hp qe esta no box A e coloquei no boxhp
            boxMP.GetComponent<RectTransform>().anchoredPosition = posB;
        }
        else
        {
            boxHP.GetComponent<RectTransform>().anchoredPosition = posB; //se n tiver pot de life ai a mana vai pro lugar do life
            boxMP.GetComponent<RectTransform>().anchoredPosition = posA;  
        }
    }

    void ControleBarraVida() 
        {
       

        if(Input.GetButtonDown("itemA") && vidaAtual < vidamax ) //verifica se ele apertar a tecla (E) ele vai usar pocao de vida caso tenha
        {
            _GameController.usarPocao(0); //1: igual a pocao de cura
            audioController.tocarFx(audioController.fxHealthP, 1);
        }
        if(vidaAtual > vidamax)
        {
            vidaAtual = vidamax;
        }
        if(vidaAtual < 0)
        {
            vidaAtual = 0;
        }
          barraVidaUi.rectTransform.sizeDelta = new Vector2(vidaAtual/vidamax*250,43); // 43 é o numero da altura da barra e 250 e a largura que vai diminuir
        }


void    ControleBarraMana()
{
    if(Input.GetButtonDown("itemB") && manaAtual < manaMaxima ) //verifica se ele apertar a tecla (E) ele vai usar pocao de vida caso tenha
        {
            _GameController.usarPocao(0); //1: igual a pocao de cura
            audioController.tocarFx(audioController.fxHealthP, 1);
        }
          if(manaAtual > manaMaxima)
        {
            manaAtual = manaMaxima;
        }
        if(manaAtual < 0)
        {
            manaAtual = 0;
        }
          barraManaUi.rectTransform.sizeDelta = new Vector2(manaAtual/manaMaxima*250,43);
          print(barraManaUi.rectTransform.sizeDelta);
        
    
}













 //=====================  SISTEMA DE LIFE OBSOLETA, ALTERADA PARA O SISTEMA ACIMA ======================///
    void ControleBarraVida2() 
        {


        percVida = (float) _GameController.vidaAtual / (float) _GameController.vidaMaxima; //calcula o percentual de vida q vai de 0 a 1// maza //

        
        if(Input.GetButtonDown("itemA") && percVida < 1 ) //verifica se ele apertar a tecla (E) ele vai usar pocao de vida caso tenha
        {
            _GameController.usarPocao(0); //1: igual a pocao de cura
            audioController.tocarFx(audioController.fxHealthP, 1);
        }
        // representa 100% de vida
        
    }

    //=====================  SISTEMA DE LIFE OBSOLETA, ALTERADA PARA O SISTEMA ACIMA ======================///




    
  //=====================  SISTEMA DE MANA OBSOLETA, ALTERADA PARA O SISTEMA ACIMA ======================///   
void ControleBarraMana2()
        {

        percMana = (float) _GameController.manaAtual / (float) _GameController.manaMaxima; //calcula o percentual de vida q vai de 0 a 1// maza //


            
       
        if(Input.GetButtonDown("itemB") && percMana < 1)
        {
            audioController.tocarFx(audioController.fxHealthP, 1); // CHAMA AUDIO FX QUANDO USA MAGIA 
            _GameController.usarPocao(1); //1: igual a pocao de mana 
        }
         //representa 100% de vida
        
        
    }
     //=====================  SISTEMA DE MANA OBSOLETA, ALTERADA PARA O SISTEMA ACIMA ======================///   
    
    public void verificarClassePersonagem()
    {
        if(_GameController.idClasse[_GameController.idPersonagem] == 2) // se a classe for mago
        {
            sword.SetActive(false); // coloco a staff como imagem
            bow.SetActive(false);
            staff.SetActive(true); // coloco a staff como imagem
        }
         else if(_GameController.idClasse[_GameController.idPersonagem] == 1) // se a classe for paladin
        {
            iconFlechas.sprite = _GameController.iconFlecha[_GameController.idFlechaEquipada]; //eu disse que o hud vai receber o id da flecha do game controller
            painelFlechas.SetActive(true);
            sword.SetActive(false); // coloco a staff como imagem
            bow.SetActive(true);
            staff.SetActive(false); // coloco a staff como imagem
        }
        else if(_GameController.idClasse[_GameController.idPersonagem] == 1) // se a classe for paladin
        {
            sword.SetActive(true); // coloco a staff como imagem
            bow.SetActive(false);
            staff.SetActive(false); // coloco a staff como imagem
        }
    }
 
}
