using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLimbus
{
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
}
