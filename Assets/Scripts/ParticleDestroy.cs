using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
	void Update ()
	{
        Destroy(this.gameObject, 2);
	}
}
