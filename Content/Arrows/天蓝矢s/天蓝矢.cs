using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace FKsCRE.Content.Arrows.天蓝矢s
{
    public class 天蓝矢 : Arrow
    {
        public override void SetDefaults()
        {
            Item.shoot = ModContent.ProjectileType<天蓝矢_Proje>();
            base.SetDefaults();
        }
    }
}
