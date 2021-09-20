using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 能力，使用物品
/// </summary>
public class AbilityUseItem : AbilitiesBase
{
    int curItemId = 0;

    protected override void onAdd()
    {
        base.onAdd();
    }

    protected override void onActive()
    {
        base.onActive();
        if (curItemId != 0)
        {
            beginUseItem();
        }
    }

    protected override void onDeactive()
    {
        base.onDeactive();
        endUseItem();
    }

    protected override void onAddListener()
    {
        base.onAddListener();
        EventCenter.Instance.register<int>(AbilityEvent.ABEvent_UseItem, onUseItemEvent);
    }

    protected override void onRemoveListener()
    {
        base.onRemoveListener();
        EventCenter.Instance.unRegister<int>(AbilityEvent.ABEvent_UseItem, onUseItemEvent);
    }

    void onUseItemEvent(int itemId)
    {
        if (curItemId != 0)
        {
            Log.logError("already use item");
            return;
        }
        Log.logError("onUseItemEvent", itemId);
        curItemId = itemId;
        activeSelf();
    }

    void beginUseItem()
    {
        Log.logError("beginUseItem", curItemId);
        TimerManager.instance.Once(3000, useItemSuccess);
    }

    void useItemSuccess()
    {
        Log.logError("useItemSuccess", curItemId);
        curItemId = 0;
        deActiveSelf();
    }

    void endUseItem()
    {
        Log.logError("endUseItem");
        if (curItemId != 0)
        {
            Log.logError("中途打断了使用物品");
        }
        curItemId = 0;
        TimerManager.instance.ClearTimer(useItemSuccess);
    }
}
