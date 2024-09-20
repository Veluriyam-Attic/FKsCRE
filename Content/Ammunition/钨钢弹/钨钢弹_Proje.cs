using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace NanTing.Content.Ammunition.钨钢弹
{
    public class 钨钢弹_Proje : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.damage = 统一.伤害;
            Projectile.knockBack = 1.0f;
            Projectile.aiStyle = ProjAIStyleID.Bubble;
            //10秒
            Projectile.timeLeft = 600;
            base.SetDefaults();
        }
    }
}
