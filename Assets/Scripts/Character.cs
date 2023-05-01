using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour {

    public Node actualNode;
    private Node newNode;
    public int nodeCount;
    public int dir = 0;
    public float animTime;
    public bool move;
    public bool canMove = true;
    public bool alreadyDeath;
    public Vector3 nextDirection;
    public GameObject deathParticle;
    protected AudioSource _as;
    public AudioClip walk;
    public NavMeshAgent agent;
    private Animator _anim;
    private float _y;
    private float _remainTimer;
    private int _speedHash;

    private void Start()
    {
        _anim = this.GetComponentInChildren<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        _as = this.GetComponent<AudioSource>();
        _y = this.transform.position.y;
        _speedHash = Animator.StringToHash("Speed");
    }

    private void Update()
    {
        if (_remainTimer > 0)
        {
            float speed = 0;

            if (agent.remainingDistance < 0.01)
            {
                _anim.SetFloat(_speedHash, speed);
                _remainTimer = 0;
                return;
            }

            if (agent.remainingDistance < _remainTimer / 2)
                speed = agent.remainingDistance - _remainTimer;
            else
                speed = agent.remainingDistance;

            _anim.SetFloat(_speedHash, speed);
        }
    }

    protected virtual Vector3 DirectionToMove()
    {
        if (actualNode == null)
            return Vector3.zero;

        if (dir == 1 && actualNode.up != null)
        {
            var pos = this.transform.position;
            pos.z = actualNode.up.transform.position.z;
            newNode = actualNode.up;

            return pos;
        }
        else if(dir == 2 && actualNode.down != null)
        {
            var pos = this.transform.position;
            pos.z = actualNode.down.transform.position.z;
            newNode = actualNode.down;

            return pos;
        }
        else if(dir == 3 && actualNode.right != null)
        {
            var pos = this.transform.position;
            pos.x = actualNode.right.transform.position.x;
            newNode = actualNode.right;

            return pos;
        }
        else if(dir == 4 && actualNode.left != null)
        {
            var pos = this.transform.position;
            pos.x = actualNode.left.transform.position.x;
            newNode = actualNode.left;

            return pos;
        }
        else
        {
            move = false;
            return actualNode.transform.position;
        }
    }

    public void Move()
    {
        if(canMove)
        {
            nextDirection = DirectionToMove();
            canMove = !move ? true : false;
        }
        else
        {
            if (agent.remainingDistance == 0)
            {
                _as.pitch = 1;
                canMove = true;
                _as.Stop();
                actualNode = newNode;
                dir = 0;
                canMove = true;
                move = false;
            }
        }

        if(move)
        {
            _as.clip = walk;
            if (!_as.isPlaying)
            {
                _as.pitch = Random.Range(1, 1.25f);
                _as.Play();
            }

            agent.SetDestination(nextDirection);
            this.transform.forward = Vector3.Slerp(this.transform.forward, (nextDirection - this.transform.position).normalized, Mathf.Sin(Time.deltaTime) * 10);
            _remainTimer = agent.remainingDistance;
        }

    }

    public virtual void Death()
    {
        if (!alreadyDeath)
        {
            var particle = GameObject.Instantiate(deathParticle);
            particle.transform.position = new Vector3(this.transform.position.x, _y, this.transform.position.z);
            this.gameObject.SetActive(false);
            alreadyDeath = true;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    protected virtual void OnTriggerStay(Collider node)
    {
        if (node.gameObject.layer == LayerMask.NameToLayer("Node") && move == false)
        {
            actualNode = node.GetComponent<Node>();
        }
    }

    public void LevelEnded()
    {
        _anim.SetFloat(_speedHash, 1);
        agent.Move(transform.forward * 5 * Time.deltaTime);
    }
}
