/**************************************************************************************
**
** FILE NAME:   Kit acronym name    
**
** PURPOSE:    
**             
** 
** NOTES:
**                                                 
**************************************************************************************/
using System;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Linq;

using TemplateLib;
using DataTypes.ProtocolResults;
using SystemConfiguration;
using DataTypes.Template;
using CRC;
using System.Collections;
using Template;

namespace GC2
{

    public class GC2 : TPL_BaseTemplate
    {
        // NVC is not calculated in CalcRatio so number of markers is counted without him
        public const int NUMBER_OF_MARKERS_PER_SAPMLE_WITHOUT_NVC = 9;

        // total number of markers per sample include NVC (+1)
        public const int NUMBER_OF_MARKERS_PER_SAPMLE = NUMBER_OF_MARKERS_PER_SAPMLE_WITHOUT_NVC + 1;

        // const of marker names
        public const string HIS1 = "HIS1";
        public const string HIS2 = "HIS2";
        public const string HIS3 = "HIS3";
        public const string HIS4 = "HIS4";
        public const string MECA = "mecA";
        public const string SCCMEC = "Sccmec";
        public const string NUC = "nuc";
        public const string SPA = "spa";
        public const string VANAB = "vanA/B";
        public const string DDL_EFS = "ddl-efs";
        public const string DDL_EFM = "ddl-efm";
        public const string PHOE = "phoE";
        public const string KPC = "KPC";
        public const string NVC = "NVC";

        public const string BKG = "Bkg.";
        public const string PC = "PC";
        public const string PC_FAIL = "PC Fail";
        public const string PC_PASS = "PC Pass";
        public const string NONE = "None";
        public const string RESULT = "Result";
        public const string PASS = "Pass";
        public const string FAIL_MASSAGE = "Fail";
        public const string OK = "OK";
        public const string NOA = "NOA";
        public const string INVALID = "Invalid";

        public const string NEGATIVE = "Negative";
        public const string MSSA = "MSSA";
        public const string SCC_POSITIVE_MSSA = "Scc Positive MSSA";
        public const string CON_MR = "CoN-MR";
        public const string CON_MR_MSSA = "CoN-MR+MSSA";
        public const string POSSIBLE_MRSA = "Possible MRSA";
        public const string RESULT_MRSA = "MRSA";

        public const string NONE_EFS_EFM_VAN = "None efs/efm Van+";
        public const string EFM = "efm";
        public const string VRE_EFM = "VRE EFM";
        public const string EFS = "efs";
        public const string VRE_EFS = "VRE EFS";
        public const string EFS_EFM = "efs/efm";
        public const string VRE_EFM_EFS = "VRE EFM/EFS";

        public const string KPC_NON_KP = "KPC (non KP)";
        public const string KP = "KP";
        public const string KP_KPC = "KP-KPC";

        public const string INVALID_SAMPLE = "Invalid Sample";

        public const string INVALID_SAMPLE_SUMMARY = "Invalid Sample (Insufficient extraction)";

        public const string PCR_Failed = "PCR Failed";

        //////////////////////////////////////////////////////////////////////////////////////////

        // special sample (contain of 2 pads: Bkg. and PC). shown in index number 0 at reporter 2 (for IPC)
        public const int NUMBER_OF_SPECIAL_SAMPLES = 1;

        public const int MINIMUN_SIGNAL_MecA = 5000;
        public const int MINIMUN_RATIO_MecA = 7;

        public const int MINIMUN_SIGNAL_nuc = 5000;
        public const int MINIMUN_RATIO_nuc = 8;

        public const int MINIMUN_SIGNAL_SCC_Mec = 4000;
        public const int MINIMUN_RATIO_SCC_Mec = 5;

        public const int MINIMUN_SIGNAL_SPA = 7000;
        public const int MINIMUN_RATIO_SPA = 5;

        public const int MINIMUN_SIGNAL_EFS = 9000;
        public const int MINIMUN_RATIO_EFS = 8;

        public const int MINIMUN_SIGNAL_EFM = 3000;
        public const int MINIMUN_RATIO_EFM = 6;

        public const int MINIMUN_SIGNAL_VAN_A_B = 6000;
        public const int MINIMUN_RATIO_VAN_A_B = 15;

        public const int MINIMUN_SIGNAL_PhoE = 9000;
        public const int MINIMUN_RATIO_PhoE = 10;

        public const int MINIMUN_SIGNAL_KPC = 6000;
        public const int MINIMUN_RATIO_KPC = 9;

        public const int MINIMUN_SIGNAL_PC = 9000;
        public const int MINIMUN_RATIO_PC = 9;

        public const int MINIMUN_SIGNAL_NVC = 3000;
        public const int MINIMUN_RATIO_NVC = 4;

        enum AnalyzeGeneralColumnsNames { Number, Position, SampleID };
        enum AnalyzeReporterNames { ReporterMix1, ReporterMix2, ReporterMix3, ReporterMix4, ReporterMix5 };
        enum AnalyzeColumnsNamesOfEachReporter { Marker, Green, Ratio };
        enum AnalyzeCustomColumnsNames { Interpretation };
        enum color { RED, GREEN, RESERVED1, RESERVED2 };

        public const int ANALYZE_INDEX_COLUMN = 0;
        public const int ANALYZE_POSITION_COLUMN = 1;
        public const int ANALYZE_SAMPLE_NAME_COLUMN = 2;
        public const int ANALYZE_INVALID_SAMPLE = 3;

        public const int ANALYZE_MECA_ROW = 3;
        public const int ANALYZE_EFS_ROW = 4;
        public const int ANALYZE_PHOE_ROW = 5;

        public const int MARKER_LIST = 1;
        public const int SAMPLE_LIST = 0;

        public const int COLOR_COLUMN = 1;

        // each special sample consists of 2 rows
        public const int SPECIAL_SAMPLE_STRUCTURE = 2;
        //////////////////////////////////////////////////////////////////////////////////////////

        public const int TEST_REFERENCE = 10000;

        public const int RESET_COUNT_NUMBER_OF_PASSES = 0;

        // set a number to a column for easier understanding of code
        public const int REPORTER_COLUMN = 0;
        public const int MARKER_COLUMN = 1;
        public const int REFERENCE1_COLUMN = 2;
        public const int REFERENCE2_COLUMN = 3;
        public const int PASS_FAIL_COLUMN = 4;

        public const int NUMBER_OF_COLORS_BEEN_USED = 2;

        public const string REFERENCE_MRSA_FAIL = "MRSA Ref. failed";
        public const string REFERENCE_VRE_FAIL = "VRE Ref. failed";
        public const string REFERENCE_KPC_FAIL = "KPC Ref. failed";

        enum ReferenceColumnsNames { Target, ReporterSet, GMinBkg, GDivBkg, RMinBkg, RDivBkg, PassFail, Sum };

        //////////////////////////////////////////////////////////////////////////////////////////

        public const string SUCCESS = "1";
        public const string FAIL = "0";

        public const int DETAILS_INDEX_COLUMN = 0;
        public const int DETAILS_POSITION_COLUMN = 1;
        public const int DETAILS_SAMPLE_ID_COLUMN = 2;
        public const int DETAILS_MRSA_COLUMN = 3;
        public const int DETAILS_VRE_COLUMN = 4;
        public const int DETAILS_KPC_COLUMN = 5;

        enum DetailsColumnNames { Number, Position, SampleID, MRSA, VRE, KPC };

        //////////////////////////////////////////////////////////////////////////////////////////

        string[] DetailsColumnHeaders;

        public const int COLUMN_SIZE = 1;

        public const int SUMMARY_INDEX_COLUMN = 0;
        public const int SUMMARY_POSITION_COLUMN = 1;
        public const int SUMMARY_SAMPLE_NAME_COLUMN = 2;
        public const int SUMMARY_RESULT_COLUMN = 3;

        enum SummaryColumnsNames { Number, Position, SampleID, Result };

        //////////////////////////////////////////////////////////////////////////////////////////

        public const int TWO_DECIMAL = 2;
        public const int ONE_DECIMAL = 1;
        public const int NO_DECIMAL = 0;

        //////////////////////////////////////////////////////////////////////////////////////////


        //****************************************************************************
        #region Private Members

        //Template type:
        private TPL_SaveTemplateTypes saveTemplateType;

        /// <summary>
        /// Template name
        /// </summary>
        private string name;//**

        //Acronym name
        private string acronym;//**

        /// <summary>
        /// Description
        /// </summary>
        private string description;//**

        /// <summary>
        /// Version
        /// </summary>
        private string version;//**

        /// <summary>
        /// Name of template creator
        /// </summary>
        private string createdBy;//**

        /// <summary>
        /// Creation date
        /// </summary>
        private DateTime creationDate;//**

        /// <summary>
        /// Indicates if template process includes PCR stage
        /// </summary>
        private bool pcrIncluded;//**

        /// <summary>
        /// Array of reagent bottles
        /// </summary>
        //private TPL_ReagetBottle[] reagentBottlesArr;

        /// <summary>
        /// Array of reagent packs.
        /// </summary>
        //private TPL_ReagentPack[] reagentPacksArr;

        /// <summary>
        /// /Materials configuration
        /// </summary>
        private TPL_MaterialsConfiguration materialsConfiguration;//**

        /// <summary>
        /// Maximum allowed cartridge pads reuses.
        /// </summary>
        private int padsReuses;

        /// <summary>
        /// PCR program indexes: [0]- directory index , [1]- program index
        /// </summary>
        private int[] pcrProgram;

        /// <summary>
        /// Number of wells per sample
        /// </summary>
        private int wellPerSample;

        /// <summary>
        /// Array of diseases.
        /// </summary>
        private List<DTT_Disease> diseasesArr = new List<DTT_Disease>();//**

        /// <summary>
        /// Mutation assignment
        /// </summary>
        private List<TPL_MutationAssignment> mutationsAssignment;

        /// <summary>
        /// Loading pattern
        /// </summary>
        //private TPL_LoadingPattern loadingPattern;

        /// <summary>
        /// Loading Patterns
        /// </summary>
        private DTT_LoadingPattern[] loadingPatterns;


        /// <summary>
        /// Template process.
        /// </summary>
        private TPL_Process process;


        //XPath variables
        /*private string versionXPath;
        private string acronymXPath;
        private string descriptionXPath;
        private string padsReusesXPath;*/

        #endregion




        /// <summary>
        /// Read template data.
        /// </summary>
        public virtual bool Initialize()
        {
            bool result = false;
            result = SetXmlFile();
            if (result == true)
            {
                ParseTemplateFile();
            }
            return result;
        }




        /// <summary>
        /// This function loads xml file from Templates/Development folder
        /// </summary>
        /// <returns>true - file was loaded and its valid(check checksum) , false - else</returns>
        protected bool SetXmlFile()
        {
            bool result;
            result = LoadXmlFile();
            if (result == true)
            {
                //Get checksum
                string checksum = templateXml.Attributes[TPL_FileTags.CHECKSUM].Value;
                //result = Check checksum
                result = VerifyChecksum(checksum);
            }
            return result;
        }




        /// <summary>
        /// Parse template file
        /// </summary>
        private void ParseTemplateFile()
        {
            try
            {
                //Update description, acronym, version,pads reuses
                InitializeBasicInformation();

                //Update materials configuration
                InitializeMaterials();

                //Update diseases
                InitializeDiseases();

            }
            catch { }
        }







        /// <summary>
        /// Initialize basic information: get from template xml: version,acronym, description,pads reuses.
        /// </summary>
        private void InitializeBasicInformation()
        {
            XmlNode basicInformation = templateXml.SelectSingleNode(TPL_FileTags.BASE_INFORMATION_PART);
            //Name:
            name = basicInformation.Attributes[TPL_FileTags.NAME_ATTRIBUTE].Value;
            //Version:
            version = basicInformation.Attributes[TPL_FileTags.VERSION_ATTRIBUTE].Value;
            //Acronym:
            acronym = basicInformation.Attributes[TPL_FileTags.ACRONYM_ATTRIBUTE].Value;

            //Description:
            description = basicInformation.Attributes[TPL_FileTags.DESCRIPTION_ATTRIBUTE].Value;

            //Pads resuses:
            padsReuses = Convert.ToInt32(basicInformation.Attributes[TPL_FileTags.PADS_REUSES_ATTRIBUTE].Value);

            //Wells per sample
            wellPerSample = Convert.ToInt32(basicInformation.Attributes[TPL_FileTags.WELLS_PER_SAMPLE_ATTRIBUTE].Value);


            //Creation date: 
            try
            {
                string date = basicInformation.Attributes[TPL_FileTags.CREATDATE_ATTRIBUTE].Value;
                creationDate = Convert.ToDateTime(basicInformation.Attributes[TPL_FileTags.CREATDATE_ATTRIBUTE].Value);
            }
            catch
            {
                creationDate = DateTime.Now;
            }

            //Created by:
            createdBy = basicInformation.Attributes[TPL_FileTags.CREATOR_ATTRIBUTE].Value;
        }



        /// <summary>
        /// Initialize diseases information.
        /// </summary>
        private void InitializeDiseases()
        {
            int i;
            //Get diseases node:
            XmlNode mainDiseasesNode = templateXml.SelectSingleNode(TPL_FileTags.MAIN_DISEASES_PART);
            if (mainDiseasesNode != null)                                                                       //idan check catch
            {
                XmlNode diseasesNode = mainDiseasesNode.SelectSingleNode(TPL_FileTags.DISEASES_PART);
                //Get number of diseases:
                int diseasesCnt = diseasesNode.ChildNodes.Count;

                //Initialize diseases array:
                diseasesArr = new List<DTT_Disease>();

                //Initialize mutations assignment array:
                mutationsAssignment = new List<TPL_MutationAssignment>();
                DTT_Disease disease;
                TPL_MutationAssignment assignment;
                XmlNode diseaseNode;
                int mutationsCnt;
                for (i = 0; i < diseasesCnt; i++)
                {
                    diseaseNode = diseasesNode.ChildNodes[i];
                    disease = new DTT_Disease();
                    disease.DiseaseName = diseaseNode.Attributes[TPL_FileTags.NAME_ATTRIBUTE].Value;
                    disease.DiseaseDescription = diseaseNode.Attributes[TPL_FileTags.DESCRIPTION_ATTRIBUTE].Value;

                    //Get mutation:
                    mutationsCnt = diseaseNode.ChildNodes.Count;
                    //Initialize mutations array:
                    disease.Mutations = new string[mutationsCnt];
                    for (int j = 0; j < mutationsCnt; j++)
                    {
                        disease.Mutations[j] = diseaseNode.ChildNodes[j].Attributes[TPL_FileTags.NAME_ATTRIBUTE].Value;

                    }
                    //Add diseases information:
                    diseasesArr.Add(disease);
                }

                XmlNode assignmentsNode = mainDiseasesNode.SelectSingleNode(TPL_FileTags.MUTATION_ASSIGNMENT_PART);
                for (i = 0; i < assignmentsNode.ChildNodes.Count; i++)
                {
                    assignment = new TPL_MutationAssignment();
                    assignment.MutationName = assignmentsNode.ChildNodes[i].Attributes[TPL_FileTags.NAME_ATTRIBUTE].Value;
                    assignment.CaptureIndex = assignmentsNode.ChildNodes[i].Attributes[TPL_FileTags.MUTATION_CAPTURE_ATTR].Value;
                    assignment.ReporterIndex = assignmentsNode.ChildNodes[i].Attributes[TPL_FileTags.MUTATION_REPORTER_ATTR].Value;

                    mutationsAssignment.Add(assignment);
                }

            }
        }



        /// <summary>
        /// Initialize materials: reagent bottles and reagent packs.
        /// </summary>
        private void InitializeMaterials()
        {
            //Get materials node:
            XmlNode materialsNode = templateXml.SelectSingleNode(TPL_FileTags.MATERIALS_CONFIGURATION_PART);
            //Initialize materials node:
            materialsConfiguration = new TPL_MaterialsConfiguration(materialsNode);

        }



        /// <summary>
        /// Verifies that the recorded and calculated CHECKSUMs match
        /// </summary>
        /// <returns>true if match, false otherwise</returns>
        private bool VerifyChecksum(string checksum)
        {
            string calculatedCRC = this.CalculateChecksum();
            bool res;
            if (checksum == calculatedCRC)
            {
                res = true;
            }
            else
            {
                res = false;
            }
            res = true; //Elena - for now not checking checksum
            return res;
        }



        /// <summary>
        /// Calculate check sum
        /// </summary>
        /// <returns></returns>
        private string CalculateChecksum()
        {
            string myCRC = CRCTool.CalculateCRC(templateXml);
            return myCRC;
        }

        //****************************************************************************



        #region Protected Methods
        /// <summary>
        /// Read template xml from embeded resource.
        /// </summary>
        /// <param name="?"></param>
        protected override bool LoadXmlFile()
        {
            bool result = false;

            try
            {
                string className = this.GetType().ToString();
                string[] classNameParts = className.Split('.');
                className = classNameParts[classNameParts.Length - 1];
                //XML is embedded resourse
                System.Reflection.Assembly myAssembly = Assembly.GetExecutingAssembly();
                string[] allNames = myAssembly.GetManifestResourceNames();
                for (int i = 0; i < allNames.Length; i++)
                {
                    string name = allNames[i];
                }
                System.IO.Stream stream = myAssembly.GetManifestResourceStream("Template." + className + "Template.xml");
                templateDoc = new XmlDocument();
                templateDoc.Load(stream);
                //Get template node:
                templateXml = templateDoc.SelectSingleNode(TPL_FileTags.ROOT_PART);
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// This function loads loading pattern xml file.
        /// </summary>
        /// <returns>true - file was loaded , false - else</returns>
        /// 
        protected override bool LoadLoadingPatternFile()
        {
            bool result = false;
            string className = this.GetType().ToString();
            string[] classNameParts = className.Split('.');
            className = classNameParts[classNameParts.Length - 1];

            loadingPatternFileName = "Template." + className + TPL_FileTags.LOADING_PATTERN_FILE_SUFFIX + ".xml";
            System.Reflection.Assembly myAssembly = Assembly.GetExecutingAssembly();

            System.IO.Stream stream = myAssembly.GetManifestResourceStream(loadingPatternFileName);
            try
            {
                loadingPatternXml = new XmlDocument();
                loadingPatternXml.Load(stream);
                result = true;
                loadingPatternFileName = className + TPL_FileTags.LOADING_PATTERN_FILE_SUFFIX + ".xml";
            }
            catch
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region DataAnalysis Methods


        /// <summary>
        /// Analyze results of all samples and build raw data table
        /// </summary>
        /// <param name="MarkerTruthTable">Truth Table of the specific Marker</param>
        /// <param name="Data">data of signals values</param>
        /// <param name="RawDataReporterColumnNo">amount of RawData Table column</param>
        /// <returns></returns>return the raw data table  

        public string[,] init()
        {
            Initialize();

            int currentPosition = 0;

            // get the max number of raporters and captuers in order to init MarkerName to the correct size 
            int RepMax = 0;
            int CepMax = 0;

            // set the max size of each caputer and reporter for marker  
            for (currentPosition = 0; currentPosition < mutationsAssignment.Count; currentPosition++)
            {
                if (Convert.ToInt32(mutationsAssignment[currentPosition].CaptureIndex) > CepMax)
                {
                    CepMax = Convert.ToInt32(mutationsAssignment[currentPosition].CaptureIndex);
                }

                if (Convert.ToInt32(mutationsAssignment[currentPosition].ReporterIndex) > RepMax)
                {
                    RepMax = Convert.ToInt32(mutationsAssignment[currentPosition].ReporterIndex);
                }
            }

            // init MarkerName table 
            string[,] MarkerName = new string[CepMax, RepMax];

            // set marker names in the table
            for (currentPosition = 0; currentPosition < mutationsAssignment.Count; currentPosition++)
            {
                MarkerName[Convert.ToInt32(mutationsAssignment[currentPosition].CaptureIndex) - 1, Convert.ToInt32(mutationsAssignment[currentPosition].ReporterIndex) - 1] = mutationsAssignment[currentPosition].MutationName;
            }

            return MarkerName;
        }
        public override string[,] Analyze(
                                            double[, , ,] signals,
                                            out string[] GeneralColumnsNames,
                                            out string[] ReporterNames,
                                            out string[] ColumnsNamesOfEachReporter,
                                            out string[] CustomColumnsNames,
                                            string[] SampleNameList,
                                            int[] SamplePositionList,
                                            double[,] referenceSignalsData,
                                            double[,] backgroundSignals
                                            )
        {
            //Headers
            GeneralColumnsNames = new string[9] { "Index", "Sample", "Patogen", "Capture", "Reporter", "Color", "Control", "Value", "Inter" };
            ReporterNames = new string[1] { "" };
            ColumnsNamesOfEachReporter = new string[1] { "" };
            CustomColumnsNames = new string[1] { "" };
            //

            List<AnalyzeResult> analyzeResults = BL.AnalyzeSignals(acronym, signals, init());
            return AnalyzeResult.FromModel(analyzeResults.OrderBy(x => x.Sample).ThenByDescending(x => x.Color).ToList());
            //return(AnalyzeResult.CustomizeResultToLegendArray(analyzeResults));
        }

        public override string[,] reference(double[,] referenceSignals,
                                    double[,] backgroundSignals,
                                    out string[] ReferenceColumnHeaders)
        {
            const int INSTANCESNUMBER = 2;
            const int NUMBEROFCOLORS = 2;
            ReferenceColumnHeaders = CustomValues.GetReferenceColumnHeaders();

            List<Patogen> patogens = Patogen.ToModel(init());
            List<ReferenceSignal> referenceSignalModels = ReferenceSignal.ToModel(referenceSignals, NUMBEROFCOLORS, INSTANCESNUMBER, init().GetLength(0), init().GetLength(1));

            switch (acronym)
            {
                case "GC2":
                    patogens = CustomValues.CustomizeGC2(patogens);
                    break;
            }
            List<PatogenResult> patogenResults = new List<PatogenResult>();

            //adding controls to results
            foreach (var patogen in patogens.Where(x => x.isControl == true))
            {
                decimal avgBkg = referenceSignalModels.Where(x => x.Reporter == patogen.Reporter && x.Reference == patogen.Capture && x.Color == patogen.Color).Average(x => x.Value);//average of 2 instances
                if (avgBkg < 1) avgBkg = 1;
                patogenResults.Add(
                    new PatogenResult()
                    {
                        Reporter = patogen.Reporter,
                        Color = patogen.Color,
                        AvgBkg = avgBkg,
                        isAvgBkg = true,
                        ControlPatogenName = patogen.ControlPatogenName,
                        isControl = true
                    });
            }

            //adding patogens to results
            for (int instance = 1; instance <= INSTANCESNUMBER; instance++)
                foreach (var patogen in patogens.Where(x => x.isControl != true))
                {
                    ReferenceSignal referenceSignal = referenceSignalModels.SingleOrDefault(x => x.Reporter == patogen.Reporter && x.Reference == patogen.Capture && x.Color == patogen.Color && x.Instance == instance);
                    decimal? avgBkg = null;
                    if (patogenResults.Any(x => x.Reporter == patogen.Reporter && x.Color == patogen.Color && x.isControl == true && x.ControlPatogenName == null))
                        avgBkg = patogenResults.SingleOrDefault(x => x.Reporter == patogen.Reporter && x.Color == patogen.Color && x.isControl == true && x.ControlPatogenName == null).AvgBkg;
                    if (patogenResults.Any(x => x.Reporter == patogen.Reporter && x.Color == patogen.Color && x.isControl == true && x.ControlPatogenName != null))
                        avgBkg = patogenResults.SingleOrDefault(x => x.Reporter == patogen.Reporter && x.Color == patogen.Color && x.isControl == true && x.ControlPatogenName == patogen.Name).AvgBkg;

                    if (!avgBkg.HasValue)
                        throw new IndexOutOfRangeException();//no control

                    decimal minBkg = referenceSignal.Value - avgBkg.Value;
                    decimal divBkg = referenceSignal.Value / avgBkg.Value;
                    bool isPass = false;
                    switch (acronym)
                    {
                        case "GC2":
                            isPass = CustomValues.isGC2PatogenPass(patogen.Name, minBkg, divBkg);
                            break;
                    }
                    patogenResults.Add(
                        new PatogenResult()
                        {
                            Capture = patogen.Capture,
                            Color = patogen.Color,
                            DivBkg = divBkg,
                            isControl = false,
                            MinBkg = minBkg,
                            MixSet = instance,
                            Name = patogen.Name,
                            Reporter = patogen.Reporter,
                            IsPass = isPass,
                            IsMixPass = false
                        });
                }

            //sorting
            foreach (var patogen in patogens.Where(x => x.isControl != true))
                if (patogenResults.Where(x => x.Name == patogen.Name).Any(x => x.IsPass == true))
                    patogenResults.Where(x => x.Name == patogen.Name).ToList().ForEach(x => x.IsMixPass = true);

            return PatogenResult.FromModel(patogenResults.OrderBy(x => x.Reporter).ThenByDescending(x => x.Name).ToList());
        }
        public override string[,] Details(double[, , ,] signals, out string[] DetailsColumnHeaders, string[] SampleNameList, int[] SamplePositionList, double[,] referenceSignalsData, double[,] backgroundSignals)
        {
            List<AnalyzeResult> analyzeResults = BL.AnalyzeSignals(acronym, signals, init());
            //header
            List<string> analyzeResultsHeaders = new List<string>();
            analyzeResultsHeaders.Add("sample");
            analyzeResultsHeaders.AddRange(analyzeResults.Select(x => x.Patogen).Distinct().ToList());
            DetailsColumnHeaders = analyzeResultsHeaders.ToArray();
            //
            return BL.ConvertAnalyzeResultToDetailsArray(analyzeResults);
        }






        /// <summary>
        /// Summary results of all samples and build summary data table
        /// </summary>
        /// <param name="MarkerTruthTable">Truth Table of the specific Marker</param>
        /// <param name="Data">data of signals values</param>
        /// <param name="RawDataReporterColumnNo">amount of Summary Data Table column</param>
        /// <returns></returns>return the summary data table  

        public override string[,] Summary(double[, , ,] signals, out string[] SummaryColumnHeaders, string[] SampleNameList, int[] SamplePositionList, double[,] referenceSignalsData, double[,] backgroundSignals)
        {

            //int currentPosition = 0;
            //int currentSample = 0;
            //int row = 0;
            //int column = 0;

            //// get number of pads
            //int numberOfMarkers = signals.GetLength( MARKER_LIST );

            //// get number of samples
            //int numberOfSampleList = signals.GetLength( SAMPLE_LIST );


            //// init size from enum
            //int EnumSizeSummarylColumnsNames = Enum.GetNames( typeof( SummaryColumnsNames ) ).Length;

            //// set headlines of the table        
            //SummaryColumnHeaders = new string[EnumSizeSummarylColumnsNames];

            //// set name for each parameter 
            //for ( currentPosition = 0; currentPosition < SummaryColumnHeaders.Length; currentPosition++ )
            //{
            //    SummaryColumnHeaders[currentPosition] = Enum.GetName( typeof( SummaryColumnsNames ), currentPosition );
            //}

            //// get data from analyze
            //string[,] DetailsDataTable = Details( signals, out DetailsColumnHeaders, SampleNameList, SamplePositionList, referenceSignalsData, backgroundSignals );

            //// set the size of SummaryDataTable 
            //string[,] SummaryDataTable = new string[numberOfSampleList - NUMBER_OF_SPECIAL_SAMPLES, EnumSizeSummarylColumnsNames];


            //for ( column = 0; column < SummaryDataTable.GetLength( COLUMN_SIZE ); column++ )
            //{
            //    for ( row = 0; row < numberOfSampleList - NUMBER_OF_SPECIAL_SAMPLES; row++ )
            //    {
            //        SummaryDataTable[row, column] = "";
            //    }
            //}

            //string result = "";

            //for ( currentSample = 0; currentSample < numberOfSampleList - NUMBER_OF_SPECIAL_SAMPLES; currentSample++ )
            //{
            //    // set index
            //    SummaryDataTable[currentSample, SUMMARY_INDEX_COLUMN] = ( currentSample + 1 ).ToString();

            //    // set position
            //    SummaryDataTable[currentSample, SUMMARY_POSITION_COLUMN] = getWellPos( SamplePositionList[currentSample + NUMBER_OF_SPECIAL_SAMPLES] );

            //    // set sampleID
            //    SummaryDataTable[currentSample, SUMMARY_SAMPLE_NAME_COLUMN] = SampleNameList[currentSample + NUMBER_OF_SPECIAL_SAMPLES];

            //    // check if all 3 columns are negative
            //    if ( DetailsDataTable[currentSample, DETAILS_MRSA_COLUMN] == NEGATIVE && DetailsDataTable[currentSample, DETAILS_VRE_COLUMN] == NEGATIVE && DetailsDataTable[currentSample, DETAILS_KPC_COLUMN] == NEGATIVE )
            //    {
            //        // if all 3 columns are negative, negative will be written only 1 time (in mrsa column for example)
            //        result = DetailsDataTable[currentSample, DETAILS_MRSA_COLUMN];
            //    }

            //    // check if one of the 3 columns is invalid sample
            //    else if ( DetailsDataTable[currentSample, DETAILS_MRSA_COLUMN] == INVALID_SAMPLE || DetailsDataTable[currentSample, DETAILS_VRE_COLUMN] == INVALID_SAMPLE || DetailsDataTable[currentSample, DETAILS_KPC_COLUMN] == INVALID_SAMPLE )
            //    {
            //        // invalid sample will be written only 1 time
            //        result = INVALID_SAMPLE_SUMMARY;
            //    }

            //    // check if one of the 3 columns is pcr failed  
            //    else if ( DetailsDataTable[currentSample, DETAILS_MRSA_COLUMN] == PCR_Failed || DetailsDataTable[currentSample, DETAILS_VRE_COLUMN] == PCR_Failed || DetailsDataTable[currentSample, DETAILS_KPC_COLUMN] == PCR_Failed )
            //    {
            //        // pcr failed will be written only 1 time
            //        result = PCR_Failed;
            //    }

            //    else
            //    {
            //        // if MRSA is negative, don't write it to result 
            //        if ( DetailsDataTable[currentSample, DETAILS_MRSA_COLUMN] != NEGATIVE )
            //        {
            //            result = DetailsDataTable[currentSample, DETAILS_MRSA_COLUMN];
            //            result += ", ";
            //        }

            //        // if VRE is negative, don't write it to result 
            //        if ( DetailsDataTable[currentSample, DETAILS_VRE_COLUMN] != NEGATIVE )
            //        {
            //            result += DetailsDataTable[currentSample, DETAILS_VRE_COLUMN];
            //            result += ", ";
            //        }

            //        // if KPC is negative, don't write it to result 
            //        if ( DetailsDataTable[currentSample, DETAILS_KPC_COLUMN] != NEGATIVE )
            //        {
            //            result += DetailsDataTable[currentSample, DETAILS_KPC_COLUMN];
            //        }

            //    }

            //    SummaryDataTable[currentSample, SUMMARY_RESULT_COLUMN] = result;
            //    result = "";
            //}
            string[,] SummaryDataTable = new string[1, 1];
            SummaryColumnHeaders = new string[1];
            SummaryColumnHeaders[0] = "Not available for this kit.";
            return SummaryDataTable;
        }




        /// <summary>
        /// get well position according the well ID
        /// </summary>
        /// <returns></returns>
        public string getWellPos(int wellId)
        {
            //Well position:
            string WellPos = String.Empty;
            int row = Convert.ToInt32((wellId - 1) / 12/*COLUMNS_IN_SAMPLES_PLATE*/) + (int)'A';
            int column = (wellId - 1) % 12/*COLUMNS_IN_SAMPLES_PLATE*/ + 1;

            char letter = (char)(row);
            string index = column.ToString();

            WellPos = letter + index;
            return WellPos;
        }



        /// <summary>
        /// NVC Ratio
        /// </summary>
        /// <param name="rawDataTable"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns>string result</returns>

        public string NVCRatio(string[,] rawDataTable, int row, int column)
        {
            string result = "";

            double NVC = Convert.ToDouble(rawDataTable[row, column + 1]);
            double control = Convert.ToDouble(rawDataTable[row - 3, column + 1]);

            if (NVC > MINIMUN_SIGNAL_NVC)
            {
                if (NVC / Math.Abs(control) > 4)
                {
                    result = OK;
                }
                else
                {
                    result = NOA;
                }
            }
            else
            {
                result = NOA;
            }

            return result;
        }



        /// <summary>
        /// Calc Ratio
        /// </summary>
        /// <param name="rawDataTable"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>

        public string CalcRatio(string[,] rawDataTable, int row, int column)
        {
            string result = "";
            string markerName = rawDataTable[row, column].ToString();

            string NVC = "";
            double greenSig = 0;
            double control = 0;

            int minimumSignal = 0;
            int minimumRatio = 0;

            // according to the marker name position (row and column) we can set the NVC, green signal and control values 
            switch (markerName)
            {

                case SCCMEC:
                    {
                        NVC = rawDataTable[row + 2, column + 1];
                        greenSig = Convert.ToDouble(rawDataTable[row, column + 1]);
                        control = Convert.ToDouble(rawDataTable[row - 1, column + 1]);
                        minimumSignal = MINIMUN_SIGNAL_SCC_Mec;
                        minimumRatio = MINIMUN_RATIO_SCC_Mec;
                    }
                    break;

                case KPC:
                    {
                        NVC = rawDataTable[row, column + 4];
                        greenSig = Convert.ToDouble(rawDataTable[row, column + 1]);
                        control = Convert.ToDouble(rawDataTable[row - 3, column + 1]);
                        minimumSignal = MINIMUN_SIGNAL_KPC;
                        minimumRatio = MINIMUN_RATIO_KPC;
                    }
                    break;

                case DDL_EFM:
                    {
                        NVC = rawDataTable[row + 1, column + 4];
                        greenSig = Convert.ToDouble(rawDataTable[row, column + 1]);
                        control = Convert.ToDouble(rawDataTable[row - 2, column + 1]);
                        minimumSignal = MINIMUN_SIGNAL_EFM;
                        minimumRatio = MINIMUN_RATIO_EFM;
                    }
                    break;

                case NUC:
                    {
                        NVC = rawDataTable[row + 2, column + 4];
                        greenSig = Convert.ToDouble(rawDataTable[row, column + 1]);
                        control = Convert.ToDouble(rawDataTable[row - 1, column + 1]);
                        minimumSignal = MINIMUN_SIGNAL_nuc;
                        minimumRatio = MINIMUN_RATIO_nuc;
                    }
                    break;

                case PC:
                    {
                        NVC = "";
                        greenSig = Convert.ToDouble(rawDataTable[row, column + 1]);
                        control = Convert.ToDouble(rawDataTable[row - 1, column + 1]);
                        minimumSignal = MINIMUN_SIGNAL_PC;
                        minimumRatio = MINIMUN_RATIO_PC;
                    }
                    break;

                case MECA:
                    {
                        NVC = rawDataTable[row + 2, column + 7];
                        greenSig = Convert.ToDouble(rawDataTable[row, column + 1]);
                        control = Convert.ToDouble(rawDataTable[row - 1, column + 1]);
                        minimumSignal = MINIMUN_SIGNAL_MecA;
                        minimumRatio = MINIMUN_RATIO_MecA;
                    }
                    break;

                case DDL_EFS:
                    {
                        NVC = rawDataTable[row + 1, column + 7];
                        greenSig = Convert.ToDouble(rawDataTable[row, column + 1]);
                        control = Convert.ToDouble(rawDataTable[row - 2, column + 1]);
                        minimumSignal = MINIMUN_SIGNAL_EFS;
                        minimumRatio = MINIMUN_RATIO_EFS;
                    }
                    break;

                case PHOE:
                    {
                        NVC = rawDataTable[row, column + 7];
                        greenSig = Convert.ToDouble(rawDataTable[row, column + 1]);
                        control = Convert.ToDouble(rawDataTable[row - 3, column + 1]);
                        minimumSignal = MINIMUN_SIGNAL_PhoE;
                        minimumRatio = MINIMUN_RATIO_PhoE;
                    }
                    break;

                case SPA:
                    {
                        NVC = rawDataTable[row + 2, column - 2];
                        greenSig = Convert.ToDouble(rawDataTable[row, column + 1]);
                        control = Convert.ToDouble(rawDataTable[row - 1, column + 1]);
                        minimumSignal = MINIMUN_SIGNAL_SPA;
                        minimumRatio = MINIMUN_RATIO_SPA;
                    }
                    break;

                case VANAB:
                    {
                        NVC = rawDataTable[row + 1, column - 2];
                        greenSig = Convert.ToDouble(rawDataTable[row, column + 1]);
                        control = Convert.ToDouble(rawDataTable[row - 2, column + 1]);
                        minimumSignal = MINIMUN_SIGNAL_VAN_A_B;
                        minimumRatio = MINIMUN_RATIO_VAN_A_B;
                    }
                    break;

            }


            if (NVC == INVALID)
            {
                result = INVALID;
            }

            else if (greenSig > minimumSignal)
            {
                if ((greenSig / Math.Abs(control)) > minimumRatio)
                {
                    result = (greenSig / Math.Abs(control)).ToString();
                }
                else
                {
                    result = NONE;
                }
            }

            else
            {
                result = NONE;
            }


            if ((result != INVALID) && (result != NONE))
            {
                // based on result value the fixed number of decimal places behind the dot will be chosen.
                if (Convert.ToDouble(result) < 10)
                {
                    // 2 digits after the dot. 
                    result = (Math.Round(Convert.ToDouble(result), TWO_DECIMAL)).ToString();
                }
                else if (Convert.ToDouble(result) < 100)
                {
                    // 1 digit after the dot.
                    result = (Math.Round(Convert.ToDouble(result), ONE_DECIMAL)).ToString();
                }
                else
                {
                    // no digits after the dot.
                    result = (Math.Round(Convert.ToDouble(result), NO_DECIMAL)).ToString();
                }

            }

            return result;
        }




        /// <summary>
        /// Calc MRSA
        /// </summary>
        /// <param name="rawDataTable"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>

        public string CalcMRSA(string[,] rawDataTable, int row, int column)
        {
            string result = "";

            double number = 0;

            bool checkIfNumeric = false;

            int MRSA = 0;

            string MecA = rawDataTable[row, column + 2];

            string nuc = rawDataTable[row, column + 5];

            string SCCMec = rawDataTable[row, column + 8];

            string SPA = rawDataTable[row, column + 11];

            // check if the string in SPA is numeric
            checkIfNumeric = double.TryParse(SPA, out number);
            if (checkIfNumeric == true)
            {
                MRSA = MRSA + 1;
            }

            // check if the string in SCCMec is numeric
            checkIfNumeric = double.TryParse(SCCMec, out number);
            if (checkIfNumeric == true)
            {
                MRSA = MRSA + 2;
            }

            // check if the string in nuc is numeric
            checkIfNumeric = double.TryParse(nuc, out number);
            if (checkIfNumeric == true)
            {
                MRSA = MRSA + 4;
            }

            // check if the string in mecA is numeric
            checkIfNumeric = double.TryParse(MecA, out number);
            if (checkIfNumeric == true)
            {
                MRSA = MRSA + 8;
            }

            // check MRSA value and set the result

            if (MRSA == 0)
            {
                result = NEGATIVE;
            }

            else if ((MRSA == 1) || (MRSA == 4) || (MRSA == 5))
            {
                result = MSSA;
            }

            else if ((MRSA == 2) || (MRSA == 3) || (MRSA == 6) || (MRSA == 7))
            {
                result = SCC_POSITIVE_MSSA;
            }

            else if (MRSA == 8)
            {
                result = CON_MR;
            }

            else if ((MRSA == 9) || (MRSA == 12) || (MRSA == 13))
            {
                result = CON_MR_MSSA;
            }

            else if (MRSA == 10)
            {
                result = POSSIBLE_MRSA;
            }

            else if ((MRSA == 11) || (MRSA == 14) || (MRSA == 15))
            {
                result = RESULT_MRSA;
            }

            return result;
        }



        /// <summary>
        /// Calc VRE
        /// </summary>
        /// <param name="rawDataTable"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>

        public string CalcVRE(string[,] rawDataTable, int row, int column)
        {
            string result = "";

            double number = 0;

            bool checkIfNumeric = false;

            int VRE = 0;

            string EFS = rawDataTable[row, column + 2];

            string EFM = rawDataTable[row, column + 5];

            string VAN_A_B = rawDataTable[row, column + 11];

            // check if the string in VAN A B is numeric
            checkIfNumeric = double.TryParse(VAN_A_B, out number);
            if (checkIfNumeric == true)
            {
                VRE = VRE + 1;
            }

            // check if the string in EFM is numeric
            checkIfNumeric = double.TryParse(EFM, out number);
            if (checkIfNumeric == true)
            {
                VRE = VRE + 2;
            }

            // check if the string in EFS is numeric
            checkIfNumeric = double.TryParse(EFS, out number);
            if (checkIfNumeric == true)
            {
                VRE = VRE + 4;
            }

            // check VRE value and set the result

            if (VRE == 0)
            {
                result = NEGATIVE;
            }

            else if (VRE == 1)
            {
                result = NONE_EFS_EFM_VAN;
            }

            else if (VRE == 2)
            {
                result = EFM;
            }

            else if (VRE == 3)
            {
                result = VRE_EFM;
            }

            else if (VRE == 4)
            {
                result = EFS;
            }

            else if (VRE == 5)
            {
                result = VRE_EFS;
            }

            else if (VRE == 6)
            {
                result = EFS_EFM;
            }

            else if (VRE == 7)
            {
                result = VRE_EFM_EFS;
            }

            return result;
        }




        /// <summary>
        /// Calc KPC
        /// </summary>
        /// <param name="rawDataTable"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>

        public string CalcKPC(string[,] rawDataTable, int row, int column)
        {
            string result = "";

            double number = 0;

            bool checkIfNumeric = false;

            int KPCnum = 0;

            string PhoE = rawDataTable[row, column + 2];

            string KPC = rawDataTable[row, column + 5];

            // check if the string in KPC is numeric
            checkIfNumeric = double.TryParse(KPC, out number);
            if (checkIfNumeric == true)
            {
                KPCnum = KPCnum + 1;
            }

            // check if the string in PhoE is numeric
            checkIfNumeric = double.TryParse(PhoE, out number);
            if (checkIfNumeric == true)
            {
                KPCnum = KPCnum + 2;
            }

            // check VRE value and set the result

            if (KPCnum == 0)
            {
                result = NEGATIVE;
            }

            else if (KPCnum == 1)
            {
                result = KPC_NON_KP;
            }

            else if (KPCnum == 2)
            {
                result = KP;
            }

            else if (KPCnum == 3)
            {
                result = KP_KPC;
            }

            return result;
        }


        #endregion

    }

}
