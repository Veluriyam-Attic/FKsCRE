using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Build.Utilities;

namespace FKsCRE.Content.Ammunition.WulfrimBullet
{
    public class WulfrimBullet_Proje : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.damage = All.Damage;
            Projectile.knockBack = 1.0f;
            Projectile.aiStyle = ProjAIStyleID.Bubble;
            Projectile.knockBack = 0.3f;
            //10秒
            Projectile.timeLeft = 600;
            base.SetDefaults();
        }
        int num = 0;
        int 距离 = 0;
        Vector2 Mouse = default;
        Vector2 projev = default;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            #region 偏移
            /*
                     // 假设angle是给定的角度（以弧度为单位），并且已经根据需要进行了计算  
                    // vectorX 和 vectorY 是根据角度计算出的偏移向量  
                    float vectorX = (float)Math.Cos(angle);  
                    float vectorY = (float)Math.Sin(angle);  
  
                    // offset 是偏移量，你可以根据需要调整这个值  
                    float offsetX = vectorX * offset;  
                    float offsetY = vectorY * offset;  
  
                    // 将偏移量应用到弹幕的位置  
                    // 注意：这里的位置调整可能需要考虑弹幕的当前位置和速度  
                    // 假设positionX和positionY是弹幕的当前位置  
                    positionX += offsetX;  
                    positionY += offsetY;  


                            Main.NewText("玩家中心: " + player.Center);
                Mouse = Main.MouseWorld - player.Center;
                double rot = Math.Atan2(Mouse.Y,Mouse.X);
                //偏移距离
                float X = (float)Math.Sin(rot) * 20f;
                float Y = (float)Math.Cos(rot) * 20f;


             */
            #endregion
            if (num == 0)
            {
                projev = Vector2.Normalize(Main.MouseWorld - player.Center) * 12;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() -  MathHelper.Pi / 2;
            Projectile.velocity = projev;
            num++;
            base.AI();
        }
        
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            距离 = (int)Vector2.Distance(target.Center, (Main.player[Projectile.owner].Center));
            if (距离 <= 75) { target.life -= 1; CombatText.NewText(new Rectangle((int)target.Center.X, (int)target.Center.Y, 10, 10), Color.Yellow,"1"); }
            base.OnHitNPC(target, hit, damageDone);
        }
    }
}
