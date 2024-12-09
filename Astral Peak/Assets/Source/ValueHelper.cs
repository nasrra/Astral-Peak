using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.UI;

public static class ValueHelper{
    public static IEnumerator lerp_image_colour(Image image, Color color, float time){
        float elapsedTime = 0f;
        while (elapsedTime < time){
            elapsedTime += Time.deltaTime;
            image.color = Color.Lerp(image.color, color, elapsedTime/time/60); // have to divide by 100 for some reason, dunno why lol.
            yield return null;
        }
        image.color = color;
        yield break;
    }

    public static IEnumerator lerp_colours(ColorParameter start, Color end, float time){
        float elapsedTime = 0f;
        while (elapsedTime < time){
            elapsedTime += Time.deltaTime;
            Color currentColor = Color.Lerp(start.value, end, elapsedTime/time/60); // have to divide by 100 for some reason, dunno why lol.
            start.value = currentColor;
            yield return null;
        }
        start.value = end;
        yield break;
    }

    public static IEnumerator lerp_value(FloatParameter value, float n_value, float time){
        while(Mathf.Abs(value.value - n_value) > 0.05f){
            value.value = Mathf.Lerp(value.value, n_value, Time.deltaTime * time);
            yield return null;
        }
        value.value = n_value;
        yield break;     
    }
}
