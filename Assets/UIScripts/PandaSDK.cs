using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PandaSDK : MonoBehaviour
{
    const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
    // Start is called before the first frame update
    void Start()
    {
        if(Application.internetReachability != 0)
        {
            int playerSet = PlayerPrefs.GetInt("PlayerSaved");

            if (playerSet == 0)
            {
                StartCoroutine(Upload());
            }
        }
        else
        {
            Debug.Log("No internet connection");
        }
    }


    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        string GameCode = Application.identifier;
        form.AddField("GameID", GameCode);

        UnityWebRequest www = UnityWebRequest.Post("https://www.lostpandagames.com/pandasdk/save.php", form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            PlayerPrefs.SetInt("PlayerSaved", 1);
        }
    }

    public static string GenerateRandomString(int length)
    {
        string myString = "";

        for (int i = 0; i < length; i++)
        {
            myString += glyphs[Random.Range(0, glyphs.Length)];
        }

        return myString;
    }

    public static int GenerateRandomUserId()
    {
        int id = Random.Range(0, 9999999);
        return id;
    }

}
