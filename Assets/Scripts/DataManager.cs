using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class DataManager : MonoBehaviour
{

    public static DataManager current;
    public string escene;
    public GameData gameData;
    public GameObject player;
    public Character playerChara;
    public PlayerBrain playerBrain;
    public List<GameObject> enemies;
    public List<Character> enemiesChara;
    public List<EnemyBrain> enemiesBrain;
    public List<Door> door;
    public List<PlataformMove> plataform;
    public List<PushButton> button;
    public List<Node> nodes;

    public bool sk;
    public bool sa;
    public bool g;
    public bool t;
    public bool p;

    public void Awake()
    {
        
        if (current == null)
        {
            current = this;
        }
        else if (current != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void Start()
    {
        escene = SceneManager.GetActiveScene().name;
        player = GameObject.Find("Player");
        playerChara = player.GetComponent<Character>();
        playerBrain = player.GetComponent<PlayerBrain>();

        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        enemiesChara = enemies.Select(e => e.GetComponent<Character>()).ToList();
        enemiesBrain = enemies.Select(e => e.GetComponent<EnemyBrain>()).ToList();

        var puzzleObject = GameObject.FindGameObjectsWithTag("Puzzle").Select(p => p.GetComponent<PuzzleObject>()).ToList();
        foreach (var item in puzzleObject)
        {
            if (item.GetType().ToString() == "Door") door.Add(item.GetComponent<Door>());
            if (item.GetType().ToString() == "PlataformMove") plataform.Add(item.GetComponent<PlataformMove>());
            if (item.GetType().ToString() == "PushButton") button.Add(item.GetComponent<PushButton>());
        }


        nodes = GameObject.FindGameObjectsWithTag("Nodos").Select(n => n.GetComponent<Node>()).ToList();
    }

    public void DesactivateUI()
    {
        GameObject.Find("Lose Panel").SetActive(false);
    }

}

[System.Serializable]
public class GameData
{
    private SceneChange _sc = new SceneChange();
    public SceneChange sc
    {
        get { return _sc; }
        set { _sc = value; }
    }

    private PlayerChara _plc = new PlayerChara();
    public PlayerChara plc
    {
        get { return _plc; }
        set { _plc = value; }
    }

    private PlayerTransform _pt = new PlayerTransform();
    public PlayerTransform pt
    {
        get { return _pt; }
        set { _pt = value; }
    }

    private List<EnemyChara> _eyc = new List<EnemyChara>();
    public List<EnemyChara> eyc
    {
        get { return _eyc; }
        set { _eyc = value; }
    }

    private List<EnemyTransform> _et = new List<EnemyTransform>();
    public List<EnemyTransform> et
    {
        get { return _et; }
        set { _et = value; }
    }

    private List<NodeScript> _ns = new List<NodeScript>();
    public List<NodeScript> ns
    {
        get { return _ns; }
        set { _ns = value; }
    }

    private List<DoorScript> _ds = new List<DoorScript>();
    public List<DoorScript> ds
    {
        get { return _ds; }
        set { _ds = value; }
    }

    private List<PlataformMoveScript> _pm = new List<PlataformMoveScript>();
    public List<PlataformMoveScript> pm
    {
        get { return _pm; }
        set { _pm = value; }
    }

    private List<PushButtonScript> _pb = new List<PushButtonScript>();
    public List<PushButtonScript> pb
    {
        get { return _pb; }
        set { _pb = value; }
    }

}

[System.Serializable]
public class SystemData
{
    private ArchievementData _ad = new ArchievementData();
    public ArchievementData ad
    {
        get { return _ad; }
        set { _ad = value; }
    }
}

[System.Serializable]
public class ArchievementData
{
    public bool sa;
    public bool sk;
    public bool g;
    public bool t;
    public bool p;
}

[System.Serializable]
public class SceneChange
{
    public string scene;
}

[System.Serializable]
public class PlayerChara
{
    public int nodeCount;
    public bool move, canMove;
    public string actualNodeName;
}

[System.Serializable]
public class PlayerTransform
{
    public SerializableVector3 position;
    public SerializableQuaternion rotation;
}

[System.Serializable]
public class EnemyChara
{
    public bool move, canMove, alreadyDeath;
    public string actualNodeName;

    //Pertenecientes a EnemyBrain.
    public List<string> persuitNodesName = new List<string>();
    public List<int> patrolDir = new List<int>();
    public bool persuit;
    public SerializableVector3 nextDirection;
}

[System.Serializable]
public class EnemyTransform
{
    public SerializableVector3 position;
    public SerializableQuaternion rotation;
    public bool isActive;
}

[System.Serializable]
public class NodeScript
{
    public bool moveUp, moveDown, moveRight, moveLeft, finalNode;
    public string upName, downName, rightName, leftName;
}

[System.Serializable]
public class DoorScript
{
    public string objectAttachedName;
    public bool active, activable;
    public bool isActive;
    public SerializableVector3[] position = new SerializableVector3[2];
    public string actualNodeName;
    public bool cinematic, left, right, up, down;
}

[System.Serializable]
public class PlataformMoveScript
{
    public string objectAttachedName;
    public bool active, activable;
    public bool isActive;
    public SerializableVector3[] position = new SerializableVector3[2];
    public string actualNodeName;
    public bool final;
}

[System.Serializable]
public class PushButtonScript
{
    public string objectAttachedName;
    public bool active, activable;
    public bool isActive;
    public SerializableVector3[] position = new SerializableVector3[2];
    public SerializableVector3 actualColor;
}

[System.Serializable]
public struct SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(float rX, float rY, float rZ)
    {
        x = rX;
        y = rY;
        z = rZ;
    }

    public override string ToString()
    {
        return string.Format("[{0}, {1}, {2}]", x, y, z);
    }

    public static implicit operator Vector3(SerializableVector3 rValue)
    {
        return new Vector3(rValue.x, rValue.y, rValue.z);
    }

    public static implicit operator SerializableVector3(Vector3 rValue)
    {
        return new SerializableVector3(rValue.x, rValue.y, rValue.z);
    }

    public static implicit operator SerializableVector3(Color rValue)
    {
        return new SerializableVector3(rValue.r, rValue.g, rValue.b);
    }
    public static implicit operator Color(SerializableVector3 rValue)
    {
        return new Color(rValue.x, rValue.y, rValue.z);
    }
}

    [System.Serializable]
    public struct SerializableQuaternion
    {
        public float x, y, z, w;

        public SerializableQuaternion(float rX, float rY, float rZ, float rW)
        {
            x = rX;
            y = rY;
            z = rZ;
            w = rW;
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}, {3}]", x, y, z, w);
        }

        public static implicit operator Quaternion(SerializableQuaternion rValue)
        {
            return new Quaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }

        public static implicit operator SerializableQuaternion(Quaternion rValue)
        {
            return new SerializableQuaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }
    }


