using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class reSkin : MonoBehaviour
{
    private    _GameController              _GameController; //vou usar pra enviar os dados que n pode ser apagado para o game controler
    public     bool                         isPlayer; //vai verificar se é um personagem ou monstro pois estou passando o skin pro game controler
    public     Sprite[]                     sprites; // um array de sprites que irei usar pra guardar todas sprites armazenadas no Resources
    public     SpriteRenderer               sRender; // vou ter acesso aos sprites
    public     string                       spriteSheetName; //vou salvar aqui o nome dos sprites
    public     string                       loadedSpriteSheetName; // vou verificar qual sprite está carregado, pois a pessoa pode trocar de personagem
    private    Dictionary<string, Sprite>   spriteSheet; // criei um dicionadio para passar valores 
    void Start()
    {
        _GameController = FindObjectOfType(typeof(_GameController)) as _GameController; // instanciei o objeto criado
        sRender = GetComponent<SpriteRenderer>(); //instanciei o spriterender pra ter acesso depois que crio  
        LoadSpriteSheet(); 
        if(isPlayer)
        {
            spriteSheetName = _GameController.spriteSheetName[_GameController.idPersonagem]; // estou enviando para a variavel o id do personagem 
        }
        _GameController.validarArma();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(loadedSpriteSheetName != spriteSheetName)
        {
            LoadSpriteSheet(); // se o nome da sprite que esta no carregado for diferente do nome que está guardado ele vai buscar a sprite dentro do Resources e troca automatico
        }
        sRender.sprite = spriteSheet[sRender.sprite.name]; // como é um array eu uso o [] bara buscar dentro do array o nome correspondente e jogar dentro do sRender
    }

     private void LoadSpriteSheet()
     {
         sprites = Resources.LoadAll<Sprite>(spriteSheetName); // eu joguei dentro do sprite todos os nomes de Sprites que está na pasta Resources
         spriteSheet = sprites.ToDictionary(x=> x.name, x=>x); // o X é a cordenada, ai vamos por o proprio sprite dentro do spritesheet
         loadedSpriteSheetName = spriteSheetName; //passei o nome do spritesheet pra dentro do loaded pra futuramente saber qual sprite está sendo usado.
     }
}
