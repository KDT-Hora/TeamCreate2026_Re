using UnityEngine;

public class CharaAnim : MonoBehaviour
{
    Animator m_anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("攻撃入力");
            PlayAttack();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("攻撃入力");
            PlayAttack();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("ダメージ入力");
            PlayDamage();
        }
    }
    // 攻撃のアニメーションをするための処理
    public void PlayAttack()
    {
        //m_anim.SetBool("IsAttack", true);
        m_anim.CrossFade("Attack", 0.1f, 0);
        m_anim.Update(0);
    }
    public float GetAttackAnimLength()
    {
        AnimatorStateInfo animState = m_anim.GetCurrentAnimatorStateInfo(0);
        if (animState.IsName("Attack"))
        {
            return animState.length;
        }
        return 0f;
    }


    // ダメージを受けた時のアニメーションをするための処理
    public void PlayDamage()
    {

        m_anim.CrossFade("Damage",0.1f,0);
        m_anim.Update(0);
    }
}
