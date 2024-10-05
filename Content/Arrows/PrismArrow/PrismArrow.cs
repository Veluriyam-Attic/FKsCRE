using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
//无合成表
namespace FKsCRE.Content.Arrows.PrismArrow
{
    class ty
    {
        public static int dam = 10;
    }
    public class PrismArrow : Arrow
    {
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.damage = ty.dam;
            Item.ammo = AmmoID.Arrow;
            Item.DamageType = DamageClass.Ranged;
            Item.shoot = ModContent.ProjectileType<PrismArrow_Proje>();
        }
    }
    public class PrismArrow_Proje : ModProjectile
    {
        public override void SetDefaults()
        {
            //Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.damage = ty.dam;
            Projectile.timeLeft = 1000;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            //碰撞箱
            DrawOffsetX = -8;
            DrawOriginOffsetY = -40;
            base.SetDefaults();
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
            writer.Write(num);
            base.SendExtraAI(writer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            vector = new(reader.ReadSingle(), reader.ReadSingle());
            num = reader.ReadInt32();
            base.ReceiveExtraAI(reader);
        }
        Vector2 vector = default;
        int num = 0;
        public override void AI()
        {
            Vector2 playerCenton = Main.player[Projectile.owner].Center - Main.screenPosition;
            Vector2 MouseCenton = Main.MouseWorld - Main.screenPosition;
            NanTingGProje projectile = Projectile.GetGlobalProjectile<NanTingGProje>();
            Projectile.spriteDirection = Projectile.direction;
            //速度确定 只进行一次
            if (projectile.GetItem().Name.Equals("Daedalus Stormbow"))
            {
                if (num == 0) vector = Projectile.velocity;
                if (!Projectile.wet) { Projectile.damage = ty.dam + projectile.GetItem().damage; Projectile.velocity = vector; }
                if (Projectile.wet) { Projectile.damage = (int)(ty.dam * 1.5f + projectile.GetItem().damage); Projectile.velocity = vector * 2.5f; }
                Projectile.netUpdate = true;
            }
            else
            {
                if (num == 0)
                {
                    vector = Vector2.Normalize(MouseCenton - playerCenton) * 15f;
                    Projectile.netUpdate = true;
                }
                //在液体中
                if (Projectile.wet) Projectile.damage = (int)(ty.dam * 1.5f + projectile.GetItem().damage);
                if (Projectile.wet && Projectile.timeLeft % 2 == 0) vector.Y += 0.02f;
                if (Projectile.wet) Projectile.velocity = vector * 2.5f;
                if (!Projectile.wet) Projectile.damage = ty.dam + projectile.GetItem().damage;
                if (!Projectile.wet) Projectile.velocity = vector;
                if (!Projectile.wet) vector.Y += 0.25f;
            }
            //弹幕永远存在 除非被破坏
            if (Projectile.timeLeft <= 10)
            {
                Projectile.timeLeft = 1000;
            }
            //速度角 + 180°
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.Pi / 2;
            num++;
            if (num == 0)
            {
                Projectile.netUpdate = true;
            }
            else if (num % 2 == 0)
            {
                Projectile.netUpdate = true;
            }
            base.AI();
        }
        public int Getproj()
        {
            return (int)Projectile.ai[2];
        }
        public void setProj(int NUM)
        {
            Projectile.ai[2] = NUM;
        }

        //public override void OnKill(int timeLeft)
        //{
        //    // 释放天蓝色的水系粒子特效
        //    for (int i = 0; i < 10; i++)
        //    {
        //        // 随机生成粒子发射的速度和方向，沿着箭矢的朝向发射
        //        Vector2 particleVelocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(10)) * Main.rand.NextFloat(0.5f, 1.5f);

        //        // 创建天蓝色粒子，并调整缩放和亮度
        //        Dust waterDust = Dust.NewDustPerfect(Projectile.Center, DustID.Water, particleVelocity, 0, Color.LightSkyBlue, 2f); // 设置颜色为天蓝色，缩放为2倍
        //        waterDust.noGravity = true; // 无重力效果
        //        waterDust.fadeIn = 1.5f; // 增加粒子的亮度和淡入效果
        //    }
        //}

        //public override bool PreDraw(ref Color lightColor)
        //{
        //    // 如果投射物不在水中，直接返回不绘制拖尾效果
        //    if (!Projectile.wet)
        //    {
        //        return true; // 继续绘制默认的弹幕
        //    }

        //    // 获取 SpriteBatch 和投射物纹理
        //    SpriteBatch spriteBatch = Main.spriteBatch;
        //    Texture2D lightTexture = ModContent.Request<Texture2D>("FKsCRE/Content/Arrows/PrismArrow/PrismArrow").Value;

        //    // 遍历投射物的旧位置数组，绘制光学拖尾效果
        //    for (int i = 0; i < Projectile.oldPos.Length; i++)
        //    {
        //        // 计算颜色插值值，使颜色在旧位置之间平滑过渡
        //        float colorInterpolation = (float)Math.Cos(Projectile.timeLeft / 32f + Main.GlobalTimeWrappedHourly / 20f + i / (float)Projectile.oldPos.Length * MathHelper.Pi) * 0.5f + 0.5f;

        //        // 使用海蓝色渐变
        //        Color color = Color.Lerp(Color.DeepSkyBlue, Color.Cyan, colorInterpolation) * 0.4f;
        //        color.A = 0;

        //        // 计算绘制位置，将位置调整到碰撞箱的中心
        //        Vector2 drawPosition = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

        //        // 计算外部和内部的颜色
        //        Color outerColor = color;
        //        Color innerColor = color * 0.5f;

        //        // 计算强度，使拖尾逐渐变弱
        //        float intensity = 0.9f + 0.15f * (float)Math.Cos(Main.GlobalTimeWrappedHourly % 60f * MathHelper.TwoPi);
        //        intensity *= MathHelper.Lerp(0.15f, 1f, 1f - i / (float)Projectile.oldPos.Length);
        //        if (Projectile.timeLeft <= 60)
        //        {
        //            intensity *= Projectile.timeLeft / 60f; // 如果弹幕即将消失，则拖尾也逐渐消失
        //        }

        //        // 计算外部和内部的缩放比例，使拖尾具有渐变效果
        //        Vector2 outerScale = new Vector2(2f) * intensity;
        //        Vector2 innerScale = new Vector2(2f) * intensity * 0.7f;
        //        outerColor *= intensity;
        //        innerColor *= intensity;

        //        // 绘制外部的拖尾效果，并应用旋转
        //        Main.EntitySpriteDraw(lightTexture, drawPosition, null, outerColor, Projectile.rotation, lightTexture.Size() * 0.5f, outerScale * 0.6f, SpriteEffects.None, 0);

        //        // 绘制内部的拖尾效果，并应用旋转
        //        Main.EntitySpriteDraw(lightTexture, drawPosition, null, innerColor, Projectile.rotation, lightTexture.Size() * 0.5f, innerScale * 0.6f, SpriteEffects.None, 0);
        //    }

        //    // 绘制默认的弹幕，并应用旋转
        //    Main.EntitySpriteDraw(lightTexture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, lightColor, Projectile.rotation, lightTexture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);

        //    return false;
        //}


        //public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        //{
        //    //在液体(水)中
        //    if(Projectile.wet)
        //    {
        //        damageDone += 5;
        //    }
        //    base.OnHitNPC(target, hit, damageDone);
        //}
        //public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        //{
        //    if(Projectile.wet)
        //    {
        //        modifiers.FinalDamage += 5;
        //    }
        //    base.ModifyHitNPC(target, ref modifiers);
        //}
    }
}
