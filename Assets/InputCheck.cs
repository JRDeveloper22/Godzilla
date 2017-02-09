using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCheck : MonoBehaviour {

    //public Text text;
    //public PlayerManager p1;
    public GameObject Next;
    public GameObject current;
    public GameObject Ch1;
    public GameObject Ch2;
    public GameObject CSword;
    public GameObject CTestubo;
    public GameObject Sword;
    public GameObject Testubo;
    public GameObject ThisPlayer;
    public GameObject NextObject;
    private bool coose = false;
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.A)&& !coose)
        {
           // text.text = "Keyboard";
            //p1.InputID = 0;
            coose = true;
            Next.SetActive(true);
            current.SetActive(true);
            
        }
        else if (Input.GetButtonDown("A") && !coose)
        {
            //text.text = "Controller 1";
            //p1.InputID = 1;
            coose = true;
            Next.SetActive(true);
            current.SetActive(true);
        }
        /* else if (Input.GetButtonDown("A_2") && !coose)
         {
             text.text = "Controller 2";
             p1.InputID = 1;
             coose = true;
             Next.SetActive(true);
             current.SetActive(true);
         }*/
        if (Ch1.activeSelf == true)
        {
           // p1.CharacterID = 0;
            CSword.SetActive(true);
            CTestubo.SetActive(true);
            Next.SetActive(false);
            current.SetActive(false);
            //ThisPlayer.SetActive(false);

        }
        else if (Ch2.activeSelf == true)
        {
            //p1.CharacterID = 1;
            CSword.SetActive(true);
            CTestubo.SetActive(true);
            Next.SetActive(false);
            current.SetActive(false);
            //ThisPlayer.SetActive(false);
        }

        if (Sword.activeSelf == true)
        {
           // p1.weaponID = 0;
            NextObject.SetActive(true);
            ThisPlayer.SetActive(false);
            
        }
        else if (Testubo.activeSelf == true)
        {
           // p1.weaponID = 1;
            NextObject.SetActive(true);
            ThisPlayer.SetActive(false);
        }
    }
}
