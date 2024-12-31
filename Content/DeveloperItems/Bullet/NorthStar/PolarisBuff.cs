using Terraria;
using Terraria.ModLoader;

namespace Project203.OldWeapons.Ranger.PolarisParrotfishO
{
    public class PolarisBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            PPPlayer modPlayer = player.GetModPlayer<PPPlayer>();
            modPlayer.polarisBoost = true;
        }
    }
}
