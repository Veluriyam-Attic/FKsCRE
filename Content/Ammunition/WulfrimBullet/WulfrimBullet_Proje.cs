using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

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
            //10秒
            Projectile.timeLeft = 600;
            base.SetDefaults();
        }
    }
}
