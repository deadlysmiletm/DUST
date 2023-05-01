using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using System.Linq;

public class Main : MonoBehaviour {

    private GameObject _gameOver;
    private GameObject _win;
    private GameObject _pause;
    private GameObject _player;
    private Character _pchara;
    private bool _saved = false;
    public float gameTime;
    private bool _death = false;

    public static GameObject player;
    public static List<GameObject> enemies;
    public static List<GameObject> puzzleObject;
    public static List<Node> nodos;

    //Asignamos las variables.
    private void Awake()
    {

        Application.targetFrameRate = 60;

        gameTime = 0;
        _gameOver = GameObject.Find("Lose Panel");
        _win = GameObject.Find("Win Panel");
        _player = GameObject.Find("Player");
        _pchara = _player.GetComponent<Character>();
        _pause = GameObject.Find("Pause Panel");

        _pause.SetActive(false);
        _gameOver.SetActive(false);
        _win.SetActive(false);

        player = _player;
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        puzzleObject = GameObject.FindGameObjectsWithTag("Puzzle").ToList();
        var list = GameObject.FindGameObjectsWithTag("Nodos").ToList();
        nodos = list.Select(n => n.GetComponent<Node>()).ToList();
    }

    private void LateUpdate()
    {
        if(_pchara.nodeCount == 0 && !_saved)
        {
            SaveLoad.Save();
            _saved = true;
        }

        if (_player.GetComponent<Character>().actualNode.finalNode)
        {
            GameObject.Find("Camera Pivot").GetComponent<CameraPivot>().archivement = true;
            Invoke("Win", 3);
        }
        else if (!_player.activeSelf && !_death)
        {
            _death = true;
            Invoke("Lose", 1.5f);
        }

        if(_player.activeSelf && _death)
        {
            _gameOver.SetActive(false);
            _death = false;
        }

        gameTime += Time.deltaTime;
    }

    //Se encarga de cargar las escenas.
    public void SceneSwitcher(string scene)
    {
        if (scene == "Load")
        {
            SaveLoad.Load();
        }
        else
        {
            SceneManager.LoadScene(scene);
        }
    }

    public static void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause(int time)
    {
        Analytics.CustomEvent("SwitchActivated", new Dictionary<string, object>
                    {
                         {"Player Position", GameObject.Find("Player").transform.position},
                         {"Level", ActualScene()},
                         {"Time from Start level", gameTime}
                    });
        Time.timeScale = time;
    }

    //Se encarga de cerrar el juego.
    public void Quit()
    {
        Application.Quit();
    }

    private void Win()
    {
        _win.SetActive(true);
    }

    private void Lose()
    {
        _gameOver.SetActive(true);
    }

    public string ActualScene()
    {
        return SceneManager.GetActiveScene().name;
    }
}
