using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace NanTing.Content.Ammunition.HurricaneArrow
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
            Projectile.timeLeft = 300;
            //Projectile.aiStyle = 1;
            Projectile.ai[0] = 0f;
            //Projectile.soundDelay = 2;
            base.SetDefaults();
        }
        public override void AI()
        {
            if (Projectile.ai[0] == 0f)
            {
                Vector2 MouseVectorWorld = Main.MouseWorld;
                Vector2 vector2 = Main.MouseScreen;
                Vector2 PlayerVectorWorld = Main.player[Projectile.owner].Center;
                Projectile.velocity = Vector2.Normalize(MouseVectorWorld - PlayerVectorWorld) * 17f;
                //确保角度正确
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi / 2;
                Projectile.ai[0]++;
            }
            //Main.NewText(vector2);
            base.AI();
        }

        public override void OnKill(int timeLeft)
        {
            //播放声音
            SoundEngine.PlaySound(SoundID.Chat, Projectile.Center);
            base.OnKill(timeLeft);
        }
    }

}
