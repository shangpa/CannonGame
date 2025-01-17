using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{

    public GameObject explosionPrefab;
    public enum ShotState
    {
        None,Shot
    }
    public ShotState shotState= ShotState.None;
    public float range;//감지 반경

    private void OnDrawGizmos()
    {
        //와이어 스피어 기즈모를 그린다(위치,반경)
        Gizmos.DrawWireSphere(transform.position, range);
    }
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        switch (shotState)
        {
            case ShotState.None:
                {
                    int layerMask = 1 << 6;//6번레이어를 int형 점수에 담음
                                           //스피어캐스트를 사용한다.(위치,반경,방향,거리,레이어마스크)
                    RaycastHit[] hits = Physics.SphereCastAll(transform.position, range, Vector3.up, 0, layerMask);

                    if (hits.Length > 0)
                    {
                        foreach (RaycastHit hit in hits)
                        {
                            Vector3 explosionPosition = gameObject.transform.position;
                            GameObject explosion = Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);

                            // 여기가 바뀐 부분: explosionPrefab에 ParticleSystem이 있을 경우,
                            // 파티클이 끝난 후 자동으로 해당 폭발 오브젝트를 삭제하는 코드
                            ParticleSystem explosionParticles = explosion.GetComponent<ParticleSystem>();
                            if (explosionParticles != null)
                            {
                                // 파티클이 끝난 후 폭발 오브젝트를 삭제
                                Destroy(explosion, explosionParticles.main.duration);
                            }
                            hit.transform.SendMessage("EnemyDamageOn");
                            GameObject.FindGameObjectWithTag("Player").SendMessage("SkillGaugeUp", 0.1f);
                            
                            shotState =ShotState.Shot;
                            Destroy(gameObject);
                        }
                    }
                    break;
                }
        }

        
    }
    /*private void OnTriggerEnter(Collider other)
    {
        switch (other.transform.name)
        {
            case "Enemy":
                {
                    Destroy(other.gameObject);
                    break;
                }
        }
    }*/
}