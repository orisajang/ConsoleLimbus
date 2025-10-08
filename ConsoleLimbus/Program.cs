using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Threading;

namespace ConsoleLimbus
{
    abstract class Character
    {
        protected string name;
        private int maxHP;
        protected int currentHp;

        protected int mentality;
        protected int stagger;
        protected int speed;
        protected int level;
        protected int money;
        protected SkillManager characterSkill = new SkillManager();

        public int Mentality => mentality;
        public int CurrentHp => currentHp;
        public string Name => name;
        public SkillManager CharacterSkill => characterSkill;
        public Character(string _name, int _maxHp, int _level)
        {
            name = _name;
            maxHP = _maxHp + (_level * 2);  //1레벨 hp + level * hp상승량
            currentHp = maxHP; //초기 hp량
            mentality = 0; //초기 0
        }
        public void TakeDamage(int damage)
        {
            currentHp = Math.Max(0, currentHp - damage); //체력이 0이하로 떨어지지않게
            Console.WriteLine($"{Name}가 {damage}데미지 받음! 남은 hp:{currentHp}");
        }
        public void Healing(int amount)
        {
            int sumHpValue = currentHp + amount;
            currentHp = (sumHpValue > maxHP) ? maxHP : sumHpValue; //MaxHp보다 더한값이 크다면 max값 적용
            Console.WriteLine($"{Name}가 {amount}힐 받음! 남은 hp:{currentHp}");
        }

        public void SetPlayerSkills(SkillManager sm)
        {
            characterSkill = sm;
        }
    }
    class Enemy : Character
    {
        public Enemy(string _name, int _maxHp, int _level) : base(_name, _maxHp, _level)
        {
        }
    }
    class Player : Character
    {
        public int Money { get { return money; } }
        Inventory inventory = new Inventory();
        Equipment equipment = new Equipment();

        public Equipment Equipment { get { return equipment; } }

        public Player(string _name, int _maxHp, int _level) : base(_name, _maxHp, _level)
        {
            money = 10000; //초기 돈
        }
        public bool SetMoney(int value)
        {
            if(money + value < 0)
            {
                Console.WriteLine("금액부족!!");
                return false; //음수인경우 false
            }
            money += value;
            return true;
        }
        public void SetName(string _name)
        {
            name = _name;
        }
        public void AddInventoryItem(Item _item) //메서드 오버로딩1
        {
            ItemFactory itemFactory = new ItemFactory();
            Item itemBuf = itemFactory.Create(_item.itemType, _item.itemGrade, _item.name, _item.value);
            List<Item> itemListBuf = new List<Item>();
            itemListBuf.Add(itemBuf);
            inventory.AddItems(itemListBuf);
        }
        public void AddInventoryItem(List<Item> _items) //메서드 오버로딩2
        {
            inventory.AddItems(_items); //이 함수 쓰기전에 _items가 얕은복사여도 괜찮은지 확인해야함
        }
        public void DeleteInventoryItem(Item _item)
        {
            inventory.DeleteItem(_item);
        }
        public bool IsPlayerHaveItem(Item _item)
        {
            return inventory.HaveItem(_item);
        }
        public Item GetInventoryItem(string _name)
        {
            return inventory.GetItem(_name);
        }
        public Item GetInventoryItem(int index)
        {
            return inventory.GetItem(index);
        }
        public void PrintInventoryItem()
        {
            inventory.PrintInventoryBuf();
        }
       
    }

    class BattleSystem
    {
        Random rnd = new Random();
        public void UseSkill(Character character1, Character targetCharacter, SkillInfo skill)
        {
            //플레이어1가 target플레이어한테 스킬 값만큼 힐을주거나, 데미지를 줌 (target에 자기자신줘서 자신데미지, 자신힐도 가능)
            Console.WriteLine($"{character1.Name}");

            int mental = character1.Mentality; //정신력 -45~ 45까지
            //정신력이 0이라면? 50퍼 , 정신력이 -45라면? 5퍼, 정신력이 45라면 95퍼센트 확률로 
            int percent = 50 + character1.Mentality;// 5, 95



            //코인 앞뒤면 체크
            int coinFrontCount = 1; //코인 앞면 나온 횟수,  1부터 시작, 1개증가할때마다 N배가됨
            for (int i = 0; i < skill.coinCount; i++)
            {
                if (rnd.Next(0, 100) < percent) coinFrontCount++;
            }
            int amount = skill.power * coinFrontCount; //coinFrontCount가 0일떄 어떻게하지?
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

        }
    }

    enum eSkillType
    {
        SkillOne, SkillTwo, SkillThree
    }
    enum eSkillDamageType
    {
        Damage, Heal
    }
    class SkillManager
    {
        List<SkillInfo> basicSkillList = new List<SkillInfo>(); //스킬6개중에서 랜덤으로 1개씩 빠짐.(추가/삭제가 빈번하다) -> LinkedList
        Dictionary<eSkillType, SkillInfo> skillDic = new Dictionary<eSkillType, SkillInfo>(); //스킬타입과 스킬에 대한 정보
        SkillInfo[] skillSlots = new SkillInfo[2]; //스킬슬롯 2개
        Random rnd = new Random();

        public SkillInfo GetSkill(int inputSkillIndex)
        {
            int useSkillIndex = inputSkillIndex - 1; //실제 배열에서 쓰는 index로 변경
            //useSkillIndex 로 들어오는값 => 1,2,  0 1 기준으로
            //예외처리
            if (useSkillIndex < 0 || useSkillIndex >= skillSlots.Length)
            {
                Console.WriteLine("에러. 스킬선택은 1이나 2여야합니다");
                return null;
            }
            //스킬 슬롯에 있는 스킬 사용
            if (skillSlots[useSkillIndex] == null)
            {
                Console.WriteLine("스킬이 없어요. 로직이 뭔가 잘못됨.");
                return null;
            }

            //선택한 스킬 사용
            SkillInfo skillBuf = skillSlots[useSkillIndex];
            skillSlots[useSkillIndex] = null;

            SetSkillSlot(); //사용하고 바로 다시 Skill Set

            return skillBuf;
        }
        private void SetSkillSlot() //자동으로 채울것이기때문에 private
        {
            //스킬슬롯은 2개가 있고, 초기에는 6개의 스킬중 2개를 랜덤으로 뽑아서 슬롯2개에 넣는다.
            //스킬1개를 사용했다면 남은 스킬중에서 1개를 랜덤으로 뽑는다.
            //6개를 모두 다 사용했다면? 다시 SetBasicSkill로 스킬6개를 채운다
            for (int i = 0; i < skillSlots.Length; i++)
            {
                if (skillSlots[i] == null) //null이면 추가 (사용해서 null이거나 초기null이면)
                {
                    //자료구조 탐색
                    //2. 링크드 리스트로 (삭제는 빠른데 인덱스 접근이 느림)
                    //3. 딕셔너리로 enum에 스킬 갯수를 count해서 접근 
                    //  - 어려움: 이유) 딕셔너리로 value을 for문돌면서 갯수세기
                    //  - 어려움2: 이유) 랜덤int값이 나왔을때 특정enum을 가중치에 따라 선택하고 value값 --; (value값이 0인지도 확인해야함)

                    //1. 리스트로 (삭제가 느리지만 최대6개니까 그냥 사용하기로함)
                    if (basicSkillList.Count == 0) SetBasicSkill(); //다시 채움

                    int skillRemain = basicSkillList.Count; //남은 스킬 갯수
                    int rndNumber = rnd.Next(0, skillRemain); //배열은 0~5 가 6개이므로 max에 +1안해도됨
                    var k = basicSkillList[rndNumber];
                    skillSlots[i] = k;
                    basicSkillList.Remove(k);
                }
            }
        }
        public void SetSkill(SkillInfo[] skills)
        {
            //스킬1, 스킬2, 스킬3의 정보가 Set된다.
            for (int i = 0; i < 3; i++)
            {
                skillDic.Add(skills[i].eCurSkillType, skills[i]);
            }
            SetBasicSkill(); //스킬1, 스킬2, 스킬3이 몇개씩 들어가 있어야하는지 정보 설정
            SetSkillSlot(); //스킬슬롯 2개 
        }
        public void SetBasicSkill()
        {
            //skillDic 에 있는 정보를 이용하여 List에 Set
            /*
            //아래 주석친 방법대로 하려다가 foreach로 중복코드 제거 있어서 사용
            SkillInfo skill_1 = skillDic[eSkillType.SkillOne];
            SkillInfo skill_2 = skillDic[eSkillType.SkillTwo];
            SkillInfo skill_3 = skillDic[eSkillType.SkillThree];
            for (int i = 0; i<skill_1.skillCount; i++)
            {
                basicSkillList.Add(skill_1);
            }
            for(int i=0; i<skill_2.skillCount; i++)
            {
                basicSkillList.Add(skill_2);
            }
            for(int i=0; i<skill_3.skillCount; i++)
            {
                basicSkillList.Add(skill_3);
            }
            */
            foreach (var item in skillDic.Values) //skillDic에 저장되어있는 스킬갯수만큼
            {
                for (int i = 0; i < item.skillCount; i++)
                {
                    basicSkillList.Add(item); //실제 스킬List에 추가
                }
            }
        }
    }
    interface SkillInfo
    {
        string skillName { get; }
        int power { get; }
        int coinCount { get; }
        eSkillType eCurSkillType { get; }
        eSkillDamageType eCurSkillDamageType { get; }
        int skillCount { get; }
    }
    abstract class SkillParent : SkillInfo
    {
        public string skillName { get; }
        public int power { get; }
        public int coinCount { get; }
        public eSkillDamageType eCurSkillDamageType { get; }
        public abstract eSkillType eCurSkillType { get; }
        public abstract int skillCount { get; }
        public SkillParent(string _skillName, int _power, int _coinCount, eSkillDamageType _eSkillDamageType)
        {
            skillName = _skillName;
            power = _power;
            coinCount = _coinCount;
            eCurSkillDamageType = _eSkillDamageType;
        }
    }
    class SkillOne : SkillParent
    {
        public override eSkillType eCurSkillType { get { return eSkillType.SkillOne; } }
        public override int skillCount { get { return 3; } }
        public SkillOne(string _skillName, int _power, int _coinCount, eSkillDamageType _eSkillDamageType)
            : base(_skillName, _power, _coinCount, _eSkillDamageType)
        {
        }
    }
    class SkillTwo : SkillParent
    {
        public override eSkillType eCurSkillType { get { return eSkillType.SkillTwo; } }
        public override int skillCount { get { return 2; } }
        public SkillTwo(string _skillName, int _power, int _coinCount, eSkillDamageType _eSkillDamageType)
            : base(_skillName, _power, _coinCount, _eSkillDamageType) { }
    }
    class SkillThree : SkillParent
    {
        public override eSkillType eCurSkillType { get { return eSkillType.SkillThree; } }
        public override int skillCount { get { return 1; } }
        public SkillThree(string _skillName, int _power, int _coinCount, eSkillDamageType _eSkillDamageType)
            : base(_skillName, _power, _coinCount, _eSkillDamageType) { }
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
                        break;
                    default:
                        Console.WriteLine("1~5까지 입력해주세요");
                        break;
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
            Console.WriteLine("===================================================");
            Console.WriteLine("오늘의 운세는 어떠신가요? 상점보다 매우 저렴한 뽑기로 행운을 노려보세요!");
            Console.WriteLine($"몇회 뽑으시겠습니까? 1회 100 Gold 입니다! 소지금:{player.Money}");
            int count;
            if(int.TryParse(Console.ReadLine(), out count))
            {
                Gacha gacha = new Gacha();
                List<Item> gachaItem = gacha.DoGacha((player as Player), count);
                //Inventory inventory = new Inventory();
                if (player is Player)
                {
                    (player as Player)?.AddInventoryItem(gachaItem);
                    (player as Player)?.PrintInventoryItem();
                    Console.WriteLine("가챠 완료! 0을 입력하시면 메인메뉴로 돌아갑니다");
                    while(true)
                    {
                        if (Console.ReadLine() == "0") break;
                    }
                }
            }
        }

        public static void ShowEquipmentWindow(Player player)
        {
            //Equipment equipment = new Equipment();
            while(true)
            {
                Console.Clear();
                Console.WriteLine("===================================================");
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
            BattleSystem battle = new BattleSystem();
            Character player1 = new Player("히스클리프", 100, 5);
            Character enemy1 = new Enemy("마히스", 30, 10);
            player1.SetPlayerSkills(sm);

            var k2 = player1.CharacterSkill.GetSkill(1); //플레이어 입력으로 스킬슬롯1이나 스킬슬롯2를 Console.ReadLine()으로 선택
            battle.UseSkill(player1, enemy1, k2);
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
