//using CalamityMod.Items.Materials;
//using CalamityMod.Items.Mounts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
//无合成表
namespace FKsCRE.Content.Ammunition.AerialiteBullet
{
    public class AerialiteBullet : ModItem
    {
        public override void SetDefaults()
        {
            Item.consumable = true;
            Item.ammo = AmmoID.Bullet;
            Item.damage = 7;
            //暴击率
            Item.crit = 5;
            Item.shoot = ModContent.ProjectileType<AerialiteBullet_Proje>();
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            //火枪子弹
            recipe.AddIngredient(ItemID.MusketBall,200);
            //recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 1);
            //日盘块
            recipe.AddIngredient(ItemID.SunplateBlock, 1);
            //天魔
            recipe.AddTile(TileID.SkyMill);
            recipe.ReplaceResult(ModContent.ItemType<AerialiteBullet>(), 200);
            base.AddRecipes();
        }
    }

    public class AerialiteBullet_Proje : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.damage = 7;
            Projectile.aiStyle = ProjAIStyleID.Bubble;
            //不受水影响
            Projectile.ignoreWater = true;
            //Projectile.ai[0] = 0;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            base.SetDefaults();
        }

        int sum = 0;
        Vector2[] vector = new Vector2[5];
        int index = 4;
        bool SpriteBatch_new = false;
        Vector2 vector_ = default;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 plv2 = player.Center;
            //Vector2 vector = new Vector2(player.position.X+5,player.position.Y + 5);
            if (sum == 0)
            {
                vector_ = Vector2.Normalize(Main.MouseWorld - plv2) * 17;
                //Projectile.Center = Main.player[Projectile.owner].Center;
            }
            Projectile.velocity = vector_;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi / 2;
            //微弱的最终
            //    double sin = vector1._x * vector2._y - vector2._x * vector1._y;  
            //    double cos = vector1._x * vector2._x + vector1._y * vector2._y;
            //
            //return Math.Atan2(sin, cos) * (180 / Math.PI);
            if (sum == 1)
            {
                //foreach (NPC npc in Main.npc)
                //{
                //    Vector2 npcv2 = npc.Center;
                //    Vector2 projev2 = Projectile.Center;
                //    double sin = npcv2.X * projev2.X - npcv2.Y * projev2.Y;
                //    double cos = npcv2.X * projev2.X + projev2.Y * npcv2.Y;
                //    double e = Math.Atan2(sin, cos) * (180 / Math.PI);
                //    Main.NewText(Math.Abs(e));
                //    if (npc.friendly == false && Vector2.Distance(Projectile.Center, npc.Center) < 200 && npc.active && Math.Abs(e) < 10)
                //    {
                //        Main.NewText(true);
                //        sum++;
                //        Projectile.velocity = Vector2.Normalize(npcv2 - projev2) * 17;
                //        break;
                //    }
                //}
            }
            if (sum != 1)
            {
                //Projectile.velocity *= (float)Math.Cos(30);
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
            sum++;

            //SpriteBatch spriteBatch = Main.spriteBatch;
            //spriteBatch.Begin();
            //spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Content/Ammunition/天蓝子弹/天蓝子弹").Value,Projectile.);

            //spriteBatch.End();

            base.AI();
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.AbigailAttack, Projectile.Center);
            base.OnKill(timeLeft);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = Mod.Assets.Request<Texture2D>("Content/Ammunition/AerialiteBullet/AerialiteBullet_Proje").Value;
            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 v2 = Projectile.position - Main.screenPosition;
            //if (Projectile.timeLeft % 2 == 0)
            //{
                vector[index] = v2;
                index--;
            //}
            if (index == 0)
            {
                index = 4;
                SpriteBatch_new = true;
            }
            if (SpriteBatch_new)
            {
                //spriteBatch.Begin();
                for (int i = 0; i <= 4; i++)
                {
                    Main.spriteBatch.Draw(texture2D, vector[i], null, Color.White * 1 * (1 - .2f * i), Projectile.rotation, texture2D.Size() * .5f, 1 * (1 - .02f * i), SpriteEffects.None, 0);
                }
                //spriteBatch.End();
            }


            return false;
        }

    }

}
