using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Teleporter : MonoBehaviour {


    public GameObject m_Pointer; //SI LI FIQUES UN COLLIDER AL OBJECTE FA COM SI EL MANDO ABSORVIRA BOLETES
    public SteamVR_Action_Boolean m_TeleportAction;

    private SteamVR_Behaviour_Pose m_Pose = null;
    private bool m_HasPosition = false;
    private bool m_IsTeleporting = false;
    private float m_FadeTime = 0.5f;

    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    void Update () {

        //Pointer
        m_HasPosition = UpdatePointer(); //Actualitza la posicio del pointer i al mateix temps torna un booleano per a saber si esta tocant el que toca
        m_Pointer.SetActive(m_HasPosition); //Si te posició activem el pointer

        //Teleport
        if (m_TeleportAction.GetStateUp(m_Pose.inputSource))
        {
            Debug.Log("TryTeleport");
            TryTeleport(); //intentem fer el teleport si se cumpleixen les condicions
        }

	}

    private void TryTeleport()
    {
        //Check for valid position, and if already teleporting
        if (!m_HasPosition || m_IsTeleporting)
        {
            return;
        }

        //Get camera rig, and head position
        Transform cameraRig = SteamVR_Render.Top().origin; // Mira on esta la camera dins del camera rig?
        Vector3 headPosition = SteamVR_Render.Top().head.position; //Mira on esta la posicio del cap

        //Figure out translation

        Vector3 groundPosition = new Vector3(headPosition.x, cameraRig.position.y, headPosition.z);
        Vector3 translateVector = m_Pointer.transform.position - groundPosition; //vector sobre donde apuntas i donde estas

        //Move
        StartCoroutine(MoveRig(cameraRig, translateVector));
    }

    private IEnumerator MoveRig(Transform cameraRig, Vector3 translation) //Serveix per a fer la espera del fade i despres moure el camera rig al lloc indicat
    {
        //flag 
        m_IsTeleporting = true;

        //fade to black
        SteamVR_Fade.Start(Color.black, m_FadeTime, true);

        //aply transition

        yield return new WaitForSeconds(m_FadeTime);
        cameraRig.position += translation;

        //fade to clear
        SteamVR_Fade.Start(Color.clear, m_FadeTime, true);

        //de-flag
        m_IsTeleporting = false;

    }

    private bool UpdatePointer()
    {
        // Ray from the controller
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        //if it's a hit
        if (Physics.Raycast(ray, out hit)) //Se pot ficar un layer mask(o un Tag) per a que soles se teleporte a certes superficies
        {
            m_Pointer.transform.position = hit.point;
            return true;

        }
        // If not a hit
        return false;
    }

}
