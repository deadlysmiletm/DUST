using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Console : MonoBehaviour
{
    //Variable creada para el asignarle funciones.
    public delegate void ConsoleCommand();
    public Dictionary<string, CommandData> allCommand = new Dictionary<string, CommandData>();

    public KeyCode openCloseConsole;
    public GameObject console;
    public Vector3[] positionOfConsole = new Vector3[2];
    public PlayerBrain player;
    public Text consoleText;
    public InputField consoleInput;
    private float _t = 0;
    private bool _active = false;
    private bool _godMode = false;
    private bool _map = false;
    private GameObject miniMapa;
    private bool _fast = false;
    private bool _normal = true;
    private bool _slow = false;

    public static Console instance;

    // Use this for initialization
    void Awake()
    {
        if (instance != null)
        {
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        player = GameObject.Find("Player").GetComponent<PlayerBrain>();

        //El registro de todos los códigos hasta el momento de la consola.
        RegisterCommand("Clear", Clear, "Limpiar la pantalla.");
        RegisterCommand("Help", Help, "Mostrar todos los comandos.");
        RegisterCommand("Map", Map, "(On/Off) Muestra/oculta el mapa del nivel.");
        RegisterCommand("GodMode", GodMode, "(On/Off) Activa/desactiva el modo dios.");
        RegisterCommand("Quit", Quit, "Cierra el juego.");
        RegisterCommand("Time", TimeScaler, "(Slow/normal/fast) Lento/normal/rápido, modifica la velocidad general del juego.");
    }

    private void Start()
    {
        console = GameObject.Find("VentanaConsola");
        positionOfConsole[1] = console.GetComponent<RectTransform>().anchoredPosition;
        miniMapa = GameObject.Find("Mini mapa");
        miniMapa.SetActive(false);

    }

    //Función encargada de agregar texto en la consola (que haya sido tipeado por alguien).
    public void Write(string txt)
    {
        consoleText.text += "\n" + txt;
    }

    //Función que se encarga de registrar los comandos.
    public void RegisterCommand(string cmdName, ConsoleCommand cmdCommand, string description)
    {
        var cdata = new CommandData();

        cdata.name = cmdName.ToLower();
        cdata.command = cmdCommand;
        cdata.description = description;

       allCommand.Add(cmdName.ToLower(), cdata);
    }

    //Función y código de la consola encargada de mostrar y ocultar el mini-mapa 
    public void Map()
    {
        if(_map)
        {
            if (!miniMapa.activeInHierarchy)
            {
                miniMapa.SetActive(true);
                Write("Mapa activado.");
            }
            else
            {
                Write("El mapa ya se encuentra activado.");
            }
        }
        else
        {
            if (miniMapa.activeInHierarchy)
            {
                miniMapa.SetActive(false);
                Write("Mapa desactivado.");
            }
            else
            {
                Write("El mapa ya se encuentra desactivado.");
            }
        }
    }

    //Función y código de la consola encargada de limpiar la consola.
    public void Clear()
    {
        consoleText.text = "";
    }

    public void Help()
    {
        foreach (var item in allCommand)
        {
            Write(item.Value.ToString());
        }
    }

    //Función y código de la consola encargada de activar el modo Dios (no puede caer al vacío, no puede saltar y puede atravezar todo).
    public void GodMode()
    {
        if(_godMode)
        {
            Write("GodMode activado.");
            player.gameObject.layer = LayerMask.NameToLayer("PlayerGod");
        }
        else
        {
            Write("GodMode desactivado");
            player.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    //Función y código de la consola encargada de manipular la velocidad (tiempo) del juego.
    public void TimeScaler()
    {
        if (_normal)
        {
            Time.timeScale = 1;
            Write("La velocidad ahora es normal.");

        }
        else if (_fast)
        {

            Time.timeScale = 2;
            Write("¡Ahora todo se mueve dos veces más rápido!");
        }
        else if (_slow)
        {
            Time.timeScale = .5f;
            Write("¡Ahora todo se mueve más lento!");
        }
    }

    //Función y código de la consola encargada de salir del juego.
    public void Quit()
    {
        Application.Quit();
    }

    void Update()
    {
        if (Input.GetKeyDown(openCloseConsole))
        {
            OpenClose();
        }

        MoveConsole();

        if (console.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                consoleInput.text = consoleInput.text.ToLower();

                ParameterChequeer();

                //Realiza intentos para evitar errores o, si tienen lugar, ser capaz de encontrar dónde se originaron y qué tipo era.
                if (allCommand.ContainsKey(consoleInput.text))
                {
                    try
                    {
                        allCommand[consoleInput.text].command.Invoke();
                    }
                    catch (NullReferenceException nullError)
                    {
                        Write("Hubo un error de referencia nula: " + "\n\n" + nullError.StackTrace);
                    }
                    catch (Exception error)
                    {
                        Write("Ha ocurrido un error: " + error.Message + "\n\n" + error.StackTrace);
                        return;
                    }
                    finally
                    {
                    }
                }
                else
                {
                    //Sale esta ventana si el código ingresado no existe en el diccionario de la consola.
                    Write("El comando ingresado no existe.");
                }

                consoleInput.text = "";
            }
        }
    }

    //Se encarga del movimiento al activar y desactivar la consola.
    private void MoveConsole()
    {
        console.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(positionOfConsole[0], positionOfConsole[1], _t);

        if (_active)
        {
            console.SetActive(true);

            if (_t < 1)
            {
                _t += Time.deltaTime;
            }
            else
            {
                _t = 1;
                player.enabled = false;
            }

            if (console.activeSelf)
            {
                consoleInput.Select();
            }

        }
        else
        {

            if (_t > 0)
            {
                _t -= Time.deltaTime;
                player.enabled = true;
            }
            else
            {
                _t = 0;

                console.SetActive(false);
            }
        }
    }

    //Se encarga de revisar los códigos para aceptar códigos con patrones.
    private void ParameterChequeer()
    {
        if (consoleInput.text.Contains(" on"))
        {
            if (consoleInput.text.Contains("godmode"))
            {
                _godMode = true;
                consoleInput.text = "godmode";
            }
            else if(consoleInput.text.Contains("map"))
            {
                _map = true;
                consoleInput.text = "map";
            }
        }
        else if (consoleInput.text.Contains(" off"))
        {
            if (consoleInput.text.Contains("godmode"))
            {
                _godMode = false;
                consoleInput.text = "godmode";
            }
            else if(consoleInput.text.Contains("map"))
            {
                _map = false;
                consoleInput.text = "map";
            }
        }
        else if(consoleInput.text.Contains(" fast"))
        {
            _fast = true;
            _slow = false;
            _normal = false;
            consoleInput.text = "time";
        }
        else if (consoleInput.text.Contains(" slow"))
        {
            _fast = false;
            _slow = true;
            _normal = false;
            consoleInput.text = "time";
        }
        else if (consoleInput.text.Contains(" normal"))
        {
            _fast = false;
            _slow = false;
            _normal = true;
            consoleInput.text = "time";
        }
        else if (consoleInput.text.Contains("godmode") || consoleInput.text.Contains("map") || consoleInput.text.Contains("time"))
        {
            consoleInput.text = " ";
        }
    }

    public void OpenClose()
    {
        _active = (!_active) ? true : false;

        if (console.activeSelf)
        {
            consoleInput.Select();
        }
    }
}

