using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public bool moveUp;
    public bool moveDown;
    public bool moveRight;
    public bool moveLeft;
    public bool finalNode;

    private GameObject _der;
    private GameObject _izq;
    private GameObject _aba;
    private GameObject _arr;


    public Node up;
    public Node down;
    public Node right;
    public Node left;

    void Awake()
    {
        _der = this.transform.GetChild(1).gameObject;
        _izq = this.transform.GetChild(0).gameObject;
        _aba = this.transform.GetChild(2).gameObject;
        _arr = this.transform.GetChild(3).gameObject;

        NodeActualization();
    }

    private void Update()
    {
        if (!moveUp) _arr.SetActive(false);
        if (!moveDown) _aba.SetActive(false);
        if (!moveLeft) _izq.SetActive(false);
        if (!moveRight) _der.SetActive(false);
    }

    public void NodeActualization()
    {
        if (moveUp)
        {
            var info = new RaycastHit();
            if (Physics.Raycast(this.transform.position, -this.transform.forward, out info, 2) && up == null)
            {
                up = info.transform.GetComponent<Node>();
            }
            _arr.SetActive(true);
        }
        if (moveDown)
        {
            var info = new RaycastHit();
            if (Physics.Raycast(this.transform.position, this.transform.forward, out info, 2) && down == null)
            {
                down = info.transform.GetComponent<Node>();
            }
            _aba.SetActive(true);
        }
        if (moveLeft)
        {
            var info = new RaycastHit();
            if (Physics.Raycast(this.transform.position, this.transform.right, out info, 2) && left == null)
            {
                left = info.transform.GetComponent<Node>();
            }
            _izq.SetActive(true);

        }
        if (moveRight)
        {
            var info = new RaycastHit();
            if (Physics.Raycast(this.transform.position, -this.transform.right, out info, 2) && right == null)
            {
                right = info.transform.GetComponent<Node>();
            }
            _der.SetActive(true);
        }
    }
}
