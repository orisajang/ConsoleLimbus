using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLimbus
{
    class Character
    {
        protected string name;
        protected int hp;
        protected int mentality;
        protected int stagger;
        protected int speed;
        protected int level;
        protected SkillManager skillSlot = new SkillManager();

        public int Hp => hp;
        public void TakeDamage(int damage)
        {
            hp = Math.Max(0, hp - damage); //체력이 0이하로 떨어지지않게
        }
    }
    class Enemy : Character
    {

    }
    class Player : Character
    {

    }

    class BattleSystem
    {
        public void UseSkill(Character character1, Character character2, SkillInfo skill )
        {
            //플레이어1가 플레이어2한테 스킬 값만큼 힐을주거나, 데미지를 줌
            //character1.hp; //접근불가.. protected
        }
    }

    enum eSkillType
    {
        SkillOne, SkillTwo, SkillThree
    }
    class SkillManager
    {
        List<SkillInfo> basicSkillList = new List<SkillInfo>(); //스킬6개중에서 랜덤으로 1개씩 빠짐.(추가/삭제가 빈번하다) -> LinkedList
        Dictionary<eSkillType, SkillInfo> skillDic = new Dictionary<eSkillType, SkillInfo>();
        Dictionary<eSkillType, int> skillCount = new Dictionary<eSkillType, int>();
        SkillInfo[] skillSlots = new SkillInfo[2]; //스킬슬롯 2개
        Random rnd = new Random();

        public SkillInfo GetSkill(int inputSkillIndex)
        {
            int useSkillIndex = inputSkillIndex - 1; //실제 배열에서 쓰는 index로 변경
            //useSkillIndex 로 들어오는값 => 1,2,  0 1 기준으로
            //예외처리
            if(useSkillIndex < 0 || useSkillIndex >= skillSlots.Length)
            {
                Console.WriteLine("에러. 스킬선택은 1이나 2여야합니다");
                return null;
            }
            //스킬 슬롯에 있는 스킬 사용
            if (skillSlots[useSkillIndex] == null)
            {
                Console.WriteLine("스킬이 없어요. 로직이 뭔가 잘못됨."); 
            }

            //선택한 스킬 사용
            //??? 어떻게 쓰지? return으로 정보 넘겨주면되나
            int arrayIndex = useSkillIndex - 1;
            SkillInfo skillBuf = skillSlots[useSkillIndex];
            skillSlots[useSkillIndex] = null;
            
            SetSkillSlot(); //사용하고 바로 다시 Skill Set

            return skillBuf;
        }
        public void SetSkillSlot()
        {
            //스킬슬롯은 2개가 있고, 초기에는 6개의 스킬중 2개를 랜덤으로 뽑아서 슬롯2개에 넣는다.
            //스킬1개를 사용했다면 남은 스킬중에서 1개를 랜덤으로 뽑는다.
            //6개를 모두 다 사용했다면? 다시 SetBasicSkill로 스킬6개를 채운다
            for(int i=0; i< skillSlots.Length; i++)
            {
                if (skillSlots[i] == null) //null이면 추가 (사용해서 null이거나 초기null이면)
                {
                    //자료구조 탐색
                    //2. 링크드 리스트로 (삭제는 빠른데 인덱스 접근이 느림)
                    //3. 딕셔너리로 enum에 스킬 갯수를 count해서 접근 
                    //  - 어려움: 이유) 딕셔너리로 value을 for문돌면서 갯수세기
                    //  - 어려움2: 이유) 랜덤int값이 나왔을때 특정enum을 가중치에 따라 선택하고 value값 --; (value값이 0인지도 확인해야함)

                    //1. 리스트로 (삭제가 느리지만 최대6개니까 그냥 사용하기로함)
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
            //스킬1, 스킬2, 스킬3이 몇개씩 들어가 있어야하는지 정보 설정
            SetBasicSkill();

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
            foreach(var item in skillDic.Values) //skillDic에 저장되어있는 스킬갯수만큼
            {
                for(int i=0; i<item.skillCount; i++)
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
        int skillCount { get; }
    }
    abstract class SkillParent : SkillInfo
    {
        public string skillName { get; }
        public int power { get; }
        public int coinCount { get; }
        public virtual eSkillType eCurSkillType { get { return eSkillType.SkillOne; } }
        public virtual int skillCount { get { return 3; } }
        public SkillParent(string _skillName, int _power, int _coinCount)
        {
            skillName = _skillName;
            power = _power;
            coinCount = _coinCount;
        }
    }
    class SkillOne : SkillParent
    {
        public override eSkillType eCurSkillType { get { return eSkillType.SkillOne; } }
        public override int skillCount { get { return 3; } }
        public SkillOne(string _skillName, int _power, int _coinCount) : base(_skillName, _power, _coinCount)
        {
        }
    }
    class SkillTwo : SkillParent
    {
        public override eSkillType eCurSkillType { get { return eSkillType.SkillTwo; } }
        public override int skillCount { get { return 2; } }
        public SkillTwo(string _skillName, int _power, int _coinCount) : base(_skillName, _power, _coinCount) { }
    }
    class SkillThree : SkillParent
    {
        public override eSkillType eCurSkillType { get { return eSkillType.SkillThree; } }
        public override int skillCount { get { return 1; } }
        public SkillThree(string _skillName, int _power, int _coinCount) : base(_skillName, _power, _coinCount) { }
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
        static void Main(string[] args)
        {
            SkillParent skillParent1 = new SkillOne("얕은베기", 5, 2);
            SkillParent skillParent2 = new SkillTwo("횡베기", 7, 2);
            SkillParent skillParent3 = new SkillThree("약점간파", 9, 3);
            SkillInfo[] skills = new SkillInfo[3];
            skills[0] = skillParent1;
            skills[1] = skillParent2;
            skills[2] = skillParent3;
            SkillManager sm = new SkillManager();
            sm.SetSkill(skills);
            sm.SetSkillSlot();
            var k = sm.GetSkill(1); //k를 얻어왔으니 k를 쓰면됨
            //sm.SetBasicSkill();
        }
    }
}
