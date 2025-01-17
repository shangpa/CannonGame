using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CannonPlayer;

public class NewBehaviourScript : MonoBehaviour
{
    private CannonPlayer cannonPlayer;  // CannonPlayer ��ũ��Ʈ�� ������ ����
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
        // �÷��̾��� CannonPlayer ��ũ��Ʈ�� ã�� ���� ����
        cannonPlayer = Object.FindFirstObjectByType<CannonPlayer>();

        if (cannonPlayer == null)
        {
            Debug.LogError("CannonPlayer ��ũ��Ʈ�� ã�� �� �����ϴ�. ���� ������Ʈ�� �ùٸ��� �����Ǿ����� Ȯ�����ּ���.");
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // cannonPlayer�� null�� �ƴϰ� playerState�� Playing�� ���� ����
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
