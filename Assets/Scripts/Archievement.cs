using UnityEngine;
using UnityEngine.UI;

public class Archievement : MonoBehaviour
{

    public Image[] archievement = new Image[5];

    private void Start()
    {
        Color color = new Color(1f, 1f, 1f, .3f);
        if (ArchievementManager.archive.silentAssassin) archievement[0].color = color;
        if (ArchievementManager.archive.serialKiller) archievement[1].color = color;
        if (ArchievementManager.archive.genocide) archievement[2].color = color;
        if (ArchievementManager.archive.tactical) archievement[3].color = color;
        if (ArchievementManager.archive.perfectionist) archievement[4].color = color;
    }
}
