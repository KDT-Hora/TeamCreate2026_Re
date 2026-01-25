using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("音量設定")]
    [Range(0, 1)] public float bgmVolume = 0.2f;
    [Range(0, 1)] public float seVolume = 1.0f;

    // 音を鳴らすコンポーネント
    private AudioSource bgmSource;
    private AudioSource seSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // AudioSource自動追加
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;

        // SE用
        seSource = gameObject.AddComponent<AudioSource>();
        seSource.loop = false;
        seSource.playOnAwake = false;
    }

    private void Update()
    {
        bgmSource.volume = bgmVolume;
        seSource.volume = seVolume;
    }

    //  BGM 処理
    /// <summary>
    /// BGM再生
    /// </summary>
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;

        // 今流れている曲と同じなら、再・生・し・な・い（これが重要！）
        if (bgmSource.clip == clip && bgmSource.isPlaying)
        {
            return;
        }

        bgmSource.clip = clip;
        bgmSource.Play();
    }

    /// <summary>
    /// Resources/BGM
    /// </summary>
    public void PlayBGM(string bgmName)
    {
        // "BGM/" + ファイル名 で読み込む
        // フォルダ構成が Resources/BGM ならこれで正解です
        AudioClip clip = Resources.Load<AudioClip>("BGM/" + bgmName);

        if (clip != null)
        {
            PlayBGM(clip); // さっきのメソッドにバトンタッチ
        }
        else
        {
            Debug.LogError($"BGMが見つかりません: Resources/BGM/{bgmName}");
        }
    }

    /// <summary>
    /// BGM停止
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
        bgmSource.clip = null;
    }

    //  SE 処理
    /// <summary>
    /// SE再生
    /// </summary>
    public void PlaySE(AudioClip clip)
    {
        if (clip == null) return;

        // PlayOneShotは「重ねがけ」ができる再生方法
        seSource.PlayOneShot(clip, seVolume);
    }

    /// <summary>
    /// Resources/SE フォルダの中にあるファイル名でSEを再生
    /// </summary>
    public void PlaySE(string seName)
    {
        // "SE/" + ファイル名 で読み込む
        AudioClip clip = Resources.Load<AudioClip>("SE/" + seName);

        if (clip != null)
        {
            PlaySE(clip); // さっきのメソッドにバトンタッチ
        }
        else
        {
            Debug.LogError($"SEが見つかりません: Resources/SE/{seName}");
        }
    }
}