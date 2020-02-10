using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class playerScript : MonoBehaviour
{
   
    private     audioController         audioController;
    private     xpMonstro               xpMonstro;
    private     ControleDanoInimigo     ControleDanoInimigo;
    private     _GameController         _GameController;
    public     Animator                 playerAnimator;
    private     Rigidbody2D             playerRb;
    private     NPC                     NPC;
    public      bool                    Grounded;
    public      Transform               groundCheck;
    public      LayerMask               whatIsGround;
    public      float                   speed; //velocidade do personagem
    public      float                   jumpForce; //forca do personagem
    public      int                     idAnimation;
    public      bool                    lookLeft; // indica se esta olhando pra esquerda
    public      bool                    attacking; // verifica se o personagem esta executando ataque
    public      bool                    naoPodeAtacar; // se estiver false = pode atacar INDICA SE PODEMOS CRIAR UM ATAQUE

    private     float                   h, v; // pegam a velocidade do player 
    public      Material                luz2D, Padrao2D; 
    public      bool                    estaNaCaverna; // verifica se ele está em uma caverna
    public      Transform               transformPlayer; // a informacao do transform no script player
    private     SpriteRenderer          Srender; // usando para ter acesso ao sprite render do personagem

    [Header("Configurar vida Player")]

    public      int                     vidaMax, vidaAtual; //controle de vida do player
// INTERACAO COM OBJETOS

    private     Vector3                 dir = Vector3.right; //cria uma linha na frente do personagem para detectar colisoes
    public      Transform               hand;
    public      LayerMask               interacao;
    private     door                    door;
    public      GameObject              objetoInteracao;

  [Header("SISTEMA DE ARMAS")]  
    public      GameObject[]            armas, arcos, flechaArco, staffs;  // usado pra salvar os 3 estagios de armas
    
    public      int                     idArma;
    public      int                     idArmaAtual;
    public      float                   delayAtack; //tempo que ele vai esperar para atacar novamente
    public      GameObject              magiaPrefab;
    public      Transform               spawnFlecha, spawnMagia;

    [Header("ATRIBUTOS UPGRADE")]
    private      int                     critico; // vai somar o critico da arma + valor da arma atual + aprimoramento da arma +10 em upgrade da arma q vai até +10





    [Header("VERIFICA SE TOMOU RIT")]
    public         bool                     died; //indica se esta morto
    public          GameObject              danoTxtPrefab; // objeto q ira exibir o dano tomado 
    public          float[]                 ajusteDano;     // ajuste de resistencia  e fraquesa dos inimigos. 

    private         bool                    getHit; // olha se o inimigo tomou hit
    private         Animator                animator;

    private         float                   percVida; //controla o percentual de vida





  [Header("SISTEMAS DE ALERTAS")]   
   public      GameObject   alertBaloon; 
 

    void Start()
    {
       
        ControleDanoInimigo     = FindObjectOfType(typeof(ControleDanoInimigo)) as ControleDanoInimigo;
        _GameController         = FindObjectOfType(typeof(_GameController)) as _GameController; 
        audioController         = FindObjectOfType(typeof(audioController)) as audioController;
        xpMonstro               = FindObjectOfType(typeof(xpMonstro)) as xpMonstro;
        door                    = FindObjectOfType(typeof(door)) as door; //instanciei o acesso ao script game controler
        NPC                     = FindObjectOfType(typeof(NPC)) as NPC;
        
        Srender                 = GetComponent<SpriteRenderer>(); // agora tenho acesso ao sprite render do personagem
        playerRb                = GetComponent<Rigidbody2D>(); // agora tenho acesso ao rigidbory do personagem
        playerAnimator          = GetComponent<Animator>();
        getHit = false;
        
        Grounded = true;
        vidaAtual = vidaMax;

        _GameController.manaAtual = _GameController.manaMaxima;

        foreach(GameObject objeto in armas )
        {
            objeto.SetActive(false); // desaiva as imagens de ataque quando começa o jogo para não aparecer enquanto anda
        }
         foreach(GameObject objeto in arcos )
        {
            objeto.SetActive(false); 
        }
         foreach(GameObject objeto in staffs )
        {
            objeto.SetActive(false); 
        }

        trocarArma(idArma); // passei a referencia do id arma para funcao 


        // ---------------------- CARREGA OS DADOS INICIAIS DO PERSONAGEM ---------------------- \\
           
        vidaMax = _GameController.manaMaxima;
        idArma  = _GameController.idArma;

        //--------------------------------------------------------------------------------------\\
    
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        if(_GameController.currentState != GameState.GAMEPLAY)
            {
                return; // se o jogo não estiver em ação, ou seja, se estiver pausado, nenhum comando abaixo vai ser acessado
            }
        Grounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f, whatIsGround);
        playerAnimator.SetFloat("speedY", playerRb.velocity.y); 
        interagir();
            
    }
    void Update()
    {
        
       
        playerRb.velocity = new Vector2(h * speed, playerRb.velocity.y); 
        if(_GameController.currentState != GameState.GAMEPLAY)
            {
                return; // se o jogo não estiver em ação, ou seja, se estiver pausado, nenhum comando abaixo vai ser acessado
            }
          if(NPC.travaPersonagem == true)
        {    
           speed = 0;
         }
         else
         {
            NPC.travaPersonagem = false;
            v = CrossPlatformInputManager.GetAxisRaw("Vertical");
            h = CrossPlatformInputManager.GetAxis("Horizontal");
         }
         if (h > 0 && lookLeft == true && attacking == false)
        {
            flip();
        }
        else if(h <0 && lookLeft == false && attacking == false)
        {
            flip();
        }
      
        if (v < 0 )
        {
            idAnimation = 2;
            if (Grounded == true)
            {
                h = 0;
            }
        }
        
        if(h !=0) 
        {
            idAnimation =1;
        }
        else
        {
            idAnimation = 0; 
        }

      




        if(CrossPlatformInputManager.GetButtonDown("Fire1") && v >= 0 && attacking == false && objetoInteracao == null && naoPodeAtacar == false)
        {
            naoPodeAtacar = true; //ele n vai poder atacar enquanto estiver atacando
            playerAnimator.SetTrigger("atack");
           
        }
        if(CrossPlatformInputManager.GetButtonDown("Fire1") && v >= 0 && attacking == false && objetoInteracao != null)
        {
            objetoInteracao.SendMessage("interacao", SendMessageOptions.DontRequireReceiver); // quando apertar botao atack ele vai interagir e a "sendmenssage" n deixa retornar erro caso nao
           
        }
        if(CrossPlatformInputManager.GetButtonDown("Jump") && Grounded == true && attacking == false)
        {
            playerRb.AddForce(new Vector2(0, jumpForce));
        }
       
        if (attacking == true && Grounded == true )
        {
          h = 0;
        }
        playerAnimator.SetBool("grounded", Grounded); 
        playerAnimator.SetInteger("idAnimation", idAnimation); 
        playerAnimator.SetFloat("speedY", playerRb.velocity.y); 
        playerAnimator.SetFloat("idClasseArma", _GameController.idClasseArma[_GameController.idArmaAtual]); // estou pegando o id da animacao do personagem 0 1 ou 2 
 
        if(_GameController.qtdFlechas[_GameController.idFlechaEquipada] > 0)
        {
            foreach(GameObject f in flechaArco)  //se tiver flecha eu vou ativar a flecha no arco
            f.SetActive(true);
        }
        else 
        {
            foreach(GameObject f in flechaArco) //se não tiver flecha, eu desativo a flecha no arco.
            f.SetActive(false);
     
 
 
        
        }
    }
   void LateUpdate()
    {
        if(_GameController.idArma != _GameController.idArmaAtual)
        {
            trocarArma(_GameController.idArma);
        }
    }
    void flip()
    {
      lookLeft = !lookLeft; // inverte o valor da variavel booleana
        float x = transform.localScale.x;
        x*=-1; //inverte o sinal do scaleX
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        dir.x = x;
     
    }

    public void atack(int atk)
    {
        switch(atk)
        {
            case 0: 
                attacking = false; // verifica se o personagem está atacando
                armas[0].SetActive(false);
                armas[1].SetActive(false);
                armas[2].SetActive(false); //quando não estiver atacando mais ele desativa a ultima imagem para voltar ao estado normal 
                StartCoroutine("esperarNovoAtack"); 
               
            break;

            case 1:
                attacking = true; // verifica se o personagem está atacando 
                audioController.tocarFx(audioController.fxSword, 1); // VAI TOCAR O AUDIO DO ATACK 
                
            break;
        }
    }

        public void atackFlecha(int atk)
    {
        switch(atk)
        {
            case 0: 
                attacking = false; // verifica se o personagem está atacando
                arcos[2].SetActive(false); //quando não estiver atacando mais ele desativa a ultima imagem para voltar ao estado normal
                StartCoroutine("esperarNovoAtack");  
            break;

            case 1: 
                attacking = true; // verifica se o personagem está atacando
                
            break;

            case 2: 
                if(_GameController.qtdFlechas[_GameController.idFlechaEquipada] > 0)
                { 
                    audioController.tocarFx(audioController.fxBow, 1); // VAI TOCAR O AUDIO DO ATACK 
                    _GameController.qtdFlechas[_GameController.idFlechaEquipada] -=1;
                    GameObject tempPrefabFlecha = Instantiate(_GameController.FlechaPrefab[_GameController.idFlechaEquipada], spawnFlecha.position, spawnFlecha.localRotation); // instanciei o local que a flecha vai aparecer
                    tempPrefabFlecha.GetComponent<Rigidbody2D>().velocity = new Vector2(_GameController.velocidadeFlecha[_GameController.idFlechaEquipada] * dir.x,0); // o dir.x e a variavel que verifica pra qual lado é o flip
                    tempPrefabFlecha.transform.localScale = new Vector3(tempPrefabFlecha.transform.localScale.x * dir.x, 
                    tempPrefabFlecha.transform.localScale.y, tempPrefabFlecha.transform.localScale.z); // vai mudar o lado da flecha
                    Destroy(tempPrefabFlecha, 1.5f);
                }
            break;


        }
    }





        public void atackStaff(int atk)
    {
        
        switch(atk)
        {
            case 0: 
                
                staffs[3].SetActive(false);
                attacking = false; // verifica se o personagem está atacando
                //staffs[3].SetActive(false); //quando não estiver atacando mais ele desativa a ultima imagem para voltar ao estado normal 
                StartCoroutine("esperarNovoAtack"); 
            break;

            case 1:
                attacking = true; // verifica se o personagem está atacando
                
            break;
            
            case 2: 
                if(_GameController.manaAtual >0)
                {
                    audioController.tocarFx(audioController.fxStaff, 1); // VAI TOCAR O AUDIO DO ATACK 
                    _GameController.manaAtual -=1; // ELE PEGA O MANAATUALHUD E DIMIUI NA INTERFACE A QUANTIDADE DE BLOCOS DE MANA AZUL
                    GameObject tempPrefabStaff = Instantiate(magiaPrefab, spawnMagia.position, spawnMagia.localRotation); // instanciei o local que a flecha vai aparecer
                    tempPrefabStaff.GetComponent<Rigidbody2D>().velocity = new Vector2(5 * dir.x,0);
                    Destroy(tempPrefabStaff, 1);
                }
            break;
        }
    }

void interagir()
{
    
    RaycastHit2D hit = Physics2D.Raycast(hand.position, dir, 0.15f, interacao);
    Debug.DrawRay(hand.position, dir * 0.25f, Color.red);
    if (hit == true)
    {   
        objetoInteracao = hit.collider.gameObject; 
        alertBaloon.SetActive(true);
    }
    else
    {
        objetoInteracao = null;
        alertBaloon.SetActive(false);
    }
}

void controleArma(int id)
{
     foreach(GameObject objeto in armas ) 
        {
            objeto.SetActive(false);  //desativo todos objetos e embaixo ativo apenas a que eu quero
        }
        armas[id].SetActive(true); // ativa apenas uma das imagens das armas
}
void controleArco(int id)
{
     foreach(GameObject objeto in arcos ) 
        {
            objeto.SetActive(false);   
        }
        arcos[id].SetActive(true); 
}
void controleStaff(int id)
{
     foreach(GameObject objeto in staffs ) 
        {
            objeto.SetActive(false);  
        }
        staffs[id].SetActive(true); 
}
 
void OnTriggerEnter2D(Collider2D col)
{
    switch(col.gameObject.tag)
    {
        case "coletavel":  
        col.gameObject.SendMessage("coletar", SendMessageOptions.DontRequireReceiver);
        break;

        case "luzPersonagem":  
        estaNaCaverna = true;
        gameObject.GetComponent<SpriteRenderer>().material = luz2D; // se a porta levar para ambiente claro o player vai ficar normal
        foreach(GameObject o in armas)
        {   
            o.GetComponent<SpriteRenderer>().material = luz2D;  // esse foreach vou usar para trocar o material da arma, assim troca de todos os movimentos
        }
        break;

        case "saiuDaCaverna":  // pega a tag e verifica, se for essa ele executa o comando. 
        estaNaCaverna = false;
        
        
        gameObject.GetComponent<SpriteRenderer>().material = Padrao2D; // se a porta levar para ambiente claro o player vai ficar normal
        foreach(GameObject o in armas)
        {   
            o.GetComponent<SpriteRenderer>().material = Padrao2D;  // esse foreach vou usar para trocar o material da arma, assim troca de todos os movimentos
        }
        break;

        case "inimigo":
        _GameController.vidaAtual -=1;
        if(_GameController.vidaAtual < 0 )
        {
           // morreu CALCULAR MORTE
        }
        
        break;

        case "saiuDaFase":
         _GameController.Morreu();
         gameObject.SetActive(false);
            // morreu CALCULAR MORTE

         break;

        case "apanhou":
           
            if (getHit == false)
            {

                _GameController.vidaAtual -=1;
                getHit = true; // passei o rit pra verdadeiro pois ele tomou rit MAZ-A
                
                float danoArma = Random.Range(ControleDanoInimigo.ForcaAtack[0], ControleDanoInimigo.ForcaAtack[1]); // pega no script o dando que o inimigo tomou entre maximo e minimo
                int tipoDano = ControleDanoInimigo.tipoDeDano; //  // vai pegar o tipo do dano que o inimigo tomou "agua, fogo, normal etc"
                //animator.SetTrigger("hit");





            //===================== CALCULO DANO DE ATAQUE DO INIMIGO ================//

                float tempCalc = danoArma - _GameController.atrDefesa; // AQUI TO CALCULANDO A DEFSA DO PERSONAGEM 
                // calculo dano tomado do personagem formula = danoTomado = danoArma + (danoArma * (ajusteDano[id]/100))
                float danoTomado = tempCalc; // + (danoArma * (ajusteDano[tipoDano] / 100));
  
            //===================== CALCULO DANO DE ATAQUE DO INIMIGO ================//    


                _GameController.vidaAtual -= Mathf.RoundToInt(danoTomado); // reduz da vida a quantidade do dano tomando arredondando o rit pois é fisico       

                if (_GameController.vidaAtual <= 0){
                
                    //_GameController.experienciaParaEstarNoLevel -= _GameController.ExpNecessaria/10; // AQUI GUARDA A EXPERIENCIA NECESSARIA PARA ESTAR NO LEVEL
                    _GameController.changeState(GameState.MORTO);

                    /// ==============  CALCULO DE EXPERIENCIA POS MORTE VERIFICANDO A BLESS ==================///

                    if(_GameController.atrLevel2 < 2)
                    {
                        
                        _GameController.atrDiedXpLoss = 0; // vai mostrar quanto perdi de xp
                        _GameController.atrDiedExperiencia = _GameController.atrExperiencia ; // vai mostrar a xp que vou ter dps que morri
                        _GameController.atrDiedLevel = 1; 
                    }

                        int tempCalcPerdaPorcentagem        =  10 - _GameController.atrBenca; // calculo a porcentagem fixa que é 10 - bença
                       
                        _GameController.atrDiedXpLoss       =  _GameController.ExpNecessaria/tempCalcPerdaPorcentagem; // vai mostrar quanto perdi de xp
                        _GameController.atrDiedExperiencia  =  _GameController.atrExperiencia ; // vai mostrar a xp que vou ter dps que morri
                        _GameController.atrDiedLevel        =  _GameController.atrLevel2; 
                        _GameController.atrExperiencia      -= _GameController.ExpNecessaria/tempCalcPerdaPorcentagem ; // mando pra xp o quanto perdi
                        _GameController.atrNextLevel        += _GameController.ExpNecessaria/tempCalcPerdaPorcentagem; // proximo level vai aumentar pra upar de level
                   
                    /// ==============  CALCULO DE EXPERIENCIA POS MORTE VERIFICANDO A BLESS ==================///


                   _GameController.Save(); // depois de morrer salvo o estado do jogo para o jogador n burlar o sistema
                    died = true;
                    //animator.SetInteger("idAnimation", 3); // ativa a animacao de morto do inimigo
                    speed = 0;
                    StartCoroutine("morto"); 
            
                    //gameObject.layer = 13; // quando o inimigo morre ele fica na layer "morto" ou seja invuneravel para n ser empurrado
                    //StartCoroutine("loot"); // chama a coroutine de lot
                
                }
                if(danoArma > 1){ // SO VAI MOSTRAR DANO SE ELE FOR MAIOR
                    var Randomint = Random.Range(0,3); // pega random de 0 a 2 pq o limite fica fora
                    audioController.tocarFx(audioController.fxAtackSucess[Randomint],1);
                    GameObject danoTemp = Instantiate(danoTxtPrefab, transform.position, transform.localRotation); // instanciei o dano em 1 vari temporaria
                    danoTemp.GetComponent<TextMesh>().text = Mathf.RoundToInt(danoTomado).ToString(); // a variavel temp recebe o valor do rit
                    danoTemp.GetComponent<MeshRenderer>().sortingLayerName = "HUD"; //passei o rit pra layer em cima de tudo através de comando
                    
                    //
                    // FACO AQUI VERIFICACAO DO TIPO DE DANO PARA CHAMAR ALGUMA ANIMACAO E AUDIO DIFERENTE
                    //

                    GameObject fxTemp = Instantiate(_GameController.fxDano[tipoDano], transform.position, transform.localRotation); // instanciei o rit e coloquei ele no personagem
                    Destroy(fxTemp, 1); 

                    int forcaX = 50;  //iniciei a forca do eixo X para jogar o dano pro lado
                    //if(playerEsquerda == false){forcaX *= -1; } // verifiquei o lado do personagem e jogo o valor do rit pro outro lado 
                    danoTemp.GetComponent<Rigidbody2D>().AddForce(new Vector2(forcaX, 250)); //add forca no movimento do texto para jogar ele pra cima
                    Destroy(danoTemp, 0.90f); //destruo a variavel com o valor do rit para não sobrecarregar a memoria
                }else
                    {
                        var Randomint = Random.Range(0,3); // vai pegar um random de 0 a 2 e vai jogar na variavel debaixo
                        audioController.tocarFx(audioController.fxAtackFailed[Randomint], 1); // FAZ O SOM DO CLICK NOS ITEM
                        print("miss");
                    }

                
                //GameObject knockTemp = Instantiate(knockForcePrefab, knockPosition.position, knockPosition.localRotation); // intancia a prefab com a força, posicao e rotacao
                //Destroy(knockTemp, 0.03f); // destroi a prefab que empurra o player para trás com o efeito
                StartCoroutine("invuneravel"); // vai ficar invuneravel por um determinado tempo
                //this.gameObject.SendMessage("tomeiHit", SendMessageOptions.DontRequireReceiver); //se ele tomar hit ele vai tentar chamar o tomei hit, se o TOMEIHIT nao existir ele n vai da erro por causa do dontRequieReceiver
                
            }
        break;
        
    }

}

  
    public void ChangeMaterial(Material novoMaterial)
    {
        Srender.material = novoMaterial; 
        foreach(GameObject o in armas)
        {
            o.GetComponent<SpriteRenderer>().material = novoMaterial;  // esse foreach vou usar para trocar o material da arma, assim troca de todos os movimentos
        }
        foreach(GameObject o in flechaArco)
        {
            o.GetComponent<SpriteRenderer>().material = novoMaterial;  
        }
        foreach(GameObject o in staffs)
        {
            o.GetComponent<SpriteRenderer>().material = novoMaterial;  
        }
    }

  
    public void trocarArma(int id)
    {
        _GameController.idArma = id; 
        
        switch(_GameController.idClasseArma[id])
        {
            
            case 0 : // espadas machados e martelos
                ArmaInfo  tempInfoArma = armas[0].GetComponent<ArmaInfo>();// estou pegando o tipo de dano, dano minimo, dano maximo estagio 1 do movimento

                armas[0].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas1[id]; // pega o sprite da arma dentro do conjuntos 0 = arma 1  = arco 2 = staff
                tempInfoArma = armas[0].GetComponent<ArmaInfo>();
                tempInfoArma.danoMin = _GameController.danoMinArma[idArma]; 
                tempInfoArma.danoMax = _GameController.danoMaxArma[idArma];
                tempInfoArma.tipoDeDano = _GameController.tipoDanoArma[idArma];
               

                armas[1].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas2[id]; 
                tempInfoArma = armas[1].GetComponent<ArmaInfo>();
                tempInfoArma.danoMin = _GameController.danoMinArma[idArma];
                tempInfoArma.danoMax = _GameController.danoMaxArma[idArma];
                tempInfoArma.tipoDeDano = _GameController.tipoDanoArma[idArma];

                armas[2].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas3[id]; 
                tempInfoArma = armas[2].GetComponent<ArmaInfo>();
                tempInfoArma.danoMin = _GameController.danoMinArma[idArma];
                tempInfoArma.danoMax = _GameController.danoMaxArma[idArma];
                tempInfoArma.tipoDeDano = _GameController.tipoDanoArma[idArma];
            break;

            case 1: // arco
                arcos[0].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas1[id]; // pega o sprite do arco
                arcos[1].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas2[id]; 
                arcos[2].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas3[id]; 

                

                
            break;        
            
            case 2: // staff
            
                staffs[0].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas1[id]; // pega o sprite da staff
                staffs[1].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas2[id]; 
                staffs[2].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas3[id]; 
                staffs[3].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas4[id]; 
            break;        
        }
            _GameController.idArmaAtual = _GameController.idArma; // ele faz acesso ao id da arma nessa parte para funcionar o script
    }

    IEnumerator esperarNovoAtack()
    {
        yield return new WaitForSeconds(delayAtack); //vai esperar esse tempo para poder atacar novamente
        naoPodeAtacar = false; //depois que ataca ele vai esperar esse tempo para atacar novamente
    }

    IEnumerator invuneravel()
    { // quando o personagem inimigo apanhar vai ficar ivuneravel 
       //playerScript.armas[2].SetActive(false); 
        yield return new WaitForSeconds(0.15f);
        getHit = false;
    }
     IEnumerator morto()
    { // quando o personagem inimigo apanhar vai ficar ivuneravel 
       //playerScript.armas[2].SetActive(false); 
        yield return new WaitForSeconds(0.15f);
        getHit = false;
       
        _GameController.Morreu();
    }
    
    
}
