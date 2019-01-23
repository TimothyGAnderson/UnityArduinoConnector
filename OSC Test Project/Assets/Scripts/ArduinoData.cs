using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoData : MonoBehaviour
{

    public List<string> incoming = new List<string>();
    public GameObject tree;

    public List<DataNode> Data;

    [System.Serializable]
    public class DataNode
    {
        public string name;
        
        public List<string> data = new List<string>();

        public List<float> ConvertToFloat()
        {
            List<float> list = new List<float>();

            foreach(string s in data)
            {
                float f = 0;
                if (float.TryParse(s, out f))
                {
                    list.Add(f);
                }
                else
                    Debug.LogError("Unable to parse value in " + name);
            }

            return list;
        }

        public List<int> ConvertToInt()
        {
            List<int> list = new List<int>();

            foreach (string s in data)
            {
                int f = 0;
                if (int.TryParse(s, out f))
                {
                    list.Add(f);
                }
                else
                    Debug.LogError("Unable to parse value in " + name);
            }

            return list;
        }

        public List<Vector3> ConvertToVector3()
        {
            List<Vector3> list = new List<Vector3>();

            foreach (string s in data)
            {
                Vector3 f = Vector3.zero;
                string[] ss = s.Split(',');
                if (float.TryParse(ss[0], out f.x))
                    if (float.TryParse(ss[1], out f.y))
                        if (float.TryParse(ss[2], out f.z))
                            list.Add(f);
                        
            }

            return list;
        }
    }

    private void Update()
    {
        foreach (string s in incoming)
        {

            foreach (DataNode d in Data)
            {
                if (d.name == s.Split(':')[0])
                {
                    d.data.Add(s.Split(':')[1].Replace(" ", ""));
                }
            }
        }

        foreach(string s in Data[3].data)
        {
            string[] ss = s.Split(',');
            Vector3 pos = new Vector3(int.Parse(ss[0]), 0, int.Parse(ss[2]));
            pos.x -= 500;
            pos.z -= 500;

            pos *= 0.5f;
            GameObject g = Instantiate(tree, pos, Quaternion.Euler(0, 0, 0));
            g.name = "Tree" + Time.time;
        }
        Data[3].data.Clear();
        incoming.Clear();
    }


}
