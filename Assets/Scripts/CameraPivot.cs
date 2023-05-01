using UnityEngine;

public class CameraPivot : MonoBehaviour {

    public GameObject cam;
    public Transform player;
    public Character _playerController;
    public float animMove;
    private float _t;
    public bool archivement;
    public Vector3 newPos = new Vector3(0.2f, 11.3f, 10.7f);
    public Vector3 newRot = new Vector3(7.8f, 180.6f, 0.2f);
    private Vector3 _oldPos;
    private Vector3 _oldRot;

    void Awake ()
    {
        cam = GameObject.Find("Main Camera");
        _oldPos = cam.transform.localPosition;
        _oldRot = cam.transform.localEulerAngles;
        _playerController = player.GetComponent<Character>();
    }

    void Update()
    {
        PlayerTracking();

        if (_playerController.actualNode != null && _playerController.actualNode.finalNode)
            archivement = true;
        
        cam.transform.localPosition = Vector3.Lerp(_oldPos, newPos, _t / 0.75f);
        cam.transform.localEulerAngles = Vector3.Lerp(_oldRot, newRot, _t / 0.75f);
        
        if (archivement)
        {
            _t = Mathf.Clamp(_t + Time.deltaTime/0.75f, 0, 1);
        }
        else
        {
            _t = Mathf.Clamp(_t - Time.deltaTime/0.75f, 0, 1);
        }

    }

    public void PlayerTracking()
    {
        var vectorDirector = player.position;
        vectorDirector.y = transform.position.y;
        transform.position = Vector3.Lerp(transform.position, vectorDirector, Time.deltaTime / animMove);
    }
}
