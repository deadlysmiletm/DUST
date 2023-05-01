using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : PuzzleObject {

    public  AudioSource _as;
    protected bool soundReproduced;
    public Node actualNode;
    public bool cinematic;
    public bool left, right, up, down = false;
    private BoxCollider _bx;
    public Vector3 finalPos = Vector3.zero;

    protected override void Awake()
    {
        //Se encarga del movimiento al ser activada.
        base.Awake();
        _as = GetComponent<AudioSource>();

        activable = true;
        if (finalPos == Vector3.zero)
        {
            position[1] = transform.position - new Vector3(0, 4, 0);
        }
        else
        {
            position[1] = finalPos;
        }

    }

    private void Start()
    {
        if (!cinematic)
        {
            _bx = this.GetComponent<BoxCollider>();
            actualNode.GetComponent<SphereCollider>().enabled = false;
            if (actualNode.moveUp)
            {
                up = true;
                actualNode.up.moveDown = false;
            }
            if (actualNode.moveDown)
            {
                down = true;
                actualNode.down.moveUp = false;
            }
            if (actualNode.moveRight)
            {
                right = true;
                actualNode.right.moveLeft = false;
            }
            if (actualNode.moveLeft)
            {
                left = true;
                actualNode.left.moveRight = false;
            }
        }
    }

    //Chequea si fue activada.
    protected override void Activation()
    {
        base.Activation();
        if (!cinematic)
        {
            _bx.enabled = false;
            actualNode.GetComponent<SphereCollider>().enabled = true;
            if (up)
            {
                actualNode.up.moveDown = true;
                actualNode.up.NodeActualization();
            }
            if (down)
            {
                actualNode.down.moveUp = true;
                actualNode.down.NodeActualization();
            }
            if (left)
            {
                actualNode.right.moveLeft = true;
                actualNode.right.NodeActualization();
            }
            if (right)
            {
                actualNode.left.moveRight = true;
                actualNode.left.NodeActualization();
            }
        }

        if (!soundReproduced)
        {
            _as.Play();
            soundReproduced = true;
        }
    }

}
