//using Microsoft.Xna.Framework;
//using Terraria;
//using Terraria.ModLoader;

//namespace FKsCRE.Content.Arrows.CPreMoodLord.AstralArrow
//{
//    public class AstralArrowPLAYER : ModPlayer
//    {
//        private bool hasSizeIncreased = false; // ����Ƿ��Ѿ��Ŵ��

//        public override void PostUpdate()
//        {
//            // �������Ƿ��� AstralArrowPBuff
//            if (Player.HasBuff(ModContent.BuffType<AstralArrowPBuff>()) && !hasSizeIncreased)
//            {
//                // �Ŵ���
//                float scaleFactor = 1.5f;

//                // ʹ�� Size ���Ե�����ҵ���ײ���С
//                Player.Size *= scaleFactor;

//                // ��ѡ���������λ���Ա����Ӿ�ƫ��
//                Player.position -= new Vector2(Player.width / 2f, Player.height / 2f) * (scaleFactor - 1f);

//                // ���Ϊ�ѷŴ�
//                hasSizeIncreased = true;
//            }
//            // ��ԭ��ײ���С�������Ҫ��Buff�Ƴ�ʱ�ָ���
//            else if (!Player.HasBuff(ModContent.BuffType<AstralArrowPBuff>()) && hasSizeIncreased)
//            {
//                // ��ԭ����
//                float restoreFactor = 1 / 1.5f;

//                // ʹ�� Size ���Իָ���ҵ���ײ���С
//                Player.Size *= restoreFactor;

//                // ��ѡ���������λ���Ա����Ӿ�ƫ��
//                Player.position += new Vector2(Player.width / 2f, Player.height / 2f) * (1.5f - 1f);

//                // ���ñ��
//                hasSizeIncreased = false;
//            }
//        }

//    }
//}
