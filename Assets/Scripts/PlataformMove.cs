using UnityEngine;

public class PlataformMove : PuzzleObject {

    public Node actualNode;
    public bool final;
    public Vector3 finalPos = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();
        position[1] = finalPos;
        actualNode = this.GetComponent<Node>();
        if (final) activable = true;
    }

    protected override void Activation()
    {
        base.Activation();

        if (t/animTime >= 1 && !final)
        {
            actualNode.NodeActualization();
            if (actualNode.moveUp)
            {
                actualNode.up.moveDown = true;
                actualNode.up.NodeActualization();
            }
            if (actualNode.moveDown)
            {
                actualNode.down.moveUp = true;
                actualNode.down.NodeActualization();
            }
            if (actualNode.moveLeft)
            {
                actualNode.left.moveRight = true;
                actualNode.left.NodeActualization();
            }
            if (actualNode.moveRight)
            {
                actualNode.right.moveLeft = true;
                actualNode.right.NodeActualization();
            }
            
        }
    }


}
