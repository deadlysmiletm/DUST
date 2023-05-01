using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

public class SaveLoad {

    public static void SaveSystem()
    {
        FileStream file = File.Exists(Application.persistentDataPath + "/system.dust") ? File.Open(Application.persistentDataPath + "/system.dust", FileMode.Open) : File.Create(Application.persistentDataPath + "/system.dust");

        SystemData data = new SystemData();
        ArchievementManager am = ArchievementManager.archive;
        Debug.Log(ArchievementManager.archive.gameObject.name);
        data.ad.sa = am.silentAssassin;
        data.ad.sk = am.serialKiller;
        data.ad.g = am.genocide;
        data.ad.t = am.tactical;
        data.ad.p = am.perfectionist;

        BinaryFormatter bf = new BinaryFormatter();

        bf.Serialize(file, data);

        file.Close();
    }

    public static void Save()
    {
        FileStream file = File.Exists(Application.persistentDataPath + "/checkpoint.dust") ? File.Open(Application.persistentDataPath + "/checkpoint.dust", FileMode.Open) : File.Create(Application.persistentDataPath + "/checkpoint.dust");

        GameData data = new GameData();

        data.sc.scene = DataManager.current.escene;
        data.plc.nodeCount = DataManager.current.playerChara.nodeCount;
        data.plc.move = DataManager.current.playerChara.move;
        data.plc.canMove = DataManager.current.playerChara.canMove;
        data.plc.actualNodeName = DataManager.current.playerChara.actualNode.name;

        data.pt.position = DataManager.current.player.transform.position;
        data.pt.rotation = DataManager.current.player.transform.rotation;

        for (int i = 0; i < DataManager.current.enemies.Count; i++)
        {
            var d = new EnemyChara();
            d.move = DataManager.current.enemiesChara[i].move;
            d.canMove = DataManager.current.enemiesChara[i].canMove;
            d.alreadyDeath = DataManager.current.enemiesChara[i].alreadyDeath;
            d.actualNodeName = DataManager.current.enemiesChara[i].actualNode.name;
            d.persuitNodesName = DataManager.current.enemiesBrain[i].persuitNodes.Select(p => p.name).ToList();
            d.persuit = DataManager.current.enemiesBrain[i].persuit;
            d.patrolDir = DataManager.current.enemiesBrain[i].patrolDir;
            d.nextDirection = DataManager.current.enemiesChara[i].nextDirection;
            data.eyc.Add(d);

            var t = new EnemyTransform();
            //t.position = DataManager.current.enemies[i].transform.position;
            t.position = new Vector3(DataManager.current.enemies[i].transform.position.x, 1, DataManager.current.enemies[i].transform.position.z);
            t.rotation = DataManager.current.enemies[i].transform.rotation;
            t.isActive = DataManager.current.enemies[i].activeSelf;
            data.et.Add(t);
        }
        for (int i = 0; i < DataManager.current.nodes.Count; i++)
        {
            var d = new NodeScript();

            d.moveDown = DataManager.current.nodes[i].moveDown;
            d.moveLeft = DataManager.current.nodes[i].moveLeft;
            d.moveRight = DataManager.current.nodes[i].moveRight;
            d.moveUp = DataManager.current.nodes[i].moveUp;
            if(DataManager.current.nodes[i].up != null) d.upName = DataManager.current.nodes[i].up.name;
            if (DataManager.current.nodes[i].down != null) d.downName = DataManager.current.nodes[i].down.name;
            if (DataManager.current.nodes[i].right != null) d.rightName = DataManager.current.nodes[i].right.name;
            if (DataManager.current.nodes[i].left != null) d.leftName = DataManager.current.nodes[i].left.name;
            data.ns.Add(d);
        }
        for (int i = 0; i < DataManager.current.door.Count; i++)
        {
            var d = new DoorScript();

            d.active = DataManager.current.door[i].active;
            d.activable = DataManager.current.door[i].activable;
            d.isActive = DataManager.current.door[i].enabled;
            d.position[0] = DataManager.current.door[i].position[0];
            d.position[1] = DataManager.current.door[i].position[1];
            d.actualNodeName = DataManager.current.door[i].actualNode.name;
            d.cinematic = DataManager.current.door[i].cinematic;
            d.left = DataManager.current.door[i].left;
            d.right = DataManager.current.door[i].right;
            d.up = DataManager.current.door[i].up;
            d.down = DataManager.current.door[i].down;

            data.ds.Add(d);
        }
        for (int i = 0; i < DataManager.current.plataform.Count; i++)
        {
            var p = new PlataformMoveScript();

            if(DataManager.current.plataform[i].objectAttached != null) p.objectAttachedName = DataManager.current.plataform[i].objectAttached.name;
            p.active = DataManager.current.plataform[i].active;
            p.activable = DataManager.current.plataform[i].activable;
            p.isActive = DataManager.current.plataform[i].enabled;
            p.position[0] = DataManager.current.plataform[i].position[0];
            p.position[1] = DataManager.current.plataform[i].position[1];
            p.actualNodeName = DataManager.current.plataform[i].actualNode.name;
            p.final = DataManager.current.plataform[i].final;

            data.pm.Add(p);
        }
        for (int i = 0; i < DataManager.current.button.Count; i++)
        {
            var b = new PushButtonScript();

            b.objectAttachedName = DataManager.current.button[i].objectAttached.name;
            b.active = DataManager.current.button[i].active;
            b.activable = DataManager.current.button[i].activable;
            b.isActive = DataManager.current.button[i].enabled;
            b.position[0] = DataManager.current.button[i].position[0];
            b.position[1] = DataManager.current.button[i].position[1];
            b.actualColor = DataManager.current.button[i].mr.material.color;

            data.pb.Add(b);
        }

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);

        file.Close();
    }

    public static void LoadSystem()
    {
        Debug.Log(Application.persistentDataPath);
        if(File.Exists(Application.persistentDataPath + "/system.dust"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/system.dust", FileMode.Open);
            SystemData data = new SystemData();
            BinaryFormatter bf = new BinaryFormatter();

            data = (SystemData)bf.Deserialize(file);
            file.Close();

            ArchievementManager.archive.silentAssassin = data.ad.sa;
            ArchievementManager.archive.serialKiller = data.ad.sk;
            ArchievementManager.archive.genocide = data.ad.g;
            ArchievementManager.archive.tactical = data.ad.t;
            ArchievementManager.archive.perfectionist = data.ad.p;
        }
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/checkpoint.dust"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/checkpoint.dust", FileMode.Open);
            GameData data = new GameData();

            data = (GameData)bf.Deserialize(file);
            file.Close();

            if (data.sc.scene != DataManager.current.escene)
            {
                SceneManager.LoadScene(data.sc.scene);
            }
            else
            {
                DataManager.current.playerBrain.death = false;
                DataManager.current.playerChara.alreadyDeath = false;
                DataManager.current.player.SetActive(true);
                DataManager.current.playerChara.nodeCount = data.plc.nodeCount;
                DataManager.current.playerChara.move = data.plc.move;
                DataManager.current.playerChara.canMove = data.plc.canMove;
                DataManager.current.playerChara.actualNode = GameObject.Find(data.plc.actualNodeName).GetComponent<Node>();

                DataManager.current.player.transform.position = new Vector3(data.pt.position.x, 1.5f, data.pt.position.z);
                DataManager.current.player.transform.rotation = data.pt.rotation;

                for (int i = 0; i < DataManager.current.enemies.Count; i++)
                {
                    var enemyRb = DataManager.current.enemiesChara[i].GetComponent<Rigidbody>();

                    DataManager.current.enemies[i].SetActive(data.et[i].isActive);
                    DataManager.current.enemiesChara[i].move = false;
                    DataManager.current.enemiesChara[i].canMove = true;
                    DataManager.current.enemiesChara[i].alreadyDeath = data.eyc[i].alreadyDeath;
                    DataManager.current.enemiesChara[i].actualNode = GameObject.Find(data.eyc[i].actualNodeName).GetComponent<Node>();
                    DataManager.current.enemiesChara[i].nextDirection = data.eyc[i].nextDirection;
                    DataManager.current.enemiesBrain[i].persuitNodes = data.eyc[i].persuitNodesName.Select(p => GameObject.Find(p).GetComponent<Node>()).ToList();
                    DataManager.current.enemiesBrain[i].persuit = data.eyc[i].persuit;
                    DataManager.current.enemiesBrain[i].patrolDir = data.eyc[i].patrolDir;

                    DataManager.current.enemies[i].transform.position = new Vector3(data.et[i].position.x, 0.3f, data.et[i].position.z);
                    DataManager.current.enemies[i].transform.rotation = data.et[i].rotation;
                    DataManager.current.enemiesChara[i].agent.enabled = true;
                    if (DataManager.current.enemies[i].activeSelf) DataManager.current.enemiesChara[i].agent.SetDestination(DataManager.current.enemiesChara[i].nextDirection);
                }

                for (int i = 0; i < DataManager.current.nodes.Count; i++)
                {
                    DataManager.current.nodes[i].moveDown = data.ns[i].moveDown;
                    DataManager.current.nodes[i].moveLeft = data.ns[i].moveLeft;
                    DataManager.current.nodes[i].moveRight = data.ns[i].moveRight;
                    DataManager.current.nodes[i].moveUp = data.ns[i].moveUp;
                    if (data.ns[i].upName != null) DataManager.current.nodes[i].up = GameObject.Find(data.ns[i].upName).GetComponent<Node>();
                    if (data.ns[i].downName != null) DataManager.current.nodes[i].down = GameObject.Find(data.ns[i].downName).GetComponent<Node>();
                    if (data.ns[i].leftName != null) DataManager.current.nodes[i].left = GameObject.Find(data.ns[i].leftName).GetComponent<Node>();
                    if (data.ns[i].rightName != null) DataManager.current.nodes[i].right = GameObject.Find(data.ns[i].rightName).GetComponent<Node>();
                }
                for (int i = 0; i < DataManager.current.door.Count; i++)
                {
                    DataManager.current.door[i].active = data.ds[i].active;
                    DataManager.current.door[i].activable = data.ds[i].activable;
                    DataManager.current.door[i].position[0] = data.ds[i].position[0];
                    DataManager.current.door[i].position[1] = data.ds[i].position[1];
                    DataManager.current.door[i].actualNode = GameObject.Find(data.ds[i].actualNodeName).GetComponent<Node>();
                    DataManager.current.door[i].cinematic = data.ds[i].cinematic;
                    DataManager.current.door[i].left = data.ds[i].left;
                    DataManager.current.door[i].right = data.ds[i].right;
                    DataManager.current.door[i].up = data.ds[i].up;
                    DataManager.current.door[i].down = data.ds[i].down;
                    DataManager.current.door[i].enabled = data.ds[i].isActive;
                    DataManager.current.door[i].GetComponent<BoxCollider>().enabled = data.ds[i].isActive;
                    DataManager.current.door[i].transform.position = data.ds[i].active ? data.ds[i].position[1] : data.ds[i].position[0];
                    DataManager.current.door[i].t = 0;
                }
                for (int i = 0; i < DataManager.current.button.Count; i++)
                {
                    DataManager.current.button[i].objectAttached = GameObject.Find(data.pb[i].objectAttachedName).GetComponent<PuzzleObject>();
                    DataManager.current.button[i].active = data.pb[i].active;
                    DataManager.current.button[i].activable = data.pb[i].activable;
                    DataManager.current.button[i].position[0] = data.pb[i].position[0];
                    DataManager.current.button[i].position[1] = data.pb[i].position[1];
                    DataManager.current.button[i].mr.material.color = data.pb[i].actualColor;
                    DataManager.current.button[i].enabled = data.pb[i].isActive;
                    DataManager.current.button[i].GetComponent<BoxCollider>().enabled = data.pb[i].isActive;
                    DataManager.current.button[i].transform.position = data.pb[i].active ? data.pb[i].position[1] : data.pb[i].position[0];
                    DataManager.current.button[i].t = 0;
                }
                for (int i = 0; i < DataManager.current.plataform.Count; i++)
                {
                    if (data.pm[i].objectAttachedName != null) DataManager.current.plataform[i].objectAttached = GameObject.Find(data.pm[i].objectAttachedName).GetComponent<PuzzleObject>();
                    DataManager.current.plataform[i].active = data.pm[i].active;
                    DataManager.current.plataform[i].activable = data.pm[i].activable;
                    DataManager.current.plataform[i].position[0] = data.pm[i].position[0];
                    DataManager.current.plataform[i].position[1] = data.pm[i].position[1];
                    DataManager.current.plataform[i].actualNode = GameObject.Find(data.pm[i].actualNodeName).GetComponent<Node>();
                    DataManager.current.plataform[i].final = data.pm[i].final;
                    DataManager.current.plataform[i].enabled = data.pm[i].isActive;
                    DataManager.current.plataform[i].transform.position = data.pm[i].active ? data.pm[i].position[1] : data.pm[i].position[0];
                    DataManager.current.plataform[i].t = 0;
                }
            }
        }
        DataManager.current.DesactivateUI();

    }
}
