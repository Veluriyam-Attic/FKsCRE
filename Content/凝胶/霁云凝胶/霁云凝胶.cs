using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FKsCRE.Content.凝胶.霁云凝胶
{
    public class 霁云凝胶 : 凝胶
    {
        public override void SetDefaults()
        {
            Item.consumable = true;
            base.SetDefaults();
        }
        //被消耗时执行（特定物品）
        public override void OnConsumedAsAmmo(Item weapon, Player player)
        {
            #region 废弃代码 2024/9/22
            //if (weapon.useAmmo == AmmoID.Gel)
            //{
            //    for (int i = 0; i < Main.projectile.Length - 2; i++)
            //    {
            //        if (Main.projectile[i].owner == Main.myPlayer)
            //        {
            //            被附魔弹幕 bl = Main.projectile[i].GetGlobalProjectile<被附魔弹幕>();
            //            bl.霁云凝胶_是否被附魔 = true;
            //            break;
            //        }
            //    }
            //}
            #endregion
            base.OnConsumedAsAmmo(weapon, player);
        }
        public override bool ConsumeItem(Player player)
        {
            return true;
        }
    }


}
