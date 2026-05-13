using UnityEngine;

public class Grass : MonoBehaviour
{
    private Renderer material;
    [SerializeField] private Transform objPos;

    private void Awake()
    {
        material = GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        material.material.SetVector("_Pos", objPos.position);
    }
}
