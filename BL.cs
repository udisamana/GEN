using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Template
{
    public class BL
    {

        public const string POS = "Pos";
        public const string NEG = "-";
        public const string GYRA = "gyrA";
        public const string GAP = "gap";
        public const string UREAE = "UreaE";
        public const string HCT = "hct";
        public const string PBPB = "pbpb";
        public const string OPA = "opa";
        public const string ORF1 = "orf1";
        public const string _18S = "18S";
        public const string BGLOBIN = "B-Globin";
        public const string EXTRACTIONCONTROL = "EC";
        

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


        public static List<PatRes> PathogenAnalyze(List<Signal> signals, List<Target> targets, string acronim, string[] SampleNameList = null)
        {
            List<MinSigControl> minSigControls = new List<MinSigControl>();
            if (acronim == "ST6")//new kit
            {
                targets = CustomValues.CustomizeST6(targets);//new kit
                minSigControls = CustomValues.GetST6MinSigControl();//new kit
            }
            if (acronim == "GC2" || acronim == "GCQ" || acronim == "GI2")
            {
                targets = CustomValues.CustomizeGC2(targets);
                minSigControls = CustomValues.GetGC2MinSigControl();
            }

            if (acronim == "GCT")
            {
                targets = CustomValues.CustomizeGCT(targets);
                minSigControls = CustomValues.GetGC2MinSigControl();

            }

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
                    item.Value / item.ControlValue >= minSigControl.MinimumRatio)
                    analyzeResult.Inter = POS;
                else
                    analyzeResult.Inter = "-";
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
                if (sampleRows < item.Capture)
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
        public static string[,] ConvertAnalyzeResultToLegendAnalyzeArray(List<PatRes> models, int[] SamplePositionList, string controlTarget = null)
        {
            int reporterColumns = 5;
            int extraColumns = 2;
            int anchorColumn = 0;
            int anchorRow = 0;
            List<List<PatRes>> lists = new List<List<PatRes>>();
            models.Where(x => x.TargetName == BGLOBIN);
            models.ForEach(x => x.TargetName = x.TargetName.Replace(BGLOBIN, EXTRACTIONCONTROL));
            int reporters = models.Select(x => x.Reporter).Distinct().Count();

            //Order the model
            models = models.OrderBy(x => x.SampleIndex).ThenBy(x => x.Reporter).ToList();

            //create list of lists from the model
            foreach (var item in models.Select(x => new { Sample = x.SampleIndex, x.Reporter }).Distinct())
                lists.Add(models.Where(x => x.Reporter == item.Reporter && x.SampleIndex == item.Sample).ToList());

            //create the array
            int samples = models.Select(x => x.SampleIndex).Distinct().Count();
            string[,] returnArray = new string[GetLongestListItems(lists) * samples, reporters * reporterColumns + extraColumns];


            foreach (var list in lists)
            {
                int currentColumn = anchorColumn * reporterColumns;

                var controlTargetEntity = list.FirstOrDefault(x => x.SampleIndex == 1 && x.TargetName == controlTarget);
                if (controlTargetEntity != null)
                    AddTargetToArray(SamplePositionList, anchorColumn, returnArray, currentColumn, controlTargetEntity, anchorRow);

                int rowIndex = 0;
                foreach (var target in list)
                {
                    int currentRow = anchorRow + rowIndex;
                    if (target.SampleIndex != 1)
                        AddTargetToArray(SamplePositionList, anchorColumn, returnArray, currentColumn, target, currentRow);
                    rowIndex++;
                }

                //jump to next sample
                if ((anchorColumn + 1) % reporters == 0)
                {
                    anchorRow += GetLongestListItems(lists);
                    anchorColumn = 0;
                }
                else
                    anchorColumn++;
            }

            return returnArray;
        }
        private static void AddTargetToArray(int[] SamplePositionList, int anchorColumn, string[,] returnArray, int currentColumn, PatRes target, int currentRow)
        {
            if (anchorColumn == 0)
            {
                returnArray[currentRow, currentColumn + 0] = SamplePositionList == null ? "" : SamplePositionList[target.SampleIndex - 1].ToString();
                returnArray[currentRow, currentColumn + 1] = target.SampleIndex.ToString();
            }

            if (target.SampleIndex == 1)
            {
                returnArray[currentRow, currentColumn + 0] = SamplePositionList == null ? "" : SamplePositionList[target.SampleIndex - 1].ToString();
                returnArray[currentRow, currentColumn + 1] = "PC";
            }


            returnArray[currentRow, currentColumn + 2] = target == null ? "" : target.TargetName;
            returnArray[currentRow, currentColumn + 3] = target == null ? "" : target.Color.ToString();
            returnArray[currentRow, currentColumn + 4] = target == null ? "" : target.Value.ToString("0");
            returnArray[currentRow, currentColumn + 5] = target == null ? "" : target.ControlValue.ToString("0");
            returnArray[currentRow, currentColumn + 6] = target == null ? "" : target.Inter;

        }
        private static int GetLongestListItems(List<List<PatRes>> lists)
        {
            int longestListItems = 0;
            foreach (var list in lists)
                if (list.Count() > longestListItems)
                    longestListItems = list.Count();
            return longestListItems;
        }
        private static int GetMaxReporterRowsForSmaple(List<PatRes> list, int sampleIndex)
        {
            int maxReporterRows = 0;
            foreach (var target in list.Where(x => x.SampleIndex == sampleIndex))
                if (list.Count() > maxReporterRows)
                    maxReporterRows = list.Count();
            return maxReporterRows;
        }
        public static List<Signal> AddPad(List<Signal> signals)
        {
            /*
                XDocument xdoc = XDocument.Load("data.xml");

            var SampleDataList = from lv1 in xdoc.Descendants("SESSION").Descendants("PROTOCOL").Descendants("SAMPLES_LIST_NAME").Descendants("SAMPLE_DATA")
            select new { 
               SAMPLE_NAME = lv1.Attribute("SAMPLE_NAME").Value,
               WELL_ID = lv1.Attribute("WELL_ID")
           };

            foreach (var signal in signals)
	{
            SampleDataList.First(x=>x.SAMPLE_NAME == signal.)

	}

            var samplesDataList = xdoc.Descendants("SESSION").Descendants("PROTOCOL").Descendants("SAMPLES_LIST_NAME").Descendants("SAMPLE_DATA").Select new{};
            foreach (var sample in samplesDataList)
	{
                if (sample.Descendants("SAMPLE_NAME").
	{
		 
	}
	}

            var stages = xdoc.Descendants("SESSION").Descendants("PROTOCOL").Descendants("PROCESS").Descendants("STAGE");
            foreach (var stage in stages)
	{
                if(stage.Descendants(""))
	}
            foreach (var signal in signals)
            {
                signal.SampleIndex
            }*/
            return signals;
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
        public static string[,] ConvertAnalyzeResultToDetailsArrayPathogen(List<PatRes> analyzeResults, List<string> targetsNames = null, string acronim = null)
        {
            int samplesCount = analyzeResults.Select(x => x.SampleIndex).Distinct().Count();
            int patogenCount = analyzeResults.Select(x => x.TargetName).Distinct().Count();
            string[,] DetailsDataTable = new string[samplesCount, patogenCount + 2];//SAMPLE + ""


            var bacteriaRes = analyzeResults.Where(x => CustomValues.GetTargetTypeByTarget(x.TargetName) == TargetType.bacteria);
            bacteriaRes.OrderBy(x => x.TargetName);

            var parasiteRes = analyzeResults.Where(x => CustomValues.GetTargetTypeByTarget(x.TargetName) == TargetType.Parasite).OrderBy(x => x.TargetName);
            parasiteRes.OrderBy(x => x.TargetName);

            var undefinedRes = analyzeResults.Where(x => CustomValues.GetTargetTypeByTarget(x.TargetName) == TargetType.undefined).OrderBy(x => x.TargetName);
            undefinedRes.OrderBy(x => x.TargetName);

            List<PatRes> arrangedList = new List<PatRes>();
            arrangedList.AddRange(bacteriaRes);
            arrangedList.AddRange(parasiteRes);
            arrangedList.AddRange(undefinedRes);



            //arrangedList.AddRange(analyzeResults.OrderBy(x => x.TargetName));
            for (int sample = 1; sample <= samplesCount; sample++)
            {
                DetailsDataTable[sample - 1, 0] = sample.ToString();
                if (arrangedList.Where(x => x.SampleIndex == sample && x.Inter == "-").Count() == patogenCount)
                    for (int patogen = 1; patogen <= patogenCount; patogen++)
                        DetailsDataTable[sample - 1, patogen] = "X";
                else
                {
                    List<string> unifiedcolumn1 = new List<string>();
                    List<string> unifiedcolumn2 = new List<string>();
                    int patogenIndex = 1;
                    foreach (var target in targetsNames)
                    {
                        string inter;

                        if (arrangedList.Where(x => x.SampleIndex == sample && x.TargetName == target).Count() != 1)
                        {
                            string errStr = "there supposed to be just one target per sample";
                        }
                        else
                        {


                            if (acronim == "ST6")
                            {
                                switch (target)
                                {
                                    case HCT:


                                        inter = arrangedList.FirstOrDefault(x => x.SampleIndex == sample && x.TargetName == target).Inter;
                                        unifiedcolumn1.Add(inter);
                                        if (unifiedcolumn1.Count() == 2)
                                        {
                                            if (unifiedcolumn1.Any(x => x == POS))
                                            {
                                                DetailsDataTable[sample - 1, patogenIndex] = POS;
                                            }
                                            else
                                            {
                                                DetailsDataTable[sample - 1, patogenIndex] = inter;
                                            }
                                            patogenIndex++;
                                        }
                                        break;


                                    case PBPB:

                                        inter = arrangedList.FirstOrDefault(x => x.SampleIndex == sample && x.TargetName == target).Inter;
                                        unifiedcolumn1.Add(inter);
                                        if (unifiedcolumn1.Count() == 2)
                                        {
                                            if (unifiedcolumn1.Any(x => x == POS))
                                            {
                                                DetailsDataTable[sample - 1, patogenIndex] = POS;
                                            }
                                            else
                                            {
                                                DetailsDataTable[sample - 1, patogenIndex] = inter;
                                            }
                                            patogenIndex++;
                                        }
                                        break;

                                    case OPA:
                                        inter = arrangedList.FirstOrDefault(x => x.SampleIndex == sample && x.TargetName == target).Inter;
                                        unifiedcolumn2.Add(inter);
                                        if (unifiedcolumn2.Count() == 2)
                                        {
                                            if (unifiedcolumn2.Any(x => x == POS))
                                            {
                                                DetailsDataTable[sample - 1, patogenIndex] = POS;
                                            }
                                            else
                                            {
                                                DetailsDataTable[sample - 1, patogenIndex] = inter;
                                            }
                                            patogenIndex++;

                                        }
                                        break;

                                    case ORF1:
                                        inter = arrangedList.FirstOrDefault(x => x.SampleIndex == sample && x.TargetName == target).Inter;
                                        unifiedcolumn2.Add(inter);
                                        if (unifiedcolumn2.Count() == 2)
                                        {
                                            if (unifiedcolumn2.Any(x => x == POS))
                                            {
                                                DetailsDataTable[sample - 1, patogenIndex] = POS;
                                            }
                                            else
                                            {
                                                DetailsDataTable[sample - 1, patogenIndex] = inter;
                                            }
                                            patogenIndex++;

                                        }
                                        break;


                                    default:
                                        inter = arrangedList.FirstOrDefault(x => x.SampleIndex == sample && x.TargetName == target).Inter;
                                        DetailsDataTable[sample - 1, patogenIndex] = inter;
                                        patogenIndex++;
                                        break;
                                }



                            }

                            else
                            {
                                inter = arrangedList.FirstOrDefault(x => x.SampleIndex == sample && x.TargetName == target).Inter;
                                DetailsDataTable[sample - 1, patogenIndex] = inter;
                                patogenIndex++;
                            }
                        }

                    }
                    /*
                    foreach (var patogen in arrangedList.Where(x => x.SampleIndex == sample).OrderBy(t=>t.TargetName))
                    {
                        DetailsDataTable[sample - 1, patogenIndex] = patogen.Inter;
                        patogenIndex++;
                    }
                     * */
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
        public static string[,] ReftoArray(List<MutRefResult> list, int columns)
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
        internal static bool IsMutationPass(MutRefResult entity)
        {
            if (entity.GMinusBkg > 1500 &&
                entity.RMinBkg > 1500 &&
                entity.GMinusBkg > 3 &&
                entity.RDivBkg > 3)
                return true;

            return false;
        }

        //Base
        public static string[,] ConvertDictionaryTo2dStringArray(Dictionary<string, string> Dictionary)
        {
            string[,] stringArray2d = new string[Dictionary.Count, 2];
            int i = 0;

            foreach (KeyValuePair<string, string> item in Dictionary)
            {
                stringArray2d[i, 0] = item.Key;
                stringArray2d[i, 1] = item.Value;
                i++;
            }

            return stringArray2d;
        }


    }
}
