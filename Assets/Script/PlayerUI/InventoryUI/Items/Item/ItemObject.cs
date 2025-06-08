using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/* [ClassINFO : ItemObject]
   @ Description: 아이템 프리팹에 붙어 해당 아이템의 Scriptable Object Data를 들고있어주는 홀더(holder)
   @ Attached at : Item_Name(prefab)
   @ Methods: ============================================
              [public]
              - 추후 작성 예정
              ============================================
              [private]
              - 추후 작성 예정
              ============================================
*/

/* [InterfaceINFO : IInteractable]
   @ Description: This interface is used to define the interaction methods for interactable objects.
   @ Methods : ============================================
               -GetInteractionPrompt() : Returns the interaction prompt for the object.
               - WhenInteracted() : Handles the interaction with the object when interacted with it.
               ============================================
*/

public interface IInteractable
{
    public string GetInteractionPrompt();
    public void WhenInteracted();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    // =========================== //
    //   [Reference to ItemData]    
    // =========================== //
    #region [Reference to ItemData]
    [TextArea]
    public string ScriptINFO = "아이템 프리팹에 붙어 해당 아이템의 Scriptable Object Data를 들고있어주는 홀더(holder)";

    [Space(10)]

    [Header("Item Data")]
    public BasicItemData itemData;
    #endregion


    // =========================== //
    //      [Public Methods]
    // =========================== //
    #region [Public Methods]
    #endregion


    // =========================== //
    //   [IInteractable Methods]
    // =========================== //
    #region [IInteractable Methods]
    public string GetInteractionPrompt()
    {
        return null;
    }

    public void WhenInteracted()
    {
     
    }
    #endregion
}
