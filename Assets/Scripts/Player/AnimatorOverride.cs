using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorOverride : MonoBehaviour
{
    private Animator[] animators;

    public SpriteRenderer holdItem;

    public List<AnimatorType> animatorTypes = new List<AnimatorType>();

    private Dictionary<string,Animator> animatorDict = new Dictionary<string, Animator>();

    private void Awake()
    {
        animators = GetComponentsInChildren<Animator>();
        foreach (Animator animator in animators)
        {
            animatorDict.Add(animator.name, animator);
        }

    }

    private void OnEnable()
    {
        EventHandler.ItemSelectEvent+=OnItemSelectedEvent;
    }
    private void OnDisable()
    {
        EventHandler.ItemSelectEvent -= OnItemSelectedEvent;
    }

    //WORKFLOW:
    //
    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        PartType currentType = itemDetails.itemType switch
        {
            ItemType.Seed => PartType.Carry,
            ItemType.Commodity => PartType.Carry,
            ItemType.ChopTool => PartType.Carry,
            _ => PartType.Carry
        };

        if (isSelected == false)
        {
            holdItem.enabled = false;
            currentType = PartType.None;
        }
        else
        {
            if(currentType == PartType.Carry)
            {
                holdItem.enabled = true;
                holdItem.sprite = itemDetails.itemOnWorldSprite;
            }
        }
            SwitchAnimator(currentType);
    }

    /// <summary>
    /// 根据传进来的动画类型更换 AnimatorOverrider
    /// </summary>
    /// <param name="partType"></param>
    private void SwitchAnimator(PartType partType)
    {
        foreach (var item in animatorTypes)
        {
            if (item.partType == partType)
            {
                animatorDict[item.partName.ToString()].runtimeAnimatorController = item.overrideController;
            }
        }
    }
}
