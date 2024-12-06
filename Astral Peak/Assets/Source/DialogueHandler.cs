using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueHandler : MonoBehaviour{
    public static DialogueHandler instance;
    public event Action<int> new_line;
    public event Action dialogue_ended;
    [SerializeField] string 
        dialogue_file,
        dialoge_option;
    List<string> dialogue = new List<string>();
    public TextMeshProUGUI text;    
    int index = -1;

    // text colours.
    string highlight_hex = "#00DDFF";

    void Awake(){
        instance = this;
        if(dialogue_file != "" && dialoge_option != "")
            dialogue = ExcelReader.read_dialogue(dialogue_file, dialoge_option);
    }

    public void start_dialogue(){
        index = -1;
        next_line();
    }
    public void play_dialogue(float text_time){
        StartCoroutine(dialogue_loop(text_time));
    }
    public void next_line(){
        index++;
        StopAllCoroutines();
        StartCoroutine(fade_loop(5));
    }
    public void play_line(int x){
        index = x;
        StopAllCoroutines();
        StartCoroutine(fade_loop(5));
    }
    void set_text(){
        if(index < dialogue.Count){
            text.text = $"{dialogue[index].Replace("#{highlight_hex}", highlight_hex)}";
            new_line?.Invoke(index); // invoke that a new line has started.
        }
        else
            dialogue_ended?.Invoke();
    }

    IEnumerator fade_loop(float text_time){
        Color transparent = new Color(1, 1, 1, 0);
        while (Mathf.Abs(text.color.a - transparent.a) > 0.01f){
            text.color = Color.Lerp(text.color, transparent, Time.deltaTime * 5); // Smooth fade
            yield return null;
        }
        text.color = transparent;
        set_text();
        yield return new WaitForSeconds(0.5f);
        while (Mathf.Abs(text.color.a - 1f) > 0.01f){
            text.color = Color.Lerp(text.color, Color.white, Time.deltaTime * 2.5f); // Smooth fade
            yield return null;
        }
        yield return new WaitForSeconds(text_time);
        while (Mathf.Abs(text.color.a - transparent.a) > 0.01f){
            text.color = Color.Lerp(text.color, transparent, Time.deltaTime * 5); // Smooth fade
            yield return null;
        }
        text.color = transparent;
        yield break;
    }
    IEnumerator dialogue_loop(float text_time){
        while(index < dialogue.Count) {
            index++;
            yield return StartCoroutine(fade_loop(text_time)); // Ensure it waits for the fade to finish
        }
        yield break;
    }
}
