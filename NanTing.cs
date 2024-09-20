using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace NanTing
{
    public class TimeVer
    {
        public Vector2 v2 = default;
        public TimeVer(Vector2 vector2)
        {
            v2 = vector2;
        }
    }

	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class NanTing : Mod
	{
		
	}

	public class ModTime : ModSystem
	{
		public static int Time = 0;
        public override void UpdateUI(GameTime gameTime)
        {
            if (!Main.gamePaused)
            {
                Time++;
            }
            if (Time >= 86400)
            {
                Time = 0;
            }
            base.UpdateUI(gameTime);
        }
    }

    public class µØƒªAI¿‡ : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        private int dam = 10;
        int num = 0;
        Vector2 nor = default;
        public override void AI(Projectile projectile)
        {
            if(projectile.Name.Equals("¿‚“ÌµØ_µØƒª"))
            {

            }

            base.AI(projectile);
        }

    }
}
