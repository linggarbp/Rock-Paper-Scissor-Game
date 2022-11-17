using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class BotDifficultyManager : MonoBehaviour
{
    [SerializeField] Bot bot;
    [SerializeField] int selectedDiffulty;
    [SerializeField] BotStats[] botDifficulties;

    [Header("Remote Config Parameters: ")]
    [SerializeField] bool enableRemoteConfig = false;
    [SerializeField] string difficultyKey = "Difficulty";

    struct userAttributes { };
    struct appAttributes { };

    IEnumerator Start()
    {
        //tunggu bot selesai set up
        yield return new WaitUntil(() => bot.IsReady);

        //set stats default dari difficulty manager sesuai selectedDifficulty dari inspector
        var newStats = botDifficulties[selectedDiffulty];
        bot.SetStats(newStats,true);

        //ambil difficulty dari remote config kalau enabled
        if (enableRemoteConfig == false)
            yield break;

        //tapi tunggu dulu sampe unity services siap
        yield return new WaitUntil(
            ()=>
            UnityServices.State == ServicesInitializationState.Initialized
            &&
            AuthenticationService.Instance.IsSignedIn
            );

        //daftar dulu untuk event fetch completed
        RemoteConfigService.Instance.FetchCompleted += OnRemoteConfigFetched;

        //lalu fetch disini, cukup sekali di awal permainan
        RemoteConfigService.Instance.FetchConfigsAsync(
            new userAttributes(), new appAttributes());
    }

    private void OnDestroy()
    {
        //unregister event untuk menghindari memory leak
        RemoteConfigService.Instance.FetchCompleted -= OnRemoteConfigFetched;
    }

    //setiap kali data baru didapatkan (melalui getch) fungsi ini akan dipanggil
    private void OnRemoteConfigFetched(ConfigResponse response)
    {
        if (RemoteConfigService.Instance.appConfig.HasKey(difficultyKey) == false)
        {
            Debug.LogWarning($"Difficulty Key: {difficultyKey} not found on remote config server");
            return;
        }

        switch (response.requestOrigin)
        {
            case ConfigOrigin.Default:
            case ConfigOrigin.Cached:
                break;
            case ConfigOrigin.Remote:
                selectedDiffulty = RemoteConfigService.Instance.appConfig.GetInt(difficultyKey);
                selectedDiffulty = Mathf.Clamp(selectedDiffulty, 0, botDifficulties.Length - 1);
                var newStats = botDifficulties[selectedDiffulty];
                bot.SetStats(newStats, true);
                break;
        }
    }
}
