using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace NanTing.Content.Ammunition.JuZhiShi
{
    public class JuZhiShi : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 7;
            //箭矢
            Item.ammo = AmmoID.Arrow;
            Item.shoot = ModContent.ProjectileType<JuZhiShi_Proje>();
            base.SetDefaults();
        }
    }
}
