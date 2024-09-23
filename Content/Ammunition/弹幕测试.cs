using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace FKsCRE.Content.Ammunition
{
    public class 弹幕测试 : 子弹
    {
        public override void SetDefaults()
        {
            Item.shoot = ModContent.ProjectileType<弹幕测试_Proje>();
            base.SetDefaults();
        }
    }
}
