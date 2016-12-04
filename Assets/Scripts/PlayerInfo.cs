using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo player;
	
    // Use this for initialization
	void Awake ()
    {
        player = this;
        //Camera.main.GetComponent<CameraMove>().player = this.gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
