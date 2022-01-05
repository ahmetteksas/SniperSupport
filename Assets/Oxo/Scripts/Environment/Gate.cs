using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using System.Linq;
using MoreMountains.NiceVibrations;
using DG.Tweening;
using UnityEngine.UI;
using System;
public class Gate : MonoBehaviour
{
    public List<GameObjectCollection> releatedColelctionList;

    //public FloatReference relatedFloatRight;
    //public FloatReference relatedFloatLeft;

    public int value = 2;

    [SerializeField] private bool isMultiply;

    private Text gateText;
    //private int multiplyTwo = 2;
    //private int minesThree = -3;

    //public Text plusTwoText;
    //public Text multiplyTwoText;
    //public Text minesThreeText;
    private void Awake()
    {

        gateText = GetComponentInChildren<Text>();
    }

    void Start()
    {
        if (isMultiply)
        {
            gateText.text = ("X" + value.ToString());

        }
        else
        {
            //gateText = GetComponentInChildren<Text>();
          
                gateText.text = ("+" + value.ToString());
            
            
        }
        //plusTwoText.text = ("+" + value).ToString();
        //multiplyTwoText.text = ("X" + multiplyTwo).ToString();
        //minesThreeText.text = minesThree.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerRight") && isMultiply)
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (var item in releatedColelctionList)
                {
                    GameObject egg = ObjectPool.instance.SpawnFromPool("Egg",new Vector3(transform.position.x, 0, transform.position.z-28f), Quaternion.identity);
                    egg.GetComponent<StackCollectable>().Collect(other.gameObject);
                    //releatedColelctionList.FirstOrDefault().Add(gameObject);
                    //relatedFloatRight.Value++;
                    //relatedFloatRight.Value *= 3;

                }
            }
        }
        if (other.gameObject.CompareTag("PlayerLeft") && isMultiply)
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (var item in releatedColelctionList)
                {
                    {
                        GameObject egg = ObjectPool.instance.SpawnFromPool("Egg", new Vector3(transform.position.x, 0, transform.position.z-28f), Quaternion.identity);
                        egg.GetComponent<StackCollectable>().Collect(other.gameObject);
                        //relatedFloatRight.Value++;
                        //relatedFloatLeft.Value *= 3;
                    }
                }
            }
        }
        if (other.gameObject.CompareTag("PlayerRight") && !isMultiply)
        {
            for (int i = 0; i < 2; i++)
            {
                    GameObject egg = ObjectPool.instance.SpawnFromPool("Egg", new Vector3(transform.position.x, 0, transform.position.z-28f), Quaternion.identity);
                    egg.GetComponent<StackCollectable>().Collect(other.gameObject);
                    //releatedColelctionList.FirstOrDefault().Add(gameObject);
                    //relatedFloatRight.Value++;
                    //relatedFloatRight.Value ++;
            }
        }
        if (other.gameObject.CompareTag("PlayerLeft") && !isMultiply)
        {
            for (int i = 0; i < 2; i++)
            {
                    
                        GameObject egg = ObjectPool.instance.SpawnFromPool("Egg", new Vector3(transform.position.x, 0, transform.position.z-28f), Quaternion.identity);
                        egg.GetComponent<StackCollectable>().Collect(other.gameObject);
                        //relatedFloatRight.Value++;
                        //relatedFloatLeft.Value ++;

            }
        }
  


}
}

