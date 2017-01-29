using UnityEngine;
using System.Collections;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI player_ui;
    public GameObject target;

    void Awake()
    {
        player_ui = this;
    }

    public void ShowUI()
    {
        target.SetActive(true);
    }

    public void HideUI()
    {
        target.SetActive(false);
    }
}
