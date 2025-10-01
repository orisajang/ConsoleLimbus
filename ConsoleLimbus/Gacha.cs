using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLimbus
{
    class Gacha
    {
        //뽑기로 장비를 얻고 캐릭터가 장비를 장착. 초기 시작시 100뽑 지행
        Random rnd = new Random();

        Dictionary<eItemGrade, int> itemPickPercent = new Dictionary<eItemGrade, int>()
        {
            {eItemGrade.C,65 },
            {eItemGrade.B,20 },
            {eItemGrade.A,10 },
            {eItemGrade.S,4 },
            {eItemGrade.SS,1 }
        };

        public eItemGrade GetRandomGrade() //랜덤한 아이템 등급 얻기
        {
            int percent = rnd.Next(0, 100); //0~99;
            int sum = 0;

            foreach(var item in itemPickPercent)
            {
                sum += item.Value;
                if(percent < sum)
                {
                    return item.Key;
                }
            }

            throw new Exception("뭔가 잘못됨. 여기 오면 안됨");
            return eItemGrade.C;
        }
        public eEquipment GetRandomEquipType() //랜덤한 장비타입 얻기
        {
            //Weapon, Armor, Accessory중 하나를 얻음
            Array array = Enum.GetValues(typeof(eEquipment));  //Enum의 value를 배열형태로
            int randomNum = rnd.Next(0, array.Length); //랜덤숫자 0~ 3
            return (eEquipment)array.GetValue(randomNum);
        }

        public void DoGacha(int count)
        {
            //count가 뽑기 횟수
            for(int i=0; i< count; i++)
            {
                eEquipment eRandomEquipType = GetRandomEquipType();
                eItemGrade eRandomGrade = GetRandomGrade();

                if(eRandomGrade == eItemGrade.C)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (eRandomGrade == eItemGrade.B)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else if (eRandomGrade == eItemGrade.A)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                }
                else if (eRandomGrade == eItemGrade.S)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (eRandomGrade == eItemGrade.SS)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                Console.WriteLine($"{eRandomGrade}등급의 {eRandomEquipType}를 얻었습니다!!");
                Console.ResetColor();
            }
            


        }
    }
    enum eItemGrade
    {
        C,B,A,S,SS
    }
    enum eEquipment
    {
        Weapon, Armor, Accessory
    }
    abstract class Item
    {
        eItemGrade eGradeType; //아이템 등급
        int value; //효과. 아이템별로 다름

        public abstract eEquipment eEquipType { get; }
    }
    class WeaponItem :Item
    {
        public override eEquipment eEquipType { get; } = eEquipment.Weapon; //장비타입
    }
    class ArmorItem : Item
    {
        public override eEquipment eEquipType { get;} = eEquipment.Armor; //장비타입
    }
    class AccessoryItem : Item
    {
        public override eEquipment eEquipType { get; } = eEquipment.Accessory; //장비타입
    }
    class Inventory
    {
        List<Item> items = new List<Item>();
    }
    class CharacterEquipment
    {
        //캐릭터가 장착한 것에 대한 정보
        Dictionary<eEquipment, Item> equipMentDic = new Dictionary<eEquipment, Item>();
    }
}
