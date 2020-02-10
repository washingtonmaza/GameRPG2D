using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //mudar de cena
using UnityEngine.UI;


using System.IO; 

public class titulo : MonoBehaviour
{

    private         audioController     audioController;
    public          Button              btnCarregarJogo;
    public          Button              btnCarregarSlot1;
    public          Button              btnCarregarSlot2;
    public          Button              btnCarregarSlot3;


    public          Button              btnNovoSlot1;
    public          Button              btnNovoSlot2;
    public          Button              btnNovoSlot3;



    public          GameObject          btnDelete1;
    public          GameObject          btnDelete2;
    public          GameObject          btnDelete3;


    void Start()
    {
        audioController = FindObjectOfType(typeof(audioController)) as audioController;
        verificarSaveGame();
    }



    public void selecionarPersonagem(int idPersonagem)
    {
        PlayerPrefs.SetInt("idPersonagem", idPersonagem); // estou salvando em preferencias o id do personagem para n apagar
        SceneManager.LoadScene("load");
    }

    void verificarSaveGame() 
    {
        btnCarregarJogo.interactable    = true;

        btnCarregarSlot1.interactable   = true;
        btnCarregarSlot2.interactable   = true;
        btnCarregarSlot3.interactable   = true;


        btnNovoSlot1.interactable       = true;
        btnNovoSlot2.interactable       = true;
        btnNovoSlot3.interactable       = true;


        btnDelete1.SetActive(false); // DESATIVO OS BOTOES DO SLOT DE SALVAR CASO N TENHA SAVE
        btnDelete2.SetActive(false);
        btnDelete3.SetActive(false);




        if (File.Exists(Application.persistentDataPath+"/playerdata1.dat"))
        {
            btnNovoSlot1.interactable = true;
            btnNovoSlot1.interactable = false;
            btnDelete1.SetActive(true);
        }

        if (File.Exists(Application.persistentDataPath+"/playerdata2.dat"))
        {
            btnNovoSlot2.interactable = true;
            btnNovoSlot2.interactable = false;
            btnDelete2.SetActive(true);
        }

        if (File.Exists(Application.persistentDataPath+"/playerdata3.dat"))
        {
            btnNovoSlot3.interactable = true;
            btnNovoSlot3.interactable = false;
            btnDelete3.SetActive(true);
        }

        if(btnCarregarSlot1.interactable == true || btnCarregarSlot2.interactable == true || btnCarregarSlot3.interactable == true )
        {
            btnCarregarJogo.interactable = true;
        }
    
    }

    public void novoJogo(int slot) 
    {
        switch(slot)
        {   
            case 1:
                PlayerPrefs.SetString("slot", "playerdata1.dat");
                break;
            case 2:
                PlayerPrefs.SetString("slot", "playerdata2.dat");
                break; 
            case 3:
                PlayerPrefs.SetString("slot", "playerdata3.dat");
                break;
        }  
    }

    public void carregarJogo(int slot) 
    {
        print("carregou o carregar jogo");
        switch(slot)
        {   
            case 1:
                PlayerPrefs.SetString("slot", "playerdata1.dat");
                break;
            case 2:
                PlayerPrefs.SetString("slot", "playerdata2.dat");
                break; 
            case 3:
                PlayerPrefs.SetString("slot", "playerdata3.dat");
                break;
        }  

        SceneManager.LoadScene("load"); // apos selecionar a cena eu chamo a cena load
    }


    public void deleteSave(int slot)
    {
        switch(slot)
        {
            case 1:
                if(File.Exists(Application.persistentDataPath+"/playerdata1.dat"))
                {
                    File.Delete(Application.persistentDataPath+ "/playerdata1.dat");
                }
                break;
             case 2:
                if(File.Exists(Application.persistentDataPath+"/playerdata2.dat"))
                {
                    File.Delete(Application.persistentDataPath+ "/playerdata2.dat");
                }
                break; 
             case 3:
                if(File.Exists(Application.persistentDataPath+ "/playerdata3.dat"))
                {
                    File.Delete(Application.persistentDataPath+ "/playerdata3.dat");
                } 
                break;
        }
        verificarSaveGame();
    }

    public void Click()
    {
        audioController.tocarFx(audioController.fxClick, 1); // ele vai chamar o audio do click atravez dessa funcao
    }
}