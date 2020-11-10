
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Canvas))]
public class SphereGenerator : NetworkBehaviour
{

    [SerializeField]
    private Pipe pipe;

    [SerializeField]
    private Label label;

    private int spacing = 10;

    private int spacingStep = 5;

    private int sphereRadius = 3;

    private int lastSphereRadius = 0;

    private List<Pipe> longitudeList = new List<Pipe>();

    private List<Label> longitudeLabelList = new List<Label>();

    private List<Label> latitudeLabelList = new List<Label>();

    private List<Pipe> latitudeList = new List<Pipe>();

    private int longitudeDegrees = 360;

    private int latitudeDegrees = 180;

    private Canvas cv;

    private GameObject labelParent;

    private GameObject sphereParent;

    void Awake()
    {
        cv = GetComponent<Canvas>();
        sphereParent = new GameObject("sphere");
        labelParent = new GameObject("Labels");
        sphereParent.transform.SetParent(transform);
        labelParent.transform.SetParent(transform);
        sphereParent.SetActive(false);
        labelParent.SetActive(false);
        LocalGenerate();
    }

    private Pipe CreateTorus()
    {
        var instPipe = Instantiate(pipe, transform);
        instPipe.CurveRadius = sphereRadius;
        instPipe.PipeRadius = 0.009f;
        instPipe.CurveSegmentCount = 50;
        instPipe.PipeSegmentCount = 6;
        return instPipe;
    }

    [ClientRpc]
    private void RpcGenerate(int longDeg, int latDeg, int spacing, int sphereRadius)
    {

        this.longitudeDegrees = longDeg;
        this.latitudeDegrees = latDeg;
        this.spacing = spacing;
        this.sphereRadius = sphereRadius;

        LocalGenerate();
    }

    private void LocalGenerate()
    {

        var longitudeLines = Mathf.Ceil(longitudeDegrees / spacing);
        var latitudeLines = Mathf.Ceil(latitudeDegrees / spacing);

        CreateAndAddLongitudeLines(longitudeLines);
        CreateAndAddLatituedLines(latitudeLines);

        RecalculateLongitudeLines(longitudeLines);
        RecalculateLatitudeLines(latitudeLines);

    }

    void triggerGenerate()
    {
        this.LocalGenerate();
        this.RpcGenerate(this.longitudeDegrees, this.latitudeDegrees, this.spacing, this.sphereRadius);
    }

    private void CreateAndAddLongitudeLines(float lines)
    {
        if (longitudeList.Count < lines)
        {
            var diff = lines - longitudeList.Count;
            for (int i = 0; i < diff; i++)
            {

                var label = Instantiate(this.label, transform);
                label.transform.SetParent(labelParent.transform);
                var tr = (RectTransform)label.transform;

                tr.sizeDelta = new Vector2(2, 1);
                longitudeLabelList.Add(label);
                var torus = CreateTorus();
                torus.transform.SetParent(sphereParent.transform);
                torus.RingDistance = Mathf.PI;
                longitudeList.Add(torus);
            }
        }
    }

    private void RecalculateLongitudeLines(float totalLines)
    {

        float spaceCounter = 0;
        var longitudeSpacing = longitudeDegrees / spacing;
        Debug.Log(longitudeSpacing);

        foreach (var item in longitudeList)
        {
            item.transform.rotation = Quaternion.Euler(0, 0, 0);
            if (spaceCounter > longitudeDegrees)
            {
                item.gameObject.SetActive(false);
            }
            else
            {
                item.gameObject.SetActive(true);
                var rotateDegrees = Mathf.Min((float)(spaceCounter), longitudeDegrees);
                item.transform.Rotate(0, rotateDegrees, 0);
                item.name = $"LONGITUDE: {spaceCounter}";
                item.CurveRadius = sphereRadius;
                item.Generate();

            }
            spaceCounter += spacing;
        }
        Debug.Log(spaceCounter);
        spaceCounter = 0;
        var localpos = this.transform.localPosition;

        foreach (var label in longitudeLabelList)
        {
            if (spaceCounter > longitudeDegrees)
            {
                label.gameObject.SetActive(false);
                continue;
            }
            label.gameObject.SetActive(true);
            var rotateDegrees = Mathf.Min((float)(spaceCounter), longitudeDegrees);
            var deg = spaceCounter;
            float x = localpos.x + Mathf.Cos((deg + 90) * Mathf.Deg2Rad) * (sphereRadius - 0.25f);
            float y = localpos.z + Mathf.Sin((deg + 90) * Mathf.Deg2Rad) * (sphereRadius - 0.25f);
            var labelText = longitudeDegrees - (spaceCounter);
            label.transform.rotation = Quaternion.Euler(0, 0, 0);

            labelText = labelText == 360 ? 0 : labelText;
            label.SetTextOnLabel("" + labelText);
            var pos = transform.position;
            pos.z += (float)y;
            pos.x += (float)x;
            pos.y = this.transform.position.y;
            label.transform.position = pos;
            label.transform.Rotate(0, -rotateDegrees, 0);
            spaceCounter += spacing;
        }
    }

    private void RecalculateLatitudeLines(float totalLines)
    {
        var centerPosition = transform.position.y;
        var halfLineCount = totalLines / 2;
        float lineCounter = -halfLineCount;

        Vector2[] eew = new Vector2[100];
        Debug.Log(eew.Length);

        int q = 0;
        foreach (var item in latitudeList)
        {
            if (lineCounter > totalLines || lineCounter > halfLineCount)
            {
                item.gameObject.SetActive(false);
            }
            else
            {
                item.gameObject.SetActive(true);
                item.transform.position = new Vector3(item.transform.position.x, centerPosition + (Mathf.Sin(Mathf.Deg2Rad * (spacing * lineCounter)) * sphereRadius), item.transform.position.z);
                item.CurveRadius = Mathf.Cos(Mathf.Deg2Rad * (spacing * lineCounter)) * sphereRadius;

                eew[q] = item.transform.position;
                item.name = $"LATITUDE: {spacing * lineCounter}";
                item.Generate();
                q++;
            }
            lineCounter++;
        }

        float spaceCounter = -90;
        Debug.Log(spacing);
        q = 0;
        var localpos = this.transform.localPosition;
        foreach (var label in latitudeLabelList)
        {
            if (spaceCounter > 90)
            {
                label.gameObject.SetActive(false);
                continue;
            }
            label.gameObject.SetActive(true);
            var rotateDegrees = Mathf.Min((float)(spaceCounter), latitudeDegrees);
            var deg = spaceCounter;
            float x = localpos.z + Mathf.Cos(deg * Mathf.Deg2Rad) * (sphereRadius - 0.1f);
            float y = localpos.y + Mathf.Sin(deg * Mathf.Deg2Rad) * (sphereRadius - 0.25f);
            var labelText = spaceCounter;
            label.transform.rotation = Quaternion.Euler(0, 0, 0);

            label.SetTextOnLabel("" + labelText);
            var pos = transform.position;
            pos.z += x;
            pos.y = eew[q].y;
            label.transform.position = pos;
            spaceCounter += spacing;
            q++;
        }
    }

    private void CreateAndAddLatituedLines(float lines)
    {
        if (latitudeList.Count < lines)
        {
            var diff = lines - latitudeList.Count;
            for (int i = 0; i < diff; i++)
            {
                var torus = CreateTorus();
                torus.transform.SetParent(sphereParent.transform);
                torus.transform.Rotate(90, 0, 0);
                latitudeList.Add(torus);

                var label = Instantiate(this.label, transform);
                label.transform.SetParent(labelParent.transform);
                var tr = (RectTransform)label.transform;
                tr.sizeDelta = new Vector2(2, 1);
                latitudeLabelList.Add(label);

            }
        }
    }

    void triggerFontsizing(bool increase, float size)
    {
        if (increase)
        {
            IncreaseAllLabelFontSizes(size);
        }
        else
        {
            DecreaseAllLabelFontSizes(size);
        }

        RpcFontSize(increase, size);
    }

    [ClientRpc]
    void RpcFontSize(bool increase, float size)
    {
        if (increase)
        {
            IncreaseAllLabelFontSizes(size);
        }
        else
        {
            DecreaseAllLabelFontSizes(size);
        }
    }

    private void IncreaseAllLabelFontSizes(float size)
    {
        foreach (var label in latitudeLabelList)
        {
            label.IncreaseFontSize(size);
        }
        foreach (var label in longitudeLabelList)
        {
            label.IncreaseFontSize(size);
        }
    }

    private void DecreaseAllLabelFontSizes(float size)
    {
        foreach (var label in latitudeLabelList)
        {
            label.DecreaseFontSize(size);
        }
        foreach (var label in longitudeLabelList)
        {
            label.DecreaseFontSize(size);
        }
    }

    void Update()
    {

        if (!isServer) return;
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            // Increase stepping
            this.spacing = Mathf.Min(spacing + spacingStep, 45);
            if (this.latitudeDegrees % this.spacing != 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    this.spacing += this.spacingStep;
                    if ((this.latitudeDegrees / 2) % this.spacing == 0)
                    {
                        break;
                    }
                }
            }
            triggerGenerate();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            // Decrease stepping
            this.spacing = Mathf.Max(spacing - spacingStep, 5);
            if (this.latitudeDegrees % this.spacing != 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    this.spacing -= this.spacingStep;
                    if ((this.latitudeDegrees / 2) % this.spacing == 0)
                    {
                        Debug.Log(spacing);
                        break;
                    }
                }
            }
            triggerGenerate();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            // Increase sphereRadius
            this.lastSphereRadius = this.sphereRadius;
            this.sphereRadius = Mathf.Min(sphereRadius + 1, 20);
            if (this.sphereRadius != this.lastSphereRadius)
            {
                triggerFontsizing(true, 0.04f);
            }
            triggerGenerate();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            // Decrease sphereRadius
            this.lastSphereRadius = this.sphereRadius;
            this.sphereRadius = Mathf.Max(sphereRadius - 1, 3);
            if (this.sphereRadius != this.lastSphereRadius)
            {
                triggerFontsizing(false, 0.04f);
            }
            triggerGenerate();
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            TriggerHideSphere();
        }
    }


    private void TriggerHideSphere()
    {
        HideSphere();
        RpcTriggerHideSphere();
    }

    [ClientRpc]
    private void RpcTriggerHideSphere()
    {
        HideSphere();
    }

    private void HideSphere()
    {
        sphereParent.SetActive(!sphereParent.activeSelf);
        labelParent.SetActive(!labelParent.activeSelf);
    }
}