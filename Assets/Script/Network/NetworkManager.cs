using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkManager : Singleton<NetworkManager>
{
    public IEnumerator Signin(SigninData signinData, Action success, Action<int> failure)
    {
        string jsonString = JsonUtility.ToJson(signinData);
        byte[] byteRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);

        using (UnityWebRequest www = new UnityWebRequest(Constants.ServerURL + "/users/signin", UnityWebRequest.kHttpVerbPOST))
        {
            www.uploadHandler = new UploadHandlerRaw(byteRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

#if UNITY_2020_3_OR_NEWER
            bool netErr = www.result == UnityWebRequest.Result.ConnectionError;
            bool protoErr = www.result == UnityWebRequest.Result.ProtocolError;
#else
        bool netErr  = www.isNetworkError;
        bool protoErr= www.isHttpError;
#endif
            if (netErr)
            {
                failure?.Invoke(-1);
                yield break;
            }

            var text = www.downloadHandler.text;
            // ����(200) �Ǵ� ����(401) ��� �ٵ� {result: ...}�� �� �� ����
            SigninResult resp = default;
            try { resp = JsonUtility.FromJson<SigninResult>(text); } catch { }

            if (!protoErr) // 200
            {
                if (resp.result == 2) success?.Invoke();
                else failure?.Invoke(resp.result); // �����
            }
            else           // 401 ��
            {
                // ������ 401������ {result:0|1}�� ������
                if (resp.result == 0 || resp.result == 1)
                    failure?.Invoke(resp.result);
                else
                    failure?.Invoke(-4); // ��Ÿ
            }

            //if (www.result == UnityWebRequest.Result.ConnectionError)
            //{

            //}
            //else
            //{
            //    var resultString = www.downloadHandler.text;
            //    var result = JsonUtility.FromJson<SigninResult>(resultString);

            //    if (result.result == 2)
            //    {
            //        var cookie = www.GetResponseHeader("set-cookie");
            //        if(!string.IsNullOrEmpty(cookie))
            //        {
            //          int lastIndex = cookie.LastIndexOf(';');
            //          string sid = cookie.Substring(0, lastIndex);
            //          PlayerPrefs.SetString("sid", sid);
            //        }
            //        success?.Invoke();
            //    }
            //    else
            //    {
            //        failure?.Invoke(result.result);
            //    }
            //}

        } ;
    }

    public IEnumerator Signup(SignupData signupData, Action success, Action<int> failure)
    {
        if(string.IsNullOrWhiteSpace(signupData.username) ||
            string.IsNullOrWhiteSpace(signupData.password) ||
            string.IsNullOrWhiteSpace(signupData.nickname))
        {
            failure?.Invoke(-2);
            yield break;
        }

        string jsonString = JsonUtility.ToJson(signupData);
        byte[] byteRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);

        using (UnityWebRequest www = new UnityWebRequest(Constants.ServerURL + "/users/signup", UnityWebRequest.kHttpVerbPOST))
        {
            www.uploadHandler = new UploadHandlerRaw(byteRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

#if UNITY_2020_3_OR_NEWER
            bool netErr = www.result == UnityWebRequest.Result.ConnectionError;
            bool protoErr = www.result == UnityWebRequest.Result.ProtocolError;
#else
            bool netEErr = www.isNetworkError;
            bool protoErr = www.isHttpError;
#endif
            if(netErr)
            {
                failure?.Invoke(-1);
                yield break;
            }

            // ȸ�������� HTTP�����ڵ�� ����
            long code = www.responseCode;            

            if (!protoErr)
            {
                if(code == 201) { success?.Invoke(); }  // ���� ����
                else            { failure?.Invoke(-4); } // ������ ���� �ڵ�(���� �ȿ�)
                yield break;
            }
            // ���� �ڵ� ����(���� ���忡 ����)
            if (code == 400) failure?.Invoke(-2);       // �������
            else if (code == 409) failure?.Invoke(3);  // �̹� �����ϴ� �̸�
            else if (code == 500) failure?.Invoke(-3); // ���� ����
            else                 failure?.Invoke(4); // ��Ÿ
        }
    }

    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        
    }
}
