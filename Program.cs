using System;
using System.Collections.Generic;

namespace TextRPG
{
    internal class Program
    {
        static Character player;
        static List<Item> inventory;

        static void Main(string[] args)
        {
            GameDataSetting();
            DisplayGameIntro();
        }

        static void GameDataSetting()
        {
            player = new Character("Chad", "전사", 1, 10, 5, 100, 1500); // 캐릭터 정보 세팅

            inventory = new List<Item> // 아이템을 리스트로 나열
            {
                new Item("천갑옷", "방어력 +5 | 천으로 덧댄갑옷. 오래되어 낡아보인다.", 0, 5),
                new Item("낡은 검", "공격력 +2 | 날이 무딘 낡은 검.", 2, 0)
            };
        }

        static void DisplayGameIntro()
        {
            Console.Clear();

            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 장착 관리");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int input = CheckValidInput(1, 2);
            switch (input)
            {
                case 1:
                    DisplayMyInfo();
                    break;

                case 2:
                    DisplayEquipMenu();
                    break;
            }
        }

        static void DisplayMyInfo()
        {
            Console.Clear();

            Console.WriteLine("상태보기");
            Console.WriteLine("캐릭터의 정보를 표시합니다.");
            Console.WriteLine();
            Console.WriteLine($"Lv.{player.Level}");
            Console.WriteLine($"{player.Name}({player.Job})");
            Console.WriteLine($"공격력 :{player.Atk}");
            Console.WriteLine($"방어력 : {player.Def}");
            Console.WriteLine($"체력 : {player.Hp}");
            Console.WriteLine($"Gold : {player.Gold} G");
            Console.WriteLine();
            Console.WriteLine("0. 나가기");

            int input = CheckValidInput(0, 0);
            switch (input)
            {
                case 0:
                    DisplayGameIntro();
                    break;
            }
        }

        static void DisplayEquipMenu()
        {
            Console.Clear();

            Console.WriteLine("인벤토리 - 장착 관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            DisplayInventory();

            Console.WriteLine("0. 나가기");

            int input = CheckValidInput(0, inventory.Count);
            if (input == 0)
            {
                DisplayGameIntro();
            }
            else
            {
                EquipItem(input);
            }
        }

        static void DisplayInventory()
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {GetEquipStatus(inventory[i])}{inventory[i].Name} | {inventory[i].Effect}");
            }
        }

        static string GetEquipStatus(Item item)
        {
            return player.IsEquipped(item) ? "[E] " : "     ";
        }

        static void EquipItem(int index)
        {
            Item selecteditem = inventory[index - 1]; //인벤토리에서 제공된 인덱스를 기반으로 선택한 아이템을 검색

            Console.WriteLine(selecteditem.Effect); //선택한 아이템의 효과와 같은 아이템이 선택됨
            Console.WriteLine("일치하는 아이템을 선택했습니다.");

            if (player.IsEquipped(selecteditem)) // 선택한 아이템이 플레이어가 장착중이라면
            {
                player.UnequipItem(selecteditem); // 선택한 아이템을 장착 해제
                Console.WriteLine("이미 장착중인 아이템을 해제했습니다.");
            }
            else // 선택한 아이템을 장착중이 아니라면
            {
                player.EquipItem(selecteditem); //선택한 아이템을 장착
                Console.WriteLine("아이템을 장착했습니다.");
            }           
            DisplayEquipMenu(); // 마지막으로 장착메뉴를 다시 Display
        }

        static int CheckValidInput(int min, int max)
        {
            while (true) // 반복으로 실핼
            {
                string input = Console.ReadLine(); // 문자열을 입력받는다

                bool parseSuccess = int.TryParse(input, out var ret); // 입력된 분자열을 정수로 변환하고 ret 변수에 값을 저장한다
                if (parseSuccess) // 변환이 성공하고 값이 min과 max 사이에 있으면 값을 반환한다 
                {
                    if (ret >= min && ret <= max)
                        return ret;
                }

                Console.WriteLine("잘못된 입력입니다."); // 반환이 실패하거나 값이 범위를 벗어나면 메세지를 출력함
            }
        }
    }

    public class Character // 캐릭터 클래스 생성 = 구성도
    {
        public string Name { get; }
        public string Job { get; }
        public int Level { get; }
        public int Atk { get; private set; }
        public int Def { get; private set; }
        public int Hp { get; }
        public int Gold { get; }

        private List<Item> equippedItems;

        public Character(string name, string job, int level, int atk, int def, int hp, int gold) // 생성자. 인스턴스를 생성할때 ~이벤트가 발생하면 좋겠다.
        {
            Name = name;
            Job = job;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
            equippedItems = new List<Item>();
        }

        public void EquipItem(Item item) //
        {
            equippedItems.Add(item);
            Atk += item.Attack;
            Def += item.Defense;
        }

        public void UnequipItem(Item item)
        {
            equippedItems.Remove(item);
            Atk -= item.Attack;
            Def -= item.Defense;
        }

        public bool IsEquipped(Item item)
        {
            return equippedItems.Contains(item);
        }
    }

    public class Item // 아이템 클래스 생성
    {
        public string Name { get; set; }
        public string Effect { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }

        public Item(string name, string effect, int attack, int defense)
        {
            Name = name;
            Effect = effect;
            Attack = attack;
            Defense = defense;
        }
    }
}