using UnityEngine;
using System.Collections;

public class AnchorMap : MonoBehaviour {

    public GameObject oculusAnchor;
    public GameObject steamAnchor;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        switch (CameraRig.cameraRigPlatform)
        {
            case CameraRigPlatform.None:
                break;

            case CameraRigPlatform.SteamVR:
                transform.position = steamAnchor.transform.position;
                transform.rotation = steamAnchor.transform.rotation;
                transform.localScale = steamAnchor.transform.localScale;
                break;

            case CameraRigPlatform.OculusVR:
                transform.position = oculusAnchor.transform.position;
                transform.rotation = oculusAnchor.transform.rotation;
                transform.localScale = oculusAnchor.transform.localScale;
                break;
        }

	}
}
