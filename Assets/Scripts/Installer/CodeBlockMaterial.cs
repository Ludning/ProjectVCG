using UnityEngine;

public class CodeBlockMaterial : MonoBehaviour
{
    [SerializeField] private MeshRenderer PokeMeshRenderer;
    [SerializeField] private MeshRenderer GrapMeshRenderer;
    
    public void Init(BlockLogicType type)
    {
        Debug.Log($"{type.ToString()}Mat");
        Material tempMat = ResourceManager.Instance.LoadResourceWithCaching<Material>($"{type.ToString()}Mat");
        
        if (tempMat == null)
            tempMat = ResourceManager.Instance.LoadResourceWithCaching<Material>("TempLogicMat");
        
        PokeMeshRenderer.materials = new[] { tempMat };
        if(GrapMeshRenderer!= null)
            GrapMeshRenderer.materials = new[] { tempMat };
    }
    public void SetMaterial(Material mat)
    {
        PokeMeshRenderer.materials = new[] { mat };
        if(GrapMeshRenderer!= null)
            GrapMeshRenderer.materials = new[] { mat };
    }
}
