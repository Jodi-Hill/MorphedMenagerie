using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

namespace KeyframeMagician.UltimateParrotAnimsSkinsPack


{

public class MaterialChanger : MonoBehaviour
{
    public Renderer parrotRenderer; // Renderer for the parrot
    public TMP_Dropdown materialDropdown; // Dropdown UI for materials
    public TMP_Dropdown texturePackDropdown; // Dropdown UI for texture pack
    public Material[] BuiltInMaterials2K; // Built-in 2K materials
    public Material[] BuiltInMaterials1K; // Built-in 1K materials
    public Material[] URPMaterials2K; // URP 2K materials
    public Material[] URPMaterials1K; // URP 1K materials
    public Material[] HDRPMaterials2K; // HDRP 2K materials
    public Material[] HDRPMaterials1K; // HDRP 1K materials

    private Material[] currentMaterials; // Current active materials array
    private int selectedMaterialIndex = 0; // Stores the current material index
    private string currentRenderPipeline = "BuiltIn"; // Default render pipeline
    private string currentTexturePack = "2K"; // Default texture pack

    void Start()
    {
        // Detect the current render pipeline
        DetectRenderPipeline();

        // Populate the material dropdown
        PopulateMaterialDropdown();

        // Set default material
        ApplyMaterial();

        // Add listeners
        materialDropdown.onValueChanged.AddListener(SetMaterialIndex);
        texturePackDropdown.onValueChanged.AddListener(SetTexturePack);
    }

    void DetectRenderPipeline()
    {
        if (GraphicsSettings.defaultRenderPipeline == null)
        {
            currentRenderPipeline = "BuiltIn";
        }
        else if (GraphicsSettings.defaultRenderPipeline.GetType().ToString().Contains("Universal"))
        {
            currentRenderPipeline = "URP";
        }
        else if (GraphicsSettings.defaultRenderPipeline.GetType().ToString().Contains("HD"))
        {
            currentRenderPipeline = "HDRP";
        }

        Debug.Log($"Detected Render Pipeline: {currentRenderPipeline}");
        UpdateMaterialsArray();
    }

    void PopulateMaterialDropdown()
    {
        materialDropdown.ClearOptions();
        foreach (Material mat in currentMaterials)
        {
            materialDropdown.options.Add(new TMP_Dropdown.OptionData(mat.name));
        }

        // Set the dropdown value to the current material index
        materialDropdown.value = selectedMaterialIndex;
        materialDropdown.RefreshShownValue();
    }

    void UpdateMaterialsArray()
    {
        if (currentRenderPipeline == "BuiltIn")
        {
            currentMaterials = currentTexturePack == "2K" ? BuiltInMaterials2K : BuiltInMaterials1K;
        }
        else if (currentRenderPipeline == "URP")
        {
            currentMaterials = currentTexturePack == "2K" ? URPMaterials2K : URPMaterials1K;
        }
        else if (currentRenderPipeline == "HDRP")
        {
            currentMaterials = currentTexturePack == "2K" ? HDRPMaterials2K : HDRPMaterials1K;
        }
    }

    void ApplyMaterial()
    {
        if (selectedMaterialIndex >= 0 && selectedMaterialIndex < currentMaterials.Length)
        {
            Debug.Log($"Applying Material: {currentMaterials[selectedMaterialIndex].name}");
            parrotRenderer.material = currentMaterials[selectedMaterialIndex];
        }
        else
        {
            Debug.LogWarning("Invalid material index selected!");
        }
    }

    void SetMaterialIndex(int index)
    {
        selectedMaterialIndex = index;
        ApplyMaterial();
    }

    void SetTexturePack(int index)
    {
        currentTexturePack = index == 0 ? "2K" : "1K";
        Debug.Log($"Switched to Texture Pack: {currentTexturePack}");

        UpdateMaterialsArray();
        PopulateMaterialDropdown(); // Refresh dropdown options for the new texture pack
        ApplyMaterial(); // Apply the current material index in the new texture pack
    }
 }
}