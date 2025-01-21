using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using TMPro;
using UnityEngine.SceneManagement;

public enum LogInType
{
    logIn,
    newName,
    PVP,
    Rank,
    Etc

}

public class Registaration
{
    private static Registaration instance = null;

    public static Registaration Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Registaration();
            }

            return instance;
        }
    }



    public void SignUp(string id, string pw, TextMeshProUGUI console)
    {
        // Step 2. 회원가입 구현하기 로직        

        Debug.Log("회원가입을 요청합니다.");

        var responceOfBackEnd = Backend.BMember.CustomSignUp(id, pw);

        if (responceOfBackEnd.IsSuccess())
        {
            console.text = $"회원가입에 성공했습니다. \nID : {id}님";

            Nickname(id, console, LogInType.Etc);
        }
        else
        {
            console.text = $"회원가입에 실패했습니다. : {responceOfBackEnd}";

        }
    }

    public void Login(string id, string pw, TextMeshProUGUI console, LogInType type, string text)
    {
        // Step 3. 로그인 구현하기 로직
        Debug.Log("로그인을 요청합니다.");

        var responceOfBackEnd = Backend.BMember.CustomLogin(id, pw);
        //Debug.Log(responceOfBackEnd);

        if (responceOfBackEnd.IsSuccess())
        {
            if (type == LogInType.logIn)
            {
                LoadingManager.name_Scene = "InGame";
                LoadingManager.loading = Loading.InGame;

                SceneManager.LoadScene(2);

            }
            else if (type == LogInType.newName)
            {

                // 아니라면 변경
                Registaration.Instance.Nickname(text, console, LogInType.newName);
                Debug.Log(responceOfBackEnd);
            }
            else if (type == LogInType.PVP)
            {
                MainMenuManager.Instance.PVPSetting();
            }
            else if (type == LogInType.Rank)
            {
                MainMenuManager.Instance.RankSetting();
            }



        }
        else
        {
            console.text = $"로그인이 실패했습니다. : {responceOfBackEnd}";


        }
    }

    public void Nickname(string nickname, TextMeshProUGUI console, LogInType type)
    {
        // Step 4. 닉네임 변경 구현하기 로직

        Debug.Log("닉네임 변경을 요청합니다.");

        var bro = Backend.BMember.UpdateNickname(nickname);

        if (type == LogInType.newName)
        {
            if (bro.IsSuccess())
            {
                console.text = "닉네임 변경 완료!";
            }
            else
            {
                console.text = ("닉네임 변경에 실패했습니다 : " + bro);
            }
        }
        else
        {
            if (bro.IsSuccess())
            {
                Debug.Log("닉네임 변경 완료!");
            }
            else
            {
                Debug.Log("닉네임 변경에 실패했습니다 : " + bro);
            }
        }


    }
}
