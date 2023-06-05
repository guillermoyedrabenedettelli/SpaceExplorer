using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    // Start is called before the first frame update
    void Start()
    {
        TextoaMostrar.text = "Sin misión asignada.";
        TextoMision.text = "";
        noTengoMision = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (noTengoMision) {
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
        }
        
    }
    private void asignaMision(int n)
    {
        switch (n)
        {
            case 1:
                TextoaMostrar.text = "Llega al destino.";
                misionPorHacerN = 1;
                misionHechaN = 0;
                TextoMision.text = misionHechaN + " / " + misionPorHacerN;
                break;
            case 2:
                TextoaMostrar.text = "Acaba con los enemigos.";
                misionPorHacerN = 3;
                misionHechaN = 0;
                TextoMision.text = misionHechaN + " / " + misionPorHacerN;
                break;
            
        }
    }

    public void actualizaMision(int n)
    {
        if (TengoMision && n == misionN)
        {
            switch (n)
            {
                
                case 1:
                    TextoaMostrar.text = "Llega al destino.";
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
                    TextoaMostrar.text = "Acaba con los enemigos.";
                    misionPorHacerN = 3;
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
            }
        }
    }

    IEnumerator CompletadaMision()
    {
        TengoMision = false;
        noTengoMision = false;
        yield return new WaitForSecondsRealtime(5f);
        noTengoMision = true;
        TextoaMostrar.text = "Sin misión asignada.";
        TextoMision.text = "";
    }
}


