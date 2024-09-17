using CalamityMod.Items.Materials;
using CalamityMod.Items.Mounts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace NanTing.Content.Ammunition.天蓝子弹
{
    public class 天蓝子弹 : ModItem
    {
        public override void SetDefaults()
        {
            Item.consumable = true;
            Item.ammo = AmmoID.Bullet;
            Item.damage = 7;
            //暴击率
            Item.crit = 5;
            Item.shoot = ModContent.ProjectileType<天蓝子弹_Proje>();
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            //火枪子弹
            recipe.AddIngredient(ItemID.MusketBall,200);
            recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 1);
            //天魔
            recipe.AddTile(TileID.SkyMill);
            recipe.ReplaceResult(ModContent.ItemType<天蓝子弹>(), 200);
            base.AddRecipes();
        }
    }

    public class 天蓝子弹_Proje : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.damage = 7;
            //不受水影响
            Projectile.ignoreWater = true;
            Projectile.ai[0] = 0;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            base.SetDefaults();
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.velocity = Vector2.Normalize(Main.MouseWorld - Projectile.Center) * 17;
                Projectile.rotation = Projectile.velocity.ToRotation() * (float)Math.PI / 2;
                //Projectile.Center = Main.player[Projectile.owner].Center;
            }
            

            /*
            int sc = Main.screenWidth;
            //大就是右边 小就是左边
            if(Main.MouseScreen.X >= Main.screenWidth / 2)
            {
                if (Projectile.ai[0] == 0)
                {

                }
            }
            */
            Projectile.ai[0]++;

            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.Begin();
            //spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Content/Ammunition/天蓝子弹/天蓝子弹").Value,Projectile.);

            spriteBatch.End();

            base.AI();
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.AbigailAttack,Projectile.Center);
            base.OnKill(timeLeft);
        }
    }

}
