using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //para acessar textos 
using UnityEngine.UI; 
using System.Xml; 
public class NPC : MonoBehaviour
{
    public      string                  nomeArquivoXML; //onde vai por o nome do arquivo xml a ser lido pela funcao 
    public      Button                  btnA;  // vou usar essa variavel pra manter sempre a primeira resposta selecionada quando abrir o chat
    public      TMP_Text                textoBtnA, textoBtnB;
    private     _GameController         _GameController;
    private     playerScript            playerScript;
    public      GameObject              painelResposta; 
    public      GameObject              canvasNPC; //vou usar pra acessar o game objeto que se encontra na interface 
    public      TMP_Text                caixaDeTexto; //vou acessar na itnerface
    public      int                     idFala;
    public      int                     idDialogo;
    private     bool                    dialogoOn;
    public      List<string>            linhasDialogo; 
    public      List<string>            fala0; //FALA DO npc apresentacao
    public      List<string>            fala1; //FALA NPC
    public      List<string>            fala2; // FRASE NEGANDO A MISSAO
    public      List<string>            fala3; //FRASE ACEITANDO A MISSAO
    public      List<string>            fala4; //FALA DE CONCLUSAO DE QUEST
    public      List<string>            fala5; //NADA A DIZER, QUANDO NAO TEM MISSAO
    public      List<string>            respostaFala0;
    private     bool                    respondendoPergunta;
    public      bool                    travaPersonagem;
    private     float                   tempSpeed;                 
    



    void Start()
    {
        _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
        playerScript = FindObjectOfType(typeof(playerScript)) as playerScript;
        canvasNPC.SetActive(false);
        painelResposta.SetActive(false);
        tempSpeed = playerScript.speed;
       
      loadDialogoData();
    }

    void Update()
    {
        if(dialogoOn == false) 
        {
            travaPersonagem = false;
            playerScript.speed = tempSpeed;
            
        }

        if(idFala == 2 && idDialogo == 0 )
        {
            
            _GameController.hudDesativar[3].SetActive(false);
        }
        if(idFala == 3 && idDialogo == 3)
        {
            
        
            _GameController.hudDesativar[3].SetActive(false);
             _GameController.hudDesativar[0].SetActive(true);
            _GameController.hudDesativar[1].SetActive(true);
            _GameController.hudDesativar[2].SetActive(true);
            _GameController.hudDesativar[4].SetActive(true);
            _GameController.hudDesativar[5].SetActive(true);
        }
 

         if(idFala == 2 && idDialogo == 3)
        {
            
        
            _GameController.hudDesativar[3].SetActive(false);
             _GameController.hudDesativar[0].SetActive(true);
            _GameController.hudDesativar[1].SetActive(true);
            _GameController.hudDesativar[2].SetActive(true);
            _GameController.hudDesativar[4].SetActive(true);
            _GameController.hudDesativar[5].SetActive(true);

        }
 
     if(idFala == 3 && idDialogo == 0)
        {
            
        
            _GameController.hudDesativar[3].SetActive(false);
             _GameController.hudDesativar[0].SetActive(true);
            _GameController.hudDesativar[1].SetActive(true);
            _GameController.hudDesativar[2].SetActive(true);
            _GameController.hudDesativar[4].SetActive(true);
            _GameController.hudDesativar[5].SetActive(true);
            

            
        
        }
 

    }
    public void interacao()
    {
        
        if(dialogoOn == false) // se ele nunca tiver conversado com o personagem, vai começar pela fala no id 0
        {   idFala = 0;
          
            if(idDialogo == 3 && _GameController.quest1 == true) // VOU VERIFICAR SE ALGUMA MISSAO FOI CUMPRIDA
            {
                idDialogo = 4;    // COMO A MISSAO FOI CUMPRIDA ELE VAI PRA FALA DE RECOMPENSA
            }
            prepararDialogo(); //chamo a conversa com o npc
            dialogo();
            canvasNPC.SetActive(true);
            dialogoOn = true;
            travaPersonagem = true;

        }
        else if(dialogoOn == true && respondendoPergunta == false)
        {
            idFala+=1; //a cada interacao ele vai passando para a proxima fala
            dialogo(); //chamo a funcao para ir até o ultimo texto
            travaPersonagem = true;
        }
        
        else
        {
            travaPersonagem = false;
        }
        
    }
   
    public void dialogo()
    {
         _GameController.hudDesativar[0].SetActive(false);
        _GameController.hudDesativar[1].SetActive(false);
        _GameController.hudDesativar[2].SetActive(false);
        _GameController.hudDesativar[3].SetActive(true);
        _GameController.hudDesativar[4].SetActive(false);
        _GameController.hudDesativar[5].SetActive(false);
   

        if(idFala < linhasDialogo.Count)
        {
            caixaDeTexto.text = linhasDialogo[idFala]; // a caixa de texto vai receber a conversa que vai puxar através do id
            if(idDialogo == 0 && idFala == 2 )
            {
                textoBtnA.text = respostaFala0[0];
                textoBtnB.text = respostaFala0[1];
                painelResposta.SetActive(true);
                btnA.Select();// vai abrir o btn e ja aparecer celecionado como primeira opcao
                respondendoPergunta = true;

            }
        
        }
        
        else //ENCERRA A CONVERSA -----  //TODA VEZ QUE AUMENTAR O ID DIALOGO PRECISO AUMENTAR LOAD DIALOGO E ID DIALOGO
        {
        switch(idDialogo)
            {
                case 0:
                   
                break;
                case 1:
                    idDialogo = 3;
                break;
                
                case 2:
                    idDialogo = 0;
                break;
                case 4:
                    idDialogo = 5; // quando finalizar o dialogo 4 ele vai pro 5
                break;
            }
            
             canvasNPC.SetActive(false); //desativo o canvas 
            dialogoOn = false;
        }
        
    }

        void prepararDialogo() //TODA VEZ QUE AUMENTAR O ID DIALOGO PRECISO AUMENTAR LOAD DIALOGO E ID DIALOGO

    {
        linhasDialogo.Clear(); //vou limpar as linhas de dialogo para n misturar

        switch(idDialogo)
            {
                case 0:
                    foreach(string s in fala0)
                    {
                        linhasDialogo.Add(s); //vou adicionar a linha de dialogo na fala 
                    }
                break;
                
                case 1:
                    foreach(string s in fala1)
                    {
                        linhasDialogo.Add(s); //vou adicionar a linha de dialogo na fala 
                    }
                break;

                case 2:
                    foreach(string s in fala2)
                    {
                        linhasDialogo.Add(s); //vou adicionar a linha de dialogo na fala 
                    }
                break;

                case 3:
                    foreach(string s in fala3)
                    {
                        linhasDialogo.Add(s); //vou adicionar a linha de dialogo na fala 
                    }
                break;

                case 4:
                    foreach(string s in fala4)
                    {
                        linhasDialogo.Add(s); //vou adicionar a linha de dialogo na fala 
                    }
                break;

                case 5:
                    foreach(string s in fala5)
                    {
                        linhasDialogo.Add(s); //vou adicionar a linha de dialogo na fala 
                    }
                break;
            }
    }
    public void btnRespostaA()
    {
        idDialogo = 1; //vai pro segundo dialogo
        prepararDialogo(); //chamo ela para verificar em qual dialogo está agora
        idFala = 0; //vai começar na primeira fala pois o array começa em 0
        respondendoPergunta = false; 
        painelResposta.SetActive(false);
        dialogo(); 

        


   
    }
    public void btnRespostaB()
    {
        idDialogo = 2; //vai pro segundo dialogo
        prepararDialogo(); //chamo ela para verificar em qual dialogo está agora
        idFala = 0; //vai começar na primeira fala pois o array começa em 0
        respondendoPergunta = false; 
        painelResposta.SetActive(false);
        dialogo();        
  

    }

    //FUNCAO PARA LER ARQUIVOS XML
    void loadDialogoData() //TODA VEZ QUE AUMENTAR O ID DIALOGO PRECISO AUMENTAR LOAD DIALOGO E ID DIALOGO
    {
        TextAsset xmlData = (TextAsset)Resources.Load(_GameController.idiomaFolder[_GameController.idioma] + "/" + nomeArquivoXML);
        XmlDocument XmlDocument = new XmlDocument();
        XmlDocument.LoadXml(xmlData.text);

        foreach(XmlNode dialogo in XmlDocument["dialogos"].ChildNodes)
        {
            string dialogoNome= dialogo.Attributes["name"].Value;
            
            foreach(XmlNode f in dialogo["falas"].ChildNodes)
            {
                switch(dialogoNome)
                {
                    case "fala0":
                        
                        fala0.Add(_GameController.textoFormatado(f.InnerText)); //ESTOU CHAMANDO A FUNCAO FORMATAR TEXTO PARA PODER SEGUIR AS FORMATACOES CASO EXISTA
                    break;

                    case "fala1":
                        fala1.Add(_GameController.textoFormatado(f.InnerText));
                    break;

                    case "fala2":
                        fala2.Add(_GameController.textoFormatado(f.InnerText));
                    break;

                    case "fala3":
                        fala3.Add(_GameController.textoFormatado(f.InnerText));
                    break;

                    case "fala4": 
                        fala4.Add(_GameController.textoFormatado(f.InnerText));
                    break;
                    case "fala5": 
                        fala5.Add(_GameController.textoFormatado(f.InnerText));
                        
                    break;

                    case "resposta0":
                        
                        respostaFala0.Add(_GameController.textoFormatado(f.InnerText));
                        
                    break;

                }
            }
        }
         
    }
    
}
