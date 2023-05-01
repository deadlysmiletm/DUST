using UnityEngine;

public class ArchievementManager : MonoBehaviour {

    public static ArchievementManager archive;
    public bool silentAssassin = false;
    public bool serialKiller = false;
    public bool genocide = false;
    public bool tactical = false;
    public bool perfectionist = false;

    public int enemiesCount;
    private string _actualScene;
    private Character _player;

    private void Awake()
    {
        if (archive == null)
        {
            archive = this;
        }
        else if (archive != this)
        {
            Destroy(archive);
            archive = this;
        }

        _player = GameObject.Find("Player").GetComponent<Character>();
        _actualScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        SaveLoad.LoadSystem();
    }

    private void Update()
    {
        if(enemiesCount == 0)
        {
            if (_actualScene == "Level 01") SilentAssassinArchievement();
            if (_actualScene == "Level 02") SerialKillerArchievement();
            if (_actualScene == "Level 03") GenocideArchievement();
        }

        if(_player.actualNode != null &&_player.actualNode.finalNode)
        {
            if (_actualScene == "Level 01" && _player.nodeCount < 40) TacticalArchievement();
            if (_actualScene == "Level 02" && _player.nodeCount < 100) PerfectionistArchievement();
        }

    }

    public void SilentAssassinArchievement()
    {
        silentAssassin = true;
        SaveLoad.SaveSystem();
    }

    public void SerialKillerArchievement()
    {
        serialKiller = true;
        SaveLoad.SaveSystem();
    }

    public void GenocideArchievement()
    {
        genocide = true;
        SaveLoad.SaveSystem();
    }

    public void TacticalArchievement()
    {
        tactical = true;
        SaveLoad.SaveSystem();
    }

    public void PerfectionistArchievement()
    {
        perfectionist = true;
        SaveLoad.SaveSystem();
    }
}
