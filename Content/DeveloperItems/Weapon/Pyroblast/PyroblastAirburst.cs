using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace FKsCRE.Content.DeveloperItems.Weapon.Pyroblast
{
    public class CorinthPrimeAirburst : BaseMassiveExplosionProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "DeveloperItems.Pyroblast";

        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override int Lifetime => 60;

        // 调整屏幕震动触发条件
        public override bool UsesScreenshake => base.Projectile.damage > 1;

        // 调整屏幕震动强度
        public override float GetScreenshakePower(float pulseCompletionRatio)
        {
            return CalamityUtils.Convert01To010(pulseCompletionRatio) * 8f; // 强度减半
        }

        public override Color GetCurrentExplosionColor(float pulseCompletionRatio)
        {
            return Color.Lerp(Color.Blue * 1.6f, Color.Cyan, MathHelper.Clamp(pulseCompletionRatio * 2.2f, 0f, 1f));
        }

        public override void SetDefaults()
        {
            base.Projectile.width = (base.Projectile.height = 2);
            base.Projectile.friendly = true;
            base.Projectile.tileCollide = false;
            base.Projectile.penetrate = -1;
            base.Projectile.usesLocalNPCImmunity = true;
            base.Projectile.localNPCHitCooldown = 10;
            base.Projectile.timeLeft = Lifetime;
            base.Projectile.DamageType = DamageClass.Ranged;
        }

        public override void PostAI()
        {
            Lighting.AddLight(base.Projectile.Center, 0f, 0f, 0.3f);
        }
    }
}
