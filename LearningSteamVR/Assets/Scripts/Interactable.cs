using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // si volem tindre varios interactuables necessitem acço...
public class Interactable : MonoBehaviour {

    public Vector3 m_Offset = Vector3.zero; //per a posicionar el objecte a una certa distancia del controller

    [HideInInspector]
    public Hand m_ActiveHand = null; //per a saber quina ma esta agarrant este objecte en eixe moment


    public  virtual void Action() //virtual es per a que si tens scripts que van a inheriting el script Interactable pots fer un override per a posarli la funcionalitat que vulgues (per exemple si tens una pistola disparar i si tens una granada ficarli un contador)
    {
        print("Action");
    }

    public void ApplyOffset(Transform hand)
    {
        transform.SetParent(hand);
        transform.localRotation = Quaternion.identity; //pone valores a 0 //al hacer que pongamos a 0 la rotacion local, cojemos la rotacion del padre //si vulguerem tindre una espasa deuriem despres rotar 90 graus per a tindrela apuntant cap amunt
        transform.localPosition = m_Offset;
        transform.SetParent(null); //ja no tindra el mando o la ma com a pare
    }


}
