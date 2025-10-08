using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Runtime.InteropServices;
using System.Threading;

namespace ConsoleLimbus
{
    class BattleSystem
    {
        Random rnd = new Random();
        public int UseSkillBuf(Character character1, Character targetCharacter, SkillInfo skill)
        {
            //플레이어1가 target플레이어한테 스킬 값만큼 힐을주거나, 데미지를 줌 (target에 자기자신줘서 자신데미지, 자신힐도 가능)

            int mental = character1.Mentality; //정신력 -45~ 45까지
            //정신력이 0이라면? 50퍼 , 정신력이 -45라면? 5퍼, 정신력이 45라면 95퍼센트 확률로 
            int percent = 50 + character1.Mentality;// 5, 95



            //코인 앞뒤면 체크
            int coinFrontCount = 1; //코인 앞면 나온 횟수,  1부터 시작, 1개증가할때마다 N배가됨
            for (int i = 0; i < skill.coinCount; i++)
            {
                if (rnd.Next(0, 100) < percent) coinFrontCount++;
            }
            int amount = skill.power * coinFrontCount;
            //코인 앞뒤면 정보와 위력 출력
            Console.Write($"{character1.Name}: 코인: ");
            for (int i = 0; i < skill.coinCount; i++)
            {
                if (i < coinFrontCount - 1) Console.Write("●");
                else Console.Write("○");

            }
            Console.WriteLine($" 위력: {amount}");

            switch (skill.eCurSkillDamageType)
            {
                case eSkillDamageType.Damage:
                    //데미지 =  기본값 * 코인 앞면 나온횟수
                    //skill.coinCount; //코인 횟수만큼 random하기, 랜덤횟수얻음
                    //skill.power; //기본 데미지값
                    targetCharacter.TakeDamage(amount);
                    break;
                case eSkillDamageType.Heal:
                    targetCharacter.Healing(amount);
                    break;
            }
            return amount; //위력을 출력해서 서로 합을 칠수 있게 설정
        }
        public int GetSkillPower(Character character1, Character targetCharacter, SkillInfo skill)
        {
            //스킬 위력을 얻어오는 메서드 (스킬위력을 서로 비교해서 플레이어와 적중에 누가 공격할지 정함

            //▼추후 추가예정(정신력시스템, 합을 지면 정신력이 내려가서.. 점점 질 확률이 늘어남)
            int mental = character1.Mentality; //정신력 -45~ 45까지
            //정신력이 0이라면? 50퍼 , 정신력이 -45라면? 5퍼, 정신력이 45라면 95퍼센트 확률로 
            int percent = 50 + character1.Mentality;// 5, 95

            //코인 앞뒤면 체크
            int coinFrontCount = 1; //코인 앞면 나온 횟수,  1부터 시작, 1개증가할때마다 N배가됨
            for (int i = 0; i < skill.coinCount; i++)
            {
                if (rnd.Next(0, 100) < percent) coinFrontCount++;
            }
            int amount = skill.power * coinFrontCount;
            //코인 앞뒤면 정보와 위력 출력
            Console.Write($"{character1.Name}: 코인: ");
            for (int i = 0; i < skill.coinCount; i++)
            {
                if (i < coinFrontCount - 1) Console.Write("●");
                else Console.Write("○");
            }
            Console.WriteLine($" 위력: {amount}");
            return amount; //위력을 출력해서 서로 합을 칠수 있게 설정
        }
        public void UseSkill(Character character1, Character targetCharacter, SkillInfo skill,int amount)
        {
            switch (skill.eCurSkillDamageType)
            {
                case eSkillDamageType.Damage:
                    //데미지 =  기본값 * 코인 앞면 나온횟수
                    targetCharacter.TakeDamage(amount);
                    break;
                case eSkillDamageType.Heal:
                    targetCharacter.Healing(amount);
                    break;
            }
        }
    }
    class Program
    {
        static void BattleWindow()
        {
            Console.WriteLine("                                             \r\n                                             \r\n   .................                         \r\n   .+%%%*%%%%%%%##-.                         \r\n   .*%%%%%%%%%%#%%-.                         \r\n   .#%%%%%%#===#%%=.                         \r\n*#%*%%%%%%#++==*%%+.                         \r\n+++=%%%%**++==+*#*+.                         \r\n-==+#%%%**%*==*++*+.                         \r\n--=%%%%%%%%#%%%=-::.                         \r\n.:#%%%%%%%%%%%%#*+=.                         \r\n:-#%%%%%%%%%%%%%*==.                         \r\n.*%%%%%%%%%%%%%%%#*=:::=-                    \r\n=#%%%%%#*%%%%%%%%%%%%%#+=                    \r\n.-##%%%==+#%%%%%%%%%%%%%#                    \r\n..+#%%%#%%%%%%%%%%%%%%%%%#*==:               \r\n...=#**%#%%%%%%+.++*#%%%%%%%%%#*-::...       \r\n    .--*%%%+%%%*===-:::+#%%%%%%%%%#*=.       \r\n    -+*#%%*:%#%#.        -##%%%%%%%%%*       \r\n    ::*%%#=.#%%#:           -+++%%%%%*       \r\n    .:##%*..####:               =====-       \r\n    .*%%#* .*%%#.                            \r\n    =###%: :#%%#-                            \r\n    *###=   =%%%%#%%%#.                      \r\n   .%%%%=   =%%%%%%%%%.                      \r\n   .#%%%=...=##%%%%#%*.                      \r\n**+--+*****++====-=++*.                      ");
            Console.SetWindowSize(120, 50);   // 가로 120, 세로 50
            Console.SetBufferSize(120, 50);   // 버퍼도 똑같이
            string[] asciiArt = new string[]
        {
            "        @@@@%@@@    ",
"    @@@@@@@%@@@@    ",
"   @%@@%@%@@@%@@    ",
"  @@@%%%%@@%%@@@@   ",
"  @@%%+#@@@@@@@@@@@ ",
"  @@@#*#%@@@@@@@@@@@",
"  @@@%#%%@@@@@%@@@@@",
"  %%@@%@%@@@@%@@@@@ ",
"  @%@@@%%%@@@@@@@@  ",
" @@@@@@%@%@@*@@%@@  ",
" @%%=#+**%@%.@@@@@  ",
" #++%%@@%-@%.@%@@@  ",
" :+#@@%@%-%%.@%%@@  ",
".*+@@%@@%*@@.@%%@   ",
".%:%%@  @@+%.%%@    ",
" :@@@@  @@%%  %%    ",
" @@@@@  @@@@  %%    ",
" @@@@    @@@  %     "
        };

            int leftX = 0;   // 왼쪽 캐릭터 시작 X좌표
            int rightX = 70; // 오른쪽 캐릭터 시작 X좌표


            // 오른쪽 출력
            for (int i = 0; i < asciiArt.Length; i++)
            {
                Console.SetCursorPosition(rightX, i);
                Console.Write(asciiArt[i]);
            }

            Console.SetCursorPosition(0, asciiArt.Length + 2);
            Console.WriteLine("왼쪽 VS 오른쪽 전투 화면!");

        }

        static void ShowMainWindow(Player player)
        {
            Console.WriteLine("===================================================");
            Console.WriteLine("안녕하세요 당신의 이름을 입력해주세요!!");
            player.SetName(Console.ReadLine());
            Console.WriteLine();
            Console.WriteLine("===================================================");
            while(true)
            {
                int width = Console.BufferWidth;
                int height = Console.BufferHeight;


                Console.Clear();
                Console.WriteLine("===================================================");
                Console.WriteLine($"환영합니다 {player.Name}님");
                Console.WriteLine("무엇을 해보시겠습니까?");
                Console.WriteLine("1. 인벤토리 2. 장비장착 3. 가챠  4. 상점 5.전투 ");
                Console.WriteLine("===================================================");

                int choice;
                int.TryParse(Console.ReadLine(), out choice);
                switch(choice)
                {
                    case 1:
                        //인벤토리 설정
                        ShowInventoryWindow(player);
                        break;
                    case 2:
                        //장비 장착
                        ShowEquipmentWindow(player);
                        break;
                    case 3:
                        //가챠
                        ShowGachaWindow(player);
                        break;
                    case 4:
                        //상점
                        ShowShopWindow(player);
                        break;
                    case 5:
                        //전투
                        ShowBattleWindow(player);
                        break;
                    default:
                        Console.WriteLine("1~5까지 입력해주세요");
                        break;
                }

            }
        }
        public static void ShowBattleWindow(Player player)
        {
            //▼스킬 초기설정
            BattleSystem battle = new BattleSystem();
            SkillInfo[] skillArray = new SkillInfo[2];
            skillArray[0] = player.CharacterSkill.GetSkill(1); //스킬 슬롯 2개 채우기
            skillArray[1] = player.CharacterSkill.GetSkill(1); //스킬 슬롯 2개 채우기
            //▼적 초기설정 (무조건 공격만 하도록)
            Character enemy1 = new Enemy("마히스", 30, 10);
            SkillParent enemySkillOne = new SkillOne("사선베기", 3, 2, eSkillDamageType.Damage);
            //▼전투
            Console.WriteLine($"전투 입장! 사용할 스킬을 선택해 주세요.");
            int turn = 1;
            while (true)
            {
                Console.WriteLine($"======================{turn}번째 턴======================");
                Console.WriteLine($"현재 나의 체력: {player.CurrentHp} / {player.MaxHp}");
                Console.WriteLine($"현재 적의 체력: {enemy1.CurrentHp} / {enemy1.MaxHp}");
                Console.WriteLine($"1번스킬: {skillArray[0].skillName} 2번스킬: {skillArray[1].skillName}");
                turn++;

                int skillIndex;
                SkillInfo selectdSkill = null;
                while (selectdSkill == null)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("선택할 스킬: ");
                    if (int.TryParse(Console.ReadLine(), out skillIndex))
                    {

                        if (skillIndex == 1 || skillIndex == 2)
                        {
                            selectdSkill = skillArray[skillIndex - 1]; //선택한 스킬 정보 저장
                            skillArray[skillIndex - 1] = player.CharacterSkill.GetSkill(1); //스킬을 1개 더 빼서 저장해둠
                        }
                    }
                }
                Console.ResetColor();

                //플레이어 스킬사용
                //만약에 힐스킬이라면 합을 치면 안됨
                int playerPower = 0;
                int enemyPower = 0;
                if (selectdSkill.eCurSkillDamageType == eSkillDamageType.Damage)
                {
                    playerPower = battle.GetSkillPower(player, enemy1, selectdSkill);
                }
                else if (selectdSkill.eCurSkillDamageType == eSkillDamageType.Heal)
                {
                    playerPower = battle.GetSkillPower(player, player, selectdSkill);
                }

                //적 스킬사용
                enemyPower = battle.GetSkillPower(enemy1, player, enemySkillOne);

                //합 시스템 (둘다 공격스킬일 경우 강한 스킬만 실행한다)
                if(selectdSkill.eCurSkillDamageType == eSkillDamageType.Damage && enemySkillOne.eCurSkillDamageType == eSkillDamageType.Damage)
                {
                    //무승부일때는 무효라고 가정
                    if (playerPower > enemyPower) battle.UseSkill(player, enemy1, selectdSkill, playerPower);
                    else if (playerPower < enemyPower) battle.UseSkill(enemy1, player, enemySkillOne, enemyPower);
                }
                else
                {   //힐일경우 합을 치지않고 그대로 진행
                    battle.UseSkill(player, player, selectdSkill, playerPower);
                    battle.UseSkill(enemy1, player, enemySkillOne, enemyPower);
                }


                if (enemy1.CurrentHp <= 0)
                {
                    Console.WriteLine("적을 처치했습니다!! 0을 입력시 메인메뉴로 돌아갑니다!");
                    Console.WriteLine($"처치보상: 500 Gold, Level Up!");
                    Console.WriteLine($"현재 플레이어 레벨 :{player.Level}, 소지금: {player.Money}");

                    while (Console.ReadLine() != "0") { }
                    return;
                }
            }
            
        }

        public static void ShowInventoryWindow(Player player)
        {
            while(true)
            {
                Console.Clear();
                player.PrintInventoryItem();
                //Console.WriteLine("버릴 아이템이 있다면 해당 숫자를,");
                Console.WriteLine("이전 화면으로 돌아가시려면 0번을 입력해주세요");
                int index = 0;
                int.TryParse(Console.ReadLine(), out index);
                if (index == 0) break;
            }
        }
        public static void ShowGachaWindow(Player player)
        {
            Console.Clear();
            Console.WriteLine("===================================================");
            Console.WriteLine("오늘의 운세는 어떠신가요? 상점보다 매우 저렴한 뽑기로 행운을 노려보세요!");
            Console.WriteLine($"몇회 뽑으시겠습니까? 1회 100 Gold 입니다! 소지금:{player.Money}");
            int count;
            if(int.TryParse(Console.ReadLine(), out count))
            {
                Gacha gacha = new Gacha();
                List<Item> gachaItem = gacha.DoGacha((player as Player), count);
                //Inventory inventory = new Inventory();
                if (gachaItem != null)
                {
                    (player as Player)?.AddInventoryItem(gachaItem);
                    (player as Player)?.PrintInventoryItem();
                    Console.WriteLine("가챠 완료! 0을 입력하시면 메인메뉴로 돌아갑니다");
                    while(true)
                    {
                        if (Console.ReadLine() == "0") break;
                    }
                }
                else
                {
                    Console.WriteLine("금액부족으로 인해 3초후 메인화면 이동");
                    Thread.Sleep(3000);
                }
            }
        }

        public static void ShowEquipmentWindow(Player player)
        {
            //Equipment equipment = new Equipment();
            while(true)
            {
                Console.Clear();
                player.Equipment.PrintEquipment();
                Console.WriteLine("===================================================");
                player.PrintInventoryItem();
                Console.WriteLine("===================================================");
                Console.WriteLine("어떤 장비를 장착하시겠습니까? -> 이름 입력");
                Console.WriteLine("메인으로 돌아가려면 0을 입력해주세요");
                string equipMentName = Console.ReadLine();

                if (equipMentName == "0") break;
                else
                {
                    Item itemBuf = player?.GetInventoryItem(equipMentName);
                    if (itemBuf != null)
                    {
                        player.Equipment.SetEquipmentDic(itemBuf);
                    }
                }
            }
        }

        public static void ShowShopWindow(Player player)
        {
            Shop shop = new Shop();
            while(true)
            {
                Console.Clear();
                Console.WriteLine("===================================================");
                Console.WriteLine($"상점에 오신걸 환영합니다!! 무엇을 원하시나요? 소지금: {player.Money}");
                Console.WriteLine("1. 구매 2. 판매  0. 메인으로 돌아가기");
                int choice;
                
                
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    if (choice == 1) //구매
                    {
                        int buyItemIndex;
                        Console.WriteLine("상점에는 이런 물건이 있습니다! 구매할 물건의 이름을 입력해주세요");
                        Console.WriteLine($"소지금: {player.Money}");
                        shop.ShowItems();
                        if(int.TryParse(Console.ReadLine(), out buyItemIndex))
                        {
                            shop.BuyItem(player, buyItemIndex); //구매
                            Console.WriteLine("구매 완료!");
                            Thread.Sleep(2000);
                        }
                    }
                    else if (choice == 2) //판매
                    {
                        string sellItemName;
                        Console.WriteLine("어떤 물건을 파시겠습니까? 상점에서 파는 물건이라면 절반 가격으로 구매합니다!");
                        Console.WriteLine("아니라면 등급별로 다른 금액으로 판매 가능합니다");
                        Console.WriteLine($"C등급 가격: {shop.GetItemPrice(eItemGrade.C)} 등급이 상승할때마다 500원씩 증가");

                        shop.ShowItems();
                        sellItemName = Console.ReadLine();
                        shop.SellItem(player, sellItemName); //판매
                    }
                    else if (choice == 0)
                    {
                        break; // 돌아가기
                    }
                }
            }
        }


        static void Main(string[] args)
        {
            //##스킬 시스템
            SkillParent skillParent1 = new SkillOne("얕은베기", 5, 2, eSkillDamageType.Damage);
            SkillParent skillParent2 = new SkillTwo("명상", 7, 2, eSkillDamageType.Heal);
            SkillParent skillParent3 = new SkillThree("약점간파", 9, 3, eSkillDamageType.Damage);
            SkillInfo[] skills = new SkillInfo[3];
            skills[0] = skillParent1;
            skills[1] = skillParent2;
            skills[2] = skillParent3;
            SkillManager sm = new SkillManager();
            sm.SetSkill(skills);
            var k = sm.GetSkill(1); //k를 얻어왔으니 k를 쓰면됨
            
            Character player1 = new Player("히스클리프", 100, 5);
            player1.SetPlayerSkills(sm);

            
            //sm.SetBasicSkill();

            //##메뉴 시작.

            ShowMainWindow(player1 as Player);




            //##가챠 + 인벤토리 시스템
            //(완료)가챠 구조 확인 및 인벤토리에 가챠 어떻게 넣을것인지,
            //(완료)팩토리 패턴 쓸것인지?, 팩토리패턴에서 생성자 추가
            //(완료)가챠하고나서 등급 , 등급별 색상 뽑기에 콘솔로 출력
            //(완료2)Print할때 등급별로 정렬되서 Print되도록 하기 + 이름 추가해서
            Gacha gacha = new Gacha();
            List<Item> gachaItem = gacha.DoGacha((player1 as Player),100);
            //Inventory inventory = new Inventory();
            if(player1 is Player)
            {
                (player1 as Player)?.AddInventoryItem(gachaItem);
                (player1 as Player)?.PrintInventoryItem();
            }
            //inventory.AddItems(gachaItem);
            //inventory.PrintInventory(); //해당메서드 삭제 예정
            //inventory.PrintInventoryBuf();

            //## 장비 시스템
            //인벤토리에서 뽑은 아이템을 장착.
            //(완료)List하나 추가.인벤토리에 현재 아이템이 없고 아이템 갯수만 가지고있어서 문제
            //(완료)등급과 장비타입이 뽑혔을때 해당하는 등급과타입인 아이템 배열에서 또 랜덤하게 선택.
            //가챠클래스에서 InitGachaItem()를 Json데이터를 읽어오는 방법 고려해보기.. 코드가 너무 김
            Equipment equipment = new Equipment();
            equipment.SetEquipmentDic((player1 as Player)?.GetInventoryItem(0));
            equipment.SetEquipmentDic((player1 as Player)?.GetInventoryItem(2));
            equipment.SetEquipmentDic((player1 as Player)?.GetInventoryItem(4));
            equipment.PrintEquipment();


            //##상점 시스템
            //그냥 가챠에서 뽑지말고 상점에서 구매할수 있도록
            //(완료)데이터베이스 클래스 만들어서 아이템 정보를 담기(싱글톤으로)
            //(완료)데이터베이스 클래스에서 상점에서 팔 아이템 골라서 넣기
            //(완료)구매,판매 기능 넣기
            Shop shop = new Shop();
            if(player1 is Player)
            {
                //shop.BuyItem(player1 as Player, "녹슨갑옷"); //구매
            }
            if(player1 is Player)
            {
                shop.SellItem(player1 as Player, "낡은목걸이"); //판매
            }
            //## 메뉴를 만든다
            //장비, 상점, 가챠, 등


            //SetCursor(); //선택지 선택

        }

        

        public static void SetCursor()
        {
            Console.Clear();
            int x = 20;
            int y = 5;
            Console.SetCursorPosition(x, y);
            Console.Write("▶");

            while(true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                Console.SetCursorPosition(x, y);
                Console.Write(" ");

                switch(keyInfo.Key)
                {
                    case ConsoleKey.LeftArrow:
                        x -= 20;
                        break;
                    case ConsoleKey.RightArrow:
                        x += 20;
                        break;
                    case ConsoleKey.UpArrow:
                        y -= 1;
                        break;
                    case ConsoleKey.DownArrow:
                        y += 1;
                        break;
                    case ConsoleKey.Enter:
                        Console.SetCursorPosition(0, 10);
                        Console.WriteLine($"선택위치: x:{x},y:{y}");
                        break;
                }
                if (x < 0) x = 0;
                else if (x >= Console.BufferWidth) x = Console.BufferWidth - 1;
                if (y < 0) y = 0;
                else if (y >= Console.BufferHeight) y = Console.BufferHeight - 1;
                Console.SetCursorPosition(x, y);
                Console.Write("▶");
            }

            /*
            if (x - 1 > 0) x -= 1;
            Console.Write(" ");

            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
            keyInfo = Console.ReadKey();
            switch (keyInfo.Key)
            {
                case ConsoleKey.LeftArrow:
                    //Console.SetCursorPosition(x - 1, y);
                    //x -= 1;
                    x -= 20;
                    break;
                case ConsoleKey.RightArrow:
                    //x += 1;
                    x += 20;
                    break;
                case ConsoleKey.UpArrow:
                    y -= 1;
                    break;
                case ConsoleKey.DownArrow:
                    y += 1;
                    break;
            }
            //x축이 120이라면? 양옆 30씩 빼서 60이라고 침
            //20,40,60 위치에 하나씩 둠
           
            Console.SetCursorPosition(x, y);
            Console.Write("▶");
            */
        }
    }

}
