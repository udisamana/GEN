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

namespace ST6 // new kit
{
    public class ST6 : TPL_BaseTemplate // new kit
    {


        // NVC is not calculated in CalcRatio so number of markers is counted without him
        public const int NUMBER_OF_MARKERS_PER_SAPMLE_WITHOUT_NVC = 9;

        // total number of markers per sample include NVC (+1)
        public const int NUMBER_OF_MARKERS_PER_SAPMLE = NUMBER_OF_MARKERS_PER_SAPMLE_WITHOUT_NVC + 1;

        // const of marker names

        public const string ExtractionControl = "Extraction Control";
        public const string Negative = "Negative";
        public const string Invalid = "Invalid";
        public const int INSTANCESNUMBER = 2;
        public const int NUMBEROFCOLORS = 2;
        public List<PatogenReferanceResult> patogenRefResults = new List<PatogenReferanceResult>();
        public List<MutRefResult> mutRfResults = new List<MutRefResult>();



        public const string PBPB = "pbpb";
        public const string ORF1 = "orf1";




        public const string BGLOBIN = "B-Globin";
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


        /// <summary>
        /// Analyze results of all samples and build raw data table
        /// </summary>
        /// <param name="MarkerTruthTable">Truth Table of the specific Marker</param>
        /// <param name="Data">data of signals values</param>
        /// <param name="RawDataReporterColumnNo">amount of RawData Table column</param>
        /// <returns></returns>return the raw data table  





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

        public const string POS = "Pos";
        public const string NEG = "-";
        public string kitType = string.Empty;
        public int reporters;
        public string[,] init()
        {
            Initialize();

            int currentPosition = 0;

            // get the max number of raporters and captuers in order to init MarkerName to the correct size 
            reporters = 0;
            int CepMax = 0;

            // set the max size of each caputer and reporter for marker  
            for (currentPosition = 0; currentPosition < mutationsAssignment.Count; currentPosition++)
            {
                if (Convert.ToInt32(mutationsAssignment[currentPosition].CaptureIndex) > CepMax)
                {
                    CepMax = Convert.ToInt32(mutationsAssignment[currentPosition].CaptureIndex);
                }


                if (Convert.ToInt32(mutationsAssignment[currentPosition].ReporterIndex) > reporters)
                {
                    reporters = Convert.ToInt32(mutationsAssignment[currentPosition].ReporterIndex);
                }
            }

            // init MarkerName table 
            string[,] MarkerName = new string[CepMax, reporters];

            // set marker names in the table
            for (currentPosition = 0; currentPosition < mutationsAssignment.Count; currentPosition++)
            {
                MarkerName[Convert.ToInt32(mutationsAssignment[currentPosition].CaptureIndex) - 1, Convert.ToInt32(mutationsAssignment[currentPosition].ReporterIndex) - 1] = mutationsAssignment[currentPosition].MutationName;
            }

            if (acronym == "CFN" || acronym == "TSD" || acronym == "AJP")
                kitType = "Mutation";
            if (acronym == "GC2" || acronym == "GCQ" || acronym == "GI2" || acronym == "ST6")//new kit, classify new acronim
                kitType = "Pathogen";

            return MarkerName;
        }
        public override string[,] Analyze(double[, , ,] signals, out string[] GeneralColumnsNames, out string[] ReporterNames, out string[] ColumnsNamesOfEachReporter, out string[] CustomColumnsNames, string[] SampleNameList, int[] SamplePositionList, double[,] referenceSignalsData, double[,] backgroundSignals)
        {


            GeneralColumnsNames = new string[2];
            ReporterNames = new string[1] { "" };
            ColumnsNamesOfEachReporter = new string[1] { "" };
            CustomColumnsNames = new string[1] { "" };
            List<Target> targets = Target.ToModel(init());
            List<Signal> signalList = Signal.ToModel(signals);



            if (kitType == "Mutation")
            {
                GeneralColumnsNames = new string[2] { "Position", "Sample" };

                ReporterNames = Enumerable.Range(1, reporters).Select(x => "Reporter Mix " + x).ToArray();

                ColumnsNamesOfEachReporter = new string[5] { "Marker", "Green", "Red", "ScaledG/R", "Call" };
                List<MutRes> MutRess = BL.MutationAnalyze(signalList, targets, acronym, SampleNameList);
                return (BL.ConvertAnalyzeResultToLegendAnalyzeArray(MutRess, SamplePositionList));

            }

            if (kitType == "Pathogen")
            {
                GeneralColumnsNames = new string[2] { "Position", "Sample" };

                ReporterNames = Enumerable.Range(1, reporters).Select(x => "Reporter Mix " + x).ToArray();

                ColumnsNamesOfEachReporter = new string[5] { "Target", "Color", "Value", "Control Value", "Inter" };
                
                List<PatRes> PatRess = BL.PathogenAnalyze(signalList, targets, acronym, SampleNameList);

                return (BL.ConvertAnalyzeResultToLegendAnalyzeArray(PatRess, SamplePositionList, CustomValues.GetControlTarget(acronym)));
            }

            if (kitType == "Pathogen_old")
            {
                GeneralColumnsNames = new string[9] { "Index", "Sample", "Patogen", "Capture", "Reporter", "Color", "Control", "Value", "Inter" };
                List<PatRes> PatRess = BL.PathogenAnalyze(signalList, targets, acronym, SampleNameList);
                return PatRes.FromModel(PatRess.OrderBy(x => x.SampleIndex).ThenByDescending(x => x.Color).ToList());
            }

            if (kitType == "Test_Mutation")
            {
                GeneralColumnsNames = new string[10] { "#", "Sample", "Rep", "Name", "Green", "GreenC", "Red", "RedC", "ScaledGR", "Call" };
                List<MutRes> Mut_Ress = BL.MutationAnalyze(signalList, targets, acronym);
                return (BL.ConvertAnalyzeResultToAnalyzeArray(Mut_Ress));
            }

            string[,] s = new string[1, 1];
            return s;




        }
        public override string[,] reference(double[,] referenceSignals, double[,] backgroundSignals, out string[] ReferenceColumnHeaders)
        {
            ReferenceColumnHeaders = new string[1];
            ReferenceColumnHeaders[0] = "Headers Not Defined";
            string[,] s = new string[1, 1];
            s[0, 0] = "Acronim Not Defined";

            init();

            ReferenceColumnHeaders = CustomValues.GetReferenceColumnHeaders();
            List<Target> patogens = Target.ToModel(init());
            List<ReferenceSignal> referenceSignalModels = ReferenceSignal.ToModel(referenceSignals, NUMBEROFCOLORS, INSTANCESNUMBER, init().GetLength(0), init().GetLength(1));

            if (kitType == "Pathogen")
            {
                if (acronym == "GC2")
                    patogens = CustomValues.CustomizeGC2(patogens);
                if (acronym == "ST6")// new kit
                    patogens = CustomValues.CustomizeST6(patogens);

                patogenRefResults = new List<PatogenReferanceResult>();

                //adding controls to results
                foreach (var patogen in patogens.Where(x => x.isControl == true))
                {
                    decimal avgBkg = referenceSignalModels.Where(x => x.Reporter == patogen.Reporter && x.Reference == patogen.Capture && x.Color == patogen.Color).Average(x => x.Value);//average of 2 instances
                    if (avgBkg < 1) avgBkg = 1;
                    patogenRefResults.Add(
                        new PatogenReferanceResult()
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
                        if (patogenRefResults.Any(x => x.Reporter == patogen.Reporter && x.Color == patogen.Color && x.isControl == true && x.ControlPatogenName == null))
                            avgBkg = patogenRefResults.SingleOrDefault(x => x.Reporter == patogen.Reporter && x.Color == patogen.Color && x.isControl == true && x.ControlPatogenName == null).AvgBkg;
                        if (patogenRefResults.Any(x => x.Reporter == patogen.Reporter && x.Color == patogen.Color && x.isControl == true && x.ControlPatogenName != null))
                            avgBkg = patogenRefResults.SingleOrDefault(x => x.Reporter == patogen.Reporter && x.Color == patogen.Color && x.isControl == true && x.ControlPatogenName == patogen.Name).AvgBkg;

                        if (!avgBkg.HasValue)
                            throw new IndexOutOfRangeException();//no control

                        decimal minBkg = referenceSignal.Value - avgBkg.Value;
                        decimal divBkg = referenceSignal.Value / avgBkg.Value;

                        bool isPass = false;
                        if (acronym == "GC2" || acronym == "GCT" || acronym == "GCQ" || acronym == "GI2")
                            isPass = CustomValues.isGC2PatogenPass(patogen.Name, minBkg, divBkg);

                        if (acronym == "ST6")// new kit
                            isPass = CustomValues.isST6PatogenPass(patogen.Name, minBkg, divBkg);

                        patogenRefResults.Add(
                            new PatogenReferanceResult()
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
                    if (patogenRefResults.Where(x => x.Name == patogen.Name).Any(x => x.IsPass == true))
                        patogenRefResults.Where(x => x.Name == patogen.Name).ToList().ForEach(x => x.IsMixPass = true);


                return PatogenReferanceResult.FromModel(patogenRefResults.OrderBy(x => x.Reporter).ThenByDescending(x => x.Name).ToList());
            }
            if (kitType == "Mutation")
            {
                //double avgGreen, avgRed;
                bool haveBkg = backgroundSignals.GetLongLength(0) != 0;

                //Define an array for reference column headers
                ReferenceColumnHeaders = new string[8];
                //Set values for reference column headers
                ReferenceColumnHeaders[0] = "Reporter Mix/Set";
                ReferenceColumnHeaders[1] = "Mutation";
                ReferenceColumnHeaders[2] = "G-bkg";
                ReferenceColumnHeaders[3] = "G/bkg";
                ReferenceColumnHeaders[4] = "R-bkg";
                ReferenceColumnHeaders[5] = "R/bkg";
                ReferenceColumnHeaders[6] = "Pass/Fail";
                ReferenceColumnHeaders[7] = "Pass/Fail";

                //sandBox
                string[,] MarkerName = init();
                int captures = MarkerName.GetLength(0);
                int reporters = MarkerName.GetLength(1);
                List<string> sets = new List<string>() { "Set A", "Set B" };
                const int headers = 8;

                for (int capture = 0; capture < captures; capture++)
                    for (var reporter = 0; reporter < reporters; reporter++)
                        for (int set = 0; set < sets.Count; set++)
                            if (MarkerName[capture, reporter] != string.Empty && MarkerName[capture, reporter] != "Control" && MarkerName[capture, reporter] != null)
                            {
                                MutRefResult refResult = new MutRefResult();
                                refResult.Reporter = reporter;
                                refResult.MixSet = set.ToString();
                                refResult.Mutation = MarkerName[capture, reporter];
                                if (haveBkg)
                                {
                                    refResult.GMinusBkg = referenceSignals[set * reporters * captures + reporters * capture + reporter, 1] - BL.trnc((backgroundSignals[reporter, 1] + backgroundSignals[reporter + reporters, 1]) / 2);
                                    refResult.GDivBkg = referenceSignals[set * reporters * captures + reporters * capture + reporter, 1] / BL.trnc((backgroundSignals[reporter, 1] + backgroundSignals[reporter + reporters, 1]) / 2);
                                    refResult.RMinBkg = referenceSignals[set * reporters * captures + reporters * capture + reporter, 0] - BL.trnc((backgroundSignals[reporter, 0] + backgroundSignals[reporter + reporters, 0]) / 2);
                                    refResult.RDivBkg = referenceSignals[set * reporters * captures + reporters * capture + reporter, 0] / BL.trnc((backgroundSignals[reporter, 0] + backgroundSignals[reporter + reporters, 0]) / 2);
                                }
                                else
                                {
                                    refResult.GMinusBkg = referenceSignals[set * reporters * captures + reporters * capture + reporter, 1] - BL.trnc(BL.getReporterControlSignal(MarkerName, referenceSignals, reporter, captures, reporters, 1) / 2);
                                    refResult.GDivBkg = referenceSignals[set * reporters * captures + reporters * capture + reporter, 1] / BL.trnc(BL.getReporterControlSignal(MarkerName, referenceSignals, reporter, captures, reporters, 1) / 2);
                                    refResult.RMinBkg = referenceSignals[set * reporters * captures + reporters * capture + reporter, 0] - BL.trnc(BL.getReporterControlSignal(MarkerName, referenceSignals, reporter, captures, reporters, 0) / 2);
                                    refResult.RDivBkg = referenceSignals[set * reporters * captures + reporters * capture + reporter, 0] / BL.trnc(BL.getReporterControlSignal(MarkerName, referenceSignals, reporter, captures, reporters, 0) / 2);
                                }
                                refResult.IsReporterPass = true;
                                refResult.IsMutantPass = BL.IsMutationPass(refResult);
                                mutRfResults.Add(refResult);
                            }


                for (int reporter = 0; reporter < reporters; reporter++)
                    if (mutRfResults.Where(x => x.Reporter == reporter).ToList().Exists(x => x.IsMutantPass == false))
                        foreach (var item in mutRfResults.Where(x => x.Reporter == reporter).ToList())
                            item.IsReporterPass = false;

                for (int reporter = 0; reporter < reporters; reporter++)
                {
                    double avgBkg;
                    if (haveBkg)
                        avgBkg = BL.trnc((backgroundSignals[reporter, 1] + backgroundSignals[reporter + reporters, 1]) / 2);
                    else
                        avgBkg = BL.trnc(BL.getReporterControlSignal(MarkerName, referenceSignals, reporter, captures, reporters, 1) / 2);

                    mutRfResults.Add(new MutRefResult()
                    {
                        Reporter = reporter,
                        isBackround = true,
                        GMinusBkg = avgBkg,
                        Mutation = "Avg. Green"
                    });
                    if (haveBkg)
                        avgBkg = BL.trnc((backgroundSignals[reporter, 0] + backgroundSignals[reporter + reporters, 0]) / 2);
                    else
                        avgBkg = BL.trnc(BL.getReporterControlSignal(MarkerName, referenceSignals, reporter, captures, reporters, 0) / 2);

                    mutRfResults.Add(new MutRefResult()
                    {
                        Reporter = reporter,
                        isBackround = true,
                        GMinusBkg = avgBkg,
                        Mutation = "Avg. Red"
                    });
                }

                List<MutRefResult> sortedRefResults = new List<MutRefResult>();

                for (int reporter = 0; reporter < reporters; reporter++)
                {
                    sortedRefResults.AddRange(mutRfResults.Where(x => x.isBackround != true && x.Reporter == reporter && x.MixSet == "0"));
                    sortedRefResults.AddRange(mutRfResults.Where(x => x.isBackround != true && x.Reporter == reporter && x.MixSet == "1"));
                    sortedRefResults.AddRange(mutRfResults.Where(x => x.isBackround == true && x.Reporter == reporter));
                }


                //string[,] NewReferenceDataTable = BL.ReftoArray(sortedRefResults, headers);
                return BL.ReftoArray(sortedRefResults, headers);

            }

            return s;

        }
        public override string[,] Details(double[, , ,] signals, out string[] DetailsColumnHeaders, string[] SampleNameList, int[] SamplePositionList, double[,] referenceSignalsData, double[,] backgroundSignals)
        {
            DetailsColumnHeaders = new string[1];
            
            List<Target> targets = Target.ToModel(init());
            List<Signal> signalList = Signal.ToModel(signals);
            List<string> analyzeResultsHeaders = new List<string>();


            if (kitType == "Pathogen")
            {

                List<PatRes> PatRess = BL.PathogenAnalyze(signalList, targets, acronym);
                List<string> targetsNames = GetSortedTargets(PatRess, acronym);
                DetailsColumnHeaders = GetTrgetsFullName(DetailsColumnHeaders, analyzeResultsHeaders, targetsNames);

                List<string> targetsforAnalyze = GetSortedTargets(PatRess, acronym);
                return BL.ConvertAnalyzeResultToDetailsArrayPathogen(PatRess, targetsforAnalyze, acronym);

            }
            if (kitType == "Mutation")
            {
                List<MutRes> MutRess = BL.MutationAnalyze(signalList, targets, acronym);
                //header
                analyzeResultsHeaders.Add("Position");
                analyzeResultsHeaders.Add("Sample");
                analyzeResultsHeaders.AddRange(MutRess.Select(x => x.TargetName).Distinct().ToList());
                DetailsColumnHeaders = analyzeResultsHeaders.ToArray();
                //

                return BL.ConvertAnalyzeResultToDetailsArrayMutation(MutRess, SampleNameList, SamplePositionList);



            }
            
            string[,] s = new string[1, 1];
            return s;

            /*

              
             */
        }
        private string[] GetTrgetsFullName(string[] DetailsColumnHeaders, List<string> analyzeResultsHeaders, List<string> targetsNames)
        {
            List<string> targetsFullNames = targetsNames;
            if (acronym == "ST6")
            {
                targetsFullNames.Remove(PBPB);
                targetsFullNames.Remove(ORF1);
            }
            targetsFullNames = CustomValues.GetTargetFullNameByTarget(targetsNames);
            analyzeResultsHeaders.Add("Sample");
            analyzeResultsHeaders.AddRange(targetsFullNames);
            analyzeResultsHeaders.Add("");
            DetailsColumnHeaders = analyzeResultsHeaders.ToArray();
            return DetailsColumnHeaders;
        }
        private static List<string> GetSortedTargets(List<PatRes> PatRess, string acronym = null)
        {
            List<string> targetsNames = PatRess.Select(x => x.TargetName).Distinct().ToList();


            List<string> sortedBacteria = new List<string>();
            foreach (var target in targetsNames)
                if (CustomValues.GetTargetTypeByTarget(target) == TargetType.bacteria)
                    sortedBacteria.Add(target);
            sortedBacteria.OrderBy(x=>x).ToList();

            List<string> sortedParasite = new List<string>();
            foreach (var target in targetsNames)
                if (CustomValues.GetTargetTypeByTarget(target) == TargetType.Parasite)
                    sortedParasite.Add(target);
            sortedParasite.OrderBy(x => x).ToList();

            List<string> sortedUndefined = new List<string>();
            foreach (var target in targetsNames)
                if (CustomValues.GetTargetTypeByTarget(target) == TargetType.undefined)
                    sortedUndefined.Add(target);
            sortedUndefined.OrderBy(x => x).ToList();

            List<string> sortedTargetNames = new List<string>();
            sortedTargetNames.AddRange(sortedBacteria);
            sortedTargetNames.AddRange(sortedParasite);
            sortedTargetNames.AddRange(sortedUndefined);

            if (acronym == "ST6")
            {
                sortedTargetNames.Remove(BGLOBIN);
                sortedTargetNames.Insert(0, BGLOBIN);
            }

            return sortedTargetNames;
        }
        public override string[,] Summary(double[, , ,] signals, out string[] SummaryColumnHeaders, string[] SampleNameList, int[] SamplePositionList, double[,] referenceSignalsData, double[,] backgroundSignals)
        {
            List<Target> targets = Target.ToModel(init());
            List<Signal> signalList = Signal.ToModel(signals);
            List<PatRes> PatRess = new List<PatRes>();
            List<MutRes> MutRess = new List<MutRes>();
            if (kitType == "Pathogen")
            {
                PatRess = BL.PathogenAnalyze(signalList, targets, acronym, SampleNameList);
            }
            if (kitType == "Mutation")
            {
                MutRess = BL.MutationAnalyze(signalList, targets, acronym, SampleNameList);
            }
            Dictionary<string, string> ress = new Dictionary<string, string>();

            SummaryColumnHeaders = new string[2];
            SummaryColumnHeaders[0] = "Sample";
            SummaryColumnHeaders[1] = "Target";

            foreach (var sample in SampleNameList)
            {
                string sampleTargets = string.Empty;
                if (kitType == "Pathogen")
                {
                    sampleTargets = getTargetSummery(PatRess, sample, sampleTargets);
                }
                if (kitType == "Mutation")
                {
                    sampleTargets = getTargetSummery(MutRess, sample, sampleTargets);
                }

                if (sampleTargets != string.Empty)
                    sampleTargets = sampleTargets.Remove(sampleTargets.Length - 2);

                if (kitType == "Mutation")
                {
                    if (sampleTargets == string.Empty)
                        sampleTargets = "No Mutation Detected";
                }

                if (kitType == "Pathogen")
                {
                    if (sampleTargets == string.Empty)
                        sampleTargets = INVALID;
                }


                if (sampleTargets == ExtractionControl)
                    sampleTargets = Negative;



                int index = sampleTargets.IndexOf(ExtractionControl);//remove the extraction control from targets
                if (index > 0)
                    sampleTargets = sampleTargets.Remove(index, ExtractionControl.Length);

                ress.Add(sample, sampleTargets);
            }

            return (BL.ConvertDictionaryTo2dStringArray(ress));

        }

        private string getTargetSummery(List<PatRes> PatRess, string sample, string sampleTargets)
        {
            foreach (var patRes in PatRess.Where(x => x.SampleName == sample && x.Inter == POS))
            {
                sampleTargets = checkIfTheTargetReporterHaveInTheReferance2FalseInTheSameTarget(sampleTargets, patRes);
            }
            return sampleTargets;
        }
        private string getTargetSummery(List<MutRes> MutRess, string sample, string sampleTargets)
        {
            foreach (var mutRes in MutRess.Where(x => x.SampleName == sample && x.Call != "-"))
            {
                sampleTargets += mutRes.TargetName;
                sampleTargets += ", ";
            }
            return sampleTargets;
        }
        private string checkIfTheTargetReporterHaveInTheReferance2FalseInTheSameTarget(string sampleTargets, PatRes patRes)
        {
            sampleTargets += CustomValues.GetTargetFullNameByTarget(patRes.TargetName);

            //find false referance
            var refRess = patogenRefResults.Where(x => x.Reporter == patRes.Reporter && x.isControl == false).ToList();

            if (refRess.FirstOrDefault().IsMixPass == false)
            {
                sampleTargets += "(Ref Fail RE-TEST: ";

                var duplicateTargets = refRess.GroupBy(x => x.Name)
               .Where(g => g.Count() > 1)
               .Select(y => y.Key)
               .ToList();
                var duplicateTargetsString = String.Join(", ", duplicateTargets.ToArray());

                sampleTargets += duplicateTargetsString;

                sampleTargets += ")";
            }
            //

            sampleTargets += ", ";

            return sampleTargets;
        }
        private string checkIfTheTargetReporterHaveInTheReferance2FalseInTheSameTarget(string sampleTargets, MutRes mutRes)
        {
            sampleTargets += CustomValues.GetTargetFullNameByTarget(mutRes.TargetName);

            //find false referance
            var refRess = mutRfResults.Where(x => x.Reporter == mutRes.Reporter);

            if (refRess.Any(x => x.IsMutantPass == false))
            {
                sampleTargets += "(RE-TEST)";
            }
            //

            sampleTargets += ", ";

            return sampleTargets;
        }

    }

}


/*
           int currentPosition = 0;
           int currentSample = 0;
           int row = 0;
           int column = 0;

           // get number of pads
           int numberOfMarkers = signals.GetLength(MARKER_LIST);

           // get number of samples
           int numberOfSampleList = signals.GetLength(SAMPLE_LIST);


           // init size from enum
           int EnumSizeSummarylColumnsNames = Enum.GetNames(typeof(SummaryColumnsNames)).Length;

           // set headlines of the table        
           SummaryColumnHeaders = new string[EnumSizeSummarylColumnsNames];

           // set name for each parameter 
           for (currentPosition = 0; currentPosition < SummaryColumnHeaders.Length; currentPosition++)
           {
               SummaryColumnHeaders[currentPosition] = Enum.GetName(typeof(SummaryColumnsNames), currentPosition);
           }

           // get data from analyze
           string[,] DetailsDataTable = Details(signals, out DetailsColumnHeaders, SampleNameList, SamplePositionList, referenceSignalsData, backgroundSignals);

           // set the size of SummaryDataTable 
           string[,] SummaryDataTable = new string[numberOfSampleList - NUMBER_OF_SPECIAL_SAMPLES, EnumSizeSummarylColumnsNames];


           for (column = 0; column < SummaryDataTable.GetLength(COLUMN_SIZE); column++)
           {
               for (row = 0; row < numberOfSampleList - NUMBER_OF_SPECIAL_SAMPLES; row++)
               {
                   SummaryDataTable[row, column] = "";
               }
           }

           string result = "";

           for (currentSample = 0; currentSample < numberOfSampleList - NUMBER_OF_SPECIAL_SAMPLES; currentSample++)
           {
               // set index
               SummaryDataTable[currentSample, SUMMARY_INDEX_COLUMN] = (currentSample + 1).ToString();

               // set position
               SummaryDataTable[currentSample, SUMMARY_POSITION_COLUMN] = getWellPos(SamplePositionList[currentSample + NUMBER_OF_SPECIAL_SAMPLES]);

               // set sampleID
               SummaryDataTable[currentSample, SUMMARY_SAMPLE_NAME_COLUMN] = SampleNameList[currentSample + NUMBER_OF_SPECIAL_SAMPLES];

               // check if all 3 columns are negative
               if (DetailsDataTable[currentSample, DETAILS_MRSA_COLUMN] == NEGATIVE && DetailsDataTable[currentSample, DETAILS_VRE_COLUMN] == NEGATIVE && DetailsDataTable[currentSample, DETAILS_KPC_COLUMN] == NEGATIVE)
               {
                   // if all 3 columns are negative, negative will be written only 1 time (in mrsa column for example)
                   result = DetailsDataTable[currentSample, DETAILS_MRSA_COLUMN];
               }

               // check if one of the 3 columns is invalid sample
               else if (DetailsDataTable[currentSample, DETAILS_MRSA_COLUMN] == INVALID_SAMPLE || DetailsDataTable[currentSample, DETAILS_VRE_COLUMN] == INVALID_SAMPLE || DetailsDataTable[currentSample, DETAILS_KPC_COLUMN] == INVALID_SAMPLE)
               {
                   // invalid sample will be written only 1 time
                   result = INVALID_SAMPLE_SUMMARY;
               }

               // check if one of the 3 columns is pcr failed  
               else if (DetailsDataTable[currentSample, DETAILS_MRSA_COLUMN] == PCR_Failed || DetailsDataTable[currentSample, DETAILS_VRE_COLUMN] == PCR_Failed || DetailsDataTable[currentSample, DETAILS_KPC_COLUMN] == PCR_Failed)
               {
                   // pcr failed will be written only 1 time
                   result = PCR_Failed;
               }

               else
               {
                   // if MRSA is negative, don't write it to result 
                   if (DetailsDataTable[currentSample, DETAILS_MRSA_COLUMN] != NEGATIVE)
                   {
                       result = DetailsDataTable[currentSample, DETAILS_MRSA_COLUMN];
                       result += ", ";
                   }

                   // if VRE is negative, don't write it to result 
                   if (DetailsDataTable[currentSample, DETAILS_VRE_COLUMN] != NEGATIVE)
                   {
                       result += DetailsDataTable[currentSample, DETAILS_VRE_COLUMN];
                       result += ", ";
                   }

                   // if KPC is negative, don't write it to result 
                   if (DetailsDataTable[currentSample, DETAILS_KPC_COLUMN] != NEGATIVE)
                   {
                       result += DetailsDataTable[currentSample, DETAILS_KPC_COLUMN];
                   }

               }

               SummaryDataTable[currentSample, SUMMARY_RESULT_COLUMN] = result;
               result = "";
           }


           return SummaryDataTable;

       string[,] SummaryDataTable1 = new string[1, 1];
       SummaryColumnHeaders = new string[1];
       SummaryColumnHeaders[0] = "Not available for this kit.";
       return SummaryDataTable1;
            
       */
