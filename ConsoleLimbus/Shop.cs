using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLimbus
{
    class ShopItem
    {
        public int price { get; }
        public Item item { get; }
        public ShopItem(Item _item, int _price)
        {
            item = _item;
            price = _price;
        }
    }
    class Shop
    {
        //물품마다 구매,판매 가격을 어디에 둘지 생각
        //Shop에 두기로함 ( 이유?: 상점마다 가격이 다르거나 이벤트로 인해 할인한다면 Shop에서 관리하는게 좋음)
        //List<Item> shopItems = new List<Item>(); //리스트 사용안함(이유: 물품마다 가격이 달라야하므로 Dictionary로 변경)
        //Dictionary<Item, int> dic;  //딕셔너리로 할까 하다가 Item이 참조형클래스라 키값으로는 맞지않는듯
        Dictionary<string, ShopItem> shopItemsDic = new Dictionary<string, ShopItem>();

        public Shop()
        {
            InitShop();
        }

        //▼구현 필요항목
        //상점에 파는 아이템 목록 리스트
        //구매시 인벤토리에 추가
        //판매시 돈 추가 (반값에)
        public void InitShop()
        {
            //▼상점에서 파는 아이템 이름과 가격
            List<(string,int)> ShopData = new List<(string, int)>() 
            {
                ("녹슨검", 1000),
                ("녹슨갑옷", 2000),
                ("낡은반지", 500),
                ("삼위일체", 10000),
                ("가시갑옷", 8000),
                ("무효화반지", 6000)
            };
            //▼ foreach로 shopItem리스트에 추가
            foreach (var item in ShopData)
            {
                Item itemBuf = ItemDatabase.Instance.GetItem(item.Item1);
                shopItemsDic[item.Item1] = new ShopItem(itemBuf, item.Item2);
            }
        }
        public void ShowItems()
        {
            Console.WriteLine("======아이템 목록======");
            int index = 1;
            foreach(var item in shopItemsDic)
            {
                Console.WriteLine($"{index}. 이름: {item.Key} 가격: {item.Value.price}");
                index++;
            }
            Console.WriteLine("======================");
        }
        public void BuyItem(Player player, string itemName)
        {
            if(!shopItemsDic.ContainsKey(itemName))
            {
                Console.WriteLine("해당 물품이 없습니다");
                return;
            }
            var itemBuf = shopItemsDic[itemName];
            if(player.Money >= itemBuf.price) //물품 구매 가능
            {
                player.SetMoney(-itemBuf.price); //돈 차감
                player.AddInventoryItem(itemBuf.item); //인벤토리에 넣기
                
            }
            else { Console.WriteLine("돈이 부족합니다"); }
        }
        
    }
}
