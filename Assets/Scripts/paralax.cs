using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paralax : MonoBehaviour
{
    public      Transform       background;
    public      Transform       cam;
    private     Vector3         previewCamPosition;

    public      float           paralaxScale; 
    public      float           velocidade; //velocidade que se movimenta

    void Start()
    {
        cam = Camera.main.transform;
        previewCamPosition = cam.position;
    }  

    // Update is called once per frame
    void LateUpdate()
    {
        float   ParalaxX    = (previewCamPosition.x - cam.position.x) * paralaxScale;
        float   bgTargetX   = background.position.x + ParalaxX; 
        Vector3 bgPos       = new Vector3(bgTargetX, background.position.y, background.position.y);
        background.position = Vector3.Lerp(background.position, bgPos, velocidade*Time.deltaTime); //pegou pelo deltatime a velocidade do frame direto no processador para independente do processador eles terem a mesma velocidade
        previewCamPosition = cam.position;
    }
}
