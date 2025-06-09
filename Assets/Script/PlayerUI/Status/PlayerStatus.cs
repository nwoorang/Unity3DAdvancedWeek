using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("Player Status")]
    public int damage; // 플레이어의 공격력(장비에 따라 변동)
    public int gather; // 플레이어의 채집 능력치(장비에 따라 변동)
    public Stat health; // 플레이어의 체력
    public Stat stamina; // 플레이어의 스테미나
    public Stat hunger; // 플레이어의 허기
    public Stat thirst; // 플레이어의 갈증

    public int basicAttack { get; private set; } = 5;
    public float temperature; // 플레이어의 체온

    private void Start()
    {
        damage = basicAttack; // 초기 공격력 설정
        gather = basicAttack; // 초기 채집 능력치 설정
    }

    void Update()
    {
        health.Regenerate(Time.deltaTime);
        if (PlayerMediator.Instance.controller.IsSprinting())
            stamina.Substract(10 * Time.deltaTime); // 달리기 중 스테미나 감소
        else
            stamina.Regenerate(Time.deltaTime);

        hunger.Regenerate(Time.deltaTime);
        thirst.Regenerate(Time.deltaTime);

        if(hunger.current <= 0 || thirst.current <=0)
        {
            health.Substract(1 * Time.deltaTime); // 허기나 갈증이 0 이하일 때 체력 감소
        }
    }
}
