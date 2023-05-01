using UnityEngine;

public class IntroAnimEvent : MonoBehaviour
{
    public void AnimationEnd()
    {
        GetComponent<Animation>().enabled = false;
    }
}
