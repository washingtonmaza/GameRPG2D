using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    private fade                fade;
    private playerScript        playerScript;
    public Transform           transformPlayer;
    public  Transform           destino;
    public  bool                escuro; // vai verificar se o ambiente é escuro ou nao 
    public  Material            luz2D, Padrao2D; 


    void Start()
    {
        fade = FindObjectOfType(typeof(fade)) as fade; // estou pegando o script fade para usar nesse script
        playerScript = FindObjectOfType(typeof(playerScript)) as playerScript; // estou pegando o script do player para usar nesse script

    }
    public void interacao()
    {
       
       StartCoroutine("acionarPorta");
    }


     
     IEnumerator acionarPorta()
    {
        fade.fadeIn();
        yield return new WaitWhile(() => fade.fume.color.a < 0.9f); // vai esperar diminuir para acionar o transform de baixo
        playerScript.gameObject.SetActive(false);
        switch(escuro) 
        {
            case true:
            playerScript.ChangeMaterial(luz2D); // se a porta levar para ambiente escuro o player vai ficar sensivel a luz
            break;

            case false:
             playerScript.ChangeMaterial(Padrao2D); // se a porta levar para ambiente claro o player vai ficar normal
            break;

        }


        playerScript.transform.position = destino.position; //ele vai mudar a posição do personagem de acordo com a posição da porta
        playerScript.gameObject.SetActive(true);
        fade.fadeOut(); 
    }
}
 