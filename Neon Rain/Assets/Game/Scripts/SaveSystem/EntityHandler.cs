using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHandler : MonoBehaviour
{
    public EntityData myData;
    
    // Start is called before the first frame update
    void Start()
    {
        if (string.IsNullOrEmpty(myData.id))
        {
            myData.id = System.DateTime.Now.ToLongDateString() + System.DateTime.Now.ToLongDateString() +
                        Random.Range(0, int.MaxValue).ToString();
            
            SaveData.Current.entityData.Add(myData);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        myData.position = transform.position;
        myData.rotation = transform.rotation;
    }
}
