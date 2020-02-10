using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGlobin : MonoBehaviour
{
    public enum    rotina
    {
        A,B,C,D
    }
    private Rigidbody2D         bossRb;
    private Animator            bossAnimator;
    public  rotina              currentRotina;
    public  float               speed;
    private int                 h; // INDICA PRA ONDE ESTA ANDANDO
    private bool                isMove; //se tem q movimentar ou nao, usado pra hora de pular
    public  Transform[]         wayPoints; // É OS PONTOS NO MAPA ATE ONDE ELE VAI 
    private Transform           target;



    private int                 idEtapa;
    private float               tempTime;
    private float               waitTime;
    public  bool                isLookLeft;







    
    void Start()
    {
        bossAnimator = GetComponent<Animator>();
        bossRb       = GetComponent<Rigidbody2D>();

        // SETUP INICIAL
        currentRotina = rotina.A;
        idEtapa  = 0;
        tempTime = 0;
        waitTime = 3;
    }

    void Update()
    {


        switch(currentRotina)
        {
            case rotina.A: 
            switch(idEtapa)
            {
                case 0: // ESPERA 3 SEGUNDOS E DEFINE O DESTINO
                    tempTime += Time.deltaTime;  
                    if(tempTime >= waitTime)
                    {
                        idEtapa +=1;
                        target = wayPoints[1];
                        h = -1;
                        isMove = true;
                    }
                break;
                case 1: //MOVE ATE DESTINO
                    if(transform.position.x <= target.position.x)
                    {
                        idEtapa +=1;
                    }
                break;
                case 2:

                break;
            }
            break;

            case rotina.B: 
            break;

            case rotina.C: 
            break;

            case rotina.D: 
            break;


        }






        if (h > 0 && isLookLeft == true)
        {
            Flip();
        }
        else if (h < 0 && isLookLeft == false)
        {
            Flip();
        }
        if(isMove == true)
        {
            bossRb.velocity = new Vector2(h*speed, bossRb.velocity.y);
        }
        bossAnimator.SetInteger("h", h);
        
    }


    void Flip()
    {
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1; // VIRAR O OBJETO AO LADO OPOSTO
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
}



