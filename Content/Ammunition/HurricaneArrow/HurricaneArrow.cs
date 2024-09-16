using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NanTing.Content.Ammunition.JuZhiShi
{
    /// <summary>
    /// 飓风箭 -> 天蓝箭
    /// </summary>
    public class HurricaneArrow : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 7;
            //箭矢
            Item.ammo = AmmoID.Arrow;
            Item.shoot = ModContent.ProjectileType<HurricaneArrow_Proje>();
            base.SetDefaults();
        }
    }

    public class HurricaneArrow_Proje : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.damage = 5;
            Projectile.friendly = true;
            Projectile.aiStyle = 1;
            base.SetDefaults();
        }

        public override void AI()
        {
            Vector2 MouseVectorWorld = Main.MouseWorld;
            Vector2 PlayerVectorWorld = Main.player[Projectile.owner].Center;

            base.AI();
        }

        public override void OnKill(int timeLeft)
        {
            //Projectile.NewProjectile(default,)
            base.OnKill(timeLeft);
        }
    }

}
