using UnityEngine;

public class DissolveSize : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetFloat("Tile",200);
        GetComponent<MeshRenderer>().SetPropertyBlock(propertyBlock);
    }
}
