using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum     enemyState{
        PARADO,
        ALERTA,
        PATRULHA,
        ATACK, 
        RECUAR,
        MORREU
}
public class IA_goblin : MonoBehaviour
{
    private         ControleDanoInimigo     ControleDanoInimigo;
    private         _GameController         _GameController;
    private         playerScript            playerScript; //acesso ao script do player
    private         audioController         audioController;
    private         obstaculos              obstaculos;
    private         Rigidbody2D             rBody;
    public          Rigidbody2D             mudarLayerPosMorte;
    private         SpriteRenderer          sRender;
    private         Animator                animator;      
    public          enemyState              currentEnemyState;
    public          enemyState              stateInicial; //qual estado inicial do inimigo
    public          GameObject              alert; //usado para aparecer o ballon no inimigo quando ele ver o personagem
    public          float                   distMudarRota;  //quando se aproximar do inimigo ele vai verificar nessa variavel e chamar alguma funcao ou trigger
    public          float                   velocidadeBase;      //velocidade padrao
    public          float                   velocidade; // velocidade atual
    public          float                   tempoEsperaIdle; //tempo para esperar
    public          float                   tempoRecuo; //tempo que ele vai ficar recuando para longe do pesonagem
    private         Vector3                 dir = Vector3.right; 
    public          LayerMask               layerObstaculos; //saber a layer dos obstaculos, vai usar pra colidir 
    public          bool                    lookLeft;
    public          float                   distSairAlerta; // se a distancia for verdadeira o inimigo sai do alerva e vai para patrulha
    public          float                   distVerPersonagem; //a distancia que vai poder enchergar o personagem para tomar alguma ação
    public          LayerMask               layerPersonagem; //vai verificar qual a layer que se encontra o personagem para poder fazer a colisao e tomar alguma ação, no caso deixara ele em estado de alerta
    public          float                   distParaAtack; //d distancia para atacar o player
    public          bool                    ignorarObstaculo; //vai verificar se ele vai ignorar obstaculos
    public          int                     idArma; //por uma arma na mao do inimigo
    public          int                     idClasse; //vou usar pra trocar a animacao de ataque
    private         bool                    attacking; //vai verificar se está atacando o personagem principal 
    public          GameObject[]            armas; //armas que o inimigo estará utilizando
    public          GameObject[]            arcos; //armas que o inimigo estará utilizando
    public          GameObject[]            staffs; //armas que o inimigo estará utilizando
    public          GameObject[]            flechaArco; //vou usar pra fazer ficar escuro
    public          bool                    ambienteEscuro; //verifica se o ambiente é escuro ou não
    public          bool                    emAlertaHit; // //se ele for atacado ele vai da true
    public          float                   tempoDeAlerta; //quanto tempo ele vai ficar em alerta caso seja atacado de longe
    


  

    void Start()
    {
        ControleDanoInimigo = FindObjectOfType(typeof(ControleDanoInimigo)) as ControleDanoInimigo;
        _GameController     = FindObjectOfType(typeof(_GameController)) as _GameController;
        obstaculos          = FindObjectOfType(typeof (obstaculos)) as obstaculos; 
        playerScript        = FindObjectOfType(typeof(playerScript)) as playerScript;
        audioController     = FindObjectOfType(typeof(audioController)) as audioController;
        mudarLayerPosMorte  = GetComponent<Rigidbody2D>();
        rBody               = GetComponent<Rigidbody2D>();
        animator            = GetComponent<Animator>(); //iniciei o acesso a interface
        sRender             = GetComponent<SpriteRenderer>(); //iniciei o sprite render 
        
        if(lookLeft == true){flip();}
        changeState(stateInicial); //o estado inicial do inimigo vai ser definido na interface
        trocarArma(idArma); //vai selecionar a arma pro inigimio quando ele for inicializado no jogo      
        if(ambienteEscuro == true) //vou verificar se o ambiente é escuro
        {
            ChangeMaterial(_GameController.luz2D);
        }
        else
        {
            ChangeMaterial(_GameController.padrao2D); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
       
    
        if (currentEnemyState != enemyState.ATACK && currentEnemyState != enemyState.RECUAR)
        {
            Debug.DrawRay(transform.position, dir * distVerPersonagem, Color.red);
            RaycastHit2D hitPersonagem = Physics2D.Raycast(transform.position, dir, distVerPersonagem, layerPersonagem); //se ele colidir na layer pesonagem usando a distancia de verificacao ele vai fazer uma acao
            if(hitPersonagem == true)
            {
                changeState(enemyState.ALERTA);
            }
        }
        if(currentEnemyState == enemyState.PATRULHA )
        {   
            Debug.DrawRay(transform.position, dir * distMudarRota, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, distMudarRota, layerObstaculos);
            if(hit == true)  //se colidir com algo ele vai da true
            {
                changeState(enemyState.PARADO);  
                //StartCoroutine("idle");
            }
        }
        if(currentEnemyState == enemyState.RECUAR )
        {   
            Debug.DrawRay(transform.position, dir * distMudarRota, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, distMudarRota, layerObstaculos);
            float dist = Vector3.Distance(transform.position, playerScript.transform.position);
           if(hit == true && ignorarObstaculo == false)  //se ele colidir com a parede e ele n puder ignorar obstaculo ele vai chamar a funcao flip
            {
                flip();
                
            }
            else if(hit == true && dist <= 1 && ignorarObstaculo == true)  //aqui ele vai ignorar o seu limite de regiao que atua e sair correndo
            {
                obstaculos.obstaculoColisao.SetActive(false);
                

            }
        }
        
        //velocidade = velocidadeBase; // a velocidade atual vai pegar a velocidade base 

        if (currentEnemyState == enemyState.ALERTA )
        {
       
            float dist = Vector3.Distance(transform.position, playerScript.transform.position); //transform position  é a posidao do inimigo e o transform do player é a posicao do player
           
            if(dist <=distParaAtack) //se a distancia entre o inimigo e o personagem for verdadeira ele vai chamar a tag de atacar
            {
                changeState(enemyState.ATACK);
            }
            else if(dist >=distSairAlerta && emAlertaHit == false) //se ele estiver longe e o alerta hit estiver falso ele vai pra parado
            {
                changeState(enemyState.PARADO);
            }

            
        }

        if(currentEnemyState != enemyState.ALERTA)
        {
            alert.SetActive(false);
        }

        rBody.velocity = new Vector2(velocidade, rBody.velocity.y); //pegando a velocidade e jogando no rBody
    
        if(velocidade == 0)
        {
            animator.SetInteger("idAnimation", 0); //chama no animator a animação referente a posição 0 
        }
        if(ControleDanoInimigo.vidaAtual < 1)
        {
            velocidade = 0;
            animator.SetInteger("idAnimation", 3); 
        }
        else if(velocidade != 0)
        {
            animator.SetInteger("idAnimation", 1); //chama no animator a animação referente a posição 1 
        }

       animator.SetFloat("idClasse", idClasse);  //vou passar o id da animacao do personagem no animator 
        

    }
    
     

     void flip() //funcao para virar o personagem
    {
      lookLeft = !lookLeft; // inverte o valor da variavel booleana
        float x = transform.localScale.x;
        x*=-1; //inverte o sinal do scaleX
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        dir.x = x;
        velocidadeBase *= -1; //uso para inverter o lado que ele vai andar
        float vAtual = velocidade *=-1; //estou invertendo o lado da velocidade, pra qual lado ele vai andar mantendo a velocidade de corrida
        velocidade = vAtual; //a velocidade base vai para velocidade que é a atual e eai ele muda de direção
     
    }

    IEnumerator idle()
    {
    
        yield return new WaitForSeconds(tempoEsperaIdle);
        flip(); //altera a direção que o personagem está olhando
        changeState(enemyState.PATRULHA);

    }

    IEnumerator recuar()
    {
        yield return new WaitForSeconds(tempoRecuo); //pegou o tempo de recuo na interface 
        flip();
        changeState(enemyState.ALERTA);
    }
        IEnumerator morreu()
    {
        animator.SetInteger("idAnimation", 3); 
       // _GameController.atrExperiencia += xpMonstro; // AQUI O PERSONAGEM AUMENTA SUA XP
        
        yield return new WaitForSeconds(0.3f); //pegou o tempo de recuo na interface
        mudarLayerPosMorte.simulated = false; //assim que ele morrer eu tiro as propriedades de simulação de massa física
    }
    void changeState(enemyState newState)
    {
     
            currentEnemyState = newState;
            switch(newState)
            {
            case enemyState.PARADO:
                velocidade = 0; // coloco a velocidade atual do personagem em estado parado
                StartCoroutine("idle");
                
            break;
            
            case enemyState.PATRULHA:
                velocidade = velocidadeBase; 
              
            break;
            
            case enemyState.ALERTA: 
                velocidade = 0;
                alert.SetActive(true);
            break;

            case enemyState.ATACK: 
                animator.SetTrigger("atack"); //estou chamando a animação de ataque
                
            break;

            case enemyState.RECUAR: 
                flip();
                velocidade = velocidadeBase *2; //vai dobrar a velocidade do player para ele correr
                StartCoroutine("recuar"); //depois de virar o inimigo e dobrar a velocidade eu chamo a corrotina pra recuar
                
                
            break;
            case enemyState.MORREU: 
                
                velocidade = 0;
                StartCoroutine("morreu"); //depois de virar o inimigo e dobrar a velocidade eu chamo a corrotina pra recuar
                
            break;           
            }
            
        
    }

  


    public void atack(int atk)
    {
        switch(atk)
        {
            case 0: 
                attacking = false; // verifica se o personagem está atacando
                armas[2].SetActive(false); //quando não estiver atacando mais ele desativa a ultima imagem para voltar ao estado normal 
                changeState(enemyState.RECUAR); //apos atacar ele ira recuar o personagem
            break;
 
            case 1:
                attacking = true; // verifica se o personagem está atacando 
                
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
            break;

            case 1: 
                attacking = true; // verifica se o personagem está atacando
            break;

            case 2: 
                if(_GameController.qtdFlechas[_GameController.idFlechaEquipada] > 0)
                { 
                   // _GameController.qtdFlechas[_GameController.idFlechaEquipada] -=1;
                   // GameObject tempPrefabFlecha = Instantiate(_GameController.FlechaPrefab[_GameController.idFlechaEquipada], spawnFlecha.position, spawnFlecha.localRotation); // instanciei o local que a flecha vai aparecer
                   // tempPrefabFlecha.GetComponent<Rigidbody2D>().velocity = new Vector2(_GameController.velocidadeFlecha[_GameController.idFlechaEquipada] * dir.x,0); // o dir.x e a variavel que verifica pra qual lado é o flip
                   // tempPrefabFlecha.transform.localScale = new Vector3(tempPrefabFlecha.transform.localScale.x * dir.x, 
                  //  tempPrefabFlecha.transform.localScale.y, tempPrefabFlecha.transform.localScale.z); // vai mudar o lado da flecha
                  //  Destroy(tempPrefabFlecha, 1.5f);
                }
            break;


        }
    }

        public void atackStaff(int atk)
    {
        switch(atk)
        {
            case 0: 
                attacking = false; // verifica se o personagem está atacando
                staffs[3].SetActive(false); //quando não estiver atacando mais ele desativa a ultima imagem para voltar ao estado normal 

            break;

            case 1:
                attacking = true; // verifica se o personagem está atacando
            break;
            
            case 2: 
                //if(_GameController.manaAtual >0)
                //{
                 //   _GameController.manaAtual -=1;
                    //GameObject tempPrefabStaff = Instantiate(magiaPrefab, spawnMagia.position, spawnMagia.localRotation); // instanciei o local que a flecha vai aparecer
                    //tempPrefabStaff.GetComponent<Rigidbody2D>().velocity = new Vector2(5 * dir.x,0);
                    //Destroy(tempPrefabStaff, 1);
                //}
            break;
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

    public void trocarArma(int id)
    {
        switch(id)
        {
            case 0 : // espadas machados e martelos
                ArmaInfo  tempInfoArma = armas[0].GetComponent<ArmaInfo>();// estou pegando o tipo de dano, dano minimo, dano maximo estagio 1 do movimento

                armas[0].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas1[idArma]; // pega o sprite da arma


                tempInfoArma = armas[0].GetComponent<ArmaInfo>();
                tempInfoArma.danoMin = _GameController.danoMinArma[idArma];
                tempInfoArma.danoMax = _GameController.danoMaxArma[idArma];
                tempInfoArma.tipoDeDano = _GameController.tipoDanoArma[idArma];

                armas[1].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas2[idArma]; // pega o sprite da arma
                tempInfoArma = armas[1].GetComponent<ArmaInfo>();
                tempInfoArma.danoMin = _GameController.danoMinArma[idArma];
                tempInfoArma.danoMax = _GameController.danoMaxArma[idArma];
                tempInfoArma.tipoDeDano = _GameController.tipoDanoArma[idArma];

                armas[2].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas3[idArma]; //pega o sprite da arma
                tempInfoArma = armas[2].GetComponent<ArmaInfo>();
                tempInfoArma.danoMin = _GameController.danoMinArma[idArma];
                tempInfoArma.danoMax = _GameController.danoMaxArma[idArma];
                tempInfoArma.tipoDeDano = _GameController.tipoDanoArma[idArma];
            break;

            case 1: // arco
                arcos[0].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas1[idArma]; // pega o sprite da arma
                arcos[1].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas2[idArma]; 
                arcos[2].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas3[idArma]; 
            break;        
   
            case 2: // staff
            
                staffs[0].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas1[idArma]; // pega o sprite da arma
                staffs[1].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas2[idArma]; 
                staffs[2].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas3[idArma]; 
                staffs[3].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas4[idArma]; 
            break;        
  
        }
    }

    public  void tomeiHit() //quando o inimigo tomar um hit vai entrar em modo de alerta  maz a 
    {
        
        emAlertaHit = true; //se ele tomar hit vai da verdadeiro pra variavel
        StartCoroutine("hitAlerta");
        changeState(enemyState.ALERTA);
    }
    IEnumerator hitAlerta()
    {
        yield return new WaitForSeconds(tempoDeAlerta);
        emAlertaHit = false;
    }
    public void ChangeMaterial(Material novoMaterial)
    {
        sRender.material = novoMaterial; 
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
}
