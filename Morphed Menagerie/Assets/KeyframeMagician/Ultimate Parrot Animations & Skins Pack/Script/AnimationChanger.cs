using UnityEngine;
using TMPro;

namespace KeyframeMagician.UltimateParrotAnimsSkinsPack 

{
public class AnimationChanger : MonoBehaviour
{
    public Animator parrotAnimator; // Reference to the Parrot's Animator
    public string[] animationNames; // Array of animation clip names (ensure they're in the Animator)
    public TMP_Dropdown animationDropdown; // Reference to the TextMeshPro Dropdown UI

    void Start()
    {
        // Populate the dropdown with animation names
        animationDropdown.ClearOptions();
        foreach (string animName in animationNames)
        {
            animationDropdown.options.Add(new TMP_Dropdown.OptionData(animName)); // Use animation names as options
        }

        // Set the default animation to the first one
        ChangeAnimation(0);
        animationDropdown.value = 0;

        // Add listener for dropdown value change
        animationDropdown.onValueChanged.AddListener(ChangeAnimation);
    }

    void ChangeAnimation(int index)
    {
        if (index >= 0 && index < animationNames.Length)
        {
            // Trigger the animation by its name
            parrotAnimator.Play(animationNames[index]);
        }
    }
}
}
