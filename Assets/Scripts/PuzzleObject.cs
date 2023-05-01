using UnityEngine;

public class PuzzleObject : MonoBehaviour {

    public PuzzleObject objectAttached;
    public bool active = false;
    public bool activable = false;
    public float animTime;
    public Vector3[] position = new Vector3[2];
    public float t = 0;

    protected virtual void Awake()
    {
        if (position.Length == 0)
        {
            position = new Vector3[2];
            position[0] = this.transform.position;
        }
        else
            position[0] = this.transform.position;
    }

    protected virtual void Update()
    {
        if (active)
        {
            Activation(); 
        }
    }

    //Chquea si está activado y realiza el cambio de posición.
    protected virtual void Activation()
    {
        this.transform.position = Vector3.Lerp(position[0], position[1], t / animTime);
        if (!activable)
        {
            if(objectAttached != null) objectAttached.active = true;
        }
        t += Time.deltaTime;

        if(t / animTime == 1)
        {
            t = 0;
            active = false;
        }

        if (this.transform.position == position[1])
        {
            this.enabled = false;
        }
    }

    //Detecta colisión con cualquier jugador.
    virtual protected void OnTriggerEnter(Collider character)
    {
        if (character.gameObject.layer == LayerMask.NameToLayer("Player") || character.gameObject.layer == LayerMask.NameToLayer("PlayerGod"))
        {
            active = true;
        }
    }
}
