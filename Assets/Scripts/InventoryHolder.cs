using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHolder : MonoBehaviour
{
    private Transform _holderPossition;//трансформ в которой помещаем подобранные объекты
    private LightController _lightController; //сюда добавим доступ к детектору интерактивных объектов
                                              //чтобы проверять есть ли в данный момент подсвеченный объект
    private InteractableItem _currenItem;//сюда сохраняем ссылку на объект в руках
    private float _forceValue = 300f;
    
    private bool _worldPossition;//флаг в котором помечаем какие координаты использовать
    private void Start() //записываем в поле трансформ нашего держателя и находим детектор светящихся объектов
    {
        _holderPossition = transform;
        _lightController = FindObjectOfType<LightController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeItem();//подбираем объекты
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ThrowItem();//швыряем объекты
        }
    }

    private void TakeItem()
    {
        if (_lightController.GetCurrentItem() != null)//если есть подсвеченные объекты
        {
            if (_currenItem!=null) //если уже есть подобранный объект сбрасываем его
            {
                DropItem();
            }
            
            _currenItem = _lightController.GetCurrentItem();//забираем ссылку на объект
            _lightController.ResetItem();//сбрасываем ссылку в контроллере
            
            _currenItem.transform.SetParent(_holderPossition,_worldPossition);//устанавливаем объект в держатель
            _currenItem.transform.localPosition = Vector3.zero; // Устанавливаем позицию в центре родителя

            var rigit = _currenItem.GetComponent<Rigidbody>(); //считываем риджит объекта
            rigit.isKinematic = true; //отключаем действие физики
        }
    }

    private void DropItem()
    {
        if (_currenItem!=null) //если есть что сбросить 
        {
            _currenItem.transform.SetParent(null,!_worldPossition); //обнуляем родителя и сохраняем мировые координаты
            var rigit = _currenItem.GetComponent<Rigidbody>(); //берем риджит объекта 
            rigit.isKinematic = false; //делаем физичным
            _currenItem = null; //удаляем ссылку на объект из текущих в инвентаре
        }
    }

    private void ThrowItem()
    {
        if (_currenItem!=null) //если есть объект в инвентаре
        {
            var rigit = _currenItem.GetComponent<Rigidbody>();//берем его риджит
            rigit.isKinematic = false; //делаем физичным
            rigit.AddForce(_holderPossition.forward*_forceValue,ForceMode.Force);//кидаем по направлению взгляда
            
            _currenItem.transform.SetParent(null,!_worldPossition);//обнуляем родителя
            _currenItem = null; //удаляем из инвентаря
        }
    }
    
}
