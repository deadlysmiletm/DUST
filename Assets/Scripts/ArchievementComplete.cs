using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArchievementComplete : MonoBehaviour {

    public static ArchievementComplete current;
    public Image archievementImage;
    private Text _archievementDetail;
    private RectTransform _rt;
    public Vector2[] position = new Vector2[2];
    private float _t = 0;
    public float animTime;
    public float timeDurable;

    public bool sa;
    public bool sk;
    public bool g;
    public bool t;
    public bool p;
    private bool _active = true;

    public Sprite[] icons = new Sprite[5];

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        _archievementDetail = this.GetComponentInChildren<Text>();
        _rt = this.GetComponent<RectTransform>();

        sa = ArchievementManager.archive.silentAssassin ? false : true;
        sk = ArchievementManager.archive.serialKiller ? false : true;
        g = ArchievementManager.archive.genocide ? false : true;
        t = ArchievementManager.archive.tactical ? false : true;
        p = ArchievementManager.archive.perfectionist ? false : true;
    }

    // Update is called once per frame
    void Update ()
    {
		if(ArchievementManager.archive.silentAssassin && sa)
        {
            _archievementDetail.text = "Silent Assassin" + "\n\n" + "Kill all the enemies in the level 1.";
            archievementImage.sprite = icons[0];
            OpenClose(ref sa);
        }
        if(ArchievementManager.archive.serialKiller && sk)
        {
            _archievementDetail.text = "Serial Killer" + "\n\n" + "Kill all the enemies in the level 2.";
            archievementImage.sprite = icons[1];
            OpenClose(ref sk);
        }
        if(ArchievementManager.archive.genocide && g)
        {
            _archievementDetail.text = "Genocide" + "\n\n" + "Kill all the enemies in the level 3.";
            archievementImage.sprite = icons[2];
            OpenClose(ref g);
        }
        if (ArchievementManager.archive.tactical && t)
        {
            _archievementDetail.text = "Tactical" + "\n\n" + "Complete level 1 with less than 40 steps.";
            archievementImage.sprite = icons[3];
            OpenClose(ref t);
        }
        if(ArchievementManager.archive.perfectionist && p)
        {
            _archievementDetail.text = "Perfectionist" + "\n\n" + "Complete level 2 with less than 100 steps.";
            archievementImage.sprite = icons[4];
            OpenClose(ref p);
        }
	}

    public void OpenClose(ref bool var)
    {
        _rt.anchoredPosition = Vector2.Lerp(position[0], position[1], _t);

        if(_active)
        {
            _t += Time.deltaTime;
            if (_t >= 1)
            {
                _t = 1;
                Invoke("Switch", timeDurable);
            }
        }
        if(!_active)
        {
            _t -= Time.deltaTime;
            if (_t <= 0)
            {
                _t = 0;
                var = false;
                _active = true;
                _rt.anchoredPosition = position[0];
            }
        }
    }

    public void Switch()
    {
        if (_active) _active = false;
    }
}
