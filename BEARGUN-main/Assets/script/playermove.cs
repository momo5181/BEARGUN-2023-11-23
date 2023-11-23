using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermove : MonoBehaviour
{
    Vector3 velocity;
    public float speed =5f;//移動速度
    public CharacterController controller;//定義角色身體
    public float gravity =-19.62f;//添加重力原為9.81但unity太慢
    public float jumphight = 2f;//設定跳高高度
    public Transform groundCheck;//設定檢測物體
    public float groundDistance =0.4f;//在物體底部添加的虛擬半徑0.4球體用於檢測
    public LayerMask groundMask;//設定可被偵測的地面
    bool isgrounded;
    public float runSpeed = 1;//設定奔跑
    public KeyCode runningKey = KeyCode.LeftShift;//為了不跟輸入法衝突
    public Animator animatorM4A1;//設置動畫控制器
    public Animator animatorMP5;
    public Animator animatorSniper;


    // Update is called once per frame
    void Update()
    {
        isgrounded = Physics.CheckSphere(groundCheck.position,groundDistance,groundMask);//設定虛擬物體為球體並檢查是否觸碰地面
        if(isgrounded && velocity.y<0)
        {
            velocity.y =-2f;//地面重力
        }
    
        if(Input.GetKey(runningKey)&& Input.GetButton("Fire1")==false&&Input.GetButton("Fire2")==false&& Input.GetKey("w"))//當按下shift且不在射擊模式
        {
            animatorM4A1.SetBool("run",true);//播放動畫
            animatorMP5.SetBool("run",true);
            animatorSniper.SetBool("run",true);
            runSpeed=2f;//奔跑開始
        }
        else
        {
            animatorM4A1.SetBool("run",false);//結束動畫
            animatorMP5.SetBool("run",false);
            animatorSniper.SetBool("run",false);
            runSpeed =1;
        }
        float x= Input.GetAxis("Horizontal");//取得角色左右移動餐數
        float z= Input.GetAxis("Vertical");//取得角色前後移動參數 
        animatorM4A1.SetFloat("movement",Mathf.Abs(x) + Mathf.Abs(z));//播放動畫
        animatorMP5.SetFloat("movement",Mathf.Abs(x) + Mathf.Abs(z));
        animatorSniper.SetFloat("movement",Mathf.Abs(x) + Mathf.Abs(z));

        Vector3 move = transform.right*x+transform.forward*z;//角色移動變數
        controller.Move(move*speed*Time.deltaTime*runSpeed);//角色移動添加到身體上
       velocity.y+=gravity*Time.deltaTime;
       controller.Move(velocity*Time.deltaTime);//重力添加到物體上
        
        if(Input.GetButtonDown("Jump") && isgrounded)//輸入空白鍵且不在懸空狀態
        {
            velocity.y=Mathf.Sqrt(jumphight*-2f*gravity);//跳高的公式:v=√2gh
        }
    }
}
