using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] public List<GameObject> enemyActive = new List<GameObject>();
    [SerializeField] private List<GameObject> enemyPos;
    [SerializeField] private CardData cardData;

    public void Spawn1Day1()
    {
        var e1 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[0].transform.position, Quaternion.identity);
        enemyActive.Add(e1.gameObject);
        var e2 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[2].transform.position, Quaternion.identity);
        enemyActive.Add(e2.gameObject);
    }

    public void Spawn1Day2()
    {
        var e1 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[0].transform.position, Quaternion.identity);
        enemyActive.Add(e1.gameObject);
        var e2 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[1].transform.position, Quaternion.identity);
        enemyActive.Add(e2.gameObject);
        var e3 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[2].transform.position, Quaternion.identity);
        enemyActive.Add(e3.gameObject);
    }

    public void Spawn1Day3()
    {
        var e1 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[0].transform.position, Quaternion.identity);
        enemyActive.Add(e1.gameObject);
        var e2 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[3].transform.position, Quaternion.identity);
        enemyActive.Add(e2.gameObject);
        var e3 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[2].transform.position, Quaternion.identity);
        enemyActive.Add(e3.gameObject);
        var e4 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[5].transform.position, Quaternion.identity);
        enemyActive.Add(e4.gameObject);
    }

    public void Spawn1Day4()
    {
        var e1 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[0].transform.position, Quaternion.identity);
        enemyActive.Add(e1.gameObject);
        var e2 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[1].transform.position, Quaternion.identity);
        enemyActive.Add(e2.gameObject);
        var e3 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[2].transform.position, Quaternion.identity);
        enemyActive.Add(e3.gameObject);
        var e4 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[3].transform.position, Quaternion.identity);
        enemyActive.Add(e4.gameObject);
        var e5 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[5].transform.position, Quaternion.identity);
        enemyActive.Add(e5.gameObject);
    }

    public void Spawn1Day5()
    {
        var e1 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[0].transform.position, Quaternion.identity);
        enemyActive.Add(e1.gameObject);
        var e2 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[1].transform.position, Quaternion.identity);
        enemyActive.Add(e2.gameObject);
        var e3 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[2].transform.position, Quaternion.identity);
        enemyActive.Add(e3.gameObject);
        var e4 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[3].transform.position, Quaternion.identity);
        enemyActive.Add(e4.gameObject);
        var e5 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[5].transform.position, Quaternion.identity);
        enemyActive.Add(e5.gameObject);
        var e6 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[4].transform.position, Quaternion.identity);
        enemyActive.Add(e6.gameObject);
    }

    public void Spawn2Day1()
    {
        var e1 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[0].transform.position, Quaternion.identity);
        enemyActive.Add(e1.gameObject);
        var e2 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[1], enemyPos[1].transform.position, Quaternion.identity);
        enemyActive.Add(e2.gameObject);
        var e3 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[2].transform.position, Quaternion.identity);
        enemyActive.Add(e3.gameObject);
    }

    public void Spawn2Day2()
    {
        var e1 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[1], enemyPos[0].transform.position, Quaternion.identity);
        enemyActive.Add(e1.gameObject);
        var e2 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[1].transform.position, Quaternion.identity);
        e2.maxHp *= 1.2f;
        e2.currentHp = e2.maxHp;
        enemyActive.Add(e2.gameObject);
        var e3 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[1], enemyPos[2].transform.position, Quaternion.identity);
        enemyActive.Add(e3.gameObject);
    }

    public void Spawn2Day3()
    {
        var e1 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[1], enemyPos[0].transform.position, Quaternion.identity);
        enemyActive.Add(e1.gameObject);
        var e2 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[1].transform.position, Quaternion.identity);
        enemyActive.Add(e2.gameObject);
        var e3 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[4].transform.position, Quaternion.identity);
        enemyActive.Add(e3.gameObject);
        var e4 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[1], enemyPos[2].transform.position, Quaternion.identity);
        enemyActive.Add(e4.gameObject);
    }

    public void Spawn2Day4()
    {
        var e1 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[1], enemyPos[0].transform.position, Quaternion.identity);
        enemyActive.Add(e1.gameObject);
        var e2 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[1], enemyPos[3].transform.position, Quaternion.identity);
        enemyActive.Add(e2.gameObject);
        var e3 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[1].transform.position, Quaternion.identity);
        enemyActive.Add(e3.gameObject);
        var e4 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[4].transform.position, Quaternion.identity);
        enemyActive.Add(e4.gameObject);
        var e5 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[1], enemyPos[2].transform.position, Quaternion.identity);
        enemyActive.Add(e5.gameObject);
        var e6 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[1], enemyPos[5].transform.position, Quaternion.identity);
        enemyActive.Add(e6.gameObject);
    }

    public void Spawn2Day5()
    {
        var e1 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[1], enemyPos[0].transform.position, Quaternion.identity);
        enemyActive.Add(e1.gameObject);
        var e2 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[1], enemyPos[3].transform.position, Quaternion.identity);
        enemyActive.Add(e2.gameObject);
        var e3 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[1], enemyPos[1].transform.position, Quaternion.identity);
        enemyActive.Add(e3.gameObject);
        var e4 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[4].transform.position, Quaternion.identity);
        e4.maxHp *= 1.5f;
        e4.currentHp = e3.maxHp;
        enemyActive.Add(e4.gameObject);
        var e5 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[1], enemyPos[2].transform.position, Quaternion.identity);
        enemyActive.Add(e5.gameObject);
        var e6 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[1], enemyPos[5].transform.position, Quaternion.identity);
        enemyActive.Add(e6.gameObject);
    }

    public void Spawn3Day1()
    {
        for (int i = 0; i <= 2; i++)
        {
            var e = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[1], enemyPos[i].transform.position, Quaternion.identity);
            enemyActive.Add(e.gameObject);
        }
    }

    public void Spawn3Day2()
    {
        var e1 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[0].transform.position, Quaternion.identity);
        enemyActive.Add(e1.gameObject);
        var e2 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[1], enemyPos[1].transform.position, Quaternion.identity);
        enemyActive.Add(e2.gameObject);
        var e3 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[3].transform.position, Quaternion.identity);
        e3.maxHp *= 1.5f;
        e3.currentHp = e2.maxHp;
        enemyActive.Add(e3.gameObject);
    }

    public void Spawn3Day3()
    {
        var e1 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[0].transform.position, Quaternion.identity);
        enemyActive.Add(e1.gameObject);
        var e2 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[1], enemyPos[1].transform.position, Quaternion.identity);
        enemyActive.Add(e2.gameObject);
        var e3 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[2].transform.position, Quaternion.identity);
        enemyActive.Add(e3.gameObject);
        var e4 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[4].transform.position, Quaternion.identity);
        enemyActive.Add(e4.gameObject);
    }

    public void Spawn3Day4()
    {
        var list = new int[] { 0, 1, 2, 3, 5 };
        foreach (var i in list)
        {
            var e = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[i == 1 ? 1 : 0], enemyPos[i].transform.position, Quaternion.identity);
            enemyActive.Add(e.gameObject);
        }
    }

    public void Spawn3Day5()
    {
        var list = new int[] { 0, 1, 2, 3, 4, 5 };
        foreach (var i in list)
        {
            var e = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[i == 1 ? 1 : 0], enemyPos[i].transform.position, Quaternion.identity);
            enemyActive.Add(e.gameObject);
        }
    }

    public void Spawn4Day1()
    {
        var e = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[1], enemyPos[1].transform.position, Quaternion.identity);
        e.maxHp *= 4f;
        e.currentHp = e.maxHp;
        e.damage *= 2f;
        e.attackSpeed *= 1.2f;
        e.transform.localScale *= 1.2f;
        enemyActive.Add(e.gameObject);
    }

    public void Spawn4Day2()
    {
        var e = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[1].transform.position, Quaternion.identity);
        e.maxHp *= 4f;
        e.currentHp = e.maxHp;
        e.damage *= 2f;
        e.attackSpeed *= 1.2f;
        e.transform.localScale *= 1.2f;
        enemyActive.Add(e.gameObject);
    }

    public void Spawn5Day1()
    {
        var e1 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[1], enemyPos[0].transform.position, Quaternion.identity);
        e1.maxHp *= 2f;
        e1.currentHp = e1.maxHp;
        e1.damage *= 1.2f;
        e1.attackSpeed *= 1.2f;
        e1.transform.localScale *= 1.2f;
        enemyActive.Add(e1.gameObject);

        var e2 = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[0], enemyPos[2].transform.position, Quaternion.identity);
        e2.maxHp *= 2f;
        e2.currentHp = e2.maxHp;
        e2.damage *= 1.2f;
        e2.attackSpeed *= 1.2f;
        e2.transform.localScale *= 1.2f;
        enemyActive.Add(e2.gameObject);
    }

    public void Spawn5Day2()
    {
        var e = PoolingManager.Spawn<EnemyController>(cardData.cardEnemy[2], enemyPos[1].transform.position, Quaternion.identity);
        enemyActive.Add(e.gameObject);
    }
}
