using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel.Design;

namespace TextRPGGame
{
    internal class Program
    {
        /// <summary>
        /// 캐릭터 인터페이스 정의
        /// </summary>
        public interface ICharacter
        {

        }

        static Player player = new Player();
        static Item item = new Item();

        static public int addAttackPower;
        static public int addDefensPower;
        static public int addHealth;
        static public int itemAddPower;
        static public int itemAddDefens;
        static public int gold;
        static public int itemPriceGold;

        static public bool isUseItem = false; // 착용가능인가?
        static public bool isWearingItem = false; // 착용중인가?
        static public bool isPurchase = false; // 구매완료인가?
        static public bool killMonstor = false; // 몬스터를 죽였는가

        static public List<string> ItemsNameList = new List<string>();
        static public List<string> ItemsExplainList = new List<string>();
        static public List<string> ItemsEffectList = new List<string>();
        static public List<string> InventoryItemsList = new List<string>();

        static public int[] itemPrice = { 100, 500, 200, 300, 200, 400 };


        static void Main(string[] args)
        {
            playerNameSetting();
        }
        /// <summary>
        /// 플레이어 클래스 정의
        /// </summary>
        public class Player
        {
            public string Name { get; set; }
            public int Level { get; set; } = 1;
            public string Job { get; set; }
            public int Health { get; set; } = 100;
            public int AttackPower { get; set; } = 10;
            public int DefensPower { get; set; } = 10;
            public int Gold { get; set; } = 1500;

        }
        /// <summary>
        /// 아이템 클래스정의
        /// </summary>
        public class Item
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Effect { get; set; }

        }
        /// <summary>
        /// 플레이어 이름 설정
        /// </summary>
        static public void playerNameSetting()
        {
            Console.WriteLine(" ");
            Console.WriteLine("■■■■■■■ WELCOME TO SPARTA ■■■■■■■");
            Console.WriteLine(" ");
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.Write($"먼저 플레이어의 이름을 설정해주세요 : ");
            player.Name = Console.ReadLine();
            Console.WriteLine(" ");
            Console.WriteLine($"플레이어의 이름은 \"[{player.Name}]\"(으)로 설정되었습니다.");

            ChooseJob();
        }
        /// <summary>
        /// 플레이어 직업선택(직업별:체력|공격력|방어력세팅)
        /// </summary>
        static public void ChooseJob()
        {
            string worrior = "전사";
            string mage = "마법사";
            string archer = "궁수";
            Console.WriteLine(" ");
            Console.WriteLine("■■■■■■■ SELECT JOB ■■■■■■■■");
            Console.WriteLine(" ");
            Console.WriteLine("직업을 선택하세요");
            Console.WriteLine("▶[1]: 전사");
            Console.WriteLine("     ▷ 체력 +20 | 공격력 +10 | 방어력 +20");
            Console.WriteLine("▶[2]: 마법사");
            Console.WriteLine("     ▷ 체력 +10 | 공격력 +20 | 방어력 + 5");
            Console.WriteLine("▶[3]: 궁수");
            Console.WriteLine("     ▷ 체력 +10 | 공격력 +20 | 방어력 +10");
            Console.WriteLine(" ");
            string job = Console.ReadLine();

            switch (job)
            {
                case "1":
                    Console.WriteLine("[전사] 를 선택하셨습니다.");
                    player.Job = worrior;
                    addHealth += 20;
                    addAttackPower += 10;
                    addDefensPower += 20;
                    GameStart();
                    break;
                case "2":
                    Console.WriteLine("[마법사] 를 선택하셨습니다.");
                    player.Job = mage;
                    addHealth += 10;
                    addAttackPower += 20;
                    addDefensPower += 5;
                    GameStart();
                    break;
                case "3":
                    Console.WriteLine("[궁수] 를 선택하셨습니다.");
                    player.Job = archer;
                    addHealth += 10;
                    addAttackPower += 20;
                    addDefensPower += 20;
                    GameStart();
                    break;
                default:
                    Console.WriteLine("올바른 값을 입력해주세요.");
                    return;
            }
            player.AttackPower += addAttackPower;
            player.DefensPower += addDefensPower;
            player.Health += addHealth;
        }

        /// <summary>
        /// 게임시작화면
        /// </summary>
        static void GameStart()
        {
            Console.Clear();
            Console.WriteLine(" ");
            Console.WriteLine("■■■■■■■GAME START■■■■■■");
            Console.WriteLine(" ");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine(" ");
            Console.WriteLine("▶[1] 상태 보기");
            Console.WriteLine("▶[2] 인벤토리");
            Console.WriteLine("▶[3] 상점");
            Console.WriteLine("▶[4] 던전입장");
            Console.WriteLine(" ");
            Console.Write("원하시는 행동을 입력해주세요: ");
            string chooseNum = Console.ReadLine();

            switch (chooseNum)
            {
                case "1":
                    Console.WriteLine("상태보기");
                    Console.WriteLine("캐릭터의 정보가 표시됩니다.");
                    CharState();
                    break;
                case "2":
                    Console.WriteLine("인벤토리");
                    Console.WriteLine("보유중인 아이템을 관리할 수 있습니다.");
                    Inven();
                    break;
                case "3":
                    Console.WriteLine("상점");
                    Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                    Store();
                    break;
                case "4":
                    Console.WriteLine("던전입장");
                    Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                    Dungeon();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다");
                    Console.ReadKey();
                    GameStart();
                    return;
            }
        }

        /// <summary>
        /// 현재 캐릭터상황
        /// </summary>
        static public void CharState()
        {
            Console.Clear();
            Console.WriteLine(" ");
            Console.WriteLine("■■■■■■■ PLAYER STATUS ■■■■■■■");
            Console.WriteLine(" ");
            Console.WriteLine($"Lv. {player.Level}");
            Console.WriteLine($"Name. {player.Name}");
            Console.WriteLine($"직업. {player.Job}");
            if (isUseItem) // 아이템 착용시 변화
            {
                Console.WriteLine($"공격력. {player.AttackPower}(+{addAttackPower})(+{itemAddPower}) ");
            }
            else
            {
                Console.WriteLine($"공격력. {player.AttackPower}(+{addAttackPower})");
            }
            if (isUseItem)
            {
                Console.WriteLine($"방어력. {player.DefensPower}(+{addDefensPower})(+{itemAddDefens})");
            }
            else
            {
                Console.WriteLine($"방어력. {player.DefensPower}(+{addDefensPower})");
            }
            Console.WriteLine($"체력. {player.Health} (+{addHealth})");
            Console.WriteLine($"Gold. {player.Gold} G");
            Console.WriteLine(" ");

            Console.WriteLine("▶[0] 나가기");
            Console.WriteLine(" ");
            Console.Write("원하시는 행동을 입력해주세요 : ");
            string exit = Console.ReadLine();

            switch (exit)
            {
                case "0":
                    Console.Clear();
                    GameStart();
                    break;

                default:
                    Console.WriteLine("올바른 값을 입력해주세요.");
                    Console.ReadKey();
                    return;
            }
        }
        /// <summary>
        /// 인벤토리
        /// </summary>
        static public void Inven()
        {
            Console.WriteLine(" ");
            Console.WriteLine("■■■■■■■ INVENTORY ■■■■■■■");
            Console.WriteLine(" ");
            Console.WriteLine("□□□□□□□ HAVE ITEM □□□□□□□");
            Console.WriteLine(" ");

            InventoryList();

            Console.WriteLine(" ");
            Console.WriteLine("▶[1] 장착관리");
            Console.WriteLine("▶[2] 상점");
            Console.WriteLine("▶[0] 나가기");
            Console.WriteLine(" ");
            Console.Write("원하시는 행동을 입력해주세요 : ");
            string exit = Console.ReadLine();

            switch (exit)
            {
                case "1":
                    Console.WriteLine("장착관리");
                    Console.WriteLine("캐릭터의 정보가 표시됩니다.");
                    InventoryOnItem();
                    break;
                case "2":
                    Console.WriteLine("상점");
                    Console.WriteLine(" ");
                    Store();
                    break;
                case "0":
                    Console.Clear();
                    GameStart();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다");
                    Console.ReadKey();
                    Inven();
                    return;
            }
        }
        /// <summary>
        /// 인벤토리내부 HaveItem 출력 조건
        /// </summary>
        static public void InventoryList()
        {
            if (InventoryItemsList.Count == 0)
            {
                Console.WriteLine("아무것도 가지고 있지 않습니다.");
                Console.WriteLine("상점에서 아이템을 구매해주세요.");
            }
            if (isPurchase == true) //구매 후
            {
                if (isWearingItem == true) //착용중
                {
                    for (int i = 0; i < InventoryItemsList.Count; i++)
                    {
                        Console.WriteLine($"[E] {InventoryItemsList[i]}");
                    }
                }
                else
                {
                    for (int i = 0; i < InventoryItemsList.Count; i++)
                    {
                        Console.WriteLine($"{InventoryItemsList[i]}");
                    }
                }
            }
        }
        /// <summary>
        /// 인벤토리- 아이템 장착 관리 화면
        /// </summary>
        static public void InventoryOnItem()
        {
            Console.WriteLine(" ");
            Console.WriteLine("■■■■■■■ INVENTORY ■■■■■■■");
            Console.WriteLine(" ");
            Console.WriteLine("□□□□□□□ HAVE ITEM □□□□□□□");
            Console.WriteLine(" ");

            InventoryList();

            Console.WriteLine("□□□□□□ ITEM SETTING □□□□□□");
            Console.WriteLine(" ");
            Console.WriteLine("▶[1] 장착하기");
            Console.WriteLine("▶[2] 장착해제하기");
            Console.WriteLine("▶[0] 나가기");
            Console.WriteLine(" ");
            Console.Write("원하시는 행동을 입력해주세요 : ");
            string exit = Console.ReadLine();

            switch (exit)
            {
                case "1":
                    Console.WriteLine("장착하기");
                    Console.WriteLine("  ");
                    //공격력+, E표시 활성화 // 장착할 아이템선택하기

                    break;
                case "2":
                    Console.WriteLine("장착해제하기");
                    Console.WriteLine(" ");
                    //해제시 공격력표시 X, E표시 해제

                    break;

                case "0":
                    GameStart();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다");
                    Console.ReadKey();
                    InventoryOnItem();
                    return;
            }

        }


        /// <summary>
        /// 상점
        /// </summary>
        static public void Store()
        {
            Console.WriteLine(" ");
            Console.WriteLine("■■■■■■■ STORE ■■■■■■■■");
            Console.WriteLine(" ");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine(" ");
            Console.WriteLine("[보유골드]");
            Console.WriteLine($"{player.Gold} G");
            Console.WriteLine(" ");
            Console.WriteLine("[아이템 목록]");
            Console.WriteLine(" ");
            ItemExplain();
            Console.WriteLine(" ");
            Console.WriteLine("▶[1] 아이템구매");
            Console.WriteLine("▶[0] 나가기");
            Console.WriteLine(" ");
            Console.Write("원하시는 행동을 입력해주세요 : ");
            string exit = Console.ReadLine();

            switch (exit)
            {
                case "1":
                    Console.WriteLine(" ");
                    Console.WriteLine("▶아이템구매하기");
                    Console.WriteLine(" ");
                    ItemSelect();
                    break;
                case "0":
                    Console.Clear();
                    GameStart();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다");
                    Console.ReadKey();
                    Store();
                    return;
            }
        }

        /// <summary>
        /// 보유중인 아이템
        /// </summary>
        static public void getItem()
        {
            if (isPurchase == true) // 구매 했을때, 착용이 가능하고,
            {
                isUseItem = true;
            }
            else if (isPurchase == true) // 구매는 했지만 착용하지 않은 상태
            {
                isUseItem = false;
            }
            else
            {
                isPurchase = false;
                isUseItem = false;
            }
        }

        /// <summary>
        /// 아이템 추가 및 설명 메서드
        /// </summary>
        static public void ItemExplain()
        {
            ItemsNameList.Add("[  낡고 허름한 검   ]");
            ItemsNameList.Add("[ 매우강력한 강철검 ]");
            ItemsNameList.Add("[     나무지팡이    ]");
            ItemsNameList.Add("[ 화려한 주술 지팡이]");
            ItemsNameList.Add("[ 초심자 초보자의활 ]");
            ItemsNameList.Add("[ 매우 단단한 강철활]");

            ItemsEffectList.Add("| 공격력 [+ 1]");
            ItemsEffectList.Add("| 공격력 [+ 7]");
            ItemsEffectList.Add("| 공격력 [+ 2]");
            ItemsEffectList.Add("| 공격력 [+ 5]");
            ItemsEffectList.Add("| 공격력 [+ 2]");
            ItemsEffectList.Add("| 공격력 [+ 5]");

            ItemsExplainList.Add("| 그냥 낡은 검입니다.");
            ItemsExplainList.Add("| 강력하고 날카로운 검입니다.");
            ItemsExplainList.Add("| 나무로 만든 지팡이입니다.");
            ItemsExplainList.Add("| 화려한 주술을 사용할수있는 지팡이입니다.");
            ItemsExplainList.Add("| 초보자의 활입니다.");
            ItemsExplainList.Add("| 강철과 같은 활입니다.");


            for (int i = 0; i < 6; i++)
            {
                item.Name = ItemsNameList[i];
                item.Effect = ItemsEffectList[i];
                item.Description = ItemsExplainList[i];
                itemPriceGold = itemPrice[i];

                Console.WriteLine($" [{i + 1}] {item.Name} {item.Effect} {item.Description} ■■ {itemPriceGold}G ");
                
            }

        }
        /// <summary>
        /// 아이템 구매하기 + 인벤토리리스트Add
        /// </summary>
        static public void ItemSelect()
        {
            Console.Write("구매할 아이템 번호를 입력해주세요 : ");
            string selectItem = Console.ReadLine();
            Console.WriteLine(" ");

            switch (selectItem)
            {
                case "1":

                    Console.WriteLine($"{ItemsNameList[0]}을 구매하였습니다.");
                    InventoryItemsList.Add($"{ItemsNameList[0]}{ItemsEffectList[0]}{ItemsExplainList[0]}");
                    itemAddPower += 1;//착용시 추가해줘야함.
                    player.Gold -= itemPriceGold;
                    Console.ReadKey();
                    Store();
                    //플레이어 공격력추가, 골드감소, 
                    break;
                case "2":
                    Console.WriteLine($"{ItemsNameList[1]}을 구매하였습니다.");
                    isPurchase = true;
                    InventoryItemsList.Add($"{ItemsNameList[1]}{ItemsEffectList[1]}{ItemsExplainList[1]}");
                    itemAddPower += 7;
                    player.Gold -= itemPriceGold;
                    Console.ReadKey();
                    Store();
                    break;
                case "3":
                    Console.WriteLine($"{ItemsNameList[2]}을 구매하였습니다.");
                    isPurchase = true;
                    InventoryItemsList.Add($"{ItemsNameList[2]}{ItemsEffectList[2]}{ItemsExplainList[2]}");
                    itemAddPower += 2;
                    player.Gold -= itemPriceGold;
                    Console.ReadKey();
                    Store();
                    break;
                case "4":
                    Console.WriteLine($"{ItemsNameList[3]}을 구매하였습니다.");
                    isPurchase = true;
                    InventoryItemsList.Add($"{ItemsNameList[3]}{ItemsEffectList[3]}{ItemsExplainList[3]}");
                    itemAddPower += 4;
                    player.Gold -= itemPriceGold;
                    Console.ReadKey();
                    Store();
                    break;
                case "5":
                    Console.WriteLine($"{ItemsNameList[4]}을 구매하였습니다.");
                    isPurchase = true;
                    InventoryItemsList.Add($"{ItemsNameList[4]}{ItemsEffectList[4]}{ItemsExplainList[4]}");
                    itemAddPower += 2;
                    player.Gold -= itemPriceGold;
                    Console.ReadKey();
                    Store();
                    break;
                case "6":
                    Console.WriteLine($"{ItemsNameList[5]}을 구매하였습니다.");
                    isPurchase = true;
                    InventoryItemsList.Add($"{ItemsNameList[5]}{ItemsEffectList[5]}{ItemsExplainList[5]}");
                    itemAddPower += 5;
                    player.Gold -= itemPriceGold;
                    Console.ReadKey();
                    Store();
                    break;
                default:
                    Console.WriteLine("아무것도 구매하지 않았습니다");
                    isPurchase = false;
                    Console.ReadKey();
                    GameStart();
                    return;
            }
        }
        /// <summary>
        /// 골드를 얻는 조건_미완성
        /// </summary>
        static public void EarnGold()
        {
            if (killMonstor == true)
            {
                gold += 100;
                player.Gold += gold;
            }
            Console.WriteLine($"{player.Gold}");
        }        

        /*        ### 상점

        - 보유중인 골드와 아이템의 정보, 가격이 표시됩니다.
        - 아이템 정보 오른쪽에는 가격이 표시가 됩니다.
        - 이미 구매를 완료한 아이템이라면 **구매완료** 로 표시됩니다.

        ** 상점**
        필요한 아이템을 얻을 수 있는 상점입니다.

        [보유 골드]
        800 G

        [아이템 목록]
        - 수련자 갑옷    | 방어력 +5  | 수련에 도움을 주는 갑옷입니다.             |  1000 G
        - 무쇠갑옷      | 방어력 +9  | 무쇠로 만들어져 튼튼한 갑옷입니다.           |  구매완료
        - 스파르타의 갑옷 | 방어력 +15 | 스파르타의 전사들이 사용했다는 전설의 갑옷입니다.|  3500 G
        - 낡은 검      | 공격력 +2  | 쉽게 볼 수 있는 낡은 검 입니다.            |  600 G
        - 청동 도끼     | 공격력 +5  |  어디선가 사용됐던거 같은 도끼입니다.        |  1500 G
        - 스파르타의 창  | 공격력 +7  | 스파르타의 전사들이 사용했다는 전설의 창입니다. |  구매완료


        ### 아이템 구매

        - **아이템 구매** 를 선택하면 아이템 목록 앞에 숫자가 표시됩니다.
        - 일치하는 아이템을 선택했다면 (예제에서 1~6선택시)
            - 이미 구매한 아이템이라면
            → **이미 구매한 아이템입니다** 출력
            - 구매가 가능하다면
                - 보유 금액이 충분하다면
                → **구매를 완료했습니다.** 출력
                재화 감소 / 인벤토리에 아이템 추가 / 상점에 구매완료 표시
                - 보유 금액이 부족하다면
                → **Gold 가 부족합니다.** 출력
        - 일치하는 아이템을 선택했지 않았다면 (예제에서 1~3이외 선택시)
            - **잘못된 입력입니다** 출력*/


        /// <summary>
        /// 던전입장
        /// </summary>
        static public void Dungeon()
        {
            Console.WriteLine(" ");
            Console.WriteLine("■■■■■■■■ DUNGEON ■■■■■■■■");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("▶[1] 쉬운 던전  | 방어력 5이상 권장");
            Console.WriteLine("▶[2] 일반 던전  | 방어력 11이상 권장");
            Console.WriteLine("▶[3] 어려운던전 | 방어력 17이상 권장");
            Console.WriteLine("▶[0] 나가기");
            Console.WriteLine(" ");
            Console.Write("원하시는 행동을 입력해주세요 : ");
            string exit = Console.ReadLine();

            switch (exit)
            {
                case "1":
                    Console.WriteLine("쉬운 던전");
                    Console.WriteLine("방어력 5이상 권장");
                    EasyMode();
                    break;
                case "2":
                    Console.WriteLine("일반 던전");
                    Console.WriteLine("방어력 11이상 권장");
                    NormalMode();
                    break;
                case "3":
                    Console.WriteLine("어려운던전");
                    Console.WriteLine("방어력 17이상 권장");
                    HardMode();
                    break;
                case "0":
                    Console.Clear();
                    GameStart();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다");
                    Console.ReadKey();
                    Inven();
                    return;
            }
        }

        static public void EasyMode()
        {
            Console.WriteLine(" ");
            Console.WriteLine("■■■■■■■■ EASY DUNGEON ■■■■■■■■");
            Console.WriteLine(" ");
            Console.WriteLine("축하합니다!!");
            Console.WriteLine("쉬운 던전을 클리어 하셨습니다.");
            Console.WriteLine("▶[0] 나가기");
            string exit = Console.ReadLine();

            switch (exit)
            {
                case "1":
                    Console.WriteLine("쉬운 던전");
                    Console.WriteLine("방어력 5이상 권장");
                    EasyMode();
                    break;
                case "0":
                    Console.Clear();
                    GameStart();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다");
                    return;
            }
        }
        static public void NormalMode()
        {
            Console.WriteLine(" ");
            Console.WriteLine("■■■■■■■■ NORMAL DUNGEON ■■■■■■■■");
            Console.WriteLine(" ");
            Console.WriteLine("축하합니다!!");
            Console.WriteLine("일반 던전을 클리어 하셨습니다.");
            Console.WriteLine("▶[0] 나가기");
            string exit = Console.ReadLine();

            switch (exit)
            {
                case "1":
                    Console.WriteLine("일반 던전");
                    Console.WriteLine("방어력 11이상 권장");
                    EasyMode();
                    break;
                case "0":
                    Console.Clear();
                    GameStart();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다");
                    Console.ReadKey();
                    return;
            }
        }
        static public void HardMode()
        {
            Console.WriteLine(" ");
            Console.WriteLine("■■■■■■■■ HARD DUNGEON ■■■■■■■■");
            Console.WriteLine(" ");
            Console.WriteLine("축하합니다!!");
            Console.WriteLine("어려운 던전을 클리어 하셨습니다.");
            Console.WriteLine("▶[0] 나가기");
            string exit = Console.ReadLine();

            switch (exit)
            {
                case "1":
                    Console.WriteLine("어려운 던전");
                    Console.WriteLine("방어력 17이상 권장");
                    EasyMode();
                    break;
                case "0":
                    Console.Clear();
                    GameStart();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다");
                    Console.ReadKey();
                    return;
            }
        }
    }

}


/* ### **인벤토리**

                           - 보유 중인 아이템을 전부 보여줍니다.
               이때 장착중인 아이템 앞에는[E] 표시를 붙여 줍니다.
               - 처음 시작시에는 아이템이 없습니다.

               ```csharp
               ** 인벤토리**
               보유 중인 아이템을 관리할 수 있습니다.

               [아이템 목록]

               1.장착 관리
               0.나가기

               원하시는 행동을 입력해주세요.
               >>
               ```

               -아이템이 있을 때

                   **인벤토리 * *
                   보유 중인 아이템을 관리할 수 있습니다.


                   [아이템 목록]
                   - [E]무쇠갑옷 | 방어력 + 5 | 무쇠로 만들어져 튼튼한 갑옷입니다.
                   - [E]스파르타의 창  | 공격력 + 7 | 스파르타의 전사들이 사용했다는 전설의 창입니다.
                   -낡은 검 | 공격력 + 2 | 쉽게 볼 수 있는 낡은 검 입니다.


                   1.장착 관리
                   2.나가기


                   원하시는 행동을 입력해주세요.
                   >>


### 장착 관리

               -장착관리가 시작되면 아이템 목록 앞에 숫자가 표시됩니다.
               -일치하는 아이템을 선택했다면(예제에서 1~3선택시)
                   -장착중이지 않다면 → 장착
                   [E] 표시 추가
                   -이미 장착중이라면 → 장착 해제
                   [E] 표시 없애기
               -일치하는 아이템을 선택했지 않았다면(예제에서 1~3이외 선택시)
                   - **잘못된 입력입니다** 출력
               -아이템의 중복 장착을 허용합니다.
                   - 창과 검을 동시에 장착가능
                   - 갑옷도 동시에 착용가능
                   -장착 갯수 제한 X

               ** 인벤토리 -장착 관리**
               보유 중인 아이템을 관리할 수 있습니다.

               [아이템 목록]
               - 1[E]무쇠갑옷 | 방어력 + 5 | 무쇠로 만들어져 튼튼한 갑옷입니다.
               - 2[E]스파르타의 창  | 공격력 + 7 | 스파르타의 전사들이 사용했다는 전설의 창입니다.
               -3 낡은 검         | 공격력 + 2 | 쉽게 볼 수 있는 낡은 검 입니다.

               0.나가기

               원하시는 행동을 입력해주세요.
               >>
               ```
*/
