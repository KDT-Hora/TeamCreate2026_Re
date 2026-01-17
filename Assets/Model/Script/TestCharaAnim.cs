using UnityEditor.Animations;
using UnityEngine;


public class TestCharaAnim : MonoBehaviour
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
        if(Input.GetKeyDown(KeyCode.Space))
        {

            Debug.Log("çUåÇì¸óÕ");
            PlayAttack();
        }
    }

    public void PlayAttack()
    {
        //m_anim.SetBool("IsAttack", true);
        m_anim.CrossFade("Attack",0.1f, 0);
        m_anim.Update(0);
    }
}// class
