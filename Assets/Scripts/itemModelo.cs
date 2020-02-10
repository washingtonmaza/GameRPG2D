using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Nova Arma", menuName="Arma")] // n preciso por em um objeto o script pra poder acessar ele

public class itemModelo : ScriptableObject // crio para n ficar vinculado ao monobehavior, atravez dele eu n preciso por o item na cena pra saber o id, posso pegar ele direto nos objetos interno
{
    public int idArma;

}
 