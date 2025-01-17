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
    public float range;//���� �ݰ�

    private void OnDrawGizmos()
    {
        //���̾� ���Ǿ� ����� �׸���(��ġ,�ݰ�)
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
                    int layerMask = 1 << 6;//6�����̾ int�� ������ ����
                                           //���Ǿ�ĳ��Ʈ�� ����Ѵ�.(��ġ,�ݰ�,����,�Ÿ�,���̾��ũ)
                    RaycastHit[] hits = Physics.SphereCastAll(transform.position, range, Vector3.up, 0, layerMask);

                    if (hits.Length > 0)
                    {
                        foreach (RaycastHit hit in hits)
                        {
                            Vector3 explosionPosition = gameObject.transform.position;
                            GameObject explosion = Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);

                            // ���Ⱑ �ٲ� �κ�: explosionPrefab�� ParticleSystem�� ���� ���,
                            // ��ƼŬ�� ���� �� �ڵ����� �ش� ���� ������Ʈ�� �����ϴ� �ڵ�
                            ParticleSystem explosionParticles = explosion.GetComponent<ParticleSystem>();
                            if (explosionParticles != null)
                            {
                                // ��ƼŬ�� ���� �� ���� ������Ʈ�� ����
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