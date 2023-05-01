using UnityEngine;

public class PushButton : PuzzleObject {

    private AudioSource _as;
    protected bool soundReproduced;
    public Color desactivedColor;
    public Color activeColor;
    public MeshRenderer mr;
    protected BoxCollider bx;

    override protected void Awake()
    {
        base.Awake();
        _as = GetComponent<AudioSource>();
        position[1] = this.transform.position - new Vector3(0, .07f, 0);
        if(objectAttached != null) objectAttached.GetComponent<MeshRenderer>().material.color = desactivedColor;
        mr = this.GetComponent<MeshRenderer>();
        mr.material.color = desactivedColor;
    }

    private void Start()
    {
        bx = this.GetComponent<BoxCollider>();
        bx.enabled = true;
    }

    protected override void Update()
    {
        base.Update();
    }

    //chequeo de activación
    protected override void Activation()
    {
        base.Activation();

        mr.material.color = Color.Lerp(activeColor, desactivedColor, t);
        if (!soundReproduced)
        {
            _as.Play();
            soundReproduced = true;
        }
        
        bx.enabled = false;
    }


}
