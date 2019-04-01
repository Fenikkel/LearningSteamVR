using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour {

    public SteamVR_Action_Boolean m_Trigger_Action = null; //Grabpinch (en el inspector)
    public SteamVR_Action_Boolean m_TouchpadAction = null; //Teleport (en el inspector)

    //public SteamVR_Action_Boolean m_GrabAction = null; //es per a lo comentat (es un poc mes complexe?)

    private SteamVR_Behaviour_Pose m_Pose= null;
    private FixedJoint m_Joint = null;

    private Interactable m_CurrentInteractable = null;
    public List<Interactable> m_ContactInteractables = new List<Interactable>(); //si un interactuable entra en contacte en la esfera collider, el posem en esta llista


    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Joint = GetComponent<FixedJoint>();
    }


	void Update () {

        //Trigger
        if (m_Trigger_Action.GetStateDown(m_Pose.inputSource))
        {
            print(m_Pose.inputSource + "Trigger Down");

            if (m_CurrentInteractable !=null) //Si ya tenemos un interactuable cogido, hacemos su accion(y no lo volvemos a recoger)
            {
                m_CurrentInteractable.Action();
                return;
            }

            PickUp();
        }

        //Touchpad

        if (m_TouchpadAction.GetStateDown(m_Pose.inputSource))
        {
            print(m_Pose.inputSource + "Touchpad Down");

            Drop();
        }




        //Per agafar objectes de la forma basica
        /*
         //Down
         if (m_GrabAction.GetStateDown(m_Pose.inputSource))
         {
             print(m_Pose.inputSource + "Trigger Down");
             PickUp();
         }

         //Up

         if (m_GrabAction.GetStateUp(m_Pose.inputSource))
         {
             print(m_Pose.inputSource + "Trigger UP");
             Drop();
         }
         */
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable")) //si no eres un objecte interactuable pasa de esta funcio
        {
            return;
        }

        m_ContactInteractables.Add(other.gameObject.GetComponent<Interactable>());
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable"))
        {
            return;
        }

        m_ContactInteractables.Remove(other.gameObject.GetComponent<Interactable>());
    }

    public void PickUp()
    {
        //Get nearest
        m_CurrentInteractable = GetNearestInteractable();

        //Null Ckeck (mirar si realment tenim algo que agafar)
        if (!m_CurrentInteractable) //comproba si es null
        {
            return;
        }

        //Already held, check (Si tenim algo que agafar, si ho te un altre controler, fer que el altre ho solte i este ho agafe)

        if (m_CurrentInteractable.m_ActiveHand) //si te una ma ja agafantlo
        {
            m_CurrentInteractable.m_ActiveHand.Drop();
        }

        //Position

        //m_CurrentInteractable.transform.position = transform.position; //si lo hacemos de la forma basica (la version que no hacemos acciones)

        m_CurrentInteractable.ApplyOffset(transform);

        //attach

        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
        m_Joint.connectedBody = targetBody;

        //set active hand

        m_CurrentInteractable.m_ActiveHand = this;
    }

    public void Drop()
    {

        //Null check

        if (!m_CurrentInteractable) 
        {
            return;
        }

        //Apply velocity (quan el soltem, volem que tinga la velocitat de la ma en eixe moment per a ser llançat)

        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
        targetBody.velocity = m_Pose.GetVelocity();
        targetBody.angularVelocity = m_Pose.GetAngularVelocity();

        //Detach
        m_Joint.connectedBody = null;

        //clear
        m_CurrentInteractable.m_ActiveHand = null;
        m_CurrentInteractable = null;
    }

    private Interactable GetNearestInteractable()
    {
        Interactable nearest = null;
        float minDistance = float.MaxValue; //li asigna un infinit al minDistance
        float distance = 0.0f;

        foreach (Interactable interactable in m_ContactInteractables)
        {
            distance = (interactable.transform.position - transform.position).sqrMagnitude; //no volem un vector 3 sino la distancia entre estos dos vectors i se fa en sqrMagnitud

            if (distance < minDistance)
            {

                minDistance = distance;
                nearest = interactable;

            }

        }

        return nearest;
    }
}
