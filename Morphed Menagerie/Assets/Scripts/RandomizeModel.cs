using UnityEngine;

public class RandomizeModel : MonoBehaviour
{
    public Texture[] textures;

    private void Start()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material.mainTexture = textures[Random.Range(0,textures.Length)];
    }
}
