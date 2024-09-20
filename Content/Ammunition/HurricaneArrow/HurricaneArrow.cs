//using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
//无合成表
namespace NanTing.Content.Ammunition.HurricaneArrow
{
    /// <summary>
    /// 飓风箭 -> 天蓝箭
    /// </summary>
    public class HurricaneArrow : ModItem
    {
        public override void SetDefaults()
        {
            Item.consumable = true;
            Item.DamageType = DamageClass.Ranged;
            //最大堆叠
            Item.maxStack = Item.CommonMaxStack;
            Item.damage = 7;
            //箭矢
            Item.ammo = AmmoID.Arrow;
            Item.shoot = ModContent.ProjectileType<HurricaneArrow_Proje>();
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            //天蓝锭
            //recipe.AddIngredient(ModContent.GetModItem(ModContent.ItemType<AerialiteBar>()), 1);
            //木箭
            recipe.AddIngredient(ItemID.WoodenArrow, 200);
            //日盘
            recipe.AddIngredient(ItemID.SunplateBlock, 1);
            recipe.AddTile(TileID.SkyMill);
            recipe.ReplaceResult(ModContent.ItemType<HurricaneArrow>(), 200);

            recipe.Register();
            base.AddRecipes();
        }
    }

    public class HurricaneArrow_Proje : ModProjectile
    {
        public override void SetDefaults()
        {
            //
            Projectile.ignoreWater = true;

            //伤害类型 远程
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.damage = 5;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            //Projectile.ai[0] = 0f;
            //Projectile.soundDelay = 2;
            base.SetDefaults();
        }
        Vector2 vector = default;
        int num = 0;
        public override void AI()
        {
            Vector2 v2 = new Vector2(0, 10);
            Vector2 v3 = new Vector2(0, -10);
            NanTingGProje proje = Projectile.GetGlobalProjectile<NanTingGProje>();
            if (proje.GetItem().Name.Equals("Daedalus Stormbow"))
            {
                if (num == 0f) { vector = Projectile.velocity; }
            }
            else
            {
                if (num == 0f)
                {
                    Vector2 MouseVectorWorld = Main.MouseWorld;
                    Vector2 vector2 = Main.MouseScreen;
                    Vector2 PlayerVectorWorld = Main.player[Projectile.owner].Center;
                    vector = Vector2.Normalize(MouseVectorWorld - PlayerVectorWorld) * 17f;
                }
                Projectile.velocity = vector;
            }
            //确保角度正确
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi / 2;
            if (num >= 20)
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust dust1 = Dust.NewDustDirect((Projectile.Center - v2), 1, 20, DustID.Adamantite, 0, 5);
                    Dust dust2 = Dust.NewDustDirect((Projectile.Center - v3), 1, -20, DustID.Adamantite, 0, -5);
                    dust1.noGravity = true;
                    dust2.noGravity = true;
                }
            }
            num++;
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
