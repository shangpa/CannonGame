using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonPlayer : MonoBehaviour
{
    
    
    [Header("플레이어 스킬")]
    public GameObject skillPrefab;
    public Slider skillGauge;
    public enum PlayerState
    {   
        Playing, GameOver
    }
    [Header("플레이어 상태")]
    public PlayerState playerState = PlayerState.Playing;

    
    private float mouseX;
    private float mouseY;
    [Header("플레이어 회전")] 
    public float mouseSpeed;
    public Transform cannonBody;
    public Transform cannonGun;
    private Vector3 originBody;
    private Vector3 originGun;
    [Header("플레이어 사격")]
    public GameObject cannonBallPrefab;
    public GameObject shotPrefab;
    public float power;
    public Transform fireDir;
    private float fireRate = 0.51f;       // 발사 간격 (초 단위)
    private float nextFireTime = 0.5f;    // 다음 발사가 가능한 시간
    // Start is called before the first frame update
    void Start()
    {
        originGun = transform.rotation.eulerAngles;
        originBody = transform.rotation.eulerAngles;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
       
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerState)
        {
            case PlayerState.Playing:
                {

                    cannonRotate();
                    cannonFire();
                    skillOn();
                    break;
                }
        }
    }
    void cannonRotate()
    {
        float h = Input.GetAxis("Mouse X");//마우스 수평 입력
        float v = Input.GetAxis("Mouse Y");//마우스 수직 입력

        mouseX += h * mouseSpeed * Time.deltaTime;
        mouseY += v * mouseSpeed * Time.deltaTime;

        mouseY = Mathf.Clamp(mouseY, -50.0f, 50.0f);
        //메인카메라의 회전값을 마수으 입력에 따라 회전시킨다.
        cannonBody.eulerAngles = new Vector3(cannonBody.eulerAngles.x, mouseX - originBody.y, cannonBody.eulerAngles.z);
        cannonGun.eulerAngles = new Vector3(cannonGun.eulerAngles.x, cannonBody.eulerAngles.y, -mouseY);
    }
    void cannonFire()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            // 다음 발사가 가능한 시간 갱신
            nextFireTime = Time.time + fireRate;

            // shotPrefab을 생성하고 fireDir 위치에 약간의 오프셋을 추가하여 위치 조정
            Vector3 offset = fireDir.forward * 1.0f;  // fireDir의 전방으로 1.0f만큼 떨어진 위치로 설정
            GameObject shot = Instantiate(shotPrefab, fireDir.position + offset, fireDir.rotation);

            // shotPrefab이 fireDir의 자식이 되도록 설정
            shot.transform.SetParent(fireDir);  // fireDir 위치에 고정

            // 생성한 포탄을 발사
            GameObject ball = Instantiate(cannonBallPrefab, fireDir.position, fireDir.rotation);
            ball.GetComponent<Rigidbody>().AddForce(fireDir.forward * power, ForceMode.Impulse);
            ball.GetComponent<Rigidbody>().AddTorque(fireDir.forward * power * 30.0f, ForceMode.Impulse);
        }
    }


    public void SkillGaugeUp(float val)
    {
        skillGauge.value += val;
    }
    void skillOn()
    {
        if(skillGauge.value >= 1.0f)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    GameObject skill = Instantiate(skillPrefab, enemy.transform.position, Quaternion.identity);
                    Destroy(skill,1.0f);
                    enemy.GetComponent<NewBehaviourScript>().EnemyDamageOn();
                      
                }
                skillGauge.value = 0;
            }
        }
    }
    public void PlayerGameOver()
    {
        playerState = PlayerState.GameOver;
    }
}
