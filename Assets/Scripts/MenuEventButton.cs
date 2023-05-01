using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuEventButton : PushButton {
    
    public bool changeScene, portal, credits, quit, logros, logrosReturn;
    public MenuEventButton logroLink;
    public GameObject playerHome, playerDestination;
    private Vector3 endPose;
    public CameraPivot pivotCamera;
    public string escena;

    protected override void Awake()
    {
        base.Awake();
        if (!File.Exists(Application.persistentDataPath + "/checkpoint.dust") && escena == "Load")
        {
            activable = false;
            mr.material.color = activeColor;
            transform.position = position[1];
        }
        else activable = true;
        active = false;

        if (playerDestination != null) endPose = playerDestination.transform.position;

    }

    protected override void Update()
    {
        if (activable)
        {
            base.Update();
            if (!active)
            {
                t = 0;
                mr.material.color = desactivedColor;
            }
        }
        else
        {
            mr.material.color = activeColor;
        }

    }

    protected override void Activation()
    {
        base.Activation();

        if(logros || logrosReturn)
        {
            logroLink.active = false;
            logroLink.activable = true;
        }
        
        if(t >= 0)
        {
            if (changeScene) Invoke("NewGame", 0.5f);
            if (quit) Invoke("Quit", 0.5f);
            if (portal) Invoke("Portal", 0.5f);
        }      

    }

    public void NewGame()
    {
        if (escena == "Load") SaveLoad.Load();
        SceneManager.LoadScene(escena);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Portal()
    {
        if (logros == true || logrosReturn == true) pivotCamera.archivement = logros;

        pivotCamera.player = playerDestination.transform;
        playerDestination.transform.position = endPose;
        playerDestination.SetActive(true);
        pivotCamera.player = playerDestination.transform;
        playerHome.SetActive(false);
        Desactivation();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            active = false;
        }
    }

    private void Desactivation()
    {
        this.transform.position = position[0];
        bx.enabled = true;
        active = false;
        activable = true;
    }

}
