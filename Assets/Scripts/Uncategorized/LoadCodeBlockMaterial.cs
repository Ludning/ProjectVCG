using UnityEngine;

public class LoadCodeBlockMaterial : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    
    public void Init(BlockLogicType type)
    {
        Material tempMat = ResourceManager.Instance.LoadResourceWithCaching<Material>($"{type.ToString()}Mat");
        
        if (tempMat == null)
            tempMat = ResourceManager.Instance.LoadResourceWithCaching<Material>("TempLogicMat");
        
        meshRenderer.materials = new[] { tempMat };
    }
}
