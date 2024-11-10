using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalamityThrowingSpear.Weapons.NewWeapons.BPrePlantera.TheLastLance
{
    public class PolterplasmArrowEDeBuff : ModBuff, ILocalizedModType
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public new string LocalizationCategory => "ModBuff";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true; // 减益
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            // 只影响敌人，不影响友方或其他
            if (!npc.friendly && npc.lifeMax > 5) // 检查 NPC 是否是敌人且生命值正常
            {
                // 使敌人完全无法移动
                npc.velocity.X = 0f;
                npc.velocity.Y = 0f;

                // 你还可以进一步禁用其他行为，例如攻击或特定动作
                npc.aiStyle = -1; // 取消 AI 行为 (可选，根据需要)

                // 将敌人的防御力设置为 10,000 点，近乎无敌
                npc.defense = 10000;
            }
        }




    }
}

