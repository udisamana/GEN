using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Template
{
    public class BL
    {


        //General
        public static List<MappedSignalTarget> Map(List<Signal> signals, List<Target> targets, bool isMutant = false)
        {
            List<MappedSignalTarget> returnModels = new List<MappedSignalTarget>();
            foreach (var signal in signals)
            {
                Target target;

                if (isMutant)
                    target = targets.SingleOrDefault(x => x.Capture == signal.Capture &&
                                                                  x.Reporter == signal.Reporter &&
                                                                  x.isControl == false);
                else
                    target = targets.SingleOrDefault(x => x.Capture == signal.Capture &&
                                                              x.Color == signal.Color &&
                                                              x.Reporter == signal.Reporter &&
                                                              x.isControl == false);


                if (target != null && target.Name != "Control")
                {
                    MappedSignalTarget mappedSignalTarget = new MappedSignalTarget();

                    mappedSignalTarget.Capture = signal.Capture;
                    mappedSignalTarget.Color = signal.Color;
                    mappedSignalTarget.SampleIndex = signal.SampleIndex;
                    mappedSignalTarget.Value = signal.Value;
                    mappedSignalTarget.TargetName = target.Name;
                    mappedSignalTarget.Reporter = signal.Reporter;


                    //calculate controlValue
                    List<Target> controlTargets;
                    if (isMutant)
                        controlTargets = targets.Where(x => x.Reporter == signal.Reporter &&
                                                                         x.isControl == true).ToList();
                    else
                        controlTargets = targets.Where(x => x.Reporter == signal.Reporter &&
                                                     x.Color == signal.Color &&
                                                     x.isControl == true).ToList();

                    if (controlTargets == null || controlTargets.Count() == 0)
                        break;
                    else
                    {
                        if (controlTargets.Count() == 1)
                        {
                            int controlTargetCapture = controlTargets.First().Capture;
                            var currentSignal = signals.Single(x => x.Capture == controlTargetCapture &&
                                                                                  x.Color == signal.Color &&
                                                                                  x.Reporter == signal.Reporter &&
                                                                                  x.SampleIndex == signal.SampleIndex
                                                                                  );
                            mappedSignalTarget.ControlValue = currentSignal.Value;
                            mappedSignalTarget.ControlCapture = currentSignal.Capture;
                            mappedSignalTarget.ControlReporter = currentSignal.Reporter;
                        }

                        else if (controlTargets.Count() > 1)
                        {
                            List<MappedSignalTarget> CONTROLS_mappedSignalTargets = new List<MappedSignalTarget>();
                            foreach (var controlTarget in controlTargets)
                            {
                                Signal controlSignal = signals.Single(
                                    x => x.Capture == controlTarget.Capture &&
                                    x.Color == controlTarget.Color &&
                                    x.Reporter == controlTarget.Reporter &&
                                    x.SampleIndex == signal.SampleIndex
                                    );

                                CONTROLS_mappedSignalTargets.Add(new MappedSignalTarget()
                                {
                                    ControlValue = controlSignal.Value,
                                    TargetName = controlTarget.ControlPatogenName,
                                });
                            }
                            mappedSignalTarget.ControlValue = CONTROLS_mappedSignalTargets.Single(x => x.TargetName == target.Name).ControlValue;
                        }
                        returnModels.Add(mappedSignalTarget);
                    }
                }
            }
            return returnModels;
        }
        public static List<PatRes> PathogenAnalyze(List<Signal> signals, List<Target> targets, string[] SampleNameList = null)
        {
            targets = CustomValues.CustomizeGC2(targets);
            List<MinSigControl> minSigControls = CustomValues.GetGC2MinSigControl();
            List<MappedSignalTarget> mappedSignalTargets = BL.Map(signals, targets);

            List<PatRes> GC2Ress = new List<PatRes>();
            foreach (var item in mappedSignalTargets)
            {
                MinSigControl minSigControl = minSigControls.Single(x => x.TargetName == item.TargetName);

                PatRes analyzeResult = new PatRes()
                {
                    Capture = item.Capture,
                    Color = item.Color,
                    Reporter = item.Reporter,
                    SampleIndex = item.SampleIndex,
                    ControlValue = item.ControlValue,
                    Value = item.Value,
                    TargetName = item.TargetName,
                    SampleName = SampleNameList == null ? "" : SampleNameList[item.SampleIndex - 1]
                };

                if (item.Value >= minSigControl.MinimunSignal &&
                    item.ControlValue >= minSigControl.MinimumRatio)
                    analyzeResult.Inter = "POS";
                else
                    analyzeResult.Inter = "NEG";
                GC2Ress.Add(analyzeResult);
            }
            return GC2Ress;
        }
        public static List<MutRes> MutationAnalyze(List<Signal> signals, List<Target> targets, string Acronim, string[] SampleNameList = null)
        {
            List<MutRes> MutRess = new List<MutRes>();
            List<MappedSignalTarget> mappedSignalTargets = Map(signals, targets, true);
            foreach (var sample in mappedSignalTargets.Select(x => x.SampleIndex).Distinct())
                foreach (var target in mappedSignalTargets.Select(x => x.TargetName).Distinct())
                {

                    MappedSignalTarget greenMappedSignalTarget = mappedSignalTargets.SingleOrDefault(x =>
                        x.TargetName == target &&
                        x.Color == Colors.Green &&
                        x.SampleIndex == sample
                        );

                    MappedSignalTarget redMappedSignalTarget = mappedSignalTargets.SingleOrDefault(x =>
                        x.TargetName == target &&
                        x.Color == Colors.Red &&
                        x.SampleIndex == sample
                        );

                    double scaleFactor = 1;
                    string scaledGR = string.Empty;
                    if (Acronim == "CFN")
                    {
                        scaleFactor = CustomValues.CFNGetMutantFactor(target);
                        scaledGR = BL.mutGeneric(greenMappedSignalTarget.Value, redMappedSignalTarget.Value, greenMappedSignalTarget.ControlValue, redMappedSignalTarget.ControlValue, scaleFactor);
                    }

                    if (Acronim == "TSD")
                    {
                        scaleFactor = CustomValues.TSDGetMutantFactor(target);
                        scaledGR = BL.mutGeneric(greenMappedSignalTarget.Value, redMappedSignalTarget.Value, greenMappedSignalTarget.ControlValue, redMappedSignalTarget.ControlValue, scaleFactor);
                    }

                    if (Acronim == "AJP")
                    {
                        scaleFactor = CustomValues.AJPGetMutantFactor(target);
                        scaledGR = BL.mutGeneric(greenMappedSignalTarget.Value, redMappedSignalTarget.Value, greenMappedSignalTarget.ControlValue, redMappedSignalTarget.ControlValue, scaleFactor);
                    }

                    string call = BL.Genotype(scaledGR);

                    MutRess.Add(new MutRes()
                    {
                        Capture = greenMappedSignalTarget.Capture,
                        GreenValue = greenMappedSignalTarget.Value,
                        RedValue = redMappedSignalTarget.Value,
                        Reporter = greenMappedSignalTarget.Reporter,
                        SampleIndex = sample,
                        TargetName = target,
                        GreenControlValue = greenMappedSignalTarget.ControlValue,
                        RedControlValue = redMappedSignalTarget.ControlValue,
                        ScaledGR = scaledGR,
                        Call = call,
                        ControlCapture = greenMappedSignalTarget.ControlCapture,
                        ControlReporter = greenMappedSignalTarget.ControlReporter,
                        SampleName = SampleNameList == null ? "" : SampleNameList[sample - 1]
                    });
                }
            return MutRess;
        }
        public static string mutGeneric(double Green, double Red, double GreenControl, double RedControl, double ScaleFactor)
        {
            string ReturnedValue;

            const double MIN_SIGNAL_LEVEL = 1.5;
            const double MinSignal = 1500;
            const double minus = -1000;
            const double LHET = 0.33;
            const double UHET = 3;
            double GreenAboveControl, RedAboveControl, ScaledRatio;


            //Avoid division by zero
            if (GreenControl == 0)
                GreenControl = 0.001;
            if (RedControl == 0)
                RedControl = 0.001;
            //Find net signals
            GreenAboveControl = Green - GreenControl;
            RedAboveControl = Red - RedControl;
            //Avoid negative values
            if (GreenAboveControl <= 1)
                GreenAboveControl = 1;
            if (RedAboveControl <= 1)
                RedAboveControl = 1;
            //If any of signals involved is less then -200 report LS
            if (Green < minus || Red < minus || GreenControl < minus || RedControl < minus)
                ReturnedValue = "LS";
            else
                //If both net signals less then 1500 report LS
                if (Green - GreenControl < MinSignal && Red - RedControl < MinSignal)
                    ReturnedValue = "LS";
                else
                    //If both signals are not twice the control
                    if (Green / GreenControl < MIN_SIGNAL_LEVEL && Red / RedControl < MIN_SIGNAL_LEVEL && Green / GreenControl > 0 && Red / RedControl > 0)
                        ReturnedValue = "LS";
                    else
                    {
                        ScaledRatio = GreenAboveControl * ScaleFactor / RedAboveControl;
                        //HET ratio but one of the signals is lower then minimum signal
                        if (ScaledRatio > LHET && ScaledRatio < UHET && (GreenAboveControl < MinSignal || RedAboveControl < MinSignal))
                            ReturnedValue = "LS";
                        else
                            //HET ratio but one of the signals is not twice the control
                            if (ScaledRatio > LHET && ScaledRatio < UHET && ((Green / GreenControl > 0 && Green / GreenControl < MIN_SIGNAL_LEVEL) || (Red / RedControl > 0 && Red / RedControl < MIN_SIGNAL_LEVEL)))
                                ReturnedValue = "LS";
                            else
                                if (ScaledRatio >= 100)
                                    ReturnedValue = ScaledRatio.ToString("0");
                                else
                                    if (ScaledRatio >= 10)
                                        ReturnedValue = ScaledRatio.ToString("0.0");
                                    else
                                        ReturnedValue = ScaledRatio.ToString("0.00");
                    }
            return ReturnedValue;
        }
        public static string mutGenericBRCA(double Green, double Red, double GreenControl, double RedControl, double ScaleFactor)
        {
            string ReturnedValue;


            //const double MinSignal = 1500;//cf19
            const double MinSignal = 1000;//BRCA
            const double minus = -1000;
            const double LHET = 0.33;
            const double UHET = 3;
            double GreenAboveControl, RedAboveControl, ScaledRatio;


            //Avoid division by zero
            if (GreenControl == 0)
                GreenControl = 0.001;
            if (RedControl == 0)
                RedControl = 0.001;
            //Find net signals
            GreenAboveControl = Green - GreenControl;
            RedAboveControl = Red - RedControl;
            //Avoid negative values
            if (GreenAboveControl <= 1)
                GreenAboveControl = 1;
            if (RedAboveControl <= 1)
                RedAboveControl = 1;
            //If any of signals involved is less then -200 report LS
            if (Green < minus || Red < minus || GreenControl < minus || RedControl < minus)
                ReturnedValue = "LS";
            else
                //If both net signals less then 1500 report LS
                if (Green - GreenControl < MinSignal && Red - RedControl < MinSignal)
                    ReturnedValue = "LS";
                else
                    //If both signals are not twice the control
                    if (Green / GreenControl < 2 && Red / RedControl < 2 && Green / GreenControl > 0 && Red / RedControl > 0)
                        ReturnedValue = "LS";
                    else
                    {
                        ScaledRatio = GreenAboveControl * ScaleFactor / RedAboveControl;
                        //HET ratio but one of the signals is lower then minimum signal
                        if (ScaledRatio > LHET && ScaledRatio < UHET && (GreenAboveControl < MinSignal || RedAboveControl < MinSignal))
                            ReturnedValue = "LS";
                        else
                            //HET ratio but one of the signals is not twice the control
                            if (ScaledRatio > LHET && ScaledRatio < UHET && ((Green / GreenControl > 0 && Green / GreenControl < 2) || (Red / RedControl > 0 && Red / RedControl < 2)))
                                ReturnedValue = "LS";
                            else
                                if (ScaledRatio >= 100)
                                    ReturnedValue = ScaledRatio.ToString("0");
                                else
                                    if (ScaledRatio >= 10)
                                        ReturnedValue = ScaledRatio.ToString("0.0");
                                    else
                                        ReturnedValue = ScaledRatio.ToString("0.00");
                    }
            return ReturnedValue;
        }
        public static string mutGenericTest(double Green, double Red, double GreenControl, double RedControl, double ScaleFactor)
        {
            const double MinSignal = 1500;
            const double minus = -750;
            const double LHET = 0.33;
            const double UHET = 3;
            string ReturnedValue;
            double GreenAboveControl, RedAboveControl, ScaledRatio;


            //Avoid division by zero
            if (GreenControl == 0)
                GreenControl = 0.001;
            if (RedControl == 0)
                RedControl = 0.001;
            //Find net signals
            GreenAboveControl = Green - GreenControl;
            RedAboveControl = Red - RedControl;
            //Avoid negative values
            if (GreenAboveControl <= 1)
                GreenAboveControl = 1;
            if (RedAboveControl <= 1)
                RedAboveControl = 1;
            //If any of signals involved is less then -200 report LS
            if (Green < minus || Red < minus || GreenControl < minus || RedControl < minus)
                ReturnedValue = "LS1";
            else
                //If both net signals less then 1500 report LS
                if (Green - GreenControl < MinSignal && Red - RedControl < MinSignal)
                    ReturnedValue = "LS2";
                else
                    //If both signals are not twice the control
                    if (Green / GreenControl < 2 && Red / RedControl < 2 && Green / GreenControl > 0 && Red / RedControl > 0)
                        ReturnedValue = "LS3";
                    else
                    {
                        ScaledRatio = GreenAboveControl * ScaleFactor / RedAboveControl;
                        //HET ratio but one of the signals is lower then minimum signal
                        if (ScaledRatio > LHET && ScaledRatio < UHET && (GreenAboveControl < MinSignal || RedAboveControl < MinSignal))
                            ReturnedValue = "LS4";
                        else
                            //HET ratio but one of the signals is not twice the control
                            if (ScaledRatio > LHET && ScaledRatio < UHET && ((Green / GreenControl > 0 && Green / GreenControl < 2) || (Red / RedControl > 0 && Red / RedControl < 2)))
                                ReturnedValue = "LS5";
                            else
                                if (ScaledRatio >= 100)
                                    ReturnedValue = ScaledRatio.ToString("0");
                                else
                                    if (ScaledRatio >= 10)
                                        ReturnedValue = ScaledRatio.ToString("0.0");
                                    else
                                        ReturnedValue = ScaledRatio.ToString("0.00");
                    }
            return ReturnedValue;
        }
        public static string Genotype(string ScaledRatio)
        {
            const double MinWT = 5;
            const double MaxHet = 3;
            const double MinHet = 0.33;
            const double MaxHom = 0.2;
            if (ScaledRatio.Contains("LS"))
                return ScaledRatio;
            else
                if (ScaledRatio == "Ref. Fail")
                    return "Ref. Fail";
                else
                {
                    double SR = Convert.ToDouble(ScaledRatio);
                    if (SR >= MinWT)
                        return "-";
                    else
                        if (SR > MaxHet)
                            return "NC";
                        else
                            if (SR > MinHet)
                                return "HET";
                            else
                                if (SR > MaxHom)
                                    return "HOM/HET";
                                else
                                    return "HOM";
                }
        }
        public static string[,] ConvertAnalyzeResultToAnalyzeArray(List<MutRes> models)
        {
            models = models.OrderBy(x => x.SampleIndex).ThenBy(x => x.Reporter).ToList();
            int index = 0;
            string[,] returnArray = new string[models.Count, 10];
            foreach (var item in models)
            {
                returnArray[index, 0] = index.ToString();
                returnArray[index, 1] = item.SampleIndex.ToString();
                returnArray[index, 2] = item.Reporter.ToString();
                returnArray[index, 3] = item.TargetName.ToString();
                returnArray[index, 4] = item.GreenValue.ToString();
                returnArray[index, 5] = item.GreenControlValue.ToString();
                returnArray[index, 6] = item.RedValue.ToString();
                returnArray[index, 7] = item.RedControlValue.ToString();
                returnArray[index, 8] = item.ScaledGR.ToString();
                returnArray[index, 9] = item.Call.ToString();

                index++;
            }
            return returnArray;
        }
        public static string[,] ConvertAnalyzeResultToLegendAnalyzeArray(List<MutRes> models, int[] SamplePositionList)
        {
            //add controls as targets
            List<MutRes> controlModels = new List<MutRes>();
            foreach (var item in models.Select(x => new { Sample = x.SampleIndex, x.ControlReporter }).Distinct())
            {
                var controlModel = models.First(x => x.Reporter == item.ControlReporter && x.SampleIndex == item.Sample);
                controlModels.Add(new MutRes()
                {
                    TargetName = "Control",
                    Reporter = controlModel.ControlReporter,
                    Capture = controlModel.ControlCapture,
                    SampleIndex = controlModel.SampleIndex,
                    GreenValue = controlModel.GreenControlValue,
                    RedValue = controlModel.RedControlValue,
                    SampleName = controlModel.SampleName
                });
            }
            models.AddRange(controlModels);
            //

            List<List<MutRes>> lists = new List<List<MutRes>>();

            models = models.OrderBy(x => x.SampleIndex).ThenBy(x => x.Reporter).ToList();

            foreach (var item in models.Select(x => new { Sample = x.SampleIndex, x.Reporter }).Distinct())
                lists.Add(models.Where(x => x.Reporter == item.Reporter && x.SampleIndex == item.Sample).ToList());

            int sampels = models.Select(x => x.SampleIndex).Distinct().Count();
            int reporters = models.Select(x => x.Reporter).Distinct().Count();
            //int sampleRows = 6;
            int reporterColumns = 5;
            int sampleRows = 0;
            foreach (var item in models)
                if(sampleRows < item.Capture)
                {
                    sampleRows = item.Capture;
                }

            //sampleRows += 1;//for empty row
            int extraColumns = 2;



            string[,] returnArray = new string[sampels * sampleRows, reporters * reporterColumns + extraColumns];

            int sample = 1;
            int anchorColumn = 0;
            int anchorRow = 0;

            foreach (var list in lists)
            {
                int rowIndex = 0;

                for (var captureIndex = 1; captureIndex <= sampleRows; captureIndex++)
                {
                    int currentRow = anchorRow + rowIndex;
                    int currentColumn = anchorColumn * reporterColumns;
                    string sampleName = list.FirstOrDefault().SampleName;
                    //bool isEmptyRow = captureIndex == sampleRows;
                    if (anchorColumn == 0/* && !isEmptyRow*/)
                    {
                        returnArray[currentRow, currentColumn + 0] = SamplePositionList == null ? "" : SamplePositionList[sample - 1].ToString();
                        returnArray[currentRow, currentColumn + 1] = sampleName;
                    }

                    var target = list.SingleOrDefault(x => x.Capture == captureIndex);

                    returnArray[currentRow, currentColumn + 2] = target == null ? "" : target.TargetName;
                    returnArray[currentRow, currentColumn + 3] = target == null ? "" : target.GreenValue.ToString("0");
                    returnArray[currentRow, currentColumn + 4] = target == null ? "" : target.RedValue.ToString("0");
                    returnArray[currentRow, currentColumn + 5] = target == null ? "" : target.ScaledGR;
                    returnArray[currentRow, currentColumn + 6] = target == null ? "" : target.Call;

                    rowIndex++;
                }


                if ((anchorColumn + 1) % reporters == 0)
                {
                    anchorRow += sampleRows;
                    anchorColumn = 0;
                    sample++;
                }
                else
                    anchorColumn++;
            }



            return returnArray;
        }
        //details
        public static string[,] ConvertAnalyzeResultToDetailsArrayMutation(List<MutRes> analyzeResults, string[] SampleNameList, int[] SamplePositionList)
        {
            int targetCount = analyzeResults.Select(x => x.TargetName).Distinct().Count();
            int samplesCount = analyzeResults.Select(x => x.SampleIndex).Distinct().Count();
            string[,] DetailsDataTable = new string[samplesCount, targetCount + 2];

            for (int sample = 1; sample <= samplesCount; sample++)
            {
                DetailsDataTable[sample - 1, 0] = SamplePositionList[sample - 1].ToString() ?? "";
                DetailsDataTable[sample - 1, 1] = SampleNameList[sample - 1] ?? "";
                int patogenIndex = 2;
                foreach (var target in analyzeResults.Where(x => x.SampleIndex == sample))
                {
                    DetailsDataTable[sample - 1, patogenIndex] = target.Call;
                    patogenIndex++;
                }
            }
            return DetailsDataTable;
        }
        public static string[,] ConvertAnalyzeResultToDetailsArrayPathogen(List<PatRes> analyzeResults)
        {
            int patogenCount = analyzeResults.Select(x => x.TargetName).Distinct().Count();
            int samplesCount = analyzeResults.Select(x => x.SampleIndex).Distinct().Count();
            string[,] DetailsDataTable = new string[samplesCount, patogenCount + 1];

            for (int sample = 1; sample <= samplesCount; sample++)
            {
                DetailsDataTable[sample - 1, 0] = sample.ToString();
                if (analyzeResults.Where(x => x.SampleIndex == sample && x.Inter == "NEG").Count() == patogenCount)
                    for (int patogen = 1; patogen <= patogenCount; patogen++)
                        DetailsDataTable[sample - 1, patogen] = "X";
                else
                {
                    int patogenIndex = 1;
                    foreach (var patogen in analyzeResults.Where(x => x.SampleIndex == sample))
                    {
                        DetailsDataTable[sample - 1, patogenIndex] = patogen.Inter;
                        patogenIndex++;
                    }
                }

            }
            return DetailsDataTable;
        }
        //FromBRCA
        public static double trnc(double signal)
        {
            double returnedValue;
            if (signal < 1)
                returnedValue = 1;
            else
                returnedValue = signal;
            return returnedValue;
        }
        public static double getReporterControlSignal(string[,] markerName, double[,] referenceSignals, int reporter, int captures, int reporters, int color)
        {
            for (int i = 0; i < captures; i++)
                if (markerName[i, reporter] == "Control")
                    return referenceSignals[reporters * i + reporter, color] + referenceSignals[reporters * captures + reporters * i + reporter, color];

            return 0;
        }
        public static string[,] ReftoArray(List<RefResult> list, int columns)
        {

            string[,] res = new string[list.Count, columns];

            for (int i = 0; i < list.Count; i++)
            {
                res[i, 1] = list[i].Mutation;
                res[i, 2] = list[i].GMinusBkg.ToString("0.0");
                if (list[i].isBackround)
                {
                    res[i, 0] = "Background";
                    res[i, 3] = "";
                    res[i, 4] = "";
                    res[i, 5] = "";
                    res[i, 6] = "";
                    res[i, 7] = "";
                }
                else
                {
                    string setType = string.Empty;

                    if (list[i].MixSet == "0")
                        setType = "Set A";
                    else if (list[i].MixSet == "1")
                        setType = "Set B";

                    res[i, 0] = "Reporter " + (list[i].Reporter + 1).ToString() + "/" + setType;
                    res[i, 3] = list[i].GDivBkg.ToString("0.0");
                    res[i, 4] = list[i].RMinBkg.ToString("0.0");
                    res[i, 5] = list[i].RDivBkg.ToString("0.0");
                    res[i, 6] = list[i].IsMutantPass ? "Pass" : "Fail";
                    res[i, 7] = list[i].IsReporterPass ? "Pass" : "Fail";
                }
            }

            return res;
            /*
        string[][] finalData = list.ToArray<RefResult>(;
        var R = finalData.Count();
        var C = finalData[0].Length;
        var res = new string[R, C];
        for (int r = 0; r != R; r++)
            for (int c = 0; c != C; c++)
                res[r, c] = finalData[r][c];
        return res;*/
        }
        internal static bool IsMutationPass(RefResult entity)
        {
            if (entity.GMinusBkg > 1500 &&
                entity.RMinBkg > 1500 &&
                entity.GMinusBkg > 3 &&
                entity.RDivBkg > 3)
                return true;

            return false;
        }




    }
}
