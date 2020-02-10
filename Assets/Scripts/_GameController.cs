using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // para usar as interfaces da unity
using UnityEngine.SceneManagement; //mudar de cena
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO; // trabalhar com partes de arquivos 
using UnityStandardAssets.CrossPlatformInput;

public enum GameState // stados do jogo
{
    PAUSE,
    GAMEPLAY, 
    ITENS, 
    LOADGAME,

    MORTO
}
public class _GameController : MonoBehaviour
{
   
   



    [Header("Informações Player")]
    private     PainelItemInfo      itemInfo;
    private     audioController     audioController;
    private     inventario          inventario;
    public      GameState           currentState;
    private     Hud                 Hud;
    private     fade                fade;
    public      int                 idioma; //VOU USAR PRA SELECIONAR O IDIOMA DAS FALAS
    public      string[]            idiomaFolder;//NOME DA PASTA DO IDIOMA
    public      string[]            tiposDano;
    public      GameObject[]        fxDano; //array com os rits do personagem  
    
    public      GameObject          fxMorte; 

   
    private     playerScript        playerScript;
    private     statusEquipes       statusEquipes; //acesso ao status do equipe
    public      string              cenaAtual; // vai guardar a cena atual que o personagem se encontra

[Header("Informações Player")]

    public      GameObject[]        btnUpgrade; //array os botoes de upgrade ativo e desativo
    public      int                 idPersonagem;
    public      string[]            nomePersonagem; //nome que vai ser escolhido pela pessoa
    public      int[]               idClasse;
    public      int                 vidaMaxima;
    public      int                 vidaAtual;
    public      int                 manaMaxima;
    public      int                 manaAtual;
    public      int                 idArma, idArmaAtual;
    public      GameObject[]          iconeClasses;
    public      GameObject[]        trocaIconeFlechas;// vai trocar o icone das flechas que esta usando


    public      int[]               qtdPocoes; //0: cura 1: mana ~~ 
    public      string[]            spriteSheetName; //nome do personagem na pasta Resources


    public      GameObject[]        ArmaInicial;

    public      itemModelo[]        armaInicialPersonagem; //CRIEI COM O SCRIPT QUE N DEPENDE DE OBJETO PRA SER INSTANCIADO
    public      int                 idArmaInicial;

    [Header("Dados Flechas")]
    public      int                 idFlechaEquipada; //saber qual flecha esta usando
    public      Sprite[]            iconFlecha;  //- 0 flecha normal  1: flecha prata 2: flecha gold ICONE DA FLECHA NO HUD
    public      Sprite[]            imgFlecha; // img usada para trocar a cor da flecha no boneco quando vai atacar 
    public      int[]               qtdFlechas; //- 0 flecha normal  1: flecha prata 2: flecha gold
    public      float[]             velocidadeFlecha; //vai controlar a velocidade da flecha
    public      GameObject[]        FlechaPrefab;


[Header("Atributos Personagem")]
    public      int                 atrLevel;
     public      int                atrLevel2;
    public      int                 atrExperiencia;
    public      int                 atrSkillPoints;
    public      int                 atrForca;
    public      int                 atrVelocidade;
    public      int                 atrDefesa;
    public      int                 atrMagico;
    public      int                 atrCritico;
    public      int                 atrBenca;
    public      int                 atrNextLevel;
    public      int                 atrDiedLevel;
    public      int                 atrDiedExperiencia;
    public      int                 atrDiedXpLoss; 
    public      int                 ExpNecessaria; // xp total que precisa para upar de 1 lvl pro outro do 0% ao 100%
    public      int                 rateXP; // QUANDO E PARA UPAR PRIMEIRO LEVEL
    public      int                 experienciaParaEstarNoLevel;
    public      int                 experienciaGanhadaNoLevelAtual; // calcula toda experiencia que ele ganhou no level atual + o resto de xp caso ele upe e sobre algo
    public      int                 restoXp;
    public      int                 restoXMorte;


    [Header("DADOS GOLD E PEDRAS")]
    public      int                 safira;
    public      int                 ruby;
    public      int                 esmeralda;
    public      int                 diamante;
    public      int                 gold; //armazena a qt de ouro 

    public      GameObject          Objsafira;
    public      GameObject          ObjRuby;
    public      GameObject          ObjEsmeralda;
    public      GameObject          arrowA;
    public      GameObject          arrowB;
    public      GameObject          arrowC;

    public      Button          btnaprimorarDiamond;
    

[Header("PEDRAS PARA UP")] // ESTOU INSTANCIANDO O OBJETO PRA ATIVAR OU DESATIVAR DE ACORDO COM A PROFISSAO NA TELA DE UGPRADE

    public      GameObject          upgradeDiamante;
    public      GameObject          upgradeEsmeralda;
    public      GameObject          upgradeRuby;
    public      GameObject          upgradeSafira;
  
     public     GameObject[]         PedrasTransparentes;

    [Header("ATRIBUTOS DO PLAYER APRIMORADOS")]

    public      float               statusDanoCritico;  // aqui vai armazenar os dados depois de upgrades
    public      float               statusDanoMinimo; // aqui vai armazenar os dados depois de upgrades

    // SEMPRE QUE FOR EXIBIR ALGO NA TELA, ADICONE EM ATR E EM SEGUIDA VINCULE O OBJETO NO GAMECONTROLER DA INTERFACE E DEPOIS ALTERE EMBAXO
        //string nextLevel = atrNextLevel.ToString("N0");
        //statusNextLevel.text = nextLevel.Replace(",",".");
    // E NOISSS QUEIROZ
    
[Header("Tela de Upgrade")]
    public      TextMeshProUGUI     txtSkillPoints;
    public      TextMeshProUGUI     txtForcaAtual;
    public      TextMeshProUGUI     txtManaAtual;
    public      TextMeshProUGUI     txtDefesaAtual;
    public      TextMeshProUGUI     txtVelocidadeAtual;
    public      TextMeshProUGUI     txtCriticoAtual;
    public      TextMeshProUGUI     txtBencaAtual; 

    public      GameObject[]        trocarCorBarraUpgrade;



    [Header("TELA HUD SLOT")]
    public      GameObject[]        slotHudItensMagia;  // VOU GUARDAR TODOS OS BOTOES DE MAGIA E ITENS PRA CHAMAR NA HUD NO SLOT 1
    public      GameObject[]        slotHudItensMagia1;  // VOU GUARDAR TODOS OS BOTOES DE MAGIA E ITENS PRA CHAMAR NA HUD   NO SLOT 2
    public      GameObject[]        slotHudItensMagia2;  // VOU GUARDAR TODOS OS BOTOES DE MAGIA E ITENS PRA CHAMAR NA HUD  NO SLOT 3
    public      GameObject[]        slotHudItensMagia3;  // VOU GUARDAR TODOS OS BOTOES DE MAGIA E ITENS PRA CHAMAR NA HUD  NO SLOT 4
    
    public      Button[]             slotItensParaHud;// SE TIVER O ITEM ELE VAI SER CLICAVEL, SE NÃO TIVER, ELE NAO VAI SER
    private     int                 idItemSlot; // VAI RECEBER O ID DO ITEM PARA POR O ITEM CERTO DENTRO DO SLOT NA FUNCAO SLOT HUD
    private     int                 idSlot; // VOU SALVAR O ID DO SLOT PRA USAR NA FUNCAO SLOT HUD QUANDO FOR POR O ITEM NA TELA
    private     int                 idUsarItemNoSlot; // quando ele clicar na magia vou dizer qual slot q esta sendo usado pra ele deixar o botao false;

    [Header("TELA CONVERSA NPC")]
    public      GameObject[]              hudDesativar; // quando for falar com o npc vai destivar a hud




    [Header("Tela de MERCHANT")]
    public      Button[]        merchant; 
    public      GameObject      msgSucesso; // quando comprar com sucesso a msg 




    [Header("QTD MOEDAS E DIAMONDS")]
     public      TextMeshProUGUI     GoldTxt; // NA HUD
    public      TextMeshProUGUI     txtQtdGold; // NO ITENS
    public      TextMeshProUGUI     txtQtdDiamond;
    public      TextMeshProUGUI     txtQtdRuby;
    public      TextMeshProUGUI     txtQtdEsmeralda;
    public      TextMeshProUGUI     txtQtdSafira;

  
    [Header("ATK MAGIA ARROW PREF")]
    public          GameObject[]              danoPrefabMagico; // vou salvar a prafab aqui pra trabalhar com os atributos do ataque dela e jogar na status info do item
    public          GameObject[]              danoPrefabArrows;





    [Header("Tela de Status")]
  
    public      TextMeshProUGUI     statusSkillPoints;
    public      TextMeshProUGUI     statusLevel;
    public      TextMeshProUGUI     statusExperiencia;
    public      TextMeshProUGUI     statusForca;
    public      TextMeshProUGUI     statusMagia;
    public      TextMeshProUGUI     statusDefesa;
    public      TextMeshProUGUI     statusVelocidade;
    public      TextMeshProUGUI     statusCritico;
    public      TextMeshProUGUI     statusBenca; 
    public      TextMeshProUGUI     statusNextLevel;

    [Header("Tela de Morto")]
    public      TextMeshProUGUI     statusDiedExperiencia;
    public      TextMeshProUGUI     statusDiedLevel;
    public      TextMeshProUGUI     statusDiedXpLoss; 

    [Header("REWARDS XP")]
    public      int                 xpMonstro; // vai receber a experiencia do monstro quando ele morrer
    public      int             xpExperienciaQueEstaMenosExperienciaParaUpar; // aqui pega a xp necessaria do level - a xp do monstro 
   // public      int[]               porcLossXP; // quanto vai perder de xp em porcentagem lvl <20 : 10% lvl <30 : 8%  lvl <40 : 7% lvl <50 : 6% lvl 50> : 5%




 
[Header("Banco de dados de Armas")]
    public      string[]            nomeArma;
    public      Sprite[]            imgInventario;
    public      int[]               precoArmaVender;
    public      int[]               precoArmaComprar;
   
    public      Sprite[]            spriteArmas1;
    public      Sprite[]            spriteArmas2;
    public      Sprite[]            spriteArmas3; // estou salvando os 3 estagios de ataque das armas em um "banco de dados"
    public      Sprite[]            spriteArmas4; // estou salvando os 3 estagios de ataque das armas em um "banco de dados"
    public      int[]               idClasseArma;  //0 : machado martelo espada   1: arcos     2: staffs SEGUIR A LISTA DOS ITEM NA MESMA ORDEM
    public      int[]               danoMinArma; 
    public      int[]               danoMaxArma;
    public      int[]               tipoDanoArma;
    public      int[]               aprimoramentoArma;


    [Header("APRIMORAMENTO ARMA +10")]
    public int[] atrAprimoramentoArma;

   [Header("CONTROLE DE QUEST")]
    public      bool                quest1; // missao concluida

    [Header("PAINEIS")]  
    public      GameObject          painelPause;
    public      GameObject          painelStatusInfo; // o painel escuro de status dos item, personagem 
    public      GameObject          painelUpgrade; // o painel escuro de status dos item, personagem 
    public      GameObject          painelItens; //painel de itens
    public      GameObject          painelItemInfo;
    public      GameObject          painelOptions;
    public      GameObject          PainelDied;
    public      GameObject          PainelMerchant;
    public      Material            luz2D, padrao2D; //vai fazer controle de luz no inimigo dentro de cavernas etc 
    public      GameObject          painelDescarte;
    public      GameObject          painelSlotHud;



    
    [Header("1° ELEMENTO DE CADA PAINEL")]  
    //public Button                   firstPainelPause;
    //public Button                   firstPainelStatus;
    //public Button                   firstPainelItens;
    //public Button                   firstPainelUpgrade;
    //public Button                   firstPainelItemInfo;
    // public Button                  firstPainelDied;


    public List<string>             itensInventario;
 
    public  bool                    verificaMorte; // quando ele morrer vai cair em 1 for falando a xpatual do personagem e la vai ter esse for










    public int  expPosPerdaLevel;
    public int  tempNecessario; 
    void Start() 
    
    {
  
            

        if(atrLevel2 < 2 )
        {
            atrLevel2 = 1;
            ExpNecessaria = ((50 * (atrLevel2) * (atrLevel2) * (atrLevel2) - 150 * (atrLevel2) * (atrLevel2) + 400 * (atrLevel2)) / rateXP); // ele apos upar vai jogar a sobra na que falta para upar e assim subir nivel ;   
            
        }
        
        //print(Application.persistentDataPath);
        audioController     = FindObjectOfType(typeof(audioController)) as audioController;
        itemInfo            = FindObjectOfType(typeof(PainelItemInfo))  as PainelItemInfo;
        statusEquipes       = FindObjectOfType(typeof(statusEquipes)) as statusEquipes; 
        inventario          = FindObjectOfType(typeof(inventario)) as inventario;
        Hud                 = FindObjectOfType(typeof(Hud)) as Hud;
        playerScript        = FindObjectOfType(typeof(playerScript)) as playerScript;
        idPersonagem        = PlayerPrefs.GetInt("idPersonagem"); //vou pegar do titulo o id do personagem

        DontDestroyOnLoad(this.gameObject); // quando mudar de faze o objeto não é destruido


            if(idClasse[idPersonagem] == 0) //estou verificando se o personagem é paladino
       
       {
         
           iconeClasses[0].SetActive(true);
            iconeClasses[1].SetActive(false);
            iconeClasses[2].SetActive(false);

            Hud.sword.SetActive(true); // coloco a staff como imagem
            Hud.bow.SetActive(false);
            Hud.staff.SetActive(false); // coloco a staff como imagem
        }


         if(idClasse[idPersonagem] == 1) //estou verificando se o personagem é paladino
       {

            iconeClasses[0].SetActive(false);
            iconeClasses[1].SetActive(true);
            iconeClasses[2].SetActive(false);

            Hud.iconFlechas.sprite = iconFlecha[idFlechaEquipada]; //eu disse que o hud vai receber o id da flecha do game controller
            Hud.painelFlechas.SetActive(true);
            Hud.sword.SetActive(false); // coloco a staff como imagem
            Hud.bow.SetActive(true);
            Hud.staff.SetActive(false); // coloco a staff como imagem







        }

     
         if(idClasse[idPersonagem] == 2) //estou verificando se o personagem é paladino
       {


            iconeClasses[0].SetActive(false);
            iconeClasses[1].SetActive(false);
            iconeClasses[2].SetActive(true);

            Hud.sword.SetActive(false); // coloco a staff como imagem
            Hud.bow.SetActive(false);
            Hud.staff.SetActive(true); // coloco a staff como imagem
        }

        fade = FindObjectOfType(typeof(fade)) as fade;
        painelOptions.SetActive(false);
        painelPause.SetActive(false); //o jogo começa e a tela não estará em pause, ou seja o valor de pause será falso.
        painelItens.SetActive(false); // inicia em falso a informacao que o painel itens esta ativo
        painelItemInfo.SetActive(false); // inicia em falso a informacao que o painel itens esta ativo
        painelStatusInfo.SetActive(false); // inicio o jogo também com o status falso
        painelUpgrade.SetActive(false); // inicio o jogo também com o status falso

        Load(PlayerPrefs.GetString("slot")); // chamo o load game 
        
        //
        // ALGUNS CODGOS FORAM MOVIDOS  PARA A FUNCAO NEWGAME
        //
  

            btnUpgrade[0].SetActive(false);
            btnUpgrade[2].SetActive(false);
            btnUpgrade[4].SetActive(false);
            btnUpgrade[6].SetActive(false);
            btnUpgrade[8].SetActive(false);
            btnUpgrade[10].SetActive(false);

            btnUpgrade[1].SetActive(true);
            btnUpgrade[3].SetActive(true);
            btnUpgrade[5].SetActive(true);
            btnUpgrade[7].SetActive(true);
            btnUpgrade[9].SetActive(true);
            btnUpgrade[11].SetActive(true);
        
        
        VerificaMerchant();
        //slotHud(); // vai verificar se precisa por algum item na hud ou magia ou deixar em branco
   
    }

    // Update is called once per frame
    void Update()
    {
        

       // ========== ATRIBUICAO DE FORCA NA HUD DE UPGRADE   ===========//
        string f = atrForca.ToString("N0");
        txtForcaAtual.text = f.Replace(",",".");
        string m = atrMagico.ToString("N0");
        txtManaAtual.text = m.Replace(",",".");
        string d = atrDefesa.ToString("N0");
        txtDefesaAtual.text = d.Replace(",",".");
        string v = atrVelocidade.ToString("N0");
        txtVelocidadeAtual.text = v.Replace(",",".");
        string c = atrCritico.ToString("N0");
        txtCriticoAtual.text = c.Replace(",",".");

        string b = atrBenca.ToString("N0");
        txtBencaAtual.text = b.Replace(",",".");

        string s = atrSkillPoints.ToString("N0");
        txtSkillPoints.text = s.Replace(",",".");


       // ========== ATRIBUICAO DE FORCA NA HUD STATUS   ===========//


        string skillpoint = atrSkillPoints.ToString("N0");
        statusSkillPoints.text = skillpoint.Replace(",",".");

        string level = atrLevel2.ToString("N0");
        statusLevel.text = level.Replace(",",".");
        
        string xp = atrExperiencia.ToString("N0");
        statusExperiencia.text = xp.Replace(",",".");

        string forca = atrForca.ToString("N0");
        statusForca.text = forca.Replace(",",".");

        string magia = atrMagico.ToString("N0");
        statusMagia.text = magia.Replace(",",".");

        string defesa = atrDefesa.ToString("N0");
        statusDefesa.text = defesa.Replace(",",".");

        string velocidade = atrVelocidade.ToString("N0");
        statusVelocidade.text = velocidade.Replace(",",".");

        string critico = atrCritico.ToString("N0");
        statusCritico.text = critico.Replace(",",".");

        string benca = atrBenca.ToString("N0");
        statusBenca.text = benca.Replace(",",".");

        string nextLevel = atrNextLevel.ToString("N0");
        statusNextLevel.text = nextLevel.Replace(",",".");

        //===================== TELA MENU POTIONS E FLECHAS ========================//

        //string arrow       = qtdFlechas[0].ToString("N0");
        //statusArrow.text = critico.Replace(",",".");

        //string silverArrow = qtdFlechas[1].ToString("N0");
       // statusSilverArrow.text = critico.Replace(",",".");

        //string GoldenArrow = qtdFlechas[2].ToString("N0");
       // statusGoldenArrow.text = critico.Replace(",",".");

        //string healthPotion = qtdPocoes[0].ToString("N0");
        //statusHealthPotion.text = benca.Replace(",",".");

        //string manaPotion = qtdPocoes[1].ToString("N0");
       // statusManaPotion.text = nextLevel.Replace(",",".");




        //=============== ATRIBUICAO TELA DE MORTE ===================//

        string dieLevel = atrDiedLevel.ToString("N0");
        statusDiedLevel.text = dieLevel.Replace(",",".");

        string experienceDie = atrDiedXpLoss.ToString("N0");  // experiencia que perdeu quando morreu
        statusDiedXpLoss.text = experienceDie.Replace(",",".");

        string expecienceAtual = atrDiedExperiencia.ToString("N0"); // experiencia que possui depois da morte
        statusDiedExperiencia.text = expecienceAtual.Replace(",",".");

        
      
        if(Input.GetKeyDown(KeyCode.S))
        {
            print("salve game");
            Save(); 

        }
        if(currentState == GameState.GAMEPLAY)
        {
            if(playerScript == null){playerScript = FindObjectOfType(typeof(playerScript)) as playerScript;} //se n achar o script ele busca de forma manual
            
            
            string g = gold.ToString("N0");
            GoldTxt.text = g.Replace(",",".");


            validarArma();
            if(CrossPlatformInputManager.GetButtonDown("OpenMerchant")) //cancela o pause caso a aba itens n esteja ativa
                {
                    PainelMerchant.SetActive(true);
                    changeState(GameState.ITENS);

                }
            
            if(CrossPlatformInputManager.GetButtonDown("Cancel") && currentState != GameState.ITENS) //cancela o pause caso a aba itens n esteja ativa
                {
                    PauseGame();

                }
        }
    }

    public void VerificaMerchant()
    {
        int healthPotion    = 100; // caso 0
        int manaPotion      = 80; // caso 1
        int arrow           = 80; // caso 2
        int silverArrow     = 120; // caso 3
        int goldenArrow     = 180; // caso 4
        
                if(gold < healthPotion)
                {
                   merchant[0].interactable = false;
                }
             
                 if(gold < manaPotion)
                {
                    merchant[1].interactable = false;
                }

                 if(gold < arrow || idClasse[idPersonagem] !=1)
                {
                  merchant[2].interactable = false;
               
                }

                if(gold < silverArrow || idClasse[idPersonagem] !=1)
                {
                   merchant[3].interactable = false;
                  
                }
            
          
                if(gold < goldenArrow || idClasse[idPersonagem] !=1)
                {
                  merchant[4].interactable = false;
                  
                }

        }


    public void compraMerchant(int id)
    {

        switch(id)
        {
            case 0:
                qtdPocoes[0] +=10;
                slotItensParaHud[0].interactable = true;
                gold -=100;
                
            break;

            case 1:
                qtdPocoes[1] +=10;
                slotItensParaHud[1].interactable = true;
                gold -=80;
            break;

            case 2:
                qtdFlechas[0] +=100;
                gold -=80;
            break;

            case 3:
                qtdFlechas[1] +=100;
                gold -=120;
            break;

            case 4:
                qtdFlechas[2] +=100;
                gold -=160;
            break;

        }

    }

   public void validarArma()
    {
        if(idClasseArma[idArma] != idClasse[idPersonagem])
        {
            idArma = idArmaInicial; // verifico se a arma é da classe do personagem
            
           
        } 
    }



    public void PauseGame()
        {
            bool    pauseState = painelPause.activeSelf;
            pauseState = !pauseState;
            painelPause.SetActive(!painelPause.activeSelf);

            switch(pauseState)
            {
                case true:
                audioController.tocarFx(audioController.fxClick, 1); // QUANDO ELE APERTAR O BOTAO DE MENU ELE VAI FAZER O SOM DE BARULHO CASO SEJA PELO ESC                changeState(GameState.PAUSE); //chama a funcao changeState e ai atribui ao estado do jogo o PAUSE
                //audioController.trocarMusica(audioController.musicaTitulo, "", false); // O FALSE SIGNIFICA Q N ESTOU TROCANDO DE CENA
                //firstPainelPause.Select(); //para deixar o botao "fechar" selecionado como primeira opcao
                changeState(GameState.ITENS);
                break;

                case false:
                changeState(GameState.GAMEPLAY);
                //audioController.trocarMusica(audioController.musicaFase1, "", false); // O FALSE SIGNIFICA Q N ESTOU TROCANDO DE CENA
                break;
            }
            
        }
    public void changeState(GameState newState)
    {
        currentState = newState; //pega o estado da maquina para jogar ou nao
        switch(newState)
        {
            case GameState.GAMEPLAY:
            Time.timeScale = 1; // se ele voltar pro jogo, o tempo de jogo  do pause
            break;
            case GameState.PAUSE:
            Time.timeScale = 0; //eu paro o tempo do jogo pra nada poder ser executado
            break;
            case GameState.ITENS:
            Time.timeScale = 0; //eu paro o tempo do jogo pra nada poder ser executado
            break;

            case GameState.MORTO:
            Time.timeScale = 1;
            break;
        }
    }




    public void usarItemArma(int idArma)
    {
        playerScript.trocarArma(idArma);
        
    }

    public void openItemInfo()
    {
        painelItemInfo.SetActive(true);
        //firstPainelItemInfo.Select();
    }

     public void openMsgSucesso()
    {
        msgSucesso.SetActive(true);
    }


   

      public void closepainelSlotHud()
    {
        painelSlotHud.SetActive(false);
    }

    public void openPainelDescarte()
    {
        painelDescarte.SetActive(true);
    }

    public void CloseCancelJogarFora()
    {
        painelDescarte.SetActive(false);
 
    }
    

    public void closeMerchant()
    {
        PainelMerchant.SetActive(false);
        changeState(GameState.GAMEPLAY);
    }

    
    public void closeDied()
    {
        string nomeCena = "titulo";

        audioController.trocarMusica(audioController.musicaTitulo, nomeCena, true);
        PainelDied.SetActive(false);
        painelOptions.SetActive(false);
        painelPause.SetActive(false);
        painelStatusInfo.SetActive(false);
        painelItens.SetActive(false);
        painelUpgrade.SetActive(false);
        //firstPainelItemInfo.Select();
        DestroyObject(this.gameObject);
        atrDiedXpLoss = 0;// DEPOIS QUE A XP LOSS FOR USADA EM CIMA PRA MOSTRAR QUANTO PERDEU, EU ZERO ELA PARA QUE NO XPMONSTRO.ExpNecessaria NAO SEJA PREJUDICADA PARA FAZER O CALCULO
                       
    }

  

 



    public void Morreu()
    {
       //var Randomint = UnityEngine.Random.Range(0,15); // pega random de 0 a 2 pq o limite fica fora
        vidaAtual = 0;
        var Randomint = UnityEngine.Random.Range(0,11); // vai pegar um random de 0 a 2 e vai jogar na variavel debaixo
        audioController.trocarMusica(audioController.musicaTriste[Randomint], "", false);
        PainelDied.SetActive(true);
        painelOptions.SetActive(false);
        painelStatusInfo.SetActive(false);
        painelPause.SetActive(false);
        painelItens.SetActive(false);
        painelUpgrade.SetActive(false);
        //firstPainelItemInfo.Select();
       

    }

    public void voltarGamePlay()
    {
        painelItens.SetActive(false);
        painelPause.SetActive(false);
        painelItemInfo.SetActive(false);
        changeState(GameState.GAMEPLAY);  //troquei o estado do jogo de pause para play, para o cenário se mover  
    }

    public void excluirItem(int idSlot)
    {
        inventario.itemInventario.RemoveAt(idSlot); //uso o removeAt para passar por referencia o que quero excluir
        inventario.carregarInventario(); //recarrego o inventario com a funcao 
        painelItemInfo.SetActive(false); //desativo o painel de informaçoes do item já que eu exclui ele
        //firstPainelItens.Select();  // seleciono novamente o painel de tras para mover com o teclado
        painelDescarte.SetActive(false);
    }
    public void aprimorarArma(int idArma) // É O UPGRADE DA ARMA 
    {
        
    
        int apTemp = aprimoramentoArma[idArma];
        if (apTemp < 10 )
        {
            apTemp +=1;
            aprimoramentoArma[idArma] = apTemp;
            //itemInfo.carregarInfoItem();
          
        }
         
    }

    public void MostrarStatusItens() // QUANDO CHAMAR O ITEM ELE VAI EXECUTAR ESSE SCRIPT
    {
    
        //tempInfoArma.tipoDeDano = _GameController.tipoDanoArma[idArma];
        float tempAjusteAprimoramento = (aprimoramentoArma[idArma] * 9)/10; // AQUI CALCULA O APRIMORAMENTO DA ARMA + PORCENTAGEM PARA POR NO DANO MINIMO QUE AUMENTA DE ACORDO QUE MELHORA A HABILIDADE EM UTILIZAR AQUELE TIPO DE ARMA
     
       // statusDanoCritico =   (danoMaxArma[idArma] * atrCritico) /10; //ele calcula o ataque da arma + quanto o personagem tem de critico
        
        
        statusDanoMinimo  =   danoMinArma[idArma] + tempAjusteAprimoramento;
       
        //float tempAjusteDano = danoArma * (ControleDanoInimigo.ajusteDano[tipoDano] / 100); // CASO O INIMIGO TENHA FRAQUESA

        //print(tempAjusteAprimoramento+ "dano aprimorado");
        //print(statusDanoCritico + "dano critico");
        //print( statusDanoMinimo + "dano minimo" + idArma);
    }
     public void verificaClasse()
    {
      int idParaVerificarSePodeUsarArma =  idPersonagem;
      
    }

    public  void    swapItens(int idSlot)
    {
        GameObject          t1 = inventario.itemInventario[0];
        GameObject          t2 = inventario.itemInventario[idSlot];
        // ESSES CONJUNTOS DE CODGO É PARA TROCAR O ITEM QUE ELE QUER EQUIPAR PELO PRIMEIRO ITEM, O PRIMEIRO ITEM SEMPRE SERA O QUE ELE ESTA USANDO
        inventario.itemInventario[0] = t2;
        inventario.itemInventario[idSlot] = t1; 
        voltarGamePlay();
    }

    public void ColetarItens(GameObject objetoColetado) //coletar itens é uma funcao para pegar os item que estiver no chao e quiser coletar
    {
        inventario.itemInventario.Add(objetoColetado);
    }

    public void usarPocao(int idPocao) // vou poder ter muitas pocoes depois basta implementar
    {
        if(qtdPocoes[idPocao] > 0)
        {
            //qtdPocoes[idPocao] -=1; //verifico se tem a pocao, se tiver ele vai usar a pocao referente ao que precisa ser usado
            
       
            switch(idPocao)
            {
                case 0: //pocao de cura
                    audioController.tocarFx(audioController.fxHealthP, 1);
                    
                    switch(idClasse[idPersonagem])
                    {
                        case 0: // KNIGHT
                            vidaAtual +=15;
                            if (vidaAtual > vidaMaxima) 
                            {
                                vidaAtual = vidaMaxima;
                            } 
                        break;

                        case 1: // PALADINO
                            vidaAtual +=10;
                            if (vidaAtual > vidaMaxima) 
                            {
                                vidaAtual = vidaMaxima;
                            } 
                        break;

                        case 2: // MAGO
                            vidaAtual +=5;
                            if (vidaAtual > vidaMaxima) 
                            {
                                vidaAtual = vidaMaxima;
                            } 
                        break;
                    }
                    
                
                break;

                case 1: // pocao de mana
                    audioController.tocarFx(audioController.fxManaP, 1);
                     
                        switch(idClasse[idPersonagem])
                    {
                        case 0: // KNIGHT
                            manaAtual +=5;
                            if (manaAtual > manaMaxima) 
                            {
                                manaAtual = manaMaxima;
                            } 
                        break;

                        case 1: // PALADINO
                            manaAtual +=8;
                            if (manaAtual > manaMaxima) 
                            {
                                manaAtual = manaMaxima;
                            } 
                        break;

                        case 2: // MAGO
                            manaAtual +=15;
                            if (manaAtual > manaMaxima) 
                            {
                                manaAtual = manaMaxima;
                            } 
                            
                        break;
                    }
                    
                break;
            }
        }
    }


    public void adicionarItemHud(int idSlotParametro) // vou adicionar item a tela
    {
     idSlot = idSlotParametro;   
            painelSlotHud.SetActive(true); // peguei o id do slot e chamei a funcao pra abrir a tela
            if(qtdPocoes[0] <1)
            {slotItensParaHud[0].interactable = false;} 
            if(qtdPocoes[1] <1)
            {slotItensParaHud[1].interactable = false;} 
    }

        public void idItemHud(int idItem) // vou jogar o id do item que peguei dentro do id Item SLot
    {
        idItemSlot = idItem;
        painelSlotHud.SetActive(false);
        if(idSlot == 0)
        {
                 switch(idItemSlot)
           {
               case 0:
                    slotHudItensMagia[idItemSlot].SetActive(true);
               break;

               case 1:
                  slotHudItensMagia[idItemSlot].SetActive(true); 
               break;
           }
          
        }
        if(idSlot == 1)
        {
                 switch(idItemSlot)
           {
               case 0:
                    slotHudItensMagia1[idItemSlot].SetActive(true);
               break;

               case 1:
                  slotHudItensMagia1[idItemSlot].SetActive(true); 
               break;
           }
         
        }
         if(idSlot == 2)
        {
                 switch(idItemSlot)
           {
               case 0:
                    slotHudItensMagia2[idItemSlot].SetActive(true);
               break;

               case 1:
                  slotHudItensMagia2[idItemSlot].SetActive(true); 
               break;
           }
      
        }
         if(idSlot == 3)
        {
                 switch(idItemSlot)
           {
               case 0:
                    slotHudItensMagia3[idItemSlot].SetActive(true);
               break;

               case 1:
                  slotHudItensMagia3[idItemSlot].SetActive(true); 
               break;
              
           }
  
        }
   
        
    }



    public void UsarItemsLotHud(int id) // COLOCAR MAGIA OU ITEM NOS SLOTS DA HUD
    {
    
        idUsarItemNoSlot = id; // quando eu usar um item ou magia, vou pegar de qual slot e pra poder sumir o item caso seja 0 o valor

       

                switch(idUsarItemNoSlot)
                {
                case 0:
                if(qtdPocoes[0] <2) // como ele vai executar pra depois verificar ele vai usar potion pois ainda tem 1
                    {
                       
                                slotHudItensMagia[0].SetActive(false);
                                slotHudItensMagia1[0].SetActive(false);
                                slotHudItensMagia2[0].SetActive(false);
                                slotHudItensMagia3[0].SetActive(false);
                                if(qtdPocoes[0] < 1)
                                {
                                    qtdPocoes[0] = 0;   
                                }  
                                qtdPocoes[0] = 0;
                    }
                    if(qtdPocoes[0] >= 1){
                        qtdPocoes[0] -=1;

                    }

                break;

                case 1:
                    if(qtdPocoes[1] <2) // como ele vai executar pra depois verificar ele vai usar potion pois ainda tem 1
                    {
                       
                                slotHudItensMagia[1].SetActive(false);
                                slotHudItensMagia1[1].SetActive(false);
                                slotHudItensMagia2[1].SetActive(false);
                                slotHudItensMagia3[1].SetActive(false);
                                if(qtdPocoes[1] < 1)
                                {
                                    qtdPocoes[1] = 0;   
                                }  
                                qtdPocoes[1] = 0;
                    }
                    if(qtdPocoes[1] >= 1){
                        qtdPocoes[1] -=1;
                    }
                        
                break;

                }
                
            



          
        

    }











    public  string  textoFormatado(string frase)
    {
        // [negrito]                <b>
        // [fim]                    </b>  
        // [italico]                <i>
        // [fim]                    </i> 
        // [vermelho200]            <color=#EF9A9A>
        // [vermelho600]            <color=#F44336>
        // [vermelho900]            <color=#B71C1C>
        // [roxo200]                <color=#CE93D8>
        // [roxo600]                <color=#8E24AA>
        // [roxo900]                <color=#4A148C>
        // [azul200]                <color=#90CAF9>
        // [azul600]                <color=#1E88E5>
        // [azul900]                <color=#0D47A1>
        // [verde200]               <color=#80CBC4>
        // [verde600]               <color=#00897B>
        // [verde900]               <color=#004D40>
        // [yellow200]              <color=#FFFF00>
        // [yellow500]              <color=#FFEB3B>
        // [yellow600]              <color=#FDD835>
        // [yellow600]              <color=#FDD835>

        
        string  tempCor = frase;

        tempCor = tempCor.Replace("[yellow]", "<color=#ffff00ff>"); //usado para colorir os textos do xml
        tempCor = tempCor.Replace("[fimyellow]", "</color>"); //usado para fechar o codgo de colocar cor nas conversas xml
        tempCor = tempCor.Replace("[yellow]", "<color=#ffff00ff>"); //usado para colorir os textos do xml
        tempCor = tempCor.Replace("[fimyellow]", "</color>"); //usado para fechar o codgo de colocar cor nas conversas xml

        tempCor = tempCor.Replace("[red]", "<color=#EF9A9A>"); //usado para colorir os textos do xml
        tempCor = tempCor.Replace("[fimred]", "</color>"); //usado para fechar o codgo de colocar cor nas conversas xml

        tempCor = tempCor.Replace("[green]", "<color=#00897B>"); //usado para colorir os textos do xml
        tempCor = tempCor.Replace("[fimgreen]", "</color>"); //usado para fechar o codgo de colocar cor nas conversas xml
        

        tempCor = tempCor.Replace("[blue]", "<color=#1E88E5>"); //usado para colorir os textos do xml
        tempCor = tempCor.Replace("[fimblue]", "</color>"); //usado para fechar o codgo de colocar cor nas conversas xml

        

        tempCor = tempCor.Replace("[negrito]", "<b>"); //utilizado pra escrever em negrito nos textos
        tempCor = tempCor.Replace("[fimnegrito]", "</b>"); //usado para fechar o comando de escrever negrito nos textos xml

        tempCor = tempCor.Replace("[italico]", "<i>"); //utilizado pra escrever em negrito nos textos
        tempCor = tempCor.Replace("[fimitalico]", "</i>"); //usado para fechar o comando de escrever negrito nos textos xml
        return tempCor; // estou retornando o valor do temp
    }   


    public void calcXpNecessaria()
    {
        ExpNecessaria = ((50 * (atrLevel2) * (atrLevel2) * (atrLevel2) - 150 * (atrLevel2) * (atrLevel2) + 400 * (atrLevel2)) / rateXP); // ele apos upar vai jogar a sobra na que falta para upar e assim subir nivel ;   
       
    }
      public void calcularExperiencia()
    {
        calcXpNecessaria();
        atrExperiencia += xpMonstro;
 
          experienciaGanhadaNoLevelAtual +=xpMonstro;
        if(experienciaGanhadaNoLevelAtual < ExpNecessaria)
        {
                atrNextLevel -=xpMonstro;
        }
        else if (experienciaGanhadaNoLevelAtual >= ExpNecessaria)
        {
            restoXp = experienciaGanhadaNoLevelAtual - ExpNecessaria;
            atrLevel2 +=1;
            atrSkillPoints +=1; // a skill points sobe +1
            audioController.tocarFx(audioController.levelUp[0], 1);
            calcXpNecessaria();
            experienciaParaEstarNoLevel += ExpNecessaria; // sempre que pegar level ele vai somar a experiencia total para upar o level atual + a experiencia total do proximo level, para saber sempre o maximo de experiencia q precisa sempre acumulando  a experiencia do personagem
            atrNextLevel = ExpNecessaria - restoXp;
            experienciaGanhadaNoLevelAtual =restoXp; 
            restoXp = 0;
        }
        
    }
   



    public void Save()
    {
        string nomeArquivoSave      =   PlayerPrefs.GetString("slot");
        BinaryFormatter     bf      =   new BinaryFormatter();
        FileStream          file    =   File.Create(Application.persistentDataPath+"/"+ nomeArquivoSave);
        PlayerData          data    =   new PlayerData();

        data.idioma                         =   idioma;
        data.idPersonagem                   =   idPersonagem;
        data.gold                           =   gold;
        data.idArma                         =   idArma;
        data.idFlechaEquipada               =   idFlechaEquipada;
        data.qtdFlechas                     =   qtdFlechas;
        data.qtdPocoes                      =   qtdPocoes;
        data.aprimoramentoArma              =   aprimoramentoArma;
        data.idArma                         =   idArma;
        data.idArmaAtual                    =   idArma;
        data.manaAtual                      =   manaAtual;
        data.vidaAtual                      =   vidaAtual;
        data.cenaAtual                      =   cenaAtual;
        data.atrLevel2                      =   atrLevel2;
        data.atrExperiencia                 =   atrExperiencia;
        data.atrSkillPoints                 =   atrSkillPoints;
        data.atrForca                       =   atrForca;
        data.atrVelocidade                  =   atrVelocidade;
        data.atrDefesa                      =   atrDefesa;
        data.atrMagico                      =   atrMagico;
        data.atrCritico                     =   atrCritico;
        data.atrBenca                       =   atrBenca;
        data.atrNextLevel                   =   atrNextLevel;
        data.atrDiedLevel                   =   atrDiedLevel;
        data.atrDiedExperiencia             =   atrDiedExperiencia;
        data.atrDiedXpLoss                  =   atrDiedXpLoss; 
        data.experienciaGanhadaNoLevelAtual =   experienciaGanhadaNoLevelAtual;
        data.restoXp                        =   restoXp;
        data.ExpNecessaria                  =   ExpNecessaria;
        data.experienciaParaEstarNoLevel    =   experienciaParaEstarNoLevel;
        data.expPosPerdaLevel               =   expPosPerdaLevel;
        data.tempNecessario                 =   tempNecessario;
        data.verificaMorte                  =   verificaMorte;
        data.diamante                       =   diamante;
        data.ruby                           =   ruby;
        data.esmeralda                      =   esmeralda;
        data.safira                         =   safira;
    
        
        if(itensInventario.Count !=0){ itensInventario.Clear();}
        foreach(GameObject i in inventario.itemInventario)
        {
            itensInventario.Add(i.name);
        }
        data.itensInventario = itensInventario;

        bf.Serialize(file, data);
        file.Close();

    }

    public void Load(string slot)
    {
        
        if(File.Exists(Application.persistentDataPath + "/" + slot))
        {
            BinaryFormatter bf          =   new BinaryFormatter();
            FileStream      file        =   File.Open(Application.persistentDataPath+"/"+ slot, FileMode.Open);

            PlayerData      data        =   (PlayerData)bf.Deserialize(file);
            file.Close();

            


            idioma                      =   data.idioma;
            gold                        =   data.gold;
            idPersonagem                =   data.idPersonagem;
            idFlechaEquipada            =   data.idFlechaEquipada;
            qtdFlechas                  =   data.qtdFlechas;
            itensInventario             =   data.itensInventario;
            aprimoramentoArma           =   data.aprimoramentoArma;
            qtdPocoes                   =   data.qtdPocoes;
            idArma                      =   data.idArma;
            idArmaAtual                 =   data.idArma;
            idArmaInicial               =   data.idArma;
            manaAtual                   =   data.manaAtual;
            vidaAtual                   =   data.vidaAtual;
            cenaAtual                   =   data.cenaAtual;
            atrLevel2                   =   data.atrLevel2;
            atrExperiencia              =   data.atrExperiencia;
            atrSkillPoints              =   data.atrSkillPoints;
            atrForca                    =   data.atrForca;
            atrVelocidade               =   data.atrVelocidade;
            atrDefesa                   =   data.atrDefesa;
            atrMagico                   =   data.atrMagico;
            atrCritico                  =   data.atrCritico;
            atrBenca                    =   data.atrBenca;
            atrNextLevel                =   data.atrNextLevel;
            atrDiedLevel                =   data.atrDiedLevel;
            atrDiedExperiencia          =   data.atrDiedExperiencia;
            atrDiedXpLoss               =   data.atrDiedXpLoss; 
            restoXp                     =   data.restoXp;
            ExpNecessaria               =   data.ExpNecessaria;
            experienciaParaEstarNoLevel =   data.experienciaParaEstarNoLevel;
            experienciaGanhadaNoLevelAtual = data.experienciaGanhadaNoLevelAtual;
            expPosPerdaLevel            =   data.expPosPerdaLevel;
            tempNecessario              =   data.tempNecessario;
            verificaMorte               =   data.verificaMorte; 
            diamante                    =   data.diamante;
            ruby                        =   data.ruby;
            esmeralda                   =   data.esmeralda;
            safira                      =   data.safira;
           


           
            
            
            inventario.itemInventario.Clear();

            foreach(string i in itensInventario)
            {
               
                inventario.itemInventario.Add(Resources.Load<GameObject>("Armas/"+i)); // ELE VAI PROCURAR TODOS ARQUIVOS COM O MESMO NOME NA PASTA ARMA E ADICIONAR
            }

             file.Close();    
             // QUANDO ELE FIZER O LOAD GAME VAI CARREGAR ESSAS CONFIGURAÇÕES NO INICIO 
            inventario.itemInventario.Add(ArmaInicial[idPersonagem]); //QUANDO FAZ LOAD TEM Q LEVAR JUNTO A ARMA INICIAL PRO INVENTARIO
            GameObject tempArma = Instantiate(ArmaInicial[idPersonagem]);
            inventario.itensCarregados.Add(tempArma);

          
            
            Hud.verificarClassePersonagem();
            changeState(GameState.GAMEPLAY);
            // SceneManager.LoadScene("cena1"); // chamo a primeira fase assim que faco o load 

            string nomeCena = "cena1"; // FUTURAMENTE O NOME DA CENA VAI VIM DO SAVE E NÃO DAQUI

            var Randomint = UnityEngine.Random.Range(0,5); // vai pegar um random de 0 a 2 e vai jogar na variavel debaixo
            audioController.trocarMusica(audioController.musicaFase[Randomint], nomeCena, true); // O TRUE ME PERGUNTA SE E PARA TROCAR DE CENA
        }   
        else
        {
            newGame();
        }
    }

    void newGame()
    {   
        cenaAtual = "cena1"; // recebe cena1 como a primeira cena 
        idPersonagem = PlayerPrefs.GetInt("idPersonagem"); //peguei o id do personagem para chamar a arma
        gold           = 500;
        atrLevel2      = 1;
        idArma         = armaInicialPersonagem[idPersonagem].idArma;
        calcXpNecessaria();
        atrNextLevel   = ExpNecessaria;
        experienciaParaEstarNoLevel = 12;
        atrSkillPoints = 50;
        diamante = 5;

  

   
        
      switch(idPersonagem)
        {

        case 1:
            qtdPocoes[0]        = 10;
            qtdPocoes[1]        = 3;
            ruby                = 3;
            
    
   
        break;
        case 5:
            qtdPocoes[0]        = 10;
            qtdPocoes[1]        = 5;
            idFlechaEquipada    = 0;
            qtdFlechas[0]       = 50;
            qtdFlechas[1]       = 25;
            qtdFlechas[2]       = 15;
            esmeralda           = 3;

        break;

        case 9:
            qtdPocoes[0]        = 5;
            qtdPocoes[1]        = 10;
            safira              = 3;
            
        break;
       
        }
       

        //inventario.itemInventario.Add(ArmaInicial[idPersonagem]); // quando começar o jogo ele vai buscar qual é a arma inicial de cada personagem para colocar como primeiro item
        //GameObject tempArma = Instantiate(ArmaInicial[idPersonagem]); //instanciei a arma para poder usar ela na linha debaixo
        //inventario.itensCarregados.Add(tempArma); //agora joguei a arma carregada no item carregado 
        //idArmaInicial = tempArma.GetComponent<item>().idItem; // peguei o id da arma que esta no componente tempArma para jogar
        //vidaAtual = vidaMaxima; //iniciei vida e mana com o valor total no inicio do jogo
        //manaAtual = manaMaxima; 
        Save();
        Load(PlayerPrefs.GetString("slot"));
    }


    public  void cenaSalva()
    {
        
        audioController.trocarMusica(audioController.musicaFase1, cenaAtual, true); // O TRUE ME PERGUNTA SE E PARA TROCAR DE CENA
      
    }
    public void mostrarBotoesUpgrade()
    {
        switch (idClasse[idPersonagem])
        {
            case 0: 
            trocarCorBarraUpgrade[0].SetActive(true);
            trocarCorBarraUpgrade[1].SetActive(true);
            trocarCorBarraUpgrade[2].SetActive(true);
            trocarCorBarraUpgrade[3].SetActive(true);
            trocarCorBarraUpgrade[4].SetActive(true);
            trocarCorBarraUpgrade[5].SetActive(true);


            trocarCorBarraUpgrade[6].SetActive(false);
            trocarCorBarraUpgrade[7].SetActive(false);
            trocarCorBarraUpgrade[8].SetActive(false);
            trocarCorBarraUpgrade[9].SetActive(false);
            trocarCorBarraUpgrade[10].SetActive(false);
            trocarCorBarraUpgrade[11].SetActive(false);

            trocarCorBarraUpgrade[12].SetActive(false);
            trocarCorBarraUpgrade[13].SetActive(false);
            trocarCorBarraUpgrade[14].SetActive(false);
            trocarCorBarraUpgrade[15].SetActive(false);
            trocarCorBarraUpgrade[16].SetActive(false);
            trocarCorBarraUpgrade[17].SetActive(false);
            break;

            case 1: 
            trocarCorBarraUpgrade[6].SetActive(true);
            trocarCorBarraUpgrade[7].SetActive(true);
            trocarCorBarraUpgrade[8].SetActive(true);
            trocarCorBarraUpgrade[9].SetActive(true);
            trocarCorBarraUpgrade[10].SetActive(true);
            trocarCorBarraUpgrade[11].SetActive(true);


            trocarCorBarraUpgrade[0].SetActive(false);
            trocarCorBarraUpgrade[1].SetActive(false);
            trocarCorBarraUpgrade[2].SetActive(false);
            trocarCorBarraUpgrade[3].SetActive(false);
            trocarCorBarraUpgrade[4].SetActive(false);
            trocarCorBarraUpgrade[5].SetActive(false);

            trocarCorBarraUpgrade[12].SetActive(false);
            trocarCorBarraUpgrade[13].SetActive(false);
            trocarCorBarraUpgrade[14].SetActive(false);
            trocarCorBarraUpgrade[15].SetActive(false);
            trocarCorBarraUpgrade[16].SetActive(false);
            trocarCorBarraUpgrade[17].SetActive(false);
            break;

            case 2: 
            trocarCorBarraUpgrade[12].SetActive(true);
            trocarCorBarraUpgrade[13].SetActive(true);
            trocarCorBarraUpgrade[14].SetActive(true);
            trocarCorBarraUpgrade[15].SetActive(true);
            trocarCorBarraUpgrade[16].SetActive(true);
            trocarCorBarraUpgrade[17].SetActive(true);


            trocarCorBarraUpgrade[0].SetActive(false);
            trocarCorBarraUpgrade[1].SetActive(false);
            trocarCorBarraUpgrade[2].SetActive(false);
            trocarCorBarraUpgrade[3].SetActive(false);
            trocarCorBarraUpgrade[4].SetActive(false);
            trocarCorBarraUpgrade[5].SetActive(false);

            trocarCorBarraUpgrade[6].SetActive(false);
            trocarCorBarraUpgrade[7].SetActive(false);
            trocarCorBarraUpgrade[8].SetActive(false);
            trocarCorBarraUpgrade[9].SetActive(false);
            trocarCorBarraUpgrade[10].SetActive(false);
            trocarCorBarraUpgrade[11].SetActive(false);
            break;

        }





        if(atrSkillPoints > 0)
            {
            btnUpgrade[0].SetActive(true);
            btnUpgrade[2].SetActive(true);
            btnUpgrade[4].SetActive(true);
            btnUpgrade[6].SetActive(true);
            btnUpgrade[8].SetActive(true);
            btnUpgrade[10].SetActive(true);

            btnUpgrade[1].SetActive(false);
            btnUpgrade[3].SetActive(false);
            btnUpgrade[5].SetActive(false);
            btnUpgrade[7].SetActive(false);
            btnUpgrade[9].SetActive(false);
            btnUpgrade[11].SetActive(false);
            }
        else 
            {
                btnUpgrade[0].SetActive(false);
                btnUpgrade[2].SetActive(false);
                btnUpgrade[4].SetActive(false);
                btnUpgrade[6].SetActive(false);
                btnUpgrade[8].SetActive(false);
                btnUpgrade[10].SetActive(false);

                btnUpgrade[1].SetActive(true);
                btnUpgrade[3].SetActive(true);
                btnUpgrade[5].SetActive(true);
                btnUpgrade[7].SetActive(true);
                btnUpgrade[9].SetActive(true);
                btnUpgrade[11].SetActive(true);
                    
            }
    }
    public   void UpgradeArma(int valor)
        {
                switch(valor)
                {
                    case 1:
                        atrForca +=1;
                        atrSkillPoints -=1;

                    break;

                    case 2:
                    atrMagico +=1;
                    atrSkillPoints -=1;
                    break;

                    case 3:
                    atrDefesa +=1;
                    atrSkillPoints -=1;
                    break;

                    case 4:
                    if(atrVelocidade < 10)
                    {
                        atrVelocidade +=1;
                        atrSkillPoints -=1;
                        playerScript.speed +=(atrVelocidade * 10)/100;
                        print(atrVelocidade);
                    }
                    else
                    {
                        btnUpgrade[6].SetActive(false);
                        btnUpgrade[7].SetActive(true);
                    }
                    break;

                    case 5:
                    atrCritico +=1;
                    atrSkillPoints -=1;
                    break;

                    case 6:

                    if(atrBenca < 10)
                    {
                        atrBenca +=1;
                        atrSkillPoints -=1;
                    }
                    else
                    {
                        btnUpgrade[10].SetActive(false);
                        btnUpgrade[11].SetActive(true);
                    }
                    
                    break;
                }

             



        }


   public void btnItensDown()
    {
        painelPause.SetActive(true);
        painelItens.SetActive(true);
        //firstPainelItens.Select(); //para deixar o botao "fechar" selecionado como primeira opcao
        inventario.carregarInventario();
        changeState(GameState.ITENS);
        
    }
    public void closeItemInfo()
    {
        painelItemInfo.SetActive(false); 
    }

    public void Click()
    {   
        audioController.tocarFx(audioController.fxClick, 1); // FAZ O SOM DO CLICK NOS ITEM
    }
     public void upgradeFx()
    {
        audioController.tocarFx(audioController.fxSons[4], 1); // FAZ O SOM DO CLICK NOS ITEM
    }
     public void healPotionFx()
    {
        audioController.tocarFx(audioController.fxSons[2], 1); // FAZ O SOM DO CLICK NOS ITEM
    }
    public void manaPotionFx()
    {
        audioController.tocarFx(audioController.fxSons[3], 1); // FAZ O SOM DO CLICK NOS ITEM
    }
    public void Descartar()
    {
        audioController.tocarFx(audioController.fxSons[1], 1); // FAZ O SOM DO CLICK NOS ITEM
    }

      public void audioCompra()
    {   
        audioController.tocarFx(audioController.fxSons[0], 1); // FAZ O SOM DO CLICK NOS ITEM
    }

    [Serializable]
     class PlayerData
    {
        public  int             idioma;
        public  int             gold;
        public  int             idPersonagem;
        public  int             idArma;
        public  int             idArmaAtual;

        public  int             idFlechaEquipada;
        public  int[]           qtdFlechas;
        public  int[]           qtdPocoes;
        
        public  List<string>    itensInventario;
        public  int[]           aprimoramentoArma;

        public  int             vidaAtual;
        public  int             manaAtual;
        public  string          cenaAtual;
           

    [Header("LEVEL DO PERSONAGEM")]
        public      int                 atrLevel2;

        public      int                 atrExperiencia;
        public      int                 atrSkillPoints;
        public      int                 atrForca;
        public      int                 atrVelocidade;
        public      int                 atrDefesa;
        public      int                 atrMagico;
        public      int                 atrCritico;
        public      int                 atrBenca;
        public      int                 atrNextLevel;
        public      int                 atrDiedLevel;
        public      int                 atrDiedExperiencia;
        public      int                 atrDiedXpLoss; 
    
        public      int                 experienciaParaEstarNoLevel;
        public      int                 experienciaGanhadaNoLevelAtual;


        public      int                 xpLevelAtual;
        public      int                 restoXp;
        public      int                 ExpNecessaria;



        public      int                 expPosPerdaLevel;
        public      int                 tempNecessario;
        public      bool                verificaMorte; 
        


        public      int                 safira;
        public      int                 ruby;
        public      int                 esmeralda;
        public      int                 diamante;
        public      bool                jaTocouIntro;

    }

}
