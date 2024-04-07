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
    [SerializeField] private GameObject _OilParticle;
    [SerializeField] List<GameObject> _OilParticlePool = new List<GameObject>();


    [SerializeField] private int listenerId;

    private void Start()
    { 
        GameEvents.current.onMove += MoveParticle;
        GameEvents.current.onCrash += CrashParticle;
        GameEvents.current.onOil += OilParticle;
         for(int i = 0; i < 6; i++)
         {
             AddPool(_smokeParticle,_smokeParticlePool);
             AddPool(_explosionParticle,_explosionParticlePool);
             AddPool(_OilParticle,_OilParticlePool);
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
    
    void MoveParticle(Transform objTrans,int id,bool selfCommand)
    {
        if(id != listenerId) return;
        GameObject poolObject = GetPooledObject(_smokeParticlePool);
        if (poolObject != null)
        {
            poolObject.transform.position = objTrans.position;
            poolObject.SetActive(true);
        }
    }

    void CrashParticle(Transform objTrans,int id)
    {
        if(id != listenerId) return;
        GameObject poolObject = GetPooledObject(_explosionParticlePool);
        if (poolObject != null)
        {
            poolObject.transform.position = objTrans.position;
            poolObject.SetActive(true);
        }
    }
    void OilParticle(Transform objTrans,int id)
    {
        if(id != listenerId) return;
        GameObject poolObject = GetPooledObject(_OilParticlePool);
        if (poolObject != null)
        {
            poolObject.transform.position = objTrans.position;
            poolObject.SetActive(true);
        }
    }
}
