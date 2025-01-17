using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CannonPlayer;

public class NewBehaviourScript : MonoBehaviour
{
    private CannonPlayer cannonPlayer;  // CannonPlayer 스크립트를 참조할 변수
    public float speed;
    private GameObject player;
    public Animator anim;
    public float playerDistance;

    public enum EnemyState
    {
        live, dead
    }
    public EnemyState enemyState = EnemyState.live;

    // Start is called before the first frame update
    void Start()
    {
        // 플레이어의 CannonPlayer 스크립트를 찾고 참조 저장
        cannonPlayer = Object.FindFirstObjectByType<CannonPlayer>();

        if (cannonPlayer == null)
        {
            Debug.LogError("CannonPlayer 스크립트를 찾을 수 없습니다. 게임 오브젝트가 올바르게 설정되었는지 확인해주세요.");
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // cannonPlayer가 null이 아니고 playerState가 Playing일 때만 실행
        if (cannonPlayer != null && cannonPlayer.playerState == CannonPlayer.PlayerState.Playing)
        {
            switch (enemyState)
            {
                case EnemyState.live:
                    {
                        playerDistance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.transform.position.x, 0, player.transform.position.z));

                        if (playerDistance <= 20)
                        {
                            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GameOver();
                        }
                        transform.position = Vector3.MoveTowards(transform.position,
                            new Vector3(player.transform.position.x,
                            transform.position.y, player.transform.position.z),
                            speed * Time.deltaTime);

                        Vector3 relation = new Vector3(player.transform.position.x, 0, player.transform.position.z)
                            - new Vector3(transform.position.x, 0, transform.position.z);
                        Quaternion rotation = Quaternion.LookRotation(relation);
                        transform.rotation = rotation;
                        break;
                    }
            }
        }
    }

    public void EnemyDamageOn()
    {
        GameObject.FindGameObjectWithTag("GameManager").SendMessage("ScoreUp", 100);
        GetComponent<Collider>().enabled = false;
        anim.SetInteger("EnemyState", 1);
        enemyState = EnemyState.dead;
        Destroy(gameObject);
    }
}
