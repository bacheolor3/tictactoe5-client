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
            // 성공(200) 또는 실패(401) 모두 바디에 {result: ...}가 올 수 있음
            SigninResult resp = default;
            try { resp = JsonUtility.FromJson<SigninResult>(text); } catch { }

            if (!protoErr) // 200
            {
                if (resp.result == 2) success?.Invoke();
                else failure?.Invoke(resp.result); // 방어적
            }
            else           // 401 등
            {
                // 서버가 401에서도 {result:0|1}을 보내줌
                if (resp.result == 0 || resp.result == 1)
                    failure?.Invoke(resp.result);
                else
                    failure?.Invoke(-4); // 기타
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

            // 회원가입은 HTTP상태코드로 판정
            long code = www.responseCode;            

            if (!protoErr)
            {
                if(code == 201) { success?.Invoke(); }  // 가입 성공
                else            { failure?.Invoke(-4); } // 예외적 성공 코드(거의 안옴)
                yield break;
            }
            // 에러 코드 매핑(서버 스펙에 맞춤)
            if (code == 400) failure?.Invoke(-2);       // 빈란있음
            else if (code == 409) failure?.Invoke(3);  // 이미 존재하는 이름
            else if (code == 500) failure?.Invoke(-3); // 서버 에러
            else                 failure?.Invoke(4); // 기타
        }
    }

    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        
    }
}
