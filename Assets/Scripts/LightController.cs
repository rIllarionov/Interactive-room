using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private InteractableItem _currentItem;

    void Update()
    {
        var ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray,out RaycastHit raycastHitInfo,2f))
        {
            DoorChecker(raycastHitInfo);
            
            if (raycastHitInfo.collider.TryGetComponent<InteractableItem>(out InteractableItem item))
            {
                ItemAwailable(item);
            }

            else
            {
                ResetItem();
            }
        }
        
        else
        {
            ResetItem();
        }

    }

    private void DoorChecker(RaycastHit item)
    {
        if (item.collider.TryGetComponent<Door>(out Door door))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                door.SwitchDoorState();
            }
        }
    }

    private void ItemAwailable (InteractableItem newItem) //если нашли новый объект интерактивный
    {
        if (_currentItem!=null) //проверяем был ли у нас до этого такой объект
        {
            _currentItem.RemoveFocus(); //если да, то выключаем его свечение
            _currentItem = null;
        }

        _currentItem = newItem; //записываем новый объект в текущий
        _currentItem.SetFocus(); //и включаем его свечение
    }
    
    public void ResetItem()
    {
        if (_currentItem!=null)
        {
            _currentItem.RemoveFocus();
            _currentItem = null;
        }
    }

    public InteractableItem GetCurrentItem()
    {
        return _currentItem;
    }
}
