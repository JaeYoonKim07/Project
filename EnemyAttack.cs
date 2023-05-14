using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    private NavMeshAgent Nav;
    private NavMeshHit hit;
    private bool blocked = false;
    public bool RunToPlayer = false;
    private float DistanceToPlayer;
    private bool IsChecking = true;
    private int FailedChecks = 0;
    [SerializeField] Transform Player;
    [SerializeField] Animator Anim;
    [SerializeField] GameObject Enemy;
    [SerializeField] float MaxRange = 35.0f;
    [SerializeField] int MaxChecks = 3;
    [SerializeField] float ChaseSpeed = 8.5f;
    [SerializeField] float WalkSpeed = 1.6f;
    [SerializeField] float AttackDistance = 2.3f;
    [SerializeField] float AttackRotateSpeed = 2.0f;
    [SerializeField] float CheckTime = 3.0f;
    [SerializeField] GameObject ChaseMusic;
    [SerializeField] GameObject HurtUI;
    [SerializeField] GameObject EnemyDamageZone;
    [SerializeField] bool IHaveKnife;
    [SerializeField] bool IHaveBat;
    [SerializeField] bool IHaveAxe;
    private bool CanRun = false;

    // Start is called before the first frame update
    void Start()
    {
        Nav = GetComponentInParent<NavMeshAgent>();

        StartCoroutine(StartElements());

    }

    // Update is called once per frame
    void Update()
    {
        if (CanRun == true)
        {
            if (EnemyDamageZone.GetComponent<EnemyDamage>().HasDied == true)
            {
                ChaseMusic.gameObject.SetActive(false);
            }
            DistanceToPlayer = Vector3.Distance(Player.position, Enemy.transform.position); //플레이어와 적 AI의 거리
            if (DistanceToPlayer < MaxRange) //적의 탐색범위안에 플레이어가 안에 있을시
            {
                if (IsChecking == true)
                {
                    IsChecking = false;

                    blocked = NavMesh.Raycast(transform.position, Player.position, out hit, NavMesh.AllAreas);

                    if (blocked == false)
                    {
                        RunToPlayer = true;
                        FailedChecks = 0;
                    }
                    if (blocked == true)
                    {
                        RunToPlayer = false;
                        Anim.SetInteger("State", 1);
                        FailedChecks++;
                    }

                    StartCoroutine(TimedCheck());
                }
            }

            if (RunToPlayer == true)
            {
                Enemy.GetComponent<EnemyMove>().enabled = false; //enemymove스크립트 비활성화
                if (EnemyDamageZone.GetComponent<EnemyDamage>().HasDied == false)
                {
                    ChaseMusic.gameObject.SetActive(true); //추격 음악 활성화
                }
                if (DistanceToPlayer > AttackDistance) //플레이어와 적의 거리 > 공격범위 
                {
                    Nav.isStopped = false;
                    Anim.SetInteger("State", 2);  //뛰는 애니메이션 활성화
                    Nav.acceleration = 24; //적이 플레이어에게 회전하는 속도
                    Nav.SetDestination(Player.position); //목표:플레이어
                    Nav.speed = ChaseSpeed;
                    HurtUI.gameObject.SetActive(false);
                }
                if (DistanceToPlayer < AttackDistance - 0.5f) //플레이어와 적의 거리 < 공격범위 - 0.5F
                {
                    Nav.isStopped = true;
                    if (IHaveAxe == true)
                    {
                        Anim.SetInteger("State", 3); //도끼 공격 애니메이션 실행
                    }
                    if (IHaveBat == true)
                    {
                        Anim.SetInteger("State", 4); //방망이 공격 애니메이션 실행
                    }
                    if (IHaveKnife == true) 
                    {
                        Anim.SetInteger("State", 5); //칼 공격 애니메이션 실행
                    }
                    Nav.acceleration = 180; //적이 플레이어에게 회전하는 속도
                    HurtUI.gameObject.SetActive(true);

                    Vector3 Pos = (Player.position - Enemy.transform.position).normalized;
                    Quaternion PosRotation = Quaternion.LookRotation(new Vector3(Pos.x, 0, Pos.z));
                    Enemy.transform.rotation = Quaternion.Slerp(Enemy.transform.rotation, PosRotation, Time.deltaTime * AttackRotateSpeed);
                }
            }
            else if (RunToPlayer == false)
            {
                Nav.isStopped = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            RunToPlayer = true;
        }
        if (other.gameObject.CompareTag("PKnife"))
        {
            Anim.SetTrigger("SmallReact");
        }
        if (other.gameObject.CompareTag("PBat"))
        {
            Anim.SetTrigger("SmallReact");
        }
        if (other.gameObject.CompareTag("PAxe"))
        {
            Anim.SetTrigger("BigReact");
        }
        if (other.gameObject.CompareTag("PCrossbow"))
        {
            Anim.SetTrigger("BigReact");
        }
    }

    IEnumerator TimedCheck()
    {
        yield return new WaitForSeconds(CheckTime);
        IsChecking = true;

        if(FailedChecks > MaxChecks)
        {
            Enemy.GetComponent<EnemyMove>().enabled = true;
            Nav.isStopped = false;
            Nav.speed = WalkSpeed;
            FailedChecks = 0;
            ChaseMusic.gameObject.SetActive(false);
        }
    }

    IEnumerator StartElements()
    {
        yield return new WaitForSeconds(0.1f);
        Player = SaveScript.PlayerChar;
        ChaseMusic = SaveScript.Chase;
        HurtUI = SaveScript.HurtScreen;
        ChaseMusic.gameObject.SetActive(false);
        CanRun = true;
        CheckTime = Random.Range(3, 15);
    }
}
