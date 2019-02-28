using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Haptics : MonoBehaviour {


    public SteamVR_Action_Vibration hapticAction; // actions/default/out/haptic
    public SteamVR_Action_Boolean trackpadAction; // actions/default/in/teleport

    //Per a que aço funcione, has d'anyadir al GameObject que te este script un altre script que es diu
    //ActionSetONLoad y ficar el ActionSet que anem a utilitzar

    void Update () {

        if (trackpadAction.GetLastStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Pulse(4, 320, 1, SteamVR_Input_Sources.LeftHand);
        }
        if (trackpadAction.GetLastStateDown(SteamVR_Input_Sources.RightHand))
        {
            Pulse(1, 75, 0.2f, SteamVR_Input_Sources.RightHand);

        }

    }

    private void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {

        
        /// Trigger the haptics at a certain time for a certain length
        
        /// secondsFromNow --> How long from the current time to execute the action (in seconds - can be 0)
        /// durationSeconds --> How long the haptic action should last (in seconds)
        /// frequency --> How often the haptic motor should bounce (0 - 320 in hz. The lower end being more useful)
        /// amplitude --> How intense the haptic action should be (0 - 1)
        /// inputSource --> The device you would like to execute the haptic action. Any if the action is not device specific.

        hapticAction.Execute(2, duration, frequency, amplitude, source);

        print("Pulse" + " " + source.ToString());
    }

}
