using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

namespace System {
    // フェード管理クラス
    public class FadeManager : MonoBehaviour
    {
        public static FadeManager Instance { get; private set; }
    
        // Fade用のImage
        private Image m_fadeImage;
    
        // 現在実行中のフェードコルーチンを保持する変数
        private Coroutine m_currentFadeCoroutine;
    
        enum FadeState
        {
            None,
            FadeIn,
            FadeOut
        }
    
        void Awake()
        {
            Debug.Log("[Class:FadeManager] FadeManager Awake");
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CreateFadeImage();
        }
    
        // フェード中かどうか
        private FadeState e_isFading = FadeState.None;
    
        // フェイド状態の取得
        public bool GetFadeState() { return e_isFading != FadeState.None; }
    
        // オブジェクト生成
        private void CreateFadeImage()
        {
            // キャンバスの生成
            GameObject canvasObj = new GameObject("FadeCanvas");
            canvasObj.transform.SetParent(transform);
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            // UIが絶対に使わないような大きな値にする
            // こいつは絶対に前に表示したいからやで
            // もし何かしらフェイドの表示がうまいこといかない場合は
            // 下の数値をいじってみてください
            // intの最大値は安全性的にやめてください
            canvas.sortingOrder = 32000;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
    
            // フェード用のImage生成
            GameObject fadeObj = new GameObject("FadeImage");
            fadeObj.transform.SetParent(canvasObj.transform);
            m_fadeImage = fadeObj.AddComponent<Image>();
            m_fadeImage.color = new Color(0, 0, 0, 0);
            m_fadeImage.raycastTarget = false;
            RectTransform rect = m_fadeImage.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
    
        // 割り込みをできるようにするため
        private void StartFadeCoroutine(IEnumerator a_coroutine)
        {
            // すでに何かが動いていたら止める（割り込み処理）
            if (m_currentFadeCoroutine != null)
            {
                StopCoroutine(m_currentFadeCoroutine);
            }
            m_currentFadeCoroutine = StartCoroutine(a_coroutine);
        }
    
        // フェードイン処理
        public static void FadeIn(float a_duration)
        {
            if (Instance == null) return;
            Debug.Log("[Class:FadeManager] FadeIn");
            //        Instance.StartCoroutine(Instance.FadeInCoroutine(duration));
            Instance.StartFadeCoroutine(Instance.FadeInCoroutine(a_duration));
        }
    
        // フェードアウト処理
        public static void FadeOut(float a_duration)
        {
            Debug.Log("[Class:FadeManager] FadeOut");
            if (Instance == null) return;
            //        Instance.StartCoroutine(Instance.FadeOutCoroutine(duration));
            Instance.StartFadeCoroutine(Instance.FadeOutCoroutine(a_duration));
        }

        /// <summary>
        /// フェードしてシーン遷移
        /// </summary>
        /// <param name="a_sceneName">切り替えたいシーン</param>
        /// <param name="a_fadeTime">フェード時間</param>
        public static void FadeChangeScene(string a_sceneName, float a_fadeTime)
        {
            if (Instance == null)
            {
                Debug.Log("[Class:FadeManager] Failed to get instance");
                return;
            }
            Debug.Log("[Class:FadeManager] Fade ChangeScene");
            //Instance.StartCoroutine(Instance.FadeChangeSceneCoroutine(sceneName, fadeTime));
            Instance.StartFadeCoroutine(Instance.FadeChangeSceneCoroutine(a_sceneName, a_fadeTime));
        }
    
        // フェードインのコルーチン
        private IEnumerator FadeInCoroutine(float a_duration)
        {
            e_isFading = FadeState.FadeIn;
            float elapsedTime = 0f;
            Color color = m_fadeImage.color;
            float startAlpha = color.a;
            while (elapsedTime < a_duration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(startAlpha, 0f, elapsedTime / a_duration);
                m_fadeImage.color = color;
                yield return null;
            }
            color.a = 0f;
            m_fadeImage.color = color;
            e_isFading = FadeState.None;
        }
    
        // フェードアウトのコルーチン
        private IEnumerator FadeOutCoroutine(float a_duration)
        {
            e_isFading = FadeState.FadeOut;
            float elapsedTime = 0f;
            Color color = m_fadeImage.color;
            float startAlpha = color.a;
    
            while (elapsedTime < a_duration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(startAlpha, 1f, elapsedTime / a_duration);
                m_fadeImage.color = color;
                yield return null;
            }
            color.a = 1f;
            m_fadeImage.color = color;
            e_isFading = FadeState.None;
        }
        // フェードしてシーン遷移のコルーチン
        private IEnumerator FadeChangeSceneCoroutine(string a_sceneName, float a_fadeTime)
        {
            yield return StartCoroutine(FadeOutCoroutine(a_fadeTime));
            SceneManager.LoadScene(a_sceneName);
            yield return StartCoroutine(FadeInCoroutine(a_fadeTime));
        }
    }
}