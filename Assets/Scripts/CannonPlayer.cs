using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonPlayer : MonoBehaviour
{
    
    
    [Header("�÷��̾� ��ų")]
    public GameObject skillPrefab;
    public Slider skillGauge;
    public enum PlayerState
    {   
        Playing, GameOver
    }
    [Header("�÷��̾� ����")]
    public PlayerState playerState = PlayerState.Playing;

    
    private float mouseX;
    private float mouseY;
    [Header("�÷��̾� ȸ��")] 
    public float mouseSpeed;
    public Transform cannonBody;
    public Transform cannonGun;
    private Vector3 originBody;
    private Vector3 originGun;
    [Header("�÷��̾� ���")]
    public GameObject cannonBallPrefab;
    public GameObject shotPrefab;
    public float power;
    public Transform fireDir;
    private float fireRate = 0.51f;       // �߻� ���� (�� ����)
    private float nextFireTime = 0.5f;    // ���� �߻簡 ������ �ð�
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
        float h = Input.GetAxis("Mouse X");//���콺 ���� �Է�
        float v = Input.GetAxis("Mouse Y");//���콺 ���� �Է�

        mouseX += h * mouseSpeed * Time.deltaTime;
        mouseY += v * mouseSpeed * Time.deltaTime;

        mouseY = Mathf.Clamp(mouseY, -50.0f, 50.0f);
        //����ī�޶��� ȸ������ ������ �Է¿� ���� ȸ����Ų��.
        cannonBody.eulerAngles = new Vector3(cannonBody.eulerAngles.x, mouseX - originBody.y, cannonBody.eulerAngles.z);
        cannonGun.eulerAngles = new Vector3(cannonGun.eulerAngles.x, cannonBody.eulerAngles.y, -mouseY);
    }
    void cannonFire()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            // ���� �߻簡 ������ �ð� ����
            nextFireTime = Time.time + fireRate;

            // shotPrefab�� �����ϰ� fireDir ��ġ�� �ణ�� �������� �߰��Ͽ� ��ġ ����
            Vector3 offset = fireDir.forward * 1.0f;  // fireDir�� �������� 1.0f��ŭ ������ ��ġ�� ����
            GameObject shot = Instantiate(shotPrefab, fireDir.position + offset, fireDir.rotation);

            // shotPrefab�� fireDir�� �ڽ��� �ǵ��� ����
            shot.transform.SetParent(fireDir);  // fireDir ��ġ�� ����

            // ������ ��ź�� �߻�
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
