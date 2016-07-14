using UnityEngine;
using System.Collections;

// Action Defination

public class MenuAction
{
    bool MenuButton;

    public MenuAction()
    {
        MenuButton = false;
    }

    public void Update()
    {
        MenuButton = OVRInput.Get(OVRInput.RawButton.A);
        if (MenuButton)
        {
            Debug.Log("Hahahahaha");
        }
    }
}

public class InputMap : MonoBehaviour {

    MenuAction Action_Menu;

    // Use this for initialization
    void Start () {

        Action_Menu = new MenuAction();

	}
	
	// Update is called once per frame
	void Update () {

        Action_Menu.Update();

    }
}
