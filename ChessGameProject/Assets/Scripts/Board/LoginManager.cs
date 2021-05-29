using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SheetData
{
    public string order, result, msg, value;
}

public class LoginManager : MonoBehaviour
{
    const string URL = "https://script.google.com/macros/s/AKfycby_yLHFYSr7dFn4POjihqO1z1QOxtxCp25IzsdAsfY-Moj5tekQ/exec";
    public InputField IDInput, PassInput;
    string id, pass;
    public SheetData SD;

    public GameObject LoginPanel;
    public GameObject MenuPanel;

    bool SetIDPass()
    {
        id = IDInput.text.Trim();
        pass = PassInput.text.Trim();

        if (id == "" || pass == "") return false;
        else return true;
    }
    public void Register()
    {
        if (!SetIDPass())
        {
            print("아이디 또는 비밀번호가 비어있습니다.");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "register");
        form.AddField("id", id);
        form.AddField("pass", pass);

        StartCoroutine(Post(form));
    }

    public void Login()
    {
        if (!SetIDPass())
        {
            print("아이디 또는 비밀번호가 비어있습니다.");
            return;
        }
        WWWForm form = new WWWForm();
        form.AddField("order", "login");
        form.AddField("id", id);
        form.AddField("pass", pass);

        StartCoroutine(Post(form));

    }

    public void OnApplicationQuit()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "logout");

        StartCoroutine(Post(form));
    }


    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone) Response(www.downloadHandler.text);
            else print("웹의 응답이 없습니다.");
        }
    }
    void Response(string json)
    {
        if (string.IsNullOrEmpty(json)) return;
        SD = JsonUtility.FromJson<SheetData>(json);

        if (SD.result == "ERROR")
        {
            print(SD.order + "을 실행할 수 없습니다. 에러 메시지 : " + SD.msg);
            return;
        }

        print(SD.order + "을 실행했습니다. 메시지 : " + SD.msg);

        if (SD.order == "login")
        {
            if (SD.value == "success")
            {
                LoginPanel.SetActive(false);
                MenuPanel.SetActive(true);
            }
        }

        if (SD.order == "logout")
        {
            if (SD.value == "success")
            {
                SceneManager.LoadScene("Login");
            }
        }


    }
}
