using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("MyGame/Player")] // 这一句的作用是把该脚本放到【Component】→【MyGame】下的【Player】，便于自行管理脚本
public class Player : MonoBehaviour // 需要由Unity控制生命周期的脚本都要继承MonoBehaviour类，并且要挂在一个游戏体上
{
    public int life = 10;

    private new Transform transform; // 此处使用new关键字隐藏父类的同名成员
    private CharacterController controller;
    private float speed = 3.0f;
    private float gravity = 2.0f;

    private Transform cameraTransform; // 摄像机的Transform组件
    private Vector3 cameraRotation; // 摄像机旋转角度
    private float cameraHeight = 1.7f; // 摄像机高度（即主角的眼睛高度）


    /// <summary>
    /// 初始化,获取Transform和Character Controller组件
    /// </summary>
    void Start()
    {
        transform = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
        // 获取摄像机
        cameraTransform = Camera.main.GetComponent<Transform>();
        // 设置摄像机初始位置
        Vector3 position = transform.position;
        position.y += cameraHeight;
        cameraTransform.position = position;
        // 设置摄像机的初始旋转方向
        cameraTransform.rotation = transform.rotation;
        cameraRotation = cameraTransform.eulerAngles;
        // 锁定鼠标
        Cursor.lockState = CursorLockMode.Locked;
    }


    /// <summary>
    /// 控制主角行动，如果生命为0则什么也不做
    /// </summary>
    void Update()
    {
        if (life <= 0)
        {
            return;
        }
        Control();
    }

    /// <summary>
    /// 在Unity编辑器中为主角显示一个图标
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(GetComponent<Transform>().position, "Spawn.tif");
    }

    /// <summary>
    /// 控制主角的重力运动和前后左右移动
    /// </summary>
    private void Control()
    {

        float x = 0, y = 0, z = 0;
        // 重力运动
        y -= gravity * Time.deltaTime;
        // 前后移动
        if (Input.GetKey(KeyCode.W))
        {
            z += speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            z -= speed * Time.deltaTime;
        }
        // 左右移动
        if (Input.GetKey(KeyCode.A))
        {
            x -= speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            x += speed * Time.deltaTime;
        }

        // 使用Character Controller而不是Transform提供的Move方法
        // 因为Character Controller提供的Move方法会自动进行碰撞检测
        controller.Move(transform.TransformDirection(new Vector3(x, y, z)));

      // 获取鼠标移动距离
        float rh = Input.GetAxis("Mouse X");
        float rv = Input.GetAxis("Mouse Y");
        // 旋转摄像机
        cameraRotation.x -= rv;
        cameraRotation.y += rh;
        cameraTransform.eulerAngles = cameraRotation;
        // 使主角的面向方向与摄像机一致
        Vector3 rotation = cameraTransform.eulerAngles;
        rotation.x = 0;
        rotation.z = 0;
        transform.eulerAngles = rotation;

        // 控制主角运动

        // 使摄像机的位置与主角一致
        Vector3 position = transform.position;
        position.y += cameraHeight;
        cameraTransform.position = position;
    }



}

