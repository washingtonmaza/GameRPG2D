using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControleDanoInimigo : MonoBehaviour
{
    private         Hud                     Hud;
    private         IA_goblin               IA_goblin;
    private         _GameController         _GameController; // criei uma variavel gamecontrole do tipo gamecontroler do outro script para pegar seus dados
    private         destroyCollision        destroyCollision;
    private         playerScript            playerScript;
    private         SpriteRenderer          sRender;
    private         Animator                animator;


    // vIDA DO INIMIGOOOO
    [Header("Configuração de vida")]
    public          int                     vidaDoInimigo; // controla a vida do inimigo e o estado atual dela
    public          int                     vidaAtual; //pega o valor de vida atual do monster
    public          GameObject              barrasVida; // obj contendo todas as barras ( duas atual)
    public          Transform               hpBar; //obj indicador de quantidade de vida
    private         bool                    getHit; // olha se o inimigo tomou hit
    public          Color[]                 characterColor; // controle de cor do inimigo
    private         float                   percVida; //controla o percentual de vida
    public          GameObject              danoTxtPrefab; // objeto q ira exibir o dano tomado 




     [Header("Resistencia / Fraquesa DANO")]
    public          float[]                 ajusteDano;     // ajuste de resistencia  e fraquesa dos inimigos.
    public          int[]                   ForcaAtack; // vai pegar a variacao da forca maxima e minima do inimigo para calcular no playerscript o atack 
    public          float                   danoTomado;
    public          int                     tipoDeDano;
    public          int                     defesaDoInimigo;




     [Header("Configuração de empurrar o inimigo KnockBack")]
    public          GameObject              knockForcePrefab; // forca que empurra o inimigo copiado do prefabs
    public          Transform               knockPosition; // posicao do inimigo para joga-lo para tras ( recuar) washington Maza

    public          float                   knockX; // valor padrao da posicao x
    private         float                   kx; // vai armazenar o valor de knockx 

  
  [Header("CALCULO EXPERIENCIA UP")]
    public  int                 expMonstro;
    private bool                estaMorto;


    
     [Header("Configurar Ground")]
    public          Transform               groundCheck;
    public          LayerMask               whatIsGround;


    [Header("Configurar Lot")] 

    public      GameObject          loots;
    


     [Header("Configuração flip do inimigo")]
    public          bool                    olhandoAEsquerda, playerEsquerda; // verifica se o inimigo do cão está olhando para esqerda MAZA
    public         bool                    died; //indica se esta morto


    void Start()
    {
        IA_goblin       = FindObjectOfType(typeof(IA_goblin)) as IA_goblin;   
        _GameController = FindObjectOfType(typeof(_GameController)) as _GameController; // para acessar o script _gameControle dentro deste script
        playerScript    = FindObjectOfType(typeof(playerScript)) as playerScript;
        sRender         = GetComponent<SpriteRenderer>();
        animator        = GetComponent<Animator>();
        Hud             = FindObjectOfType(typeof(Hud)) as Hud;


        sRender.color   = characterColor[0];
        barrasVida.SetActive(false);
        vidaAtual = vidaDoInimigo;
        hpBar.localScale = new Vector3(1,1,1);

        if(olhandoAEsquerda == true)
        {
            float x = transform.localScale.x;
            x*=-1; //inverte o sinal do scaleX
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
            barrasVida.transform.localScale = new Vector3 (x, barrasVida.transform.localScale.x, barrasVida.transform.localScale.y); // flipa o life do inimigo
            
        }
        
        estaMorto = false;
    }

  
    void Update()
    {
        
        if(vidaAtual < 1 & estaMorto == false)
        {   
            _GameController.xpMonstro = expMonstro;
            _GameController.calcularExperiencia(); // chamo a funcao  pra calcular a xp q o player vai ganhar
            estaMorto = true; // quando o monstro morrer ele va fazer o calculo e vai sair deste loop
        }



        // verifica o lado que está o inimigo em comparacao ao player
        float xPlayer = playerScript.transform.position.x;
        if(xPlayer < transform.position.x)
        {
            playerEsquerda = true;
           
        }
        else if (xPlayer > transform.position.x)
        {
             playerEsquerda = false;
             
        }

        if(olhandoAEsquerda == true && playerEsquerda == true)
        {
            kx = knockX;
        }
        else if(olhandoAEsquerda == false && playerEsquerda == true)
        {
            kx = knockX * -1;
        }
        else if(olhandoAEsquerda == true && playerEsquerda == false)
        {
            kx = knockX * -1;
        }
        else if(olhandoAEsquerda == false && playerEsquerda == false)
        {
            kx = knockX ;
        }

        // knockPosition.localPosition = new Vector3(kx, knockPosition.localPosition.y, 0); // ESTAVA DANDO ERRO QUANDO MATAVA O MONSTRO AI TIREI
    
        animator.SetBool("grounded", true); // estou informando que o chao está marcado 
      
    }

    void OnTriggerEnter2D(Collider2D col)
    {   if (died == true) {return;} // usado pra encerrar o comando 
    
        switch(col.gameObject.tag)
        {

            case "saiuDaFase":
            DestroyObject(this.gameObject);
            //gameObject.SetActive(false);
            break;
            case "arma":
        
            if (getHit == false)
            {
                getHit = true; // passei o rit pra verdadeiro pois ele tomou rit MAZ-A
                barrasVida.SetActive(true); // quando tomar o rit vai mostrar a barra de vida do inimigo 
                ArmaInfo infoArma = col.gameObject.GetComponent<ArmaInfo>();
                float tempAjusteAprimoramento = (_GameController.aprimoramentoArma[_GameController.idArma] * 9)/10;
                //float danoArma = Random.Range(infoArma.danoMin, infoArma.danoMax); // pega no script o dando que o inimigo tomou entre maximo e minimo
                int tipoDano = infoArma.tipoDeDano; // vai pegar o tipo do dano que o inimigo tomou "agua, fogo, normal etc"
                animator.SetTrigger("hit");
                       
           

        //==================== CALCULO RIT DO PERSONAGEM NO INIMIGO ================ //


                // calculo dano tomado do personagem formula = danoTomado = danoArma + (danoArma * (ajusteDano[id]/100))
                //float  tempCritico =  (danoArma * _GameController.atrCritico) /10; //ele calcula o ataque da arma + quanto o personagem tem de critico
                //
                

                   switch(_GameController.idClasse[_GameController.idPersonagem])
                    {
                    case 0:
                            
                            float danoMinKnight = infoArma.danoMin + _GameController.aprimoramentoArma[_GameController.idArma];
                            float danoArmaKnight = Random.Range(danoMinKnight, infoArma.danoMax); // pega no script o dando que o inimigo tomou entre maximo e minimo
                            float tempAjusteDanoKnight = danoArmaKnight * (ajusteDano[tipoDano] / 100); // pega o dano * ajuste do dano que ta no tipo de dano logo em cima
                            float  tempCriticoKnight =  ((danoArmaKnight * _GameController.atrCritico) /10);
                            danoTomado = (danoArmaKnight + tempCriticoKnight +  tempAjusteDanoKnight + _GameController.atrForca)- defesaDoInimigo; // CALCULEI ATAQUE DO GUERREIRO
                        
                        if(danoTomado < 1)
                        {
                                danoTomado = danoMinKnight;
                        }
                    break;
                    case 1:
                            // Como o paladino o ataque sai da flecha prefab, preciso dizer que o ataque do bow evouido também vai contar como dano critico
                            float danoMinPaladin = infoArma.danoMin + _GameController.aprimoramentoArma[_GameController.idArma];
                            float danoArmaPaladin = Random.Range(danoMinPaladin, infoArma.danoMax); // pega no script o dando que o inimigo tomou entre maximo e minimo
                            float tempAjusteDanoPaladin = danoArmaPaladin * (ajusteDano[tipoDano] / 100);
                            float  tempCriticoPaladin =  ((danoArmaPaladin * _GameController.atrCritico) /10)+ _GameController.aprimoramentoArma[_GameController.idArma];
                            danoTomado = (danoArmaPaladin + tempCriticoPaladin +  tempAjusteDanoPaladin + _GameController.atrForca) - defesaDoInimigo;  // CALCULEI ATAQUE DO PALADIN


                            if(danoTomado < 1)
                            {
                                danoTomado = danoMinPaladin;
                            }
                    break;
                    case 2: 
                            float danoMinMage = infoArma.danoMin + _GameController.aprimoramentoArma[_GameController.idArma];
                            float danoArmaMage = Random.Range(danoMinMage, infoArma.danoMax); // pega no script o dando que o inimigo tomou entre maximo e minimo
                            float tempAjusteDanoMage = danoArmaMage * (ajusteDano[tipoDano] / 100);
                            float  tempCriticoMago =  ((danoArmaMage * _GameController.atrCritico) /10);
                            danoTomado = (danoArmaMage + tempCriticoMago +  danoMinMage +  _GameController.atrMagico)- defesaDoInimigo;  // CALCULEI ATAQUE DO MAGE
                            if(danoTomado < 1)
                            {
                                danoTomado = danoMinMage;
                            }
   

                    break;
                    }


               // float danoTomado = danoArma + tempCritico +  tempAjusteDano + _GameController.atrForca; // estou calculando o atack critico + o ataque da arma + ajuste de dano por tipo de inimigo


        //==================== CALCULO RIT DO PERSONAGEM NO INIMIGO ================ //

                vidaAtual -= Mathf.RoundToInt(danoTomado); // reduz da vida a quantidade do dano tomando arredondando o rit pois é fisico
                percVida = (float) vidaAtual /(float) vidaDoInimigo; // calcula o porcentual de vida
                if (percVida < 0){percVida = 0;} // pra evitar a barra ficar negativa;
                hpBar.localScale = new Vector3 (percVida, 1,1); //atualiza a barra depois de tomar o hit e verificar as condiçoes
                if (vidaAtual <= 0 )
                {
                    died = true;
                    animator.SetInteger("idAnimation", 3); // ativa a animacao de morto do inimigo
                    IA_goblin.velocidade = 0;
            
                    //gameObject.layer = 13; // quando o inimigo morre ele fica na layer "morto" ou seja invuneravel para n ser empurrado
                    StartCoroutine("loot"); // chama a coroutine de lot
                    
                
            
                
                }

                GameObject danoTemp = Instantiate(danoTxtPrefab, transform.position, transform.localRotation); // instanciei o dano em 1 vari temporaria
                danoTemp.GetComponent<TextMesh>().text = Mathf.RoundToInt(danoTomado).ToString(); // a variavel temp recebe o valor do rit
                danoTemp.GetComponent<MeshRenderer>().sortingLayerName = "HUD"; //passei o rit pra layer em cima de tudo através de comando


                GameObject fxTemp = Instantiate(_GameController.fxDano[tipoDano], transform.position, transform.localRotation); // instanciei o rit e coloquei ele no meu inimigo
                Destroy(fxTemp, 1); 

                int forcaX = 50;  //iniciei a forca do eixo X para jogar o dano pro lado
                if(playerEsquerda == false){forcaX *= -1; } // verifiquei o lado do personagem e jogo o valor do rit pro outro lado 
                danoTemp.GetComponent<Rigidbody2D>().AddForce(new Vector2(forcaX, 250)); //add forca no movimento do texto para jogar ele pra cima
                Destroy(danoTemp, 0.90f); //destruo a variavel com o valor do rit para não sobrecarregar a memoria
                GameObject knockTemp = Instantiate(knockForcePrefab, knockPosition.position, knockPosition.localRotation); // intancia a prefab com a força, posicao e rotacao
                Destroy(knockTemp, 0.03f); // destroi a prefab que empurra o player para trás com o efeito
                StartCoroutine("invuneravel"); // vai ficar invuneravel por um determinado tempo
                this.gameObject.SendMessage("tomeiHit", SendMessageOptions.DontRequireReceiver); //se ele tomar hit ele vai tentar chamar o tomei hit, se o TOMEIHIT nao existir ele n vai da erro por causa do dontRequieReceiver
                
            }
        break;      
    } 

    }
      void flip()
    {
      float x = transform.localScale.x;
            x*=-1; //inverte o sinal do scaleX
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);  
            olhandoAEsquerda = !olhandoAEsquerda; // inverte o valor da variavel booleana
            barrasVida.transform.localScale = new Vector3 (x, barrasVida.transform.localScale.x, barrasVida.transform.localScale.y); // flipa o life do inimigo
     
      
     
    }
    IEnumerator loot()
    {
        
        yield return new WaitForSeconds(1);
        GameObject fxMorte = Instantiate(_GameController.fxMorte, groundCheck.position, transform.localRotation); // instanciei a morte e to usando variaveis do controle do game
        yield return new WaitForSeconds(0.5f);
        sRender.enabled = false;

        //controle de moedas
        int qntMoedas = Random.RandomRange(0,5);
        for (int l = 0; l <= qntMoedas; l++ ){
            GameObject lootTemp = Instantiate(loots, transform.position, transform.localRotation); 
            lootTemp.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-25,25), 80));
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.55f);
        Destroy(fxMorte);
        Destroy(this.gameObject);


    }
    IEnumerator invuneravel()
    { // quando o personagem inimigo apanhar vai ficar ivuneravel 
       IA_goblin.armas[2].SetActive(false); 
        sRender.color = characterColor[1]; 
        yield return new WaitForSeconds(0.15f);
        sRender.color = characterColor[0];
        yield return new WaitForSeconds(0.15f);
        sRender.color = characterColor[1];
        yield return new WaitForSeconds(0.15f);
        sRender.color = characterColor[0];
        yield return new WaitForSeconds(0.15f);
        sRender.color = characterColor[1];
        yield return new WaitForSeconds(0.15f);
        sRender.color = characterColor[0];
        yield return new WaitForSeconds(0.15f);
        getHit = false;
        barrasVida.SetActive(false);
       

    }
}
