using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> sellectBox = new List<GameObject>();
    [SerializeField] public List<SelectBoxController> BoxChoice = new List<SelectBoxController>();

    private void OnDisable()
    {
        BoxChoice.Clear();
    }

    void Update()
    {
        // Xoá box null
        for (int i = BoxChoice.Count - 1; i >= 0; i--)
        {
            if (BoxChoice[i] == null)
            {
                BoxChoice.RemoveAt(i);
            }
        }

        // Nếu không có box nào đang active thì active box đầu tiên
        if (!BoxChoice.Any(x => x.gameObject.activeSelf) && BoxChoice.Count > 0 && Camera.main.gameObject.transform.position.y == 0f)
        {
            BoxChoice[0].gameObject.SetActive(true);
        }
    }

    public void SpawnChoices1()
    {
        SelectBoxController spawn = PoolingManager.Spawn<SelectBoxController>(sellectBox[0], transform.position, Quaternion.identity);
        BoxChoice.Add(spawn);
        ActiveOnlyOneBox(spawn);
    }

    public void SpawnChoices2()
    {
        SelectBoxController spawn = PoolingManager.Spawn<SelectBoxController>(sellectBox[1], transform.position, Quaternion.identity);
        BoxChoice.Add(spawn);
        ActiveOnlyOneBox(spawn);
    }

    public void ActiveOnlyOneBox(SelectBoxController activeBox)
    {
        foreach (var box in BoxChoice)
        {
            if (box == activeBox)
                box.gameObject.SetActive(true);
            else
                box.gameObject.SetActive(false);
        }
    }
}