using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TemplateLib;

namespace Template
{
    public class CustomValues
    {

        public const string DF = "Df";
        public const string CRYPTO = "Crypto";
        public const string GIA = "Gia";
        public const string SAL = "Sal";
        public const string BH = "Bh";
        public const string EH = "Eh";
        public const string ED = "Ed";
        public const string SHI = "Shi";
        public const string CCOLI = "C.coli";     //asp
        public const string CJEJUNI = "C.jejuni";   //mapA
        public const string NVC = "NVC";


        //reference
        internal static List<Target> CustomizeGC2(List<Target> originalPatogens)
        {
            originalPatogens.RemoveAll(x => x.Reporter == 2 && x.isControl == true);
            originalPatogens.RemoveAll(x => x.Reporter == 5 && x.isControl == true);

            originalPatogens.Add(
                new Target()
                {
                    Capture = 1,
                    Color = Colors.Red,
                    isControl = false,
                    Name = DF,
                    Reporter = 1,
                });


            originalPatogens.Add(
    new Target()
    {
        Capture = 2,
        Color = Colors.Red,
        isControl = true,
        Reporter = 1,

    });
            originalPatogens.Add(
new Target()
{
    Capture = 2,
    Color = Colors.Red,
    isControl = true,
    Reporter = 2
});
            originalPatogens.Add(
    new Target()
    {
        Capture = 3,
        Color = Colors.Red,
        isControl = false,
        Name = GIA,
        Reporter = 2
    });
            originalPatogens.Add(
    new Target()
    {
        Capture = 1,
        Color = Colors.Green,
        isControl = true,
        Reporter = 2
    });

            originalPatogens.Add(
new Target()
{
    Capture = 1,
    Color = Colors.Green,
    isControl = true,
    Reporter = 2,
    ControlPatogenName = NVC
});
            originalPatogens.Add(
new Target()
{
    Capture = 2,
    Color = Colors.Green,
    isControl = true,
    Reporter = 2,
    ControlPatogenName = CJEJUNI
});
            originalPatogens.Add(
new Target()
{
    Capture = 3,
    Color = Colors.Green,
    isControl = true,
    Reporter = 5,
    ControlPatogenName = SHI
});
            originalPatogens.Add(
new Target()
{
    Capture = 4,
    Color = Colors.Green,
    isControl = true,
    Reporter = 5,
    ControlPatogenName = SAL
});



            return originalPatogens;
        }
        internal static bool isGC2PatogenPass(string name, decimal minBkg, decimal divBkg)
        {
            switch (name)
            {
                case DF:
                    if (minBkg < 4900 || divBkg < 4)
                        return false;
                    return true;
                case CRYPTO:
                    if (minBkg < 6500 || divBkg < 2)
                        return false;
                    return true;
                case GIA:
                    if (minBkg < 5500 || divBkg < (decimal)5.5)
                        return false;
                    return true;
                case SAL:
                    if (minBkg < 7500 || divBkg < 5)
                        return false;
                    return true;
                case BH:
                    if (minBkg < 4500 || divBkg < 3)
                        return false;
                    return true;
                case EH:
                    if (minBkg < 6000 || divBkg < 3)
                        return false;
                    return true;
                case ED:
                    if (minBkg < 3000 || divBkg < 5)
                        return false;
                    return true;
                case SHI:
                    if (minBkg < 10000 || divBkg < 5)
                        return false;
                    return true;
                case CCOLI:
                    if (minBkg < 10000 || divBkg < (decimal)3.5)
                        return false;
                    return true;
                case CJEJUNI:
                    if (minBkg < 9000 || divBkg < 6)
                        return false;
                    return true;
                case NVC:
                    if (minBkg < 2500 || divBkg < 2)
                        return false;
                    return true;
            }
            return false;
        }
        internal static string[] GetReferenceColumnHeaders()
        {
            string[] referenceColumnHeaders = new string[10];
            referenceColumnHeaders[0] = "Reporter";
            referenceColumnHeaders[1] = "Name";
            referenceColumnHeaders[2] = "MixSet";
            referenceColumnHeaders[3] = "Color";

            referenceColumnHeaders[4] = "MinBkg";
            referenceColumnHeaders[5] = "DivBkg";
            referenceColumnHeaders[6] = "IsPass";
            referenceColumnHeaders[7] = "IsMixPass";
            referenceColumnHeaders[8] = "AvgBkg";
            referenceColumnHeaders[9] = "ControlPatogen";

            //string[] referenceColumnHeaders = new string[new PatogenResult().GetType().GetProperties().Count()];

            //int i = 0;
            //foreach (var item in new PatogenResult().GetType().GetProperties())
            //{
            //    referenceColumnHeaders[i] = item.Name;
            //    i++;
            //}
            return referenceColumnHeaders;
        }

        //anlyze
        internal static List<MinSigControl> GetGC2MinSigControl()
        {
            List<MinSigControl> MinSigControls = new List<MinSigControl>();

            MinSigControls.Add(new MinSigControl() { TargetName = DF, Capture = 1, PadCont = 2, MinimunSignal = 4900, MinimumRatio = 4 });
            MinSigControls.Add(new MinSigControl() { TargetName = CRYPTO, Capture = 1, PadCont = 4, MinimunSignal = 6500, MinimumRatio = 2 });
            MinSigControls.Add(new MinSigControl() { TargetName = GIA, Capture = 3, PadCont = 2, MinimunSignal = 5500, MinimumRatio = 5.5 });
            MinSigControls.Add(new MinSigControl() { TargetName = SAL, Capture = 1, PadCont = 4, MinimunSignal = 7500, MinimumRatio = 5 });
            MinSigControls.Add(new MinSigControl() { TargetName = BH, Capture = 4, PadCont = 2, MinimunSignal = 4500, MinimumRatio = 3 });
            MinSigControls.Add(new MinSigControl() { TargetName = EH, Capture = 4, PadCont = 1, MinimunSignal = 6000, MinimumRatio = 3 });
            MinSigControls.Add(new MinSigControl() { TargetName = ED, Capture = 2, PadCont = 1, MinimunSignal = 3000, MinimumRatio = 5 });
            MinSigControls.Add(new MinSigControl() { TargetName = SHI, Capture = 2, PadCont = 3, MinimunSignal = 10000, MinimumRatio = 5 });
            MinSigControls.Add(new MinSigControl() { TargetName = CCOLI, Capture = 3, PadCont = 4, MinimunSignal = 10000, MinimumRatio = 3.5 });
            MinSigControls.Add(new MinSigControl() { TargetName = CJEJUNI, Capture = 3, PadCont = 2, MinimunSignal = 9000, MinimumRatio = 6 });
            MinSigControls.Add(new MinSigControl() { TargetName = NVC, Capture = 4, PadCont = 1, MinimunSignal = 2500, MinimumRatio = 2 });

            return MinSigControls;
        }


        //factors
        internal static double GetBrcaMutantFactor(string name)
        {
            double factor;
            switch (name)
            {
                case "6174delT":
                    factor = 0.8;
                    break;
                case "185delAG":
                    factor = 0.7;
                    break;
                case "W1508X":
                    factor = 0.7;
                    break;
                case "C61G":
                    factor = 0.35;
                    break;
                case "981delAT":
                    factor = 0.7;
                    break;
                case "E720X":
                    factor = 1;
                    break;
                case "8765delAG":
                    factor = 1;
                    break;
                case "5164del4":
                    factor = 0.7;
                    break;
                case "A1708E":
                    factor = 0.6;
                    break;
                case "5328insC":
                    factor = 0.7;
                    break;
                case "Y978X":
                    factor = 0.4;
                    break;
                case "4153delA":
                    factor = 0.7;
                    break;
                case "IVS2G>A":
                    factor = 1;
                    break;
                case "R2336P":
                    factor = 0.7;
                    break;

                default:
                    factor = 1;
                    break;
            }
            return factor;
        }
        internal static double CFNGetMutantFactor(string name)
        {
            double factor;
            switch (name)
            {
                case "F508del":
                    factor = 1;
                    break;
                case "1717-1 G>A":
                    factor = 1;
                    break;
                case "N1303K":
                    factor = 0.7;
                    break;
                case "3849+10kb C&gt;T":
                    factor = 1;
                    break;
                case "3120+1kb del8.6":
                    factor = 1;
                    break;
                case "G85E":
                    factor = 1;
                    break;
                case "G542X":
                    factor = 1;
                    break;
                case "I1234V":
                    factor = 1;
                    break;
                case "W1282X":
                    factor = 0.71;
                    break;
                case "2183AA>G":
                    factor = 1.2;
                    break;
                case "S549R T>G":
                    factor = 0.5;
                    break;
                case "Q359K/T360K":
                    factor = 0.35;
                    break;
                case "W1089X":
                    factor = 0.3;
                    break;
                case "CFTR del2.3":
                    factor = 0.4;
                    break;
                case "3121-1 G>A":
                    factor = 1;
                    break;
                case "405+1 G>A":
                    factor = 0.7;
                    break;
                case "D1152H":
                    factor = 0.7;
                    break;
                case "4010 delTATT":
                    factor = 0.5;
                    break;
                case "Y1092X":
                    factor = 1;
                    break;

                default:
                    factor = 1;
                    break;
            }
            return factor;
        }
        internal static double AJPGetMutantFactor(string name)
        {
            double factor;
            switch (name)
            {
                case "ML4-Del[ex1-7]":
                    factor = 2.5;
                    break;
                case "CAN-854":
                    factor = 1;
                    break;
                case "MSUD-R183P":
                    factor = 0.9;
                    break;
                case "FD-R696P":
                    factor = 1;
                    break;
                case "G35T":
                    factor = 1.6;
                    break;
                case "BLM":
                    factor = 1;
                    break;
                case "FD-2507":
                    factor = 1.3;
                    break;
                case "GSD1A":
                    factor = 1;
                    break;
                case "NPA-R496L":
                    factor = 0.8;
                    break;
                case "NPB-delR608":
                    factor = 0.5;
                    break;
                case "NMLN":
                    factor = 1;
                    break;
                case "ML4-IVS3 A>G":
                    factor = 1;
                    break;
                case "USHT1":
                    factor = 0.8;
                    break;
                case "FAC-IVS4":
                    factor = 1;
                    break;
                case "NPA-L302P":
                    factor = 0.5;
                    break;
                case "CAN-693":
                    factor = 0.8;
                    break;
                case "AAT-PIZ":
                    factor = 0.6;
                    break;
                case "NPA-fsP":
                    factor = 1;
                    break;
                case "N48K":
                    factor = 1;
                    break;

                default:
                    factor = 1;
                    break;
            }
            return factor;
        }
        internal static double TSDGetMutantFactor(string name)
        {
            double factor;
            switch (name)
            {
                case "IVS12+1 G>C":
                    factor = 0.4;
                    break;
                case "IVS5-2 A>G":
                    factor = 1;
                    break;
                case "R170Q G>A":
                    factor = 0.75;
                    break;
                case "1278insTATC":
                    factor = 1;
                    break;
                case "G269S":
                    factor = 1;
                    break;
                case "DF304/305":
                    factor = 1;
                    break;

                default:
                    factor = 1;
                    break;
            }
            return factor;
        }


    }
}
