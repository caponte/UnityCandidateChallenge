using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class TableController : MonoBehaviour
{
    //Constants
    public const string JSON_FILE_PATH = "/StreamingAssets/JsonChallenge.json";

    //Prefabs
    public GameObject item;
    public GameObject col;
    public GameObject row;

    //Containers
    public GameObject title;
    public GameObject rowContainer;
    public GameObject colContainer;

    //Util    
    private string jsonString;
    private JsonData jsonData;

    // Start is called before the first frame update
    void Start()
    {

        jsonString = File.ReadAllText(Application.dataPath + JSON_FILE_PATH);
        jsonData = StringToJsoData(jsonString);
        if (jsonData != null)
        {
            title.GetComponent<Text>().text = (string)jsonData["Title"];
            GetHeadersColumns(col,jsonData);
            GetItemContent(item, row, jsonData);
        }
        else
        {
            title.GetComponent<Text>().text = "The file is not a valid JSON format.";
        }

    }

    //Get the string from file and create JsonData, returning null if the parse fail.
    JsonData StringToJsoData(string jsonString)
    {

        JsonData jsonData;
        try
        {
            jsonData = JsonMapper.ToObject(jsonString);
        }
        catch
        {
            jsonData = null;
        }

        return jsonData;
    }


    //Get the column headers from JsonData
    void GetHeadersColumns(GameObject col,JsonData jsonData)
    {
        GameObject newCol;

        foreach(JsonData colValue in jsonData["ColumnHeaders"])
        {
            newCol = Instantiate(col, transform.position, Quaternion.identity);
            newCol.transform.parent = colContainer.transform;
            newCol.transform.localScale = new Vector3(1, 1, 1);
            newCol.GetComponent<Text>().text = (string) colValue;
        }
    }

    //Get the item from JsonData
    void GetItemContent(GameObject item, GameObject row, JsonData jsonData)
    {
        GameObject newItem, newRow;

        foreach(JsonData itemValue in jsonData["Data"])
        {
            newItem = Instantiate(item, item.transform.position, Quaternion.identity);
            newItem.transform.parent = rowContainer.transform;
            newItem.transform.localScale = new Vector3(1, 1, newItem.transform.localScale.z);
            
            for(int i = 0; i < itemValue.Count; i++)
            {
                newRow = Instantiate(row, transform.position, Quaternion.identity);
                newRow.transform.parent = newItem.transform;
                newRow.transform.localScale = new Vector3(1, 1, 1);
                newRow.GetComponent<Text>().text = (string) itemValue[i];
            }
        }
    }

}
