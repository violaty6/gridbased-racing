using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    
    [SerializeField] private GameObject _smokeParticle;
    [SerializeField] List<GameObject> _smokeParticlePool = new List<GameObject>();
    [SerializeField] private GameObject _explosionParticle;
    [SerializeField] List<GameObject> _explosionParticlePool = new List<GameObject>();

    private void Start()
    { 
        GameEvents.current.onMove += MoveParticle;
         for(int i = 0; i < 6; i++)
         {
             AddPool(_smokeParticle,_smokeParticlePool);
             AddPool(_explosionParticle,_explosionParticlePool);
         }
    }

    void AddPool(GameObject particle, List<GameObject> poolList)
    {
        GameObject tmp = Instantiate(particle);
        tmp.SetActive(false);
        poolList.Add(tmp);
    }

    public GameObject GetPooledObject(List<GameObject> pool)
    {
        for(int i = 0; i < pool.Count; i++)
        {
            if(!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }
        return null;
    }
    
    void MoveParticle(Transform objTrans)
    {
        GameObject poolObject = GetPooledObject(_smokeParticlePool);
        Debug.Log(poolObject);
        if (poolObject != null)
        {
            poolObject.transform.position = objTrans.position;
            poolObject.SetActive(true);
        }
    }
}
