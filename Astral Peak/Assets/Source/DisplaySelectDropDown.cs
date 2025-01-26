using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySelectDropDown : MonoBehaviour{
    [SerializeField] Dropdown drop_down;
    void Awake(){
        // clear drop down.
        drop_down.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 1; i < Display.displays.Length; i++)
            options.Add("Display " + (i + 1));  // Display 1, Display 2, etc.
        // add available displays to drop down.
        drop_down.AddOptions(options);
    }
    public void display_selected(int index) => DisplaySettingsManager.set_display(index);
}
