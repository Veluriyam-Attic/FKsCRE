using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Typeless;
using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Graphics;

namespace FKsCRE.Content.DeveloperItems.Bullet.ShadowsBullet
{
    public class ShadowsBulletPROJ : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.ShadowsBullet";


        private bool hasSplit = false; // 确保弹幕只分裂一次
        private int frameCounter = 0; // 用于记录帧数
        private static readonly int[] VanillaProjectiles = new int[]
        {
            1, 4, 5, 41, 82, 91, 103, 117, 120, 172, 225, 282, 357, 469, 474, 485, 495, 631, 639, 932, 1006
        };
        private static readonly int[] CalamityProjectiles = new int[]
        {
            ModContent.ProjectileType<BarinadeArrow>(),
            ModContent.ProjectileType<Shell>()
        };
        private static readonly string[] CustomModProjectiles = new string[]
        {
            "AerialiteArrowPROJ","BloodBeadsArrowPROJ","PurifiedGelArrowPROJ",
            "StarblightSootArrowPROJ",
            "AstralArrowPROJ","LifeAlloyArrowPROJ","PerennialArrowPROJ","ScoriaArrowPROJ",
            "DivineGeodeArrowPROJ","EffulgentFeatherArrowPROJ","PolterplasmArrowPROJ","UelibloomArrowPROJ",
            "AuricArrowPROJ", "MiracleMatterArrowPROJ",

            "MaoMaoChongPROJ","PlasmaDriveCorePrototypeArrowPROJ","TimeLeaperPROJ"
        };

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1; // 无限穿透
            Projectile.timeLeft = 200;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 5; // 增加更新次数
            Projectile.arrow = true;
        }

        public override void AI()
        {
            // 调整弹幕的旋转，使其在飞行时保持水平
            //Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;
            frameCounter++;
            Projectile.velocity *= 1.006f; // 逐渐加速

            // 在飞行过程中生成黑色粒子特效
            for (int j = 0; j < 10; j++)
            {
                Vector2 particleVelocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(15 * (j % 2 == 0 ? 1 : -1))) * Main.rand.NextFloat(1f, 2.6f);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Smoke, particleVelocity, 0, Color.Black, Main.rand.NextFloat(0.9f, 1.6f));
                dust.noGravity = true;
            }

            if (frameCounter >= 10 && !hasSplit)
            {
                SplitProjectile();
                hasSplit = true; // 确保只分裂一次
            }
        }

        private void SplitProjectile()
        {
            int splitCount = Main.rand.Next(2, 5); // 随机生成 2 到 4 个弹幕

            for (int i = 0; i < splitCount; i++)
            {
                float angle = MathHelper.ToRadians(Main.rand.Next(-10, 11));
                Vector2 newVelocity = Projectile.velocity.RotatedBy(angle) * 0.9f;

                int randomType = Main.rand.Next(3); // 随机选择 0-原版弹幕, 1-CalamityMod, 2-自定义模组弹幕
                if (randomType == 0)
                {
                    // 原版弹幕
                    int selectedVanilla = VanillaProjectiles[Main.rand.Next(VanillaProjectiles.Length)];
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, newVelocity, selectedVanilla, (Projectile.damage) * 2, Projectile.knockBack, Projectile.owner);
                }
                else if (randomType == 1)
                {
                    // Calamity 弹幕
                    int selectedCalamity = CalamityProjectiles[Main.rand.Next(CalamityProjectiles.Length)];
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, newVelocity, selectedCalamity, (Projectile.damage) * 2, Projectile.knockBack, Projectile.owner);
                }
                else
                {
                    // 自定义模组弹幕
                    string selectedProjectile = CustomModProjectiles[Main.rand.Next(CustomModProjectiles.Length)];
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, newVelocity, Mod.Find<ModProjectile>(selectedProjectile).Type, (Projectile.damage) * 2, Projectile.knockBack, Projectile.owner);
                }

                // 黑色粒子效果
                for (int j = 0; j < 15; j++)
                {
                    Vector2 particleVelocity = newVelocity.RotatedBy(MathHelper.ToRadians(15 * (j % 2 == 0 ? 1 : -1))) * Main.rand.NextFloat(1f, 2.6f);
                    Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Smoke, particleVelocity, 0, Color.Black, Main.rand.NextFloat(0.9f, 1.6f));
                    dust.noGravity = true;
                }
            }
        }











        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            // 获取 SpriteBatch 和投射物纹理
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D lightTexture = ModContent.Request<Texture2D>("FKsCRE/Content/DeveloperItems/Bullet/ShadowsBullet/ShadowsBulletPROJ").Value;

            // 遍历投射物的旧位置数组，绘制光学拖尾效果
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                // 计算颜色插值值，使颜色在旧位置之间平滑过渡
                float colorInterpolation = (float)Math.Cos(Projectile.timeLeft / 32f + Main.GlobalTimeWrappedHourly / 20f + i / (float)Projectile.oldPos.Length * MathHelper.Pi) * 0.5f + 0.5f;

                // 使用天蓝色渐变
                Color color = Color.Lerp(Color.Black, Color.Black, colorInterpolation) * 0.4f;
                color.A = 0;

                // 计算绘制位置，将位置调整到碰撞箱的中心
                Vector2 drawPosition = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

                // 计算外部和内部的颜色
                Color outerColor = color;
                Color innerColor = color * 0.5f;

                // 计算强度，使拖尾逐渐变弱
                float intensity = 0.9f + 0.15f * (float)Math.Cos(Main.GlobalTimeWrappedHourly % 60f * MathHelper.TwoPi);
                intensity *= MathHelper.Lerp(0.15f, 1f, 1f - i / (float)Projectile.oldPos.Length);
                if (Projectile.timeLeft <= 60)
                {
                    intensity *= Projectile.timeLeft / 60f; // 如果弹幕即将消失，则拖尾也逐渐消失
                }

                // 计算外部和内部的缩放比例，使拖尾具有渐变效果
                Vector2 outerScale = new Vector2(2f) * intensity;
                Vector2 innerScale = new Vector2(2f) * intensity * 0.7f;
                outerColor *= intensity;
                innerColor *= intensity;

                // 绘制外部的拖尾效果，并应用旋转
                Main.EntitySpriteDraw(lightTexture, drawPosition, null, outerColor, Projectile.rotation, lightTexture.Size() * 0.5f, outerScale * 0.6f, SpriteEffects.None, 0);

                // 绘制内部的拖尾效果，并应用旋转
                Main.EntitySpriteDraw(lightTexture, drawPosition, null, innerColor, Projectile.rotation, lightTexture.Size() * 0.5f, innerScale * 0.6f, SpriteEffects.None, 0);
            }

            // 绘制默认的弹幕，并应用旋转
            Main.EntitySpriteDraw(lightTexture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, lightColor, Projectile.rotation, lightTexture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}