using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; // para el manejo de RED
using TMPro;
using System;

public class Red : MonoBehaviour
{
    public TMP_Text resultado;

    //Enviar
    public TMP_InputField ifNombre;
    public TMP_InputField ifPuntos;

    // La estructura que se convierte a JSON
    public struct DatosUsuario
    {
        public string nombre;
        public int puntos;
    }
    public DatosUsuario datos;   //al servidor
    
    //Click del botón leer JSON
    public void LeerJSON()
    {
        StartCoroutine(descargarJSON());
    }

    IEnumerator descargarJSON()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://10.48.115.1/unity/enviaJSON.php");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            //Descarga exitosa
            string textoJson = request.downloadHandler.text;
            resultado.text = textoJson;

            DatosUsuario datos = JsonUtility.FromJson<DatosUsuario>(textoJson);

            resultado.text = resultado.text + "\n\n" + datos.nombre + " - " + datos.puntos;
        }
        else
        {
            resultado.text = "Error en la descarga " + request.responseCode.ToString();
        }
        request.Dispose();
    }

    //Click del botón Subir Json
    public void EscribirJSON()
    {
        StartCoroutine(SubirJSON());
    }

    IEnumerator SubirJSON()
    {
        //Llenar los campos de la estructura
        datos.nombre = ifNombre.text;
        datos.puntos = Int32.Parse(ifPuntos.text);

        WWWForm forma = new WWWForm();
        forma.AddField("datosJSON", JsonUtility.ToJson(datos));
        print(JsonUtility.ToJson(datos));

        UnityWebRequest request = UnityWebRequest.Post("http://10.48.115.1/unity/recibeJSON.php", forma);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            resultado.text = request.downloadHandler.text;
        }
        else
        {
            resultado.text = "Error en la descarga " + request.responseCode.ToString();
        }
    }

    //Click del botón Enviar texto
    public void EscribirTextoPlano()
    {
        StartCoroutine(SubirTextoPlano());
    }

    IEnumerator SubirTextoPlano()
    {
        WWWForm forma = new WWWForm();
        forma.AddField("nombre", ifNombre.text);
        forma.AddField("puntos", ifPuntos.text);

        UnityWebRequest request = UnityWebRequest.Post("http://10.48.115.1/unity/recibeTextoPlano.php", forma);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            resultado.text = request.downloadHandler.text;
        }
        else
        {
            resultado.text = "Error en la descarga " + request.responseCode.ToString();
        }
    }

    //Click del botón Descargar Texto Plano
    public void leerTextoPlano()
    {
        StartCoroutine(DescargarTextoPlano());
    }

    IEnumerator DescargarTextoPlano()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://10.48.115.1/unity/textoPlano.txt");
        yield return request.SendWebRequest(); 
        if(request.result == UnityWebRequest.Result.Success)
        {
            //Descarga exitosa
            resultado.text = request.downloadHandler.text;
        }
        else
        {
            resultado.text = "Error en la descarga " + request.responseCode.ToString();
        }
    }
}
