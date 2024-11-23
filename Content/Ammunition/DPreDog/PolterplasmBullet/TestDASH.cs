//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;

//namespace FKsCRE.Content.Ammunition.DPreDog.PolterplasmBullet
//{
//    public class TestDASH : ModItem
//    {
//        public override void SetStaticDefaults()
//        {

//        }

//        public override void SetDefaults()
//        {
//            Item.width = 28;
//            Item.height = 28;
//            Item.accessory = true; // 设置为饰品
//            Item.rare = ItemRarityID.Green;
//            Item.value = Item.sellPrice(gold: 1);
//        }

//        public override void UpdateAccessory(Player player, bool hideVisual)
//        {
//            var calamityPlayer = player.GetModPlayer<CalamityMod.CalPlayer.CalamityPlayer>();

//            // 将 PolterplasmBulletDASH 的 DashID 绑定到玩家
//            calamityPlayer.DashID = PolterplasmBulletDASH.ID;
//        }
//    }
//}
