using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLimbus
{
    class Equipment
    {
        //장착부위 (enum)으로 확인
        //어떤 장비를 착용하고있는지 확인 (Print)
        //장비 장착 창(인벤토리와 연동해서 어떤 장비를 가지고 있는지
        Dictionary<eItemType, Item> equipmentDic = new Dictionary<eItemType, Item>();

        public Equipment()
        {
            InitEquipmentDic(); //딕셔너리에 enum순서대로 정렬되어 추가되도록 초기값 생성
        }
        public void InitEquipmentDic() //장비 초기화
        {
            var k = Enum.GetValues(typeof(eItemType));
            foreach (eItemType item in k)
            {
                ItemFactory itemFactory = new ItemFactory();
                Item itemBuf = itemFactory.Create(item, eItemGrade.C,"초기장비",0);
                equipmentDic.Add(item, itemBuf);
            }
        }
        public void SetEquipmentDic(Item item) //장비 장착
        {
            //어떤 아이템이 들어왔는지 어떻게 알지? -> 각각 들어있음 itemType이
            equipmentDic[item.itemType] = item;
        }
        public void PrintEquipment() //현재 가지고있는 장비 출력
        {
            Console.WriteLine("======플레이어 장착 장비======");
            var k = Enum.GetValues(typeof(eItemType));
            foreach(eItemType item in k)
            {
                Item itemBuf = equipmentDic[item];
                Console.WriteLine($"{itemBuf.itemType}은 {itemBuf.itemGrade} 등급 {itemBuf.name}를 장착중입니다");
            }
        }
    }
}
