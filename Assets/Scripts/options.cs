using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class options : MonoBehaviour
{
    private     audioController     audioController;
    public      Slider              volumeMusica;
    public      Slider              volumeFx;

    void Start()
    {
        audioController     = FindObjectOfType(typeof(audioController)) as audioController;
        volumeMusica.value  = audioController.volumeMaximoMusica; // DEFINE VALOR INICIAL DAS BARRINHAS 
        volumeFx.value      = audioController.volumeMaximoFx;
    }
  


    public void AlterarVolumeMusica()
    {
        float tempVolumeMusic = volumeMusica.value;
        audioController.volumeMaximoMusica = tempVolumeMusic;
        audioController.sMusic.volume = tempVolumeMusic;
        PlayerPrefs.SetFloat("volumeMaximoMusica", tempVolumeMusic); // ESTOU GRAVANDO OS VALORES
    }

    public void AlterarVolumeFx()
    {
        float tempVolumeFx = volumeFx.value;
        audioController.volumeMaximoFx = tempVolumeFx; // passo pra variavel o valor do volume 
        PlayerPrefs.SetFloat("volumeMaximoFx", tempVolumeFx);  // ESTOU GRAVANDO OS VALORES 
    }
}
