using System;
using LGD_OC_AstractPlatForm.Enums;
using LGD_OC_AstractPlatForm.CommonAPI;
using BSQH_Csharp_Library;
using System.Drawing;

namespace LGD_OC_AstractPlatForm.OpticCompensation
{
    public class OCInfiniteLoopCheck
    {
        IBusinessAPI api;

        public int InfiniteLoopCount = 0;
        public bool IsInfiniteLoop = false;
        RGB[] Temp_Gamma;
        RGB[] Diif_Gamma;

        public OCInfiniteLoopCheck(IBusinessAPI _api)
        {
            Temp_Gamma = new RGB[6];
            Diif_Gamma = new RGB[5];
            Initialize();
            api = _api;
        }

        public void Initialize()
        {
            InfiniteLoopCount = 0;
            IsInfiniteLoop = false;
        }

        public void Check_InfiniteLoop(int loopCount,RGB Gamma)
        {
            if (loopCount == 0) Temp_Gamma[0].Equal_Value(Gamma);
            else if (loopCount == 1) Temp_Gamma[1].Equal_Value(Gamma);
            else if (loopCount == 2) Temp_Gamma[2].Equal_Value(Gamma);
            else if (loopCount == 3) Temp_Gamma[3].Equal_Value(Gamma);
            else if (loopCount == 4) Temp_Gamma[4].Equal_Value(Gamma);//Added On 200218
            else if (loopCount == 5) Temp_Gamma[5].Equal_Value(Gamma);//Added On 200218
            else Infinite_Loop_Check_When_Loopcount_Is_Bigger_Than_5(Gamma);
        }

        private void Infinite_Loop_Check_When_Loopcount_Is_Bigger_Than_5(RGB Gamma)
        {
            RGB_Ininite_Loop_Check_Initialize(Gamma);

            if (Is_Infinite_True_Case1())//Added On 200218
            {
                api.WriteLine("Infinite Loop Case 1", Color.Purple);
                IsInfiniteLoop = true;
                InfiniteLoopCount++;
            }
            else if (Is_Infinite_True_Case2())
            {
                api.WriteLine("Infinite Loop Case 2", Color.Blue);
                IsInfiniteLoop = true;
                InfiniteLoopCount++;
            }
            else
            {
                IsInfiniteLoop = false;
            }

            Show_Infinite_and_InfiniteCount();
        }

        private void Show_Infinite_and_InfiniteCount()
        {
            if (IsInfiniteLoop) api.WriteLine("IsInfinite : " + IsInfiniteLoop, Color.Red);
            else api.WriteLine("IsInfinite : " + IsInfiniteLoop, Color.Green);

            if (InfiniteLoopCount >= 3) api.WriteLine("InfiniteLoopCount = " + InfiniteLoopCount, Color.Red);
            else api.WriteLine("InfiniteLoopCount = " + InfiniteLoopCount, Color.Green);
        }

        private bool Is_Infinite_True_Case1()
        {
            return (Diif_Gamma[2].R == Diif_Gamma[4].R)
                && (Diif_Gamma[2].G == Diif_Gamma[4].G)
                && (Diif_Gamma[2].B == Diif_Gamma[4].B)
                && ((Diif_Gamma[2].R != Diif_Gamma[3].R) || (Diif_Gamma[2].G != Diif_Gamma[3].G) || (Diif_Gamma[2].B != Diif_Gamma[3].B))
                && (((Diif_Gamma[3].int_R >= 0) && (Diif_Gamma[4].int_R < 0))//Added On 200218
                    || ((Diif_Gamma[3].int_R < 0) && (Diif_Gamma[4].int_R >= 0))//Added On 200218
                    || ((Diif_Gamma[3].int_G >= 0) && (Diif_Gamma[4].int_G < 0))//Added On 200218
                    || ((Diif_Gamma[3].int_G < 0) && (Diif_Gamma[4].int_G >= 0))//Added On 200218
                    || ((Diif_Gamma[3].int_B >= 0) && (Diif_Gamma[4].int_B < 0))//Added On 200218
                    || ((Diif_Gamma[3].int_B < 0) && (Diif_Gamma[4].int_B >= 0)));
        }

        private bool Is_Infinite_True_Case2()
        {
            return (Temp_Gamma[0].Is_RGB_Equal(Temp_Gamma[3])
                && Temp_Gamma[1].Is_RGB_Equal(Temp_Gamma[4])
                && Temp_Gamma[2].Is_RGB_Equal(Temp_Gamma[5]));
        }

        private void RGB_Ininite_Loop_Check_Initialize(RGB Gamma)
        {
            RGB Temp = new RGB();

            Temp.Equal_Value(Temp_Gamma[1]);
            Temp_Gamma[0].Equal_Value(Temp);

            Temp.Equal_Value(Temp_Gamma[2]);
            Temp_Gamma[1].Equal_Value(Temp);

            Temp.Equal_Value(Temp_Gamma[3]);//Added On 200218
            Temp_Gamma[2].Equal_Value(Temp);//Added On 200218

            Temp.Equal_Value(Temp_Gamma[4]);//Added On 200218
            Temp_Gamma[3].Equal_Value(Temp);//Added On 200218

            Temp.Equal_Value(Temp_Gamma[5]);//Added On 200218
            Temp_Gamma[4].Equal_Value(Temp);//Added On 200218

            Temp_Gamma[5].Equal_Value(Gamma);//Added On 200218

            Diif_Gamma[0] = Temp_Gamma[1] - Temp_Gamma[0];
            Diif_Gamma[1] = Temp_Gamma[2] - Temp_Gamma[1];
            Diif_Gamma[2] = Temp_Gamma[3] - Temp_Gamma[2];
            Diif_Gamma[3] = Temp_Gamma[4] - Temp_Gamma[3];
            Diif_Gamma[4] = Temp_Gamma[5] - Temp_Gamma[4];
        }
    }
}
