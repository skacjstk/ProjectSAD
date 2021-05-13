using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour
{
    const string URL = "https://script.google.com/macros/s/AKfycby_yLHFYSr7dFn4POjihqO1z1QOxtxCp25IzsdAsfY-Moj5tekQ/exec";
    public InputField IDInput, PassInput;
    string id, pass;

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
    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone) print(www.downloadHandler.text);
            else print("웹의 응답이 없습니다.");
        }
    }
}