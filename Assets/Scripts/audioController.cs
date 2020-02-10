using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //ter controle na troca de cenas 

public class audioController : MonoBehaviour
{
    public  AudioSource             sMusic; // EFEITOS DE MUSICA 
    public  AudioSource             sFx; // EFEITOS SONOROS

    [Header ("Musicas")]
    public  AudioClip               musicaTitulo;
    public  AudioClip               musicaFase1;
    public  AudioClip[]               musicaFase;
    public  AudioClip[]             musicaTriste; 


    [Header("Sounds Efex")]
    public  AudioClip               fxClick; //quando clica em algum botao 
    public  AudioClip               fxSword;
    public  AudioClip               fxAxe;
    public  AudioClip               fxBow;
    public  AudioClip               fxStaff;
    public  AudioClip               fxHealthP;
    public  AudioClip               fxManaP;
    public  AudioClip[]             fxAtackFailed;
    public  AudioClip[]             fxAtackSucess;
    public  AudioClip[]             levelUp;
    public  AudioClip[]             fxSons; // qualquer som aleatorio que eu queira guardar
            

    //CONFIGURACOES DO AUDIO

    public float                   volumeMaximoMusica;
    public float                   volumeMaximoFx;

    //CONFIGURACOES DA TROCA DE MUSICA

    private AudioClip               novaMusica;
    private string                  novaCena;
    private bool                    trocarCena;



    private         bool                jaCarregouIntro = false;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject); // SERVE PARA NAO DESTRUIR ESSE OBJETO NA TROCA DE CENAS 

        if (PlayerPrefs.GetInt("valoresIniciais") == 0)
        {
            PlayerPrefs.SetInt("valoresIniciais", 1);
            PlayerPrefs.SetFloat("volumeMaximoMusica", 1);
            PlayerPrefs.SetFloat("volumeMaximoFx", 1);

        }
        //CARREGA AS CONFIGURACOES DE AUDIO DO APARELHO
        volumeMaximoMusica          = PlayerPrefs.GetFloat("volumeMaximoMusica");
        volumeMaximoFx              = PlayerPrefs.GetFloat("volumeMaximoFx"); // estou pegando na preferencias o volume maximo

        
            StartCoroutine("intro");
           
       

        //sMusic.clip                 = musicaTitulo;
        //sMusic.volume               = volumeMaximoMusica;
        //sMusic.Play(); //tocar a musica depois de fazer as configuracoes



        

    }

    public void trocarMusica(AudioClip clip, string nomeCena, bool mudarCena)
    {
        novaMusica      = clip;
        novaCena        = nomeCena;
        trocarCena      = mudarCena;

        

        StartCoroutine("changeMusic");
    }

    // CORROTINA PRA ABAIXAR A MUSICA ATUAL, TROCAR PRA NOVA MUSICA E AUMENTAR PRA NOVA MUSICA
    IEnumerator intro()
    {
        yield return new WaitForSeconds(10f); //vai esperar esse tempo para poder atacar novamente
        trocarMusica(musicaTitulo, "titulo", true);
    }
    IEnumerator changeMusic()
    {
        for (float volume = volumeMaximoMusica; volume>=0; volume-=0.1f)
        {
            yield return new WaitForSecondsRealtime(0.01f); // vai esperar esse tempo e ir abaixando a musica O REALTIME MESMO O JOGO EM PAUSE
            sMusic.volume = volume;
        }

        sMusic.volume = 0; // quando terminar o for ele vai estar em 0 entao pra evitar erros o vlume recebe 0 para garantir o processo

        sMusic.clip = novaMusica; //ele vai abaixar toda musica e vai começar a tocar a outra

        sMusic.Play(); // começa a tocar
        for (float volume = 0; volume < volumeMaximoMusica; volume+=0.1f)
        {
            yield return new WaitForSeconds(0.01f); // vai esperar esse tempo e ir abaixando a musica
            sMusic.volume = volume;
        }
        sMusic.volume = volumeMaximoMusica; // quando fizer todo processo de abaixar musica trocar musica colocar nova musica aumentar volume, ele agora recebe o valor total da musica

        if(trocarCena == true)
        {
            SceneManager.LoadScene(novaCena);
        }
    }

    public void tocarFx(AudioClip fx, float volume)
    {
        
        float   tempVolume = volume;
        if(volume > volumeMaximoFx) // se o volume passar da configuracao ele n vai tocar mais alto que o definido
        { 
            tempVolume = volumeMaximoFx;
        }
        sFx.volume = tempVolume;// estou garantindo q o valor n passa do que eu quero
        sFx.PlayOneShot(fx); // oneshot apenas uma vz
     
    }

}

