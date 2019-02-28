using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveInput : MonoBehaviour {

    //[SteamVR_DefauldAction("Squeeze")] //ja no funciona, pera per a que se autoconfigurara soles cada vegada que entraves al Unity? i no generar cada vegada els controls
   
        //RECORDA: tambe hi ha que posar el script de Action_SetOnLoad

    SteamVR_Action_Boolean grabPinchAction = SteamVR_Actions._default.GrabPinch; //mes eficient

    public SteamVR_Action_Single squeezeAction;

    public SteamVR_Action_Vector2 touchPadAction;

	void Update () {

        if (SteamVR_Actions._default.Teleport.GetStateDown(SteamVR_Input_Sources.Any)) //versions anteriors se feia aixina: SteamVR_Input._default.inActions.Teleport.GetStateUp(SteamVR_Input_Sources.Any)
        {
            print("Teleport Down");
        }

        if (SteamVR_Actions._default.Teleport.GetStateUp(SteamVR_Input_Sources.Any)) //versions anteriors se feia aixina: SteamVR_Input._default.inActions.Teleport.GetStateUp(SteamVR_Input_Sources.Any)
        {
            print("Teleport UP");
        }

        if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any))
        {
            print("Grab pinch down");
        }

        if (grabPinchAction.GetStateUp(SteamVR_Input_Sources.Any))
        {
            print("Grab pinch up");
        }

        float triggerValue = squeezeAction.GetAxis(SteamVR_Input_Sources.Any);

        if(triggerValue > 0.0f)
        {
            print(triggerValue);
        }

        Vector2 touchPadvalue = touchPadAction.GetAxis(SteamVR_Input_Sources.Any);

        if(touchPadvalue != Vector2.zero)
        {
            print(touchPadvalue);
        }
        



    }
}
