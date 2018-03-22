using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDebugTextOutput : MonoBehaviour {
  public Camera cp;
  public GameObject refer;

  private GenerateCrosshair gen;

  private Text t;

  void Awake ()
  {
    t = GetComponent<Text>();
    gen = refer ? refer.GetComponent<GenerateCrosshair>() : null;

    set();
  }

  void Start ()
  {
    
  }

  void Update ()
  {
    set();
  }


  void set ()
  {
    string fov = cp ? cp.fieldOfView.ToString() : "N/A";
    string den = gen ? gen.CHDensity.ToString() : "N/A";
    string cen = gen ? gen.CHCenterSize.ToString() : "N/A";
    string len = gen ? gen.CHLength.ToString() : "N/A";
    string dis = gen ? gen.CHDistance.ToString() : "N/A";

    t.text = "        Field of View: " + fov + " deg\n"
           + "    Crosshair Density: " + den + " px\n"
           + "Crosshair Center Size: " + cen + " px\n"
           + "     Crosshair Length: " + len + " px\n"
           + "   Crosshair Distance: " + dis + " px\n";
  }
}
