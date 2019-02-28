using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // si volem tindre varios interactuables necessitem acço...
public class Interactable : MonoBehaviour {


    [HideInInspector]
    public Hand m_ActiveHand = null; //per a saber quina ma esta agarrant este objecte en eixe moment



}
