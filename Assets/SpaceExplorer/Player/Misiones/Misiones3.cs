using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Misiones3 : MonoBehaviour
{

    [Header("Textos A mostrar")]
    public TextMeshProUGUI TextoaMostrar;
    public TextMeshProUGUI TextoMision;

    private int misionPorHacerN;
    private int misionHechaN;

    private bool noTengoMision;
    private bool TengoMision;
    private int misionN;

    [SerializeField] Canvas[] conversationCanvas;
    [SerializeField] GameObject[] gameObjectToActive;

    GameObject NextConversation=null;

    // Start is called before the first frame update
    void Start()
    {
        TextoaMostrar.text = "Sin misión asignada.";
        TextoMision.text = "";
        //noTengoMision = true;


        TengoMision = true;
        misionN = 1;
        asignaMision(misionN);


    }

    // Update is called once per frame
    void Update()
    {
        /*if (noTengoMision) {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                TengoMision = true;
                misionN = 1;
                asignaMision(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                misionN = 2;
                TengoMision = true;
                asignaMision(2);
            }
        }*/
        
    }
    public void asignaMision(int n)
    {
        gameObjectToActive[misionN-1].SetActive(false);

        misionN = n;
        TengoMision = true;
        switch (n)
        {
            case 1:
                TextoaMostrar.text = "Llega al destino";
                misionPorHacerN = 1;
                misionHechaN = 0;
                TextoMision.text = misionHechaN + " / " + misionPorHacerN;
                break;
            case 2:
                TextoaMostrar.text = "Recupera chatarra";
                misionPorHacerN = 7;
                misionHechaN = 0;
                TextoMision.text = misionHechaN + " / " + misionPorHacerN;
                break;
            case 3:
                TextoaMostrar.text = "Vuelve a la estacion";
                misionPorHacerN = 1;
                misionHechaN = 0;
                TextoMision.text = misionHechaN + " / " + misionPorHacerN;
                break;

        }
        gameObjectToActive[misionN-1].SetActive(true);
        PlayerMovementController.currentObjetive = gameObjectToActive[misionN - 1];
    }

    public void actualizaMision(int n)
    {
        if (TengoMision && n == misionN)
        {
            switch (n)
            {
                
                case 1:
                    TextoaMostrar.text = "Llega al destino";
                    misionPorHacerN = 1;
                    misionHechaN = misionHechaN + 1;
                    TextoMision.text = misionHechaN + " / " + misionPorHacerN;
                    if (misionHechaN >= misionPorHacerN)
                    {
                        TextoaMostrar.text = "Misión completada!";
                        TextoMision.text = "";
                        StartCoroutine(CompletadaMision());
                    }
                    break;
                case 2:
                    TextoaMostrar.text = "Recupera chatarra";
                    misionPorHacerN = 7;
                    misionHechaN = misionHechaN + 1;
                    if (misionHechaN >= misionPorHacerN)
                    {
                        TextoaMostrar.text = "Misión completada!";
                        TextoMision.text = "";
                        StartCoroutine(CompletadaMision());
                    }
                    else
                    {
                        TextoMision.text = misionHechaN + " / " + misionPorHacerN;
                    }
                    break;
                case 3:
                    TextoaMostrar.text = "Vuelve a la estacion";
                    misionPorHacerN = 1;
                    misionHechaN = misionHechaN + 1;
                    TextoMision.text = misionHechaN + " / " + misionPorHacerN;
                    if (misionHechaN >= misionPorHacerN)
                    {
                        TextoaMostrar.text = "Misión completada!";
                        TextoMision.text = "";
                        StartCoroutine(CompletadaMision());
                    }
                    break;
            }
        }
    }

    IEnumerator CompletadaMision()
    {
        CreateNewConversation();
        TengoMision = false;
        yield return new WaitForSecondsRealtime(5f);
        noTengoMision = true;
        /*TextoaMostrar.text = "Tienes una llamada";
        TextoMision.text = "";*/

    }


    //Conversation functions for player
    private void CreateNewConversation()
    {
        if(misionN - 1 < conversationCanvas.Length)
        {
            if (conversationCanvas[misionN-1]!= null)
            {
                NextConversation = Instantiate(conversationCanvas[misionN - 1].gameObject);
                ConversationManager cm = NextConversation.GetComponent<ConversationManager>();
                cm.SetMision(gameObject, this);
            }
            else
            {
                asignaMision(misionN + 1);
            }
        }
        
    }

    public int GetCurrentMission()
    {
        return misionN;
    }

    public bool IsConversationEnabled()
    {
       return (NextConversation != null) ;
    }
    public bool ActiveNextConversation()
    {
        if(NextConversation != null)
        {
            NextConversation.SetActive(true);
            return true;
        }
        return false;
    }
}


