using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using System.Linq;

public class EnemyBrain : MonoBehaviour{

    private AudioSource _as;
    public AudioClip alert;
    private Character chara;
    public List<string> patrolOrders = new List<string>();
    public List<int> patrolDir = new List<int>();
    public List<Node> persuitNodes;
    public bool persuit = false;
    private GameObject _player;
    private Node _playerNode;
    public int dot;

	// Use this for initialization
	void Awake () {
        chara = this.GetComponent<Character>();
        _as = this.GetComponent<AudioSource>();
        _player = GameObject.Find("Player");
	}

    // Update is called once per frame

    private void Start()
    {
        if (patrolOrders.Count != 0)
        {
            patrolDir = patrolOrders.Select(o => o.Contains("up") ? 1 : o.Contains("down") ? 2 : o.Contains("right") ? 3 : o.Contains("left") ? 4 : 0).ToList();
        }
        ArchievementManager.archive.enemiesCount++;
    }

    void Update()
    {
        dot = Mathf.RoundToInt(Vector3.Dot(this.transform.forward, Vector3.forward));
        _playerNode = _player.GetComponent<Character>().actualNode;
        if (_player.GetComponent<Character>().move && this.chara.canMove)
        {
            this.chara.dir = !persuit ? NextDir(ref patrolDir) : NextNode(persuitNodes);
            this.chara.move = true;
        }
        chara.Move();

        var hit = new RaycastHit();

        if(Mathf.RoundToInt(Vector3.Distance(this.transform.position, _player.transform.position)) <= 3 && _player.GetComponent<Character>().canMove)
        {
            if(Physics.Raycast(this.transform.position, this.transform.forward, out hit, 3))
            {
                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    if(Mathf.Round((_player.transform.position - this.transform.position).magnitude) <= 1.5f)
                    {
                        this.GetComponentInChildren<Animator>().Play("Attack");
                        GameObject.Find("Player").GetComponent<PlayerBrain>().death = true;
                    }

                    else if (!persuit)
                    {
                        _as.clip = alert;
                        _as.Play();
                        persuitNodes = new List<Node>();
                        persuit = true;
                        persuitNodes = ToThePlayer(persuitNodes);
                    }
                }
            }
        }

        if (persuit)
        {
            if(persuitNodes[persuitNodes.Count - 1] != _playerNode) persuitNodes.Add(_playerNode);
            if(persuitNodes.Count > 5) persuitNodes.Remove(persuitNodes[0]);
        }

	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerGod"))
        {
            Analytics.CustomEvent("EnemyKilledInfo", new Dictionary<string, object>
                    {
                         {"Player Position", collision.transform.position },
                         {"Death Position", this.transform.position},
                         {"Level Death", GameObject.Find("Main Camera").GetComponent<Main>().ActualScene()},
                         {"Time from Start level", GameObject.Find("Main Camera").GetComponent<Main>().gameTime}
                    });
            ArchievementManager.archive.enemiesCount--;
            chara.Death();
        }
    }

    public List<Node> ToThePlayer(List<Node> lista)
    {
        if (lista.Count == 0)
        {
            lista.Add(this.chara.actualNode);
        }

        else if (lista.Contains(_playerNode)) return lista;
        else
        {
            if (Mathf.RoundToInt(Vector3.Dot(this.transform.forward, Vector3.forward)) == -1)
            {
                lista.Add(lista[lista.Count - 1].up);
            }
            else if (Mathf.RoundToInt(Vector3.Dot(this.transform.forward, Vector3.forward)) == 1)
            {
                lista.Add(lista[lista.Count - 1].down);
            }
            else if (Mathf.RoundToInt(Vector3.Dot(this.transform.forward, Vector3.right)) == 1)
            {
                lista.Add(lista[lista.Count - 1].left);
            }
            else if (Mathf.RoundToInt(Vector3.Dot(this.transform.forward, Vector3.right)) == -1)
            {
                lista.Add(lista[lista.Count - 1].right);
            }
        }

        return ToThePlayer(lista);

    }

    public int NextNode(List<Node> lista)
    {
        if (lista.Count == 1 || lista.Count == 0) return 0;

        if(lista[0] == chara.actualNode)
        {
            return chara.actualNode.up == lista[1] ? 1 : chara.actualNode.down == lista[1] ? 2 : chara.actualNode.right == lista[1] ? 3 : 4;
        }
        else
        {
            return NextNode(lista.GetRange(1, lista.Count - 1));
        }
    }

    private int NextDir(ref List<int> lista)
    {
        if (lista.Count == 0) return 0;
        if (lista.Count == 1) return lista[0];

        var num = lista[0];

        lista.RemoveAt(0);
        lista.Add(num);
        return num;
    }

}
