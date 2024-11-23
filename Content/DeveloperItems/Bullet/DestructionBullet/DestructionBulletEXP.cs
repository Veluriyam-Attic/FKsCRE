using CalamityMod.Buffs.DamageOverTime;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Buffs.StatDebuffs;

namespace FKsCRE.Content.DeveloperItems.Bullet.DestructionBullet
{
    public class DestructionBulletEXP : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "DeveloperItems.DestructionBullet";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            // 请叫我buff王，哈哈
            target.AddBuff(BuffID.Daybreak, 300); // 破晓
            target.AddBuff(BuffID.Electrified, 300); // 带电
            target.AddBuff(BuffID.Ichor, 300); // 灵液
            target.AddBuff(BuffID.CursedInferno, 300); // 诅咒狱火
            target.AddBuff(BuffID.Midas, 300); // 迈达斯
            target.AddBuff(BuffID.BetsysCurse, 300); // 双足翼龙之怒火
            target.AddBuff(BuffID.Venom, 300); // 酸性毒液

            


            target.AddBuff(ModContent.BuffType<ElementalMix>(), 300); // 元素紊乱
            target.AddBuff(ModContent.BuffType<MarkedforDeath>(), 300); // 死亡标记
            target.AddBuff(ModContent.BuffType<DestructionBulletPoisonIvy>(), 300); // 常春藤毒素
            target.AddBuff(ModContent.BuffType<DestructionBulletDestruction>(), 300); // 灭世
        }

        public override void AI()
        {

        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => CalamityUtils.CircularHitboxCollision(Projectile.Center, 400, targetHitbox);
    }
}
