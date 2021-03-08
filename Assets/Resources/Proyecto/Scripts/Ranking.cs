using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Ranking : MonoBehaviour
{
    [SerializeField]
    List<string> DatosDeUsuarios;

    // link del servidor

    string ServerLocation = "";

    // nombre del archivo para registrarse en el servidor

    string RegistrarseLocation = "/";

    // nombre del archivo para iniciar sesión en el servidor

    string IniciarSecionLocation = "/";

    // variables que tomaran los archivos php en el servidor, separadas por un caracter de su preferencia, yo prefiero este '|' (recomiendo utilizar el mismo nombre para las variables de cada archivo)

    string values = "";

    GameObject content;
    Text mensaje;
    InputField usuario, contraseña;
    Color skinColor;

    void Awake()
    {
        content = GameObject.Find("Content");
        usuario = GameObject.Find("Usuario").transform.GetChild(0).GetComponent<InputField>();
        contraseña = GameObject.Find("Contraseña").transform.GetChild(0).GetComponent<InputField>();
        mensaje = GameObject.Find("Mensaje").GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MostrarRanking());
    }

    IEnumerator MostrarRanking()
    {
        // conexión con la web donde esta el ranking

        UnityWebRequest server = UnityWebRequest.Get(ServerLocation);

        yield return server.SendWebRequest();

        // si la conexión es exitosa se procede a mostrar el ranking

        if (server.result == UnityWebRequest.Result.Success)
        {
            // Conexión exitosa

            Debug.Log("Conexión exitosa");

            // primero se limpia el lugar donde aparecerá el ranking

            for (int s = 0; s < content.transform.childCount; s++)
                Destroy(content.transform.GetChild(s).gameObject);

            // incluyendo la lista de datos

            DatosDeUsuarios.Clear();

            // luego de tener todo limpio se obtienen los datos de la web (usuario, skin y score)

            string[] datos = server.downloadHandler.text.Split('|');

            // filtramos los datos para obtener solo el usuario, la skin y el score

            for (int s = 1; s < datos.Length; s+=2)
                DatosDeUsuarios.Add(datos[s]);

            // usamos un bucle para instanciar tantas plantillas de datos como sean necesarias y también se les asigna el valor adeacuado

            for (int s = 0; s < DatosDeUsuarios.Count / 3; s++)
            {
                // Plantilla
                
                GameObject Platilla = Instantiate(Resources.Load<GameObject>("Practica/Prefabs/Plantilla"), content.transform);

                // asignación del usuario

                Platilla.transform.GetChild(1).GetComponent<Text>().text = DatosDeUsuarios[s * 3];

                // asignación de la skin

                Image img = Platilla.transform.GetChild(0).GetComponent<Image>();
                img.sprite = Resources.Load<Sprite>("BayatGames/Free Platform Game Assets/Character/Character Animation ( Update 1.8 )/Run/1x");

                switch (DatosDeUsuarios[(s * 3) + 1])
                {
                    case "normal":
                        skinColor = Color.white;
                        break;
                    case "magenta":
                        skinColor = Color.magenta;
                        break;
                    case "verde":
                        skinColor = Color.green;
                        break;
                }
                img.color = skinColor;

                // asignación del score

                Platilla.transform.GetChild(2).GetComponent<Text>().text = DatosDeUsuarios[s * 3 + 2];

                // actualización del tamaño del contenido

                RectTransform contSize = content.GetComponent<RectTransform>();
                contSize.offsetMin = new Vector2(0, (-content.transform.childCount * 99) + contSize.offsetMax.y);
            }
        }
        else
        {
            // Error de conexión

            Debug.Log("Error de conexión");
        }
    }

    IEnumerator Registrarse()
    {
        // conexión con la web donde está el archivo php para registrarse

        UnityWebRequest server = UnityWebRequest.Get(ServerLocation + RegistrarseLocation + values.Split('|')[1] + usuario.text
                                                                                          + values.Split('|')[2] + contraseña.text
                                                                                          + values.Split('|')[3] + Character.score
                                                                                          + values.Split('|')[4] + Character.color);

        yield return server.SendWebRequest();

        // si la conexión es exitosa, se procede a ejecutar el archivo php del para registrarse

        if (server.result == UnityWebRequest.Result.Success)
        {
            // Conexión exitosa

            Debug.Log("Conexión exitosa");

            // si el archivo php manda mensaje de error, es porque ese usuario ya existía y no se registra

            if (server.downloadHandler.text.Substring(0, 6) == "_error")
            {
                Debug.Log("Usuario existente");
                mensaje.text = "USUARIO EXISTENTE";
            }

            // si el usuario no existía, el archivo php registrará al nuevo usuario en la base de datos y se actualizará el ranking

            else
            {
                StartCoroutine(MostrarRanking());
                mensaje.text = "USUARIO REGISTRADO";
            }
        }
        else
        {
            // Error de conexión

            Debug.Log("Error de conexión");
        }
    }

    IEnumerator IniciarSesion()
    {
        // conexión con la web donde está el archivo php para iniciar sesión

        UnityWebRequest server = UnityWebRequest.Get(ServerLocation + IniciarSecionLocation + values.Split('|')[1] + usuario.text
                                                                                            + values.Split('|')[2] + contraseña.text
                                                                                            + values.Split('|')[3] + Character.score
                                                                                            + values.Split('|')[4] + Character.color);

        yield return server.SendWebRequest();

        // si la conexión es exitosa se procede a ejecutar el archivo php del para iniciar sesión

        if (server.result == UnityWebRequest.Result.Success)
        {
            // Conexión exitosa

            Debug.Log("Conexión exitosa");

            // si el archivo php manda mensaje de error, es porque ese usuario no existe y no se inicia la sesión

            if (server.downloadHandler.text.Substring(0, 6) == "_error")
            {
                Debug.Log("Error en los datos");
                mensaje.text = "ERROR EN LOS DATOS";
            }

            // si el usuario existe, el archivo php revisará si supero su puntuación registrada

            else
            {
                // si no se supero la puntuación registrada entonces no se actualiza la puntuación

                if (server.downloadHandler.text.Substring(20, 30) == "no supero la puntuación maxima")
                {
                    Debug.Log("no se supero la puntuación maxima");
                    mensaje.text = "NO SE SUPERO LA PUNTUACIÓN MAXIMA";
                }

                // si supero la puntuación registrada entonces el archivo php actualizará el usuario en la base de datos y se actualizará el ranking

                else
                {
                    StartCoroutine(MostrarRanking());
                    mensaje.text = "USUARIO ACTUALIZADO";
                }
            }
        }
        else
        {
            // Error de conexión

            Debug.Log("Error de conexión");
        }
    }

    public void REG()
    {
        // se le asigna función de registrarse a un botón por este medio

        StartCoroutine(Registrarse());
    }

    public void LOG()
    {
        // se le asigna función de iniciar sesión a un botón por este medio

        StartCoroutine(IniciarSesion());
    }
}
