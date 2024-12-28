using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Particles;

namespace FKsCRE.Content.Gel.DPreDog.BloodstoneCoreGel
{
    internal class BloodstoneCoreGelGP : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool IsBloodstoneCoreGelInfused = false;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo ammoSource && ammoSource.AmmoItemIdUsed == ModContent.ItemType<BloodstoneCoreGel>())
            {
                IsBloodstoneCoreGelInfused = true;
                projectile.netUpdate = true;
            }
            base.OnSpawn(projectile, source);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsBloodstoneCoreGelInfused && target.active && !target.friendly)
            {
                // 调整伤害为原来的 50%
                projectile.damage = (int)(projectile.damage * 0.5f);

                // 根据伤害恢复所有玩家血量（0.1%~0.5%）
                int healAmount = Main.rand.Next((int)(damageDone * 0.001f), (int)(damageDone * 0.005f) + 1);
                foreach (Player player in Main.player)
                {
                    if (player.active)
                    {
                        player.statLife += healAmount;
                        player.HealEffect(healAmount);
                    }
                }

                // 施加 BurningBlood Buff，持续 10 秒（600 帧）
                target.AddBuff(ModContent.BuffType<BurningBlood>(), 600);

                // 在原地生成血爆特效
                Particle bloodsplosion2 = new CustomPulse(
                    projectile.Center,
                    Vector2.Zero,
                    new Color(255, 32, 32),
                    "CalamityMod/Particles/DustyCircleHardEdge",
                    Vector2.One * 0.5f, // 缩小至原大小的 50%
                    Main.rand.NextFloat(-15f, 15f),
                    0.03f,
                    0.155f,
                    40
                );
                GeneralParticleHandler.SpawnParticle(bloodsplosion2);
            }
        }
    }
}
