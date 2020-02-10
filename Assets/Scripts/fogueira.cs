using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //ter controle na troca de cenas 

public class fogueira : MonoBehaviour
{
    

    //private string                  novaCena = "cena1";
    


    void Start()
    {
         StartCoroutine("sairIntro");
    }

    // Update is called once per frame


    IEnumerator sairIntro()
    {
       
            yield return new WaitForSecondsRealtime(3f); // vai esperar esse tempo e ir abaixando a musica O REALTIME MESMO O JOGO EM PAUSE
         
            SceneManager.LoadScene("cena1");
        
    }
}
