using System.Collections;
using System.Collections.Generic;
using System.Linq;  
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Runtime.InteropServices;

public class Controller : MonoBehaviour
{
    public Vector3 center;
    public GameObject hawkPrefab;
    public GameObject dovePrefab;
    public GameObject foodPrefab;
    public int min;
    public int max;
    public TMP_InputField hawkNumber , doveNumber , foodNumber ;
    public TMP_Dropdown population;
    public TMP_InputField foodValue, injuryloss, bluffingloss, baseenergy;
    public int foodValueInt, injuryInt, bluffingInt, baseReqInt;
    private DD_DataDiagram m_DataDiagram;
    List<GameObject> lineList = new List<GameObject>();
    private float h = 0;
    public int index = 0;
    // Start is called before the first frame update
    void Start()
    {

        foodValueInt = System.Convert.ToInt32(foodValue.text);
        injuryInt = System.Convert.ToInt32(injuryloss.text);
        bluffingInt = System.Convert.ToInt32(bluffingloss.text);
        baseReqInt = System.Convert.ToInt32(baseenergy.text);
        GameObject dd = GameObject.Find("DataDiagram");
        if (null == dd)
        {
            return;
        }
        m_DataDiagram = dd.GetComponent<DD_DataDiagram>();
        m_DataDiagram.PreDestroyLineEvent += (s, e) => { lineList.Remove(e.line); };
        AddALine();
        AddALine();
        AddALine();
        
    }

    
    // Update is called once per frame
    void Update()
    {

    }

    void AddALine()
    {

        if (null == m_DataDiagram)
            return;

        Color color = Color.HSVToRGB((h += 0.1f) > 1 ? (h - 1) : h, 0.8f, 0.8f);
        GameObject line = m_DataDiagram.AddLine(color.ToString(), color);
        if (null != line)
            lineList.Add(line);
    }


    public GameObject spawnPrefab(GameObject prefab)
    {
        Vector3 pos = center + new Vector3(Random.Range(min, max), Random.Range(min, max), 0);

        return Instantiate(prefab, pos, Quaternion.identity, transform);
    }

    public void StartButtonClick()
    {

        GameObject hawk = GameObject.Find("Hawk(Clone)");
        GameObject dove = GameObject.Find("Dove(Clone)");
        GameObject food = GameObject.Find("Food(Clone)");
        int m = System.Convert.ToInt32(hawkNumber.text);
        int n = System.Convert.ToInt32(doveNumber.text);
        int p = System.Convert.ToInt32(foodNumber.text);
        while (hawk)
        {
            GameObject.DestroyImmediate(hawk);
            hawk = GameObject.Find("Hawk(Clone)");
        }
        while (dove)
        {
            GameObject.DestroyImmediate(dove);
            dove = GameObject.Find("Dove(Clone)");
        }
        while (food)
        {
            GameObject.DestroyImmediate(food);
            food = GameObject.Find("Food(Clone)");
        }
        for (int i = 0; i < m; i++)
            spawnPrefab(hawkPrefab);

        for (int i = 0; i < n; i++)
            spawnPrefab(dovePrefab);

        for (int i = 0; i < p; i++)
            spawnPrefab(foodPrefab);


    }

    public void NextButtonClick()
    {
        List<GameObject> foods = new List<GameObject>();
        List<GameObject> hawks = new List<GameObject>();
        List<GameObject> doves = new List<GameObject>();
        List<GameObject> agents = new List<GameObject>();
        for (int i = 0; i < System.Convert.ToInt32(foodNumber.text); i++)
        {
            spawnPrefab(foodPrefab);
        }
        foreach (GameObject fhd in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (fhd.name == "Food(Clone)")
            {
                foods.Add(fhd);
            }
            if (fhd.name == "Hawk(Clone)")
            {
                agents.Add(fhd);
                hawks.Add(fhd);
            }
            if (fhd.name == "Dove(Clone)")
            {
                agents.Add(fhd);
                doves.Add(fhd);
            }
        }
        while (agents != null)
        {
            if (foods.Count > 0)
            {

                if (agents.Count == 1)
                {
                    if (doves.Contains(agents[0]))
                    {
                        Dove dove = agents[0].transform.GetComponent<Dove>();
                        dove.energy = foodValueInt - baseReqInt + dove.energy;
                    }
                    if (hawks.Contains(agents[0]))
                    {
                        Hawk hawk = agents[0].transform.GetComponent<Hawk>();
                        hawk.energy = foodValueInt - baseReqInt + hawk.energy;
                    }

                    agents.RemoveAt(0);
                }
                else
                {
                    int r1 = Random.Range(1, agents.Count )-1;
                    int r2 = Random.Range(1, agents.Count )-1;
                    GameObject competitor1 = agents[r1];
                    GameObject competitor2 = agents[r2];
                    agents.RemoveAt(r1);
                    agents.RemoveAt(r2);
                    if (hawks.Contains(competitor1) && hawks.Contains(competitor2))
                    {
                        Hawk hawk1 = competitor1.transform.GetComponent<Hawk>();
                        Hawk hawk2 = competitor2.transform.GetComponent<Hawk>();
                        hawk1.energy = hawk1.energy + foodValueInt - baseReqInt;
                        hawk2.energy = hawk1.energy - injuryInt - baseReqInt;
                    }
                    if (doves.Contains(competitor1) && doves.Contains(competitor2))
                    {
                        Dove dove1 = competitor1.transform.GetComponent<Dove>();
                        Dove dove2 = competitor2.transform.GetComponent<Dove>();
                        dove1.energy = dove1.energy + foodValueInt - bluffingInt - baseReqInt;
                        dove2.energy = dove1.energy - bluffingInt - baseReqInt;
                    }
                    if (hawks.Contains(competitor1) && doves.Contains(competitor2))
                    {
                        Hawk hawk1 = competitor1.transform.GetComponent<Hawk>();
                        Dove dove1 = competitor2.transform.GetComponent<Dove>();
                        hawk1.energy = hawk1.energy + foodValueInt - baseReqInt;
                        dove1.energy = dove1.energy - baseReqInt;
                    }
                    if(doves.Contains(competitor1) && hawks.Contains(competitor2))
                    {
                        Dove dove1 = competitor1.transform.GetComponent<Dove>();
                        Hawk hawk1 = competitor2.transform.GetComponent<Hawk>();
                        dove1.energy = dove1.energy - baseReqInt;
                        hawk1.energy = hawk1.energy + foodValueInt - baseReqInt;
                    }
                }
                int r3 = Random.Range(1, foods.Count )-1;
                GameObject.DestroyImmediate(foods[r3]);
                foods.RemoveAt(0);
            }
            else
            {
                foreach(GameObject hd in agents)
                {
                    if (hawks.Contains(hd))
                    {
                        Hawk hawk = hd.transform.GetComponent<Hawk>();
                        hawk.energy = hawk.energy - baseReqInt;
                    }
                    else
                    {
                        Dove dove = hd.transform.GetComponent<Dove>();
                        dove.energy = dove.energy - baseReqInt;
                    }
                }
                break;
            }
                
        }

        int hnumber = 0;
        int dnumber = 0;
        int item = 0;
        foreach (GameObject hd in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (hd.name == "Hawk(Clone)")
            {
                hnumber++;
            }
            if (hd.name == "Dove(Clone)")
            {
                dnumber++;
            }
        }


        if (null == m_DataDiagram)
            return;

        foreach (GameObject line in lineList)
        {
            if (item==0)
            {
                m_DataDiagram.InputPoint(line, new Vector2(1,(float)(hnumber/10.0)));
            }
            if(item==1)
            {
                m_DataDiagram.InputPoint(line, new Vector2(1, (float)(dnumber/10.0)));
            }
            if(item==2)
            {
                m_DataDiagram.InputPoint(line, new Vector2(1, (float)((hnumber+dnumber)/10.0)));
            }
            item++;
        }
       
    }


    public void createoffspring(string agent, int energy)
    {
        if (agent == "Hawk")
        {
            Hawk hawk = spawnPrefab(hawkPrefab).GetComponent<Hawk>();

        }
        else if (agent == "Dove")
        {
            Dove dove = spawnPrefab(dovePrefab).GetComponent<Dove>();
        }

    }

}
