﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sebastian.Geometry;

public class RunTimeCreator : MonoBehaviour {

    ShapeCreator shapeCreator;
    Loader loader;
    public Color fillColor;
    // Use this for initialization
    float GetAngle(Vector3 center, Vector3 point)
    {
        var x = point.x - center.x;
        var y = point.z - center.z;
        if (x == 0) return (y > 0) ? 180 : 0;
        var a = Mathf.Atan(y / x) * 180 / Mathf.PI;
        a = (x > 0) ? a + 90 : a + 270;
        return a;
    }
    public void CreateBorder()
    {
        for (int i = 0; i < shapeCreator.shapes[0].points.Count; i++)
        {
            GameObject point = (GameObject)Instantiate(Resources.Load("Prefubs/Point"));
            point.transform.position = new Vector3(shapeCreator.shapes[0].points[i].x, 5, shapeCreator.shapes[0].points[i].z);
            point.name = "point " + i.ToString();
            point.transform.SetParent(transform.parent);
            GameObject obj = (GameObject)Instantiate(Resources.Load("Prefubs/Border1"));
            obj.name = "side " + i.ToString();
            obj.GetComponent<MeshRenderer>().material.color = fillColor;
            Vector3 side;
            Vector3 len;
            float angle = 0;
            float length = 0;
            if (i == shapeCreator.shapes[0].points.Count - 1)
            {
                side = shapeCreator.shapes[0].points[i] + shapeCreator.shapes[0].points[0];
                len = shapeCreator.shapes[0].points[i] - shapeCreator.shapes[0].points[0];
                angle = GetAngle(shapeCreator.shapes[0].points[i], shapeCreator.shapes[0].points[0]);
            }
            else
            {
                side = shapeCreator.shapes[0].points[i] + shapeCreator.shapes[0].points[i + 1];
                len = shapeCreator.shapes[0].points[i + 1] - shapeCreator.shapes[0].points[i];
                angle = GetAngle(shapeCreator.shapes[0].points[i], shapeCreator.shapes[0].points[i + 1]);
            }
            length = Mathf.Sqrt((len.x * len.x) + (len.z * len.z));
            obj.transform.localScale = new Vector3(length, 0.1f, 0.1f);
            obj.transform.Rotate(new Vector3(0, -angle, 0));
            obj.transform.position = new Vector3(side.x / 2, 5, side.z / 2);
            obj.transform.SetParent(transform.parent);
        }
    }
    public void HideBorder()
    {
        transform.parent.Find("Shape Creator").gameObject.SetActive(false);
    }
    public void ShowBorder()
    {
        transform.parent.Find("Shape Creator").gameObject.SetActive(true);
    }
    public void HideShape()
    {
        transform.parent.Find("Shape Mesh").gameObject.SetActive(false);
    }
    public void ShowShape()
    {
        transform.parent.Find("Shape Mesh").gameObject.SetActive(true);
    }
    public void LoadShape()
    {
        int index = loader.FindShape(gameObject.transform.parent.name);
        if (index < 0) { return; }
        shapeCreator.AddShape(Loader.instance.Shapes[index]);
        shapeCreator.UpdateMeshDisplay();
    }
    void ChooseColor()
    {
        if (Loader.instance.colors.Count < 1)
        {
            Loader.instance.InitColors();   
        }
        fillColor = Loader.instance.colors[0];
        Loader.instance.colors.RemoveAt(0);
    }
    void Start () {
        ChooseColor();
        Load();
    }
	void Load() {
        loader = GameObject.Find("Scripts").GetComponent<Loader>();
        transform.parent.Find("Shape Mesh").GetComponent<MeshRenderer>().material.color = fillColor;
        shapeCreator = GetComponent<ShapeCreator>();
    }
}