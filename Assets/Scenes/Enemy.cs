using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[AddComponentMenu("MyGame/Enemy")]
public class Enemy : MonoBehaviour
{
    private new Transform transform;
    private Animator animator; // 动画组件
    private Player player; // 主角
    private NavMeshAgent agent; // 寻路组件
    private float moveSpeed = 1.5f; // 角色移动速度
    private float rotateSpeed = 30; // 角色旋转速度
    private float timer = 2; // 计时器
    private int life = 10; // 生命值

    void Awake() { 
}
    /// <summary>
    /// 初始化
    /// </summary>
    void Start()
    {
        transform = GetComponent<Transform>();
        // 获取动画组件
        animator = GetComponent<Animator>();
        // 获取主角
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();  //
        // 获取寻路组件
        
            agent = GetComponent<NavMeshAgent>();
        
        // 指定寻路器的行走速度
        agent.speed = moveSpeed;
    }
    /// <summary>
        /// 转向目标方向
        /// </summary>
    private void RotateTo()
    {
        // 获取目标方向
        Vector3 targetDirection = player.transform.position - transform.position;
        // 计算旋转角度
        Vector3 newDirection = Vector3.RotateTowards(transform.forward,
 targetDirection, rotateSpeed * Time.deltaTime, 0);
        // 旋转至新方向
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
       /// <summary>
    /// 更新敌人行为
    /// </summary>
    void Update()
    {
        // 如果主角生命为0，则什么也不做
        if (player.life <= 0)
        {
            return;
        }     // 获取当前动画状态
               AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);       // 待机状态

 if (info.fullPathHash == Animator.StringToHash("Base Layer.idle")
 && !animator.IsInTransition(0))
        {
            animator.SetBool("idle", false);
            // 待机一定时间
            timer -= Time.deltaTime;
            if (timer > 0)
            {
                return;
            }
            // 如果距离主角1.5米以内，则进入攻击状态
            if (Vector3.Distance(transform.position, player.transform.position) < 1.5f)
            {
                animator.SetBool("attack", true);
            }
            else
            {
                // 重置定时器
                timer = 1;
                // 恢复寻路
                agent.Resume();
                // 设置寻路目标
                agent.SetDestination(player.transform.position);
                // 面向主角
                RotateTo();
                Debug.Log(agent.destination);
                // 进入行走状态
                animator.SetBool("run", true);
            }
        }    // 行走状态

 if (info.fullPathHash == Animator.StringToHash("Base Layer.run")
 && !animator.IsInTransition(0))
        {
            animator.SetBool("run", false);
            // 每隔1秒重新定位主角的位置
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 1;
                // 面向主角
                RotateTo();
                agent.SetDestination(player.transform.position);
            }
            // 如果距离主角1.5米以内，则进攻主角
            if (Vector3.Distance(transform.position, player.transform.position) < 1.5f)
            {
                // 停止寻路
                agent.Stop();
                // 进入攻击状态
                animator.SetBool("attack", true);
            }
        }      // 攻击状态

 if (info.fullPathHash == Animator.StringToHash("Base Layer.attack")
 && !animator.IsInTransition(0))
        {
            animator.SetBool("attack", false);
            // 面向主角
            RotateTo();
            // 如果动画播放完，重新进入待机状态
            if (info.normalizedTime >= 1)
            {
                animator.SetBool("idle", true);
                // 重置计时器
                timer = 2;
            }
        }
    }


}
