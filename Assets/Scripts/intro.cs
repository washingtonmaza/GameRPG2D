using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class intro : MonoBehaviour
{
    audioController         audioController;
    // Start is called before the first frame update
    void Start()
    {
        audioController = FindObjectOfType(typeof(audioController)) as audioController;
         
        StartCoroutine("chamarIntro");
    }

    // Update is called once per frame
    IEnumerator chamarIntro()
    {
       
            yield return new WaitForSecondsRealtime(10f); // vai esperar esse tempo e ir abaixando a musica O REALTIME MESMO O JOGO EM PAUSE
            SceneManager.LoadScene("preTitulo");
            

    }
}
