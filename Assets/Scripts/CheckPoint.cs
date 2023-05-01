using UnityEngine;

public class CheckPoint : MonoBehaviour {
    public Material ActiveMaterial;

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && other.GetComponent<Character>().canMove)
        {
            SaveLoad.Save();
            this.GetComponent<BoxCollider>().enabled = false;
            this.GetComponentInChildren<MeshRenderer>().material = ActiveMaterial;
            GetComponentInChildren<Animator>().Play("Activated");
        }
    }
}
