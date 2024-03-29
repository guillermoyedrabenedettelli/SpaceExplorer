using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Misiones3 : MonoBehaviour
{

    [Header("Textos A mostrar")]
    public TextMeshProUGUI TextoMostrar;
    public TextMeshProUGUI TextoMision;
    public TextMeshProUGUI TextoMoney;
    public int Money = 0;

    private int misionPorHacerN;
    private int misionHechaN;

    //private bool noTengoMision;
    //private bool TengoMision;
    [SerializeField] int misionN=1;

    [SerializeField] Canvas[] conversationCanvas;
    [SerializeField] GameObject[] gameObjectToActive;

    GameObject NextConversation=null;

    [SerializeField] PauseMenu Credits;

    // Start is called before the first frame update
    void Start()
    {
        TextoMostrar.text = "Sin misi�n asignada.";
        TextoMision.text = "";
        TextoMoney.text = Money.ToString();
        //noTengoMision = true;


        //
        //TengoMision = true;
        asignaMision(misionN);
        foreach(GameObject go in gameObjectToActive)
        {
            go.SetActive(false);
        }


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
        switch (n)
        {
            case 1:
                TextoMostrar.text = "Llega al destino";
                misionPorHacerN = 1;
                misionHechaN = 0;
                TextoMision.text = misionHechaN + " / " + misionPorHacerN;
                break;
            case 2:
                TextoMostrar.text = "Recupera chatarra";
                misionPorHacerN = 7;
                misionHechaN = 0;
                TextoMision.text = misionHechaN + " / " + misionPorHacerN;
                break;
            case 3:
                TextoMostrar.text = "Vuelve a la estacion";
                misionPorHacerN = 1;
                misionHechaN = 0;
                TextoMision.text = misionHechaN + " / " + misionPorHacerN;
                break;
            case 4:
                TextoMostrar.text = "Encuentra a Mael Bahi y engancha el remolque a su nave";
                misionPorHacerN = 1;
                misionHechaN = 0;
                TextoMision.text = misionHechaN + " / " + misionPorHacerN;
                break;
            case 5:
                TextoMostrar.text = "Remolca a Mael Bahi hasta la estacion espacial";
                misionPorHacerN = 1;
                misionHechaN = 0;
                TextoMision.text = misionHechaN + " / " + misionPorHacerN;
                break;

        }
        if (misionN - 1< gameObjectToActive.Length)
        {
            gameObjectToActive[misionN - 1].SetActive(true);
            PlayerMovementController.currentObjetive = gameObjectToActive[misionN - 1];
            
        }
        else
        {
            if (Credits != null)
                Credits.PauseGame();
        }
    }

    public void actualizaMision(int n)
    {
        if (n == misionN)
        {
            switch (n)
            {
                
                case 1:
                    TextoMostrar.text = "Llega al destino";
                    misionPorHacerN = 1;
                    misionHechaN = misionHechaN + 1;
                    TextoMision.text = misionHechaN + " / " + misionPorHacerN;
                    if (misionHechaN >= misionPorHacerN)
                    {
                        TextoMostrar.text = "Misi�n completada!";
                        TextoMision.text = "";
                        StartCoroutine(CompletadaMision());
                        MoneyRewards(0);
                    }
                    break;
                case 2:
                    TextoMostrar.text = "Recupera chatarra";
                    misionPorHacerN = 7;
                    misionHechaN = misionHechaN + 1;
                    if (misionHechaN >= misionPorHacerN)
                    {
                        TextoMostrar.text = "Misi�n completada!";
                        TextoMision.text = "";
                        StartCoroutine(CompletadaMision());
                        MoneyRewards(0);
                    }
                    else
                    {
                        TextoMision.text = misionHechaN + " / " + misionPorHacerN;
                    }
                    break;
                case 3:
                    TextoMostrar.text = "Vuelve a la estacion";
                    misionPorHacerN = 1;
                    misionHechaN = misionHechaN + 1;
                    TextoMision.text = misionHechaN + " / " + misionPorHacerN;
                    if (misionHechaN >= misionPorHacerN)
                    {
                        TextoMostrar.text = "Misi�n completada!";
                        TextoMision.text = "";
                        StartCoroutine(CompletadaMision());
                        MoneyRewards(100);
                    }
                    break;
                case 4:
                    misionPorHacerN = 1;
                    misionHechaN = misionHechaN + 1;
                    TextoMision.text = misionHechaN + " / " + misionPorHacerN;
                    if (misionHechaN >= misionPorHacerN)
                    {
                        TextoMostrar.text = "Misi�n completada!";
                        TextoMision.text = "";
                        StartCoroutine(CompletadaMision());
                        MoneyRewards(0);
                    }
                    break;
                case 5:
                    misionPorHacerN = 1;
                    misionHechaN = misionHechaN + 1;
                    TextoMision.text = misionHechaN + " / " + misionPorHacerN;
                    if (misionHechaN >= misionPorHacerN)
                    {
                        TextoMostrar.text = "Misi�n completada!";
                        TextoMision.text = "";
                        StartCoroutine(CompletadaMision());
                        MoneyRewards(300);
                    }
                    break;
            }
        }
    }
    private void MoneyRewards(int MoneyR)
    {
        TextoMoney.text = Money.ToString() + "+ " +MoneyR;
        Money += MoneyR;
        TextoMoney.text = "$ " + Money.ToString();
    }
    IEnumerator CompletadaMision()
    {
        CreateNewConversation();
       //TengoMision = false;
        yield return new WaitForSecondsRealtime(5f);
        //noTengoMision = true;
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


