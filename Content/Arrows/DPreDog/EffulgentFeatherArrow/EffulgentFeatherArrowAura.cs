﻿using CalamityMod.Buffs.Potions;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.NPCs;
using CalamityMod.NPCs.AcidRain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;

namespace FKsCRE.Content.Arrows.DPreDog.EffulgentFeatherArrow
{
    public class EffulgentFeatherArrowAura : ModProjectile
    {
        private const float radius = 98f;
        private const int framesX = 3;
        private const int framesY = 6;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 218;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 18000;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.timeLeft *= 5;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 25;
        }

        public override void AI()
        {
            // 查找与自己配对的箭矢
            bool foundArrow = false;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.type == ModContent.ProjectileType<EffulgentFeatherArrowPROJ>() && proj.whoAmI == Projectile.ai[0])
                {
                    // 确保 Aura 位置与箭矢位置匹配
                    Projectile.Center = proj.Center;
                    foundArrow = true;
                    break;
                }
            }

            // 如果找不到配对的箭矢，销毁自己
            if (!foundArrow)
            {
                Projectile.Kill();
            }
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Electrified, 180);
            target.AddBuff(ModContent.BuffType<GalvanicCorrosion>(), 6);

            if (target.knockBackResist <= 0f)
                return;

            // 07AUG2023: Ozzatron: TML was giving NaN knockback, probably due to 0 base knockback. Do not use hit.Knockback
            if (CalamityGlobalNPC.ShouldAffectNPC(target))
            {
                float knockbackMultiplier = MathHelper.Clamp(1f - target.knockBackResist, 0f, 1f);
                Vector2 trueKnockback = target.Center - Projectile.Center;
                trueKnockback.Normalize();
                target.velocity = trueKnockback * knockbackMultiplier;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Electrified, 180);
            target.AddBuff(ModContent.BuffType<GalvanicCorrosion>(), 6);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            Color drawColour = Color.White;
            Rectangle sourceRect = new Rectangle(Projectile.width * (int)Projectile.localAI[1], Projectile.height * (int)Projectile.localAI[0], Projectile.width, Projectile.height);
            Vector2 origin = new Vector2(Projectile.width / 2, Projectile.height / 2);

            float opacity = 1f;
            int sparkCount = 0;
            int fadeTime = 20;

            if (Projectile.timeLeft < fadeTime)
            {
                opacity = Projectile.timeLeft * (1f / fadeTime);
                sparkCount = fadeTime - Projectile.timeLeft;
            }

            for (int i = 0; i < sparkCount * 2; i++)
            {
                int dustType = 132;
                if (Main.rand.NextBool())
                {
                    dustType = 264;
                }
                float rangeDiff = 2f;

                Vector2 dustPos = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
                dustPos.Normalize();
                dustPos *= radius + Main.rand.NextFloat(-rangeDiff, rangeDiff);

                int dust = Dust.NewDust(Projectile.Center + dustPos, 1, 1, dustType, 0, 0, 0, default, 0.75f);
                Main.dust[dust].noGravity = true;
            }

            Main.EntitySpriteDraw(sprite, Projectile.Center - Main.screenPosition, sourceRect, drawColour * opacity, Projectile.rotation, origin, 1f, SpriteEffects.None, 0);
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => CalamityUtils.CircularHitboxCollision(Projectile.Center, radius, targetHitbox);

        public override bool? CanHitNPC(NPC target)
        {
            if (NPCID.Sets.ProjectileNPC[target.type] || (target.catchItem != 0 && target.type != ModContent.NPCType<Radiator>()))
                return false;

            return null;
        }
    }
}
