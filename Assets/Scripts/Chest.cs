 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private     SpriteRenderer      SpriteRenderer;
    public      Sprite[]            imagemObjeto; //fazer mais de um  bau
    public      bool                open; // abrir bau
    public      GameObject[]          loots;

    private     bool                gerouLoot;
    public      int                 qtdMinItens, qtdMaxItens;
    void Start()
    {
      
        SpriteRenderer = GetComponent<SpriteRenderer>();

      
    }
public void interacao()
        {
            if(open == false)
            {
                open = true;
                SpriteRenderer.sprite = imagemObjeto[1];
                StartCoroutine("gerarLoot"); 
                GetComponent<Collider2D>().enabled = false;
            }
        }
    IEnumerator gerarLoot()
    {
            
            gerouLoot = true; //passo para verdadeiro para abrir o vau só uma vez
            int qntMoedas = Random.RandomRange(qtdMinItens,qtdMaxItens); // gera de 1 a 10 moedas
            for (int l = 0; l <= qntMoedas; l++ ){
                int rand = 0;
                int idLot = 0; // verifica qual o valor para criar as moedas
                rand = Random.Range(0,100); //fiz um random para tentar sair outro item que n seja a mesma moeda dourada

                if (rand <= 85)
                {
                    idLot = 0; // MOEDAS
                }
                if (rand >85 && rand <=90)
                {
                    idLot = 1; // ESMERALDA
                }
                if (rand >90 && rand <=95)
                {
                    idLot = 2; // SAFIRA
                }
                if (rand >95 && rand <=99)
                {
                    idLot = 3; // RUBI
                }
                if (rand >99 && rand <=100)
                {
                    idLot = 4; // DIAMANTE
                }
                
               

                GameObject lootTemp = Instantiate(loots[idLot], transform.position, transform.localRotation); 
                lootTemp.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-25,25), 80));
                 yield return new WaitForSeconds(0.1f);
            }
    }
}
