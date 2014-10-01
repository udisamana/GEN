
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TemplateLib;

namespace Template
{
    
    public class Signal
    {
        public const int MAXCOLORS = 2;

        public int SampleIndex { get; set; }
        public int Capture { get; set; }
        public int Reporter { get; set; }
        public Colors Color { get; set; }
        public double Value { get; set; }

        internal static List<Signal> ToModel(double[, , ,] array)
        {
            const int SAMPLEDIM = 0;
            const int CAPTUREDIM = 1;
            const int REPORTERDIM = 2;

            int samples = array.GetLength(SAMPLEDIM);
            int captures = array.GetLength(CAPTUREDIM);
            int reporters = array.GetLength(REPORTERDIM);
            int colors = MAXCOLORS;

            List<Signal> models = new List<Signal>();

            for (int sample = 1; sample <= samples; sample++)
                for (int capture = 1; capture <= captures; capture++)
                    for (int reporter = 1; reporter <= reporters; reporter++)
                        for (int color = 0; color < colors; color++)
                            models.Add(new Signal()
                            {
                                Color = (Colors)color,
                                Reporter = reporter,
                                Capture = capture,
                                SampleIndex = sample,
                                Value = (double)array[sample - 1, capture - 1, reporter - 1, color],
                            });

            return models;
        }
    }
    public class Target
    {
        public string Name { get; set; }
        public bool isControl { get; set; }
        public Colors Color { get; set; }
        public int Reporter { get; set; }
        public int Capture { get; set; }
        public string ControlPatogenName { get; set; } //control per single patogen


        internal static List<Target> ToModel(string[,] array)
        {
            List<Target> patogens = new List<Target>();
            int captures = array.GetLength(0);
            int reporters = array.GetLength(1);

            for (int capture = 0; capture < captures; capture++)
                for (var reporter = 0; reporter < reporters; reporter++)
                    if (array[capture, reporter] != null)
                        if (array[capture, reporter] != "Control")
                            patogens.Add(new Target
                            {
                                Capture = capture + 1,
                                Reporter = reporter + 1,
                                Name = array[capture, reporter],
                                Color = Colors.Green,
                                isControl = false
                            });
                        else
                            patogens.Add(new Target
                            {
                                Capture = capture + 1,
                                Reporter = reporter + 1,
                                Color = Colors.Green,
                                isControl = true
                            });

            return patogens;
        }
    }
    public class MappedSignalTarget
    {
        public int Reporter { get; set; }
        public double Value { get; set; }
        public double ControlValue { get; set; }
        public Colors Color { get; set; }
        public int SampleIndex { get; set; }
        public int Capture { get; set; }
        public string TargetName { get; set; }
        public int ControlReporter { get; set; }
        public int ControlCapture { get; set; }
        //


    }
    public class MinSigControl
    {
        public string TargetName { get; set; }
        public int Capture { get; set; }
        public int PadCont { get; set; }
        public int MinimunSignal { get; set; }
        public double MinimumRatio { get; set; }
    }

    public class AnalyzeResult
    {
        public int Reporter { get; set; }
        public int SampleIndex { get; set; }
        public int Capture { get; set; }
        public string TargetName { get; set; }
        public string SampleName { get; set; }

    }
    public class PatRes : AnalyzeResult
    {
        public double Value { get; set; }
        public double ControlValue { get; set; }
        public Colors Color { get; set; }
        public string Inter { get; set; }

        internal static string[,] FromModel(List<PatRes> models)
        {
            int columns = 13;
            int rows = models.Count;
            string[,] array = new string[rows, columns];
            int modelIndex = 0;


            foreach (var model in models)
            {
                array[modelIndex, 0] = modelIndex.ToString();
                array[modelIndex, 1] = model.SampleName == null ? "" : model.SampleName.ToString();
                array[modelIndex, 2] = model.TargetName;
                array[modelIndex, 3] = model.Capture.ToString();
                array[modelIndex, 4] = model.Reporter.ToString();
                array[modelIndex, 5] = model.Color.ToString();
                array[modelIndex, 6] = model.ControlValue.ToString();
                array[modelIndex, 7] = model.Value.ToString();
                array[modelIndex, 8] = model.Inter.ToString();

                modelIndex++;
            }
            return array;
        }
    }
    public class MutRes : AnalyzeResult
    {
        public double GreenControlValue { get; set; }
        public double RedControlValue { get; set; }
        public double GreenValue { get; set; }
        public double RedValue { get; set; }
        public string ScaledGR { get; set; }
        public string Call { get; set; }
        public int ControlReporter { get; set; }
        public int ControlCapture { get; set; }
    }

    public class PatogenResult
    {
        public string Name { get; set; }
        public bool isControl { get; set; }
        public int Reporter { get; set; }
        public Colors Color { get; set; }
        public int Capture { get; set; }
        public int MixSet { get; set; }
        public decimal MinBkg { get; set; }
        public decimal DivBkg { get; set; }
        public decimal AvgBkg { get; set; }
        public bool IsPass { get; set; }
        public bool IsMixPass { get; set; }
        public bool IsControl { get; set; }
        public bool isBackround { get; set; }
        public bool isAvgBkg { get; set; }
        public string ControlPatogenName { get; set; } //control per single patogen

        internal static string[,] FromModel(List<PatogenResult> models)
        {
            //            int columns = new PatogenResult().GetType().GetProperties().Count();
            int columns = 10;
            int rows = models.Count;

            string[,] array = new string[rows, columns];


            int modelIndex = 0;
            foreach (var model in models)
            {
                array[modelIndex, 0] = model.Reporter.ToString();
                array[modelIndex, 1] = model.Name;
                array[modelIndex, 2] = model.MixSet.ToString();
                array[modelIndex, 3] = model.Color.ToString();
                array[modelIndex, 4] = Math.Round(model.MinBkg, 1).ToString();
                array[modelIndex, 5] = Math.Round(model.DivBkg, 1).ToString();
                array[modelIndex, 6] = model.IsPass.ToString();
                array[modelIndex, 7] = model.IsMixPass.ToString();
                array[modelIndex, 8] = Math.Round(model.AvgBkg, 1).ToString();
                array[modelIndex, 9] = model.ControlPatogenName ?? "";

                modelIndex++;
            }

            return array;
        }

    }
    public class ReferenceSignal
    {
        public int Row { get; set; }
        public int Instance { get; set; }
        public int Reference { get; set; }
        public int Reporter { get; set; }
        public int Pad { get; set; }
        public Decimal Value { get; set; }
        public Colors Color { get; set; }

        internal static List<ReferenceSignal> ToModel(double[,] array, int numberOfColors, int instances, int references, int reporters)
        {
            List<ReferenceSignal> models = new List<ReferenceSignal>();
            int row = 0;

            for (int color = 0; color < numberOfColors; color++)
            {
                for (int instance = 1; instance <= instances; instance++)
                    for (int reference = 1; reference <= references; reference++)
                        for (int reporter = 1; reporter <= reporters; reporter++)
                        {
                            models.Add(new ReferenceSignal()
                            {
                                Color = (Colors)color,
                                Instance = instance,
                                Reference = reference,
                                Reporter = reporter,
                                Row = row,
                                Value = (Decimal)array[row, color]
                            });
                            row++;
                        }
                row = 0;
            }

            return models;
        }

    }

    public class RefResult
    {
        public int Reporter { get; set; }
        public string MixSet { get; set; }
        public string Mutation { get; set; }
        public double GMinusBkg { get; set; }
        public double GDivBkg { get; set; }
        public double RMinBkg { get; set; }
        public double RDivBkg { get; set; }
        public bool IsMutantPass { get; set; }
        public bool IsReporterPass { get; set; }
        public bool isBackround { get; set; }
    }



}
