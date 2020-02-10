using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // para usar as interfaces da unity
using UnityEngine.SceneManagement; //mudar de cena


public class MudarCena : MonoBehaviour
{
    private     audioController     audioController;
    public      string              cenaDestino;

    private     fade                fade;
    private     _GameController     _GamerControler;
    void Start()
    {
        fade = FindObjectOfType(typeof(fade)) as fade;
        _GamerControler = FindObjectOfType(typeof(_GameController)) as _GameController;
        audioController = FindObjectOfType(typeof(audioController)) as audioController;
    }

    
    void Update()
    {
        
    }

    public void interacao()
    {
        StartCoroutine("mudancaCena");
    }
 
    IEnumerator mudancaCena()
    {
        fade.fadeIn();
        yield return new WaitWhile(() => fade.fume.color.a < 0.9f); // vai esperar diminuir para acionar o transform de baixo
        var Randomint = UnityEngine.Random.Range(0,4); // vai pegar um random de 0 a 2 e vai jogar na variavel debaixo
        audioController.trocarMusica(audioController.musicaFase[Randomint], cenaDestino, true);
        //SceneManager.LoadScene(cenaDestino); //estou dizendo que vai pra cena que está na variavel criada em cima
        if(cenaDestino == "titulo") {Destroy(_GamerControler.gameObject);}

    }

}
