using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScroller : MonoBehaviour{
    [SerializeField] GameObject prefab;
    [SerializeField] int amount = 1;
    [SerializeField] float speed = 1;
    [SerializeField] Transform start_point, end_point;
    [SerializeField] List<GameObject> objects = new List<GameObject>();
    public void start_scrolling() => StartCoroutine(scroll());
    public void stop_scrolling() => StopAllCoroutines();
    IEnumerator scroll(){
        Vector3 movement = (end_point.position - start_point.position).normalized * speed;
        while(true){
            foreach(GameObject o in objects){
                o.transform.position = Mathf.Abs((end_point.position-o.transform.position).magnitude) <=0.1f
                    ?start_point.position
                    :o.transform.position += movement; 
            }
            yield return new WaitForFixedUpdate(); 
        }
    }
    public void reverse_scroll(){
        Transform temp = start_point;
        start_point = end_point;
        end_point = temp; 
    }
    public void instantiate_objects(){
        Vector3 distance = end_point.position - start_point.position;
        float factor = (float)1/amount;
        float x = factor;
        objects.Clear();
        while(x<1){
            Vector3 pos = start_point.position + distance * x;
            objects.Add(Instantiate(prefab, pos, Quaternion.identity));
            x += factor;
        }
            objects.Add(Instantiate(prefab, end_point.position, Quaternion.identity));
    }
    public List<GameObject> get_objects() => objects;
}
