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
        protected SkillManager skillSlot = new SkillManager();
    }
    class Enemy : Character
    {

    }
    class Player : Character
    {

    }
    enum eSkillType
    {
        SkillOne, SkillTwo, SkillThree
    }
    class SkillManager
    {
        List<SkillInfo> basicSkillList = new List<SkillInfo>();
        Dictionary<eSkillType, SkillInfo> skillDic = new Dictionary<eSkillType, SkillInfo>();
        Dictionary<eSkillType, int> skillCount = new Dictionary<eSkillType, int>();
        public void UseSkill()
        {

        }
        public void SetSkill(SkillInfo[] skills)
        {
            //스킬1, 스킬2, 스킬3의 정보가 Set된다.
            for (int i = 0; i < 3; i++)
            {
                skillDic.Add(skills[i].eCurSkillType, skills[i]);
            }
            //스킬1, 스킬2, 스킬3이 몇개씩 들어가 있어야하는지 정보 설정
            for (int i = 0; i < 3; i++)
            {
                if (skills[i].eCurSkillType == eSkillType.SkillOne)
                {
                    skillCount.Add(skills[i].eCurSkillType, 3);
                }
                else if (skills[i].eCurSkillType == eSkillType.SkillTwo)
                {

                }
                else if (skills[i].eCurSkillType == eSkillType.SkillOne)
                {

                }


            }

        }
        public void SetBasicSkill()
        {
            foreach (var item in skillDic)
            {
                if (item.Key == eSkillType.SkillOne)
                {

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
        }
    }
}
