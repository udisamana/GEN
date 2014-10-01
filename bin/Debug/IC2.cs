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

using TemplateLib;
using DataTypes.ProtocolResults;
using SystemConfiguration;
using DataTypes.Template;
using CRC;
using System.Collections;


namespace IC2
{  

    public class IC2 : TPL_BaseTemplate
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
        enum AnalyzeReporterNames { ReporterMix1, ReporterMix2, ReporterMix3, ReporterMix4 };
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
         
        enum ReferenceColumnsNames { Reporter, Marker, Reference1, Reference2, PassFail };

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
            if ( result == true )
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
            if ( result == true )
            {
                //Get checksum
                string checksum = templateXml.Attributes[TPL_FileTags.CHECKSUM].Value;
                //result = Check checksum
                result = VerifyChecksum( checksum );
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
            XmlNode basicInformation = templateXml.SelectSingleNode( TPL_FileTags.BASE_INFORMATION_PART );
            //Name:
            name = basicInformation.Attributes[TPL_FileTags.NAME_ATTRIBUTE].Value;
            //Version:
            version = basicInformation.Attributes[TPL_FileTags.VERSION_ATTRIBUTE].Value;
            //Acronym:
            acronym = basicInformation.Attributes[TPL_FileTags.ACRONYM_ATTRIBUTE].Value;

            //Description:
            description = basicInformation.Attributes[TPL_FileTags.DESCRIPTION_ATTRIBUTE].Value;

            //Pads resuses:
            padsReuses = Convert.ToInt32( basicInformation.Attributes[TPL_FileTags.PADS_REUSES_ATTRIBUTE].Value );

            //Wells per sample
            wellPerSample = Convert.ToInt32( basicInformation.Attributes[TPL_FileTags.WELLS_PER_SAMPLE_ATTRIBUTE].Value );


            //Creation date: 
            try
            {
                string date = basicInformation.Attributes[TPL_FileTags.CREATDATE_ATTRIBUTE].Value;
                creationDate = Convert.ToDateTime( basicInformation.Attributes[TPL_FileTags.CREATDATE_ATTRIBUTE].Value );
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
            XmlNode mainDiseasesNode = templateXml.SelectSingleNode( TPL_FileTags.MAIN_DISEASES_PART );
            if ( mainDiseasesNode != null )                                                                       //idan check catch
            {
                XmlNode diseasesNode = mainDiseasesNode.SelectSingleNode( TPL_FileTags.DISEASES_PART );
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
                for ( i = 0; i < diseasesCnt; i++ )
                {
                    diseaseNode = diseasesNode.ChildNodes[i];
                    disease = new DTT_Disease();
                    disease.DiseaseName = diseaseNode.Attributes[TPL_FileTags.NAME_ATTRIBUTE].Value;
                    disease.DiseaseDescription = diseaseNode.Attributes[TPL_FileTags.DESCRIPTION_ATTRIBUTE].Value;

                    //Get mutation:
                    mutationsCnt = diseaseNode.ChildNodes.Count;
                    //Initialize mutations array:
                    disease.Mutations = new string[mutationsCnt];
                    for ( int j = 0; j < mutationsCnt; j++ )
                    {
                        disease.Mutations[j] = diseaseNode.ChildNodes[j].Attributes[TPL_FileTags.NAME_ATTRIBUTE].Value;

                    }
                    //Add diseases information:
                    diseasesArr.Add( disease );
                }

                XmlNode assignmentsNode = mainDiseasesNode.SelectSingleNode( TPL_FileTags.MUTATION_ASSIGNMENT_PART );
                for ( i = 0; i < assignmentsNode.ChildNodes.Count; i++ )
                {
                    assignment = new TPL_MutationAssignment();
                    assignment.MutationName = assignmentsNode.ChildNodes[i].Attributes[TPL_FileTags.NAME_ATTRIBUTE].Value;
                    assignment.CaptureIndex = assignmentsNode.ChildNodes[i].Attributes[TPL_FileTags.MUTATION_CAPTURE_ATTR].Value;
                    assignment.ReporterIndex = assignmentsNode.ChildNodes[i].Attributes[TPL_FileTags.MUTATION_REPORTER_ATTR].Value;

                    mutationsAssignment.Add( assignment );
                }

            }
        }



        /// <summary>
        /// Initialize materials: reagent bottles and reagent packs.
        /// </summary>
        private void InitializeMaterials()
        {
            //Get materials node:
            XmlNode materialsNode = templateXml.SelectSingleNode( TPL_FileTags.MATERIALS_CONFIGURATION_PART );
            //Initialize materials node:
            materialsConfiguration = new TPL_MaterialsConfiguration( materialsNode );

        }



        /// <summary>
        /// Verifies that the recorded and calculated CHECKSUMs match
        /// </summary>
        /// <returns>true if match, false otherwise</returns>
        private bool VerifyChecksum( string checksum )
        {
            string calculatedCRC = this.CalculateChecksum();
            bool res;
            if ( checksum == calculatedCRC )
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
            string myCRC = CRCTool.CalculateCRC( templateXml );
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
                string[] classNameParts = className.Split( '.' );
                className = classNameParts[classNameParts.Length - 1];
                //XML is embedded resourse
                System.Reflection.Assembly myAssembly = Assembly.GetExecutingAssembly();
                string[] allNames = myAssembly.GetManifestResourceNames();
                for ( int i = 0; i < allNames.Length; i++ )
                {
                    string name = allNames[i];
                }
                System.IO.Stream stream = myAssembly.GetManifestResourceStream( "Template." + className + "Template.xml" );
                templateDoc = new XmlDocument();
                templateDoc.Load( stream );
                //Get template node:
                templateXml = templateDoc.SelectSingleNode( TPL_FileTags.ROOT_PART );
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
            string[] classNameParts = className.Split( '.' );
            className = classNameParts[classNameParts.Length - 1];

            loadingPatternFileName = "Template." + className + TPL_FileTags.LOADING_PATTERN_FILE_SUFFIX + ".xml";
            System.Reflection.Assembly myAssembly = Assembly.GetExecutingAssembly();

            System.IO.Stream stream = myAssembly.GetManifestResourceStream( loadingPatternFileName );
            try
            {
                loadingPatternXml = new XmlDocument();
                loadingPatternXml.Load( stream );
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

        public override string[,] Analyze( double[, , ,] signals, out string[] GeneralColumnsNames, out string[] ReporterNames, out string[] ColumnsNamesOfEachReporter,
              out string[] CustomColumnsNames, string[] SampleNameList, int[] SamplePositionList, double[,] referenceSignalsData, double[,] backgroundSignals )
        {
                   
            Initialize();
           
            //string[,] MarkerName = { { "Histidine",  "Histidine", "Histidine", "Histidine" },
            //                            { "MecA",       "nuc",       "SCC Mec",   "SPA" },
            //                            { "EFS",        "EFM",       "",          "VAN A B" },
            //                            { "PhoE",       "KPC",       "NVC",       "" }};

             
            int currentPosition = 0;
            int currentColumn = 0;
            int previousColumn = 0;
            int currentRow = 0;
            int nextRow = 0;
            int previousRow = 0;
            int currentSample = 0;
            int currentMarker = 0;
            int currentReporter = 0;
            int currentSpecialSample = 0;
            int currentSpecialSampleForPCFail = 0;
            int currentRatioColumn = 0;
            int specialSampleInterpretationColumn = 0;
            int interpretationColumn = 0;
            int currentInvalidSample = 0;
            int numberOfRows = 0;
            int numberOfColumns = 0;
            int numberOfResultsInReference = 0;
             
            string[] ReferenceColumnHeaders;

            // get the max number of raporters and captuers in order to init MarkerName to the correct size 
            int RepMax = 0;
            int CepMax = 0;
            
            // ratio counter calculate the number of times that none is written in the ratio column
            int RatioCounter = 0;

            // set the max size of each caputer and reporter for marker  
            for ( currentPosition = 0; currentPosition < mutationsAssignment.Count; currentPosition++ )
            {
                if ( Convert.ToInt32( mutationsAssignment[currentPosition].CaptureIndex ) > CepMax )
                {
                    CepMax = Convert.ToInt32( mutationsAssignment[currentPosition].CaptureIndex );
                }

                if ( Convert.ToInt32( mutationsAssignment[currentPosition].ReporterIndex ) > RepMax )
                {
                    RepMax = Convert.ToInt32( mutationsAssignment[currentPosition].ReporterIndex );
                }
            }

            // init MarkerName table 
            string[,] MarkerName = new string[CepMax, RepMax];

            // set marker names in the table
            for ( currentPosition = 0; currentPosition < mutationsAssignment.Count; currentPosition++ )
            {
                MarkerName[Convert.ToInt32( mutationsAssignment[currentPosition].CaptureIndex ) - 1, Convert.ToInt32( mutationsAssignment[currentPosition].ReporterIndex ) - 1] = mutationsAssignment[currentPosition].MutationName;
            }

            // get the calculation of the current row
            int currectRow = 0;
            // get the calculation of the current column
            int currectColumn = 0;
            // get the calculation of the current column for marker 
            int currectColumnForMarker = 0;

            int endOfSample = 0;

            //*********
            //
            // signals data is received from the device. 
            // signals[0, 1, 2, 3] represent: 0 state sample number, 1 state marker, 2 state reporter, 3 state the color
            //
            //*********

            // get number of pads
            int numberOfMarkers = signals.GetLength( MARKER_LIST );
            // get number of samples
            int numberOfSampleList = signals.GetLength( SAMPLE_LIST );

            // get name of reporters from XML
            string[] NameOfReporters = materialsConfiguration.GetNamesOfReporters();

            // init size from enum
            int EnumSizeReporterNames = NameOfReporters.Length;          
            int EnumSizeGeneralColumnsNames = Enum.GetNames( typeof( AnalyzeGeneralColumnsNames ) ).Length; 
            int EnumSizeColumnsNamesOfEachReporter = Enum.GetNames( typeof( AnalyzeColumnsNamesOfEachReporter ) ).Length;
            int EnumSizeCustomColumnsNames = Enum.GetNames( typeof( AnalyzeCustomColumnsNames ) ).Length;
             
            // do not change!
            int CALCULATE_INTERPRETATION_COLUMN = EnumSizeColumnsNamesOfEachReporter * EnumSizeReporterNames;
            int ANALYZE_INTERPRETATION_COLUMN = EnumSizeGeneralColumnsNames + EnumSizeColumnsNamesOfEachReporter * EnumSizeReporterNames;


            // set headlines of the table        
            GeneralColumnsNames = new string[EnumSizeGeneralColumnsNames];
            ReporterNames = new string[EnumSizeReporterNames];
            ColumnsNamesOfEachReporter = new string[EnumSizeColumnsNamesOfEachReporter];
            CustomColumnsNames = new string[EnumSizeCustomColumnsNames];


            // set name for each parameter 
            for ( currentPosition = 0; currentPosition < GeneralColumnsNames.Length; currentPosition++ )
            {
                GeneralColumnsNames[currentPosition] = Enum.GetName( typeof( AnalyzeGeneralColumnsNames ), currentPosition );
            }
             
            for ( currentPosition = 0; currentPosition < ReporterNames.Length; currentPosition++ )
            {
                ReporterNames[currentPosition] = NameOfReporters[currentPosition].ToString();
            }

            for ( currentPosition = 0; currentPosition < ColumnsNamesOfEachReporter.Length; currentPosition++ )
            {
                ColumnsNamesOfEachReporter[currentPosition] = Enum.GetName( typeof( AnalyzeColumnsNamesOfEachReporter ), currentPosition );
            }

            for ( currentPosition = 0; currentPosition < CustomColumnsNames.Length; currentPosition++ )
            {
                CustomColumnsNames[currentPosition] = Enum.GetName( typeof( AnalyzeCustomColumnsNames ), currentPosition );
            }


            // size of column is: number of GeneralColumnsNames + ReporterNames * ColumnsNamesOfEachReporter + CustomColumnsNames 
            int column = EnumSizeGeneralColumnsNames + EnumSizeReporterNames * EnumSizeColumnsNamesOfEachReporter + EnumSizeCustomColumnsNames;
            // size of row is: number of pads * number of samples + 2 (sample 1)
            int row = numberOfMarkers * ( numberOfSampleList - NUMBER_OF_SPECIAL_SAMPLES ) + SPECIAL_SAMPLE_STRUCTURE * NUMBER_OF_SPECIAL_SAMPLES;
          
            string[,] RawDataTable = new string[row, column];

            // reset all values to ""
            for ( currentColumn = 0; currentColumn < column; currentColumn++ )
            {
                for ( currentRow = 0; currentRow < row; currentRow++ )
                {
                    RawDataTable[currentRow, currentColumn] = "";
                }
            }

             
            // set markers (capture / pads) to each reporter (scan)
            // loop that run on all the samples
            for ( currentSample = 0; currentSample < numberOfSampleList - NUMBER_OF_SPECIAL_SAMPLES; currentSample++ )
            {
                // loop that run on all the markers (or captures / pads)
                for ( currentMarker = 0; currentMarker < numberOfMarkers; currentMarker++ )
                {
                    // loop that run on all the reporters (or scans)
                    for ( currentReporter = 0; currentReporter < EnumSizeReporterNames; currentReporter++ )
                    {
                        // each special sample takes 2 rows, so we need to multiply the number of special samples by 2
                        currectRow = currentMarker + SPECIAL_SAMPLE_STRUCTURE * NUMBER_OF_SPECIAL_SAMPLES + numberOfMarkers * currentSample;

                        // 1 is represent the marker column (we need to skip on it in order to set the green value in the next column) 
                        currectColumn = MARKER_COLUMN + EnumSizeGeneralColumnsNames + EnumSizeColumnsNamesOfEachReporter * currentReporter;

                        currectColumnForMarker = EnumSizeGeneralColumnsNames + EnumSizeColumnsNamesOfEachReporter * currentReporter;
                         
                        // set index - index start from 1
                        RawDataTable[currectRow, ANALYZE_INDEX_COLUMN] = ( currentSample + 1 ).ToString();

                        // set position number
                        RawDataTable[currectRow, ANALYZE_POSITION_COLUMN] = getWellPos( SamplePositionList[currentSample + 1] );

                        // set sample name
                        RawDataTable[currectRow, ANALYZE_SAMPLE_NAME_COLUMN] = SampleNameList[currentSample + 1];

                        // set marker name
                        RawDataTable[currectRow, currectColumnForMarker] = MarkerName[currentMarker, currentReporter];

                        if ( RawDataTable[currectRow, currectColumnForMarker] != null )
                        {
                            // currentSample state sample number, currentMarker state marker, currentReporter state reporter, last one state the color
                            RawDataTable[currectRow, currectColumn] = signals[currentSample + NUMBER_OF_SPECIAL_SAMPLES, currentMarker, currentReporter, ( int )color.GREEN].ToString();
                        }

                      
                    }
                     
                }

            }
           
        

            // dov - need to fix all the values when received currect data
            for ( currentSpecialSample = 0; currentSpecialSample < NUMBER_OF_SPECIAL_SAMPLES; currentSpecialSample++ )
            {
                 
                currectRow = currentSpecialSample * SPECIAL_SAMPLE_STRUCTURE;
                nextRow = currectRow + 1;

                // 1 is represent the marker column (we need to skip on it in order to set the green value in the next column) 
                currectColumn = MARKER_COLUMN + EnumSizeGeneralColumnsNames + EnumSizeColumnsNamesOfEachReporter;

                currectColumnForMarker = EnumSizeGeneralColumnsNames + EnumSizeColumnsNamesOfEachReporter;
                
                // set sample index
                RawDataTable[currectRow, ANALYZE_INDEX_COLUMN] = "0";
                RawDataTable[nextRow, ANALYZE_INDEX_COLUMN] = "0";

                // set sample name
                RawDataTable[currectRow, ANALYZE_SAMPLE_NAME_COLUMN] = SampleNameList[currentSpecialSample];
                RawDataTable[nextRow, ANALYZE_SAMPLE_NAME_COLUMN] = SampleNameList[currentSpecialSample];

                // set marker names
                RawDataTable[currectRow, currectColumnForMarker] = BKG;
                RawDataTable[nextRow, currectColumnForMarker] = PC;

                // set green values
                RawDataTable[currectRow, currectColumn] = signals[currentSpecialSample, 0, currentSpecialSample + 1, ( int )color.GREEN].ToString();
                RawDataTable[nextRow, currectColumn] = signals[currentSpecialSample, currentSpecialSample + 1, currentSpecialSample + 1, ( int )color.GREEN].ToString();
            }

   
            
            // set ratio values
            for ( currentColumn = 0; currentColumn < column; currentColumn++ )
            {
                currentRatioColumn = currentColumn + 2;
           //     specialSampleInterpretationColumn = currentColumn + 9;
                specialSampleInterpretationColumn = column - 1;
                for ( currentRow = 0; currentRow < row; currentRow++ )
                {
                    
                    if ( RawDataTable[currentRow, currentColumn] == NVC )
                    {
                        RawDataTable[currentRow, currentRatioColumn] = NVCRatio( RawDataTable, currentRow, currentColumn );
                    }

                    else if ( ( RawDataTable[currentRow, currentColumn] == SCCMEC )  || ( RawDataTable[currentRow, currentColumn] == KPC ) 
                           || ( RawDataTable[currentRow, currentColumn] == DDL_EFM ) || ( RawDataTable[currentRow, currentColumn] == NUC ) 
                           || ( RawDataTable[currentRow, currentColumn] == MECA )    || ( RawDataTable[currentRow, currentColumn] == DDL_EFS )
                           || ( RawDataTable[currentRow, currentColumn] == PHOE )    || ( RawDataTable[currentRow, currentColumn] == SPA ) 
                           || ( RawDataTable[currentRow, currentColumn] == VANAB ) )
                    {
                        // set the return value to the appropriate ratio index
                        RawDataTable[currentRow, currentRatioColumn] = CalcRatio( RawDataTable, currentRow, currentColumn );
                    }
                       
                    else if ( currentRow > 0 )
                    {
                        previousRow = currentRow - 1; 

                        if ( ( RawDataTable[currentRow, currentColumn] == PC ) && ( RawDataTable[previousRow, currentColumn] == BKG ) )
                        {
                            RawDataTable[currentRow, currentRatioColumn] = CalcRatio( RawDataTable, currentRow, currentColumn );

                            if ( RawDataTable[currentRow, currentRatioColumn] == NONE )
                            {
                                RawDataTable[currentRow, specialSampleInterpretationColumn] = PC_FAIL;
                            }
                             
                            else
                            {
                                RawDataTable[currentRow, specialSampleInterpretationColumn] = PC_PASS;
                            }
                            
                        }

                    }
                     
                }

            }
       
 
             
            // set interpretation values
            for ( currentColumn = 0; currentColumn < column; currentColumn++ )
            {
                for ( currentRow = 0; currentRow < row; currentRow++ )
                {
                    // check for the marker that is in the row of the needed interpretation
                   
                    if ( RawDataTable[currentRow, currentColumn] == MECA )
                    {
                        // set the return value to the appropriate interpretation index
                        RawDataTable[currentRow, currentColumn + CALCULATE_INTERPRETATION_COLUMN] = CalcMRSA( RawDataTable, currentRow, currentColumn );
                    }

                    else if ( RawDataTable[currentRow, currentColumn] == DDL_EFS )
                    {
                        // set the return value to the appropriate interpretation index
                        RawDataTable[currentRow, currentColumn + CALCULATE_INTERPRETATION_COLUMN] = CalcVRE( RawDataTable, currentRow, currentColumn );
                    }

                    else if ( RawDataTable[currentRow, currentColumn] == PHOE )
                    {
                        // set the return value to the appropriate interpretation index
                        RawDataTable[currentRow, currentColumn + CALCULATE_INTERPRETATION_COLUMN] = CalcKPC( RawDataTable, currentRow, currentColumn );
                    }
                } 
         
            }


            // after calculation of interpretation values, run on all ratio values on each sample
            // if NVC is "OK" - nothing changes 
            // if NVC is "NOA" and one of the 9 marker values is different then none - nothing changes
            // if NVC is "NOA" and all 9 marker values are none then on each of the 3 interpretation rows will appear invalid sample 
 
            // loop that run on all the samples
            for ( currentSample = 0; currentSample < numberOfSampleList - NUMBER_OF_SPECIAL_SAMPLES; currentSample++ )
            {
                // check if NVC is not OK, if it is OK don't do anything.
                if ( RawDataTable[5 + 4 * currentSample, 11] == NOA )
                {
                    // reset ratio counter
                    RatioCounter = 0;
                    
                    // loop that run on all the markers (or captures / pads)
                    for ( currentMarker = 0; currentMarker < numberOfMarkers; currentMarker++ )
                    {
                        // loop that run on all the reporters (or scans)
                        for ( currentReporter = 0; currentReporter < EnumSizeReporterNames; currentReporter++ )
                        {
                            currectRow = currentMarker + SPECIAL_SAMPLE_STRUCTURE * NUMBER_OF_SPECIAL_SAMPLES + numberOfMarkers * currentSample;
                         
                            // 2 represent the marker column and the color column (we need to skip on it in order to check the ratio value in the next column) 
                            currectColumn = MARKER_COLUMN + COLOR_COLUMN + EnumSizeGeneralColumnsNames + EnumSizeColumnsNamesOfEachReporter * currentReporter;

                            // i state sample number, j state marker, k state reporter, last one state the color
                            if ( RawDataTable[currectRow, currectColumn] == NONE )
                            {
                                RatioCounter++;
                            }
                             
                        }

                    } 
                
                    // if all markers ratio are "NONE", set invalid sample in the interpretation
                    if ( RatioCounter == NUMBER_OF_MARKERS_PER_SAPMLE_WITHOUT_NVC )
                    {
                        // column + 1 in order to go to the interpretation column
                        interpretationColumn = currectColumn + 1;
                
                        for ( currentInvalidSample = 0; currentInvalidSample < ANALYZE_INVALID_SAMPLE; currentInvalidSample++ )
                        {
                            // row - currentInvalidSample in order to write invalid sample in all 3 interpretation rows
                            RawDataTable[currectRow - currentInvalidSample, interpretationColumn] = INVALID_SAMPLE;
                             
                        }
  
                    }
                }
            }
             

            // get data from reference 
            string[,] ReferenceDataTable = reference( referenceSignalsData, backgroundSignals, out ReferenceColumnHeaders );

            currentRow = 0;
            currentColumn = 0;
            numberOfRows = ReferenceDataTable.GetLength(0);
            numberOfColumns = ReferenceDataTable.GetLength( 1 );
            numberOfResultsInReference = 0;
            currentSample = 0;

            for ( currentRow = 0; currentRow < numberOfRows; currentRow++ )
            {
                for ( currentColumn = 0; currentColumn < numberOfColumns; currentColumn++ )
                {
                    if ( ReferenceDataTable[currentRow, currentColumn] == RESULT )
                    {
                        numberOfResultsInReference++;
                    }
                } 
            }

            currentRow = 0;

            endOfSample = NUMBER_OF_MARKERS_PER_SAPMLE + numberOfResultsInReference;

            while ( currentRow < numberOfRows )
            {
                // in REFERENCE1_COLUMN there is the result of all the reporters results 
                if ( ReferenceDataTable[currentRow, REFERENCE1_COLUMN] == FAIL_MASSAGE )
                {
                    // check the first marker of that reporter (currentRow - 4)
                    if ( ReferenceDataTable[currentRow - 4, MARKER_COLUMN] == MECA )
                    {
                        RawDataTable[ANALYZE_MECA_ROW + 4 * currentSample, ANALYZE_INTERPRETATION_COLUMN] = REFERENCE_MRSA_FAIL;
                    } 

                    // check the first marker of that reporter (currentRow - 3)
                    else if ( ReferenceDataTable[currentRow - 3, MARKER_COLUMN] == DDL_EFS )
                    {
                        RawDataTable[ANALYZE_EFS_ROW + 4 * currentSample, ANALYZE_INTERPRETATION_COLUMN] = REFERENCE_VRE_FAIL;
                    }

                    // check the first marker of that reporter (currentRow - 3)
                    else if ( ReferenceDataTable[currentRow - 3, MARKER_COLUMN] == PHOE )
                    {
                        RawDataTable[ANALYZE_PHOE_ROW + 4 * currentSample, ANALYZE_INTERPRETATION_COLUMN] = REFERENCE_KPC_FAIL;
                    } 
                }

                // check if reached to the end of the sample, if so add 1 to current sample in order to save data to the appropriate sample.
                if ( ( currentRow % endOfSample == 0 ) && ( currentRow != 0 ) )
                {
                    // dov
                    currentSample++;
                }
                        
                currentRow++;
            }
      
      
            
               
            // check if the interpretation of the special sample is PC Fail if so change all the interpretation column to PCR Failed
     
            for ( currentSpecialSample = 0; currentSpecialSample < NUMBER_OF_SPECIAL_SAMPLES; currentSpecialSample++ )
            { 
                previousColumn = column - 1;
                // search for pc fail in the last column and the second row of each special sample
                // column satrt from 0 so we need (-1) in order to get to the last column (interpretation)               
                if ( RawDataTable[SPECIAL_SAMPLE_STRUCTURE * currentSpecialSample + 1, previousColumn] == PC_FAIL )
                {
                    // if pc fail we need to write in each interpretation pcr failed
                    // run from end of special sample till the last row (total of row - 2 * NUMBER_OF_SPECIAL_SAMPLES)
                    for ( currentSpecialSampleForPCFail = 0; currentSpecialSampleForPCFail < row - SPECIAL_SAMPLE_STRUCTURE * NUMBER_OF_SPECIAL_SAMPLES; currentSpecialSampleForPCFail++ )
                    {
                    
                        if ( RawDataTable[currentSpecialSampleForPCFail + SPECIAL_SAMPLE_STRUCTURE * NUMBER_OF_SPECIAL_SAMPLES, previousColumn] != "" )
                        {
                            // start from the row the come after the last special sample
                            // column satrt from 0 so we need (-1) in order to get to the last column (interpretation)
                            RawDataTable[currentSpecialSampleForPCFail + SPECIAL_SAMPLE_STRUCTURE * NUMBER_OF_SPECIAL_SAMPLES, previousColumn] = PCR_Failed;
                        }
                    }
                }
            } 
             
            return RawDataTable;
        }


      


        /// <summary>
        /// reference results of all samples and build reference data table
        /// </summary>
        /// <param name="MarkerTruthTable">Truth Table of the specific Marker</param>
        /// <param name="Data">data of signals values</param>
        /// <param name="RawDataReporterColumnNo">amount of reference Data Table column</param>
        /// <returns></returns>return the reference data table  

        public override string[,] reference( double[,] referenceSignals,double[,] backgroundSignals, out string[] ReferenceColumnHeaders )
        {
             
            int currentPosition = 0;
            int currentReporter = 0;
            int currentNameOfReporter = 0;
            int currentColumn = 0;
            int currentRow = 0;
            int currentReference = 0;
            int currentMarkerInReporter = 0;
            int currentRowOfReferenceDataTable = 0;

            // get number of rows for each color (currently only red and green) 
            int numberOfRows = referenceSignals.GetLength( 0 ) / NUMBER_OF_COLORS_BEEN_USED;

            //string[,] MarkerName = { { "MecA",       "nuc",       "SCC Mec",   "SPA",     "Result" },
            //                            { "EFS",        "EFM",       "nuc",       "VAN A B", "Result" },
            //                            { "nuc",        "PhoE",       "KPC",       "NVC",    "Result" }};
             

            Initialize();
             
            // init size from enum
            int EnumSizeColumnsNames = Enum.GetNames( typeof( ReferenceColumnsNames ) ).Length;
             
            int row = 0;
            int column = EnumSizeColumnsNames;
             
            // counter that store the number of passes in each row
            int countNumberOfPasses = 0;
             

            // get the max number of raporters and captuers in order to init MarkerName to the correct size 
            int RepMax = 0;
            int CepMax = 0;
             
            for ( currentPosition = 0; currentPosition < mutationsAssignment.Count; currentPosition++ )
            {

                if ( Convert.ToInt32( mutationsAssignment[currentPosition].CaptureIndex ) > CepMax )
                {
                    CepMax = Convert.ToInt32( mutationsAssignment[currentPosition].CaptureIndex );
                }

                if ( Convert.ToInt32( mutationsAssignment[currentPosition].ReporterIndex ) > RepMax )
                {
                    RepMax = Convert.ToInt32( mutationsAssignment[currentPosition].ReporterIndex );
                }
            }

            // init MarkerName table 
            string[,] MarkerName = new string[CepMax, RepMax];

            // set marker names in the table
            for ( currentPosition = 0; currentPosition < mutationsAssignment.Count; currentPosition++ )
            {

                MarkerName[Convert.ToInt32( mutationsAssignment[currentPosition].CaptureIndex ) - 1, Convert.ToInt32( mutationsAssignment[currentPosition].ReporterIndex ) - 1] = mutationsAssignment[currentPosition].MutationName;

            }
         

            // get names of reporters
            string[] NameOfReporters = materialsConfiguration.GetNamesOfReferences();
             
            // store for each reporter the number of mutation in each row
            int[] NumberOfMrakersInEachReporter = new int [NameOfReporters.Length];
           
            // reset number of markers in each reporter
            for ( currentReporter = 0; currentReporter < NameOfReporters.Length; currentReporter++ )
            {
                NumberOfMrakersInEachReporter[currentReporter] = 0;
            }
   
            // check how many rows we need (each row contain 1 marker) 
            for ( currentRow = NUMBER_OF_SPECIAL_SAMPLES; currentRow < CepMax; currentRow++ )
            {
                for ( currentColumn = 0; currentColumn < RepMax; currentColumn++ )
                {

                    if ( MarkerName[currentRow, currentColumn] != null )
                    {
                        row++;
                        // set the number of mutation for each row (MRSA, VRE, KPC)
                        NumberOfMrakersInEachReporter[currentRow - 1]++;
                    }
                }
            }
             

            // add to row the number of reporters (at the end of each reporter that row will contain the result marker) each reporter as 1 result 
            row += NameOfReporters.Length;
             

            // set headlines of the table        
            ReferenceColumnHeaders = new string[EnumSizeColumnsNames];
      
            // set name for parameter 
            for ( currentReference = 0; currentReference < ReferenceColumnHeaders.Length; currentReference++ )
            {
                ReferenceColumnHeaders[currentReference] = Enum.GetName( typeof( ReferenceColumnsNames ), currentReference );
            }

            string[,] ReferenceDataTable = new string[row, column];
           
            // reset reference data table
            for ( currentColumn = 0; currentColumn < column; currentColumn++ )
            {
                for ( currentRow = 0; currentRow < row; currentRow++ )
                {
                    ReferenceDataTable[currentRow, currentColumn] = "";
                }
            }
 
             // set reporters names in reporter column and set result at the end of each reporter (at the marker column)
            for ( currentMarkerInReporter = 0; currentMarkerInReporter < NumberOfMrakersInEachReporter.Length; currentMarkerInReporter++ )
            {
                for ( currentRow = 0; currentRow < row; currentRow++ )
                {
                    // currentRow is the same size of the number of markers for the current reporter (meaning we got to the last row of the current reporter)
                    if ( currentRow == NumberOfMrakersInEachReporter[currentMarkerInReporter] )
                    {
                        // currentRowOfReferenceDataTable store the current row of the reference data table 
                        currentRowOfReferenceDataTable += currentRow;
                        ReferenceDataTable[currentRowOfReferenceDataTable, MARKER_COLUMN] = RESULT;
                        // increment currentRowOfReferenceDataTable because we enter data to row currentRowOfReferenceDataTable
                        currentRowOfReferenceDataTable++;
                        break;
                    }
                    else
                    {
                        // reporter name
                        ReferenceDataTable[currentRowOfReferenceDataTable + currentRow, REPORTER_COLUMN] = NameOfReporters[currentMarkerInReporter].ToString();
                         
                    }
                }                
            }

      
            // enter marker name as long as it not null 
            for ( currentNameOfReporter = 0, currentRow = 0; currentNameOfReporter < NameOfReporters.Length; currentNameOfReporter++, currentRow++ )
            {
                for ( currentReporter = 0; currentReporter < RepMax; currentReporter++ )
                {
                    // marker name
                    if ( MarkerName[currentNameOfReporter + 1, currentReporter] != null )
                    {
                        ReferenceDataTable[currentRow, MARKER_COLUMN] = MarkerName[currentNameOfReporter + 1, currentReporter];
                        currentRow++;
                    }
                }
            }
              
            

            // check the data that in reference column 1 and 2 and decide if it passed or failed
            for ( currentRow = 0; currentRow < row; currentRow++ )
            {
                if ( ReferenceDataTable[currentRow, MARKER_COLUMN] != RESULT )
                {
                    // reference 1
                    ReferenceDataTable[currentRow, REFERENCE1_COLUMN] = referenceSignals[currentRow, MARKER_COLUMN].ToString();
                    // reference 2
                    ReferenceDataTable[currentRow, REFERENCE2_COLUMN] = referenceSignals[currentRow + numberOfRows, MARKER_COLUMN].ToString();

                    if ( ( Convert.ToDouble( ReferenceDataTable[currentRow, REFERENCE1_COLUMN] ) >= TEST_REFERENCE ) || ( Convert.ToDouble( ReferenceDataTable[currentRow, REFERENCE2_COLUMN] ) >= TEST_REFERENCE ) )
                    {
                        ReferenceDataTable[currentRow, PASS_FAIL_COLUMN] = PASS;
                    }
                    else
                    {
                        ReferenceDataTable[currentRow, PASS_FAIL_COLUMN] = FAIL_MASSAGE;
                    }
 
                }
            }
             


            // count the number of passes for each mutation, if the number is smaller then the number of markers then add fail to the result of each reporter
            for ( currentReporter = 0, currentRow = 0; currentReporter < NumberOfMrakersInEachReporter.Length; currentReporter++, currentRow++ )
            {
                for ( currentMarkerInReporter = 0; currentMarkerInReporter < NumberOfMrakersInEachReporter[currentReporter]; currentMarkerInReporter++, currentRow++ )
                {
                    // count number of passes
                    if ( ReferenceDataTable[currentRow, PASS_FAIL_COLUMN] == PASS )
                    {
                        countNumberOfPasses++;
                    }
                }

                // if number of passes is smaller then the number of markers (meaning that not all markers passed) enter fail
                if ( countNumberOfPasses < NumberOfMrakersInEachReporter[currentReporter] )
                {
                    ReferenceDataTable[currentRow, REFERENCE1_COLUMN] = FAIL_MASSAGE;
                    countNumberOfPasses = RESET_COUNT_NUMBER_OF_PASSES;
                }
                else
                {
                    ReferenceDataTable[currentRow, REFERENCE1_COLUMN] = PASS;
                    countNumberOfPasses = RESET_COUNT_NUMBER_OF_PASSES;
                }
 
            }
             

            return ReferenceDataTable;
        }





        /// <summary>
        /// Details results of all samples and build details data table
        /// </summary>
        /// <param name="MarkerTruthTable">Truth Table of the specific Marker</param>
        /// <param name="Data">data of signals values</param>
        /// <param name="RawDataReporterColumnNo">amount of Details Data Table column</param>
        /// <returns></returns>return the details data table  

        public override string[,] Details( double[, , ,] signals, out string[] DetailsColumnHeaders, string[] SampleNameList, int[] SamplePositionList, double[,] referenceSignalsData, double[,] backgroundSignals )
        {

            int currentPosition = 0;
            int currentSample = 0;
            int numberOfColumns = 0;
            int currentMarker = 0;
             
            int row = 0;
            int column = 0;
 
            string[] GeneralColumnsNames;
            string[] ReporterNames;
            string[] ColumnsNamesOfEachReporter;
            string[] CustomColumnsNames;
  
            // get number of pads
            int numberOfMarkers = signals.GetLength( MARKER_LIST );

            // get number of samples
            int numberOfSampleList = signals.GetLength( SAMPLE_LIST );

            // init size from enum
            int EnumSizeDetailsColumnHeaders = Enum.GetNames( typeof( DetailsColumnNames ) ).Length;

            // set headlines of the table        
            DetailsColumnHeaders = new string[EnumSizeDetailsColumnHeaders];

            // set name for each parameter 
            for ( currentPosition = 0; currentPosition < DetailsColumnHeaders.Length; currentPosition++ )
            {
                DetailsColumnHeaders[currentPosition] = Enum.GetName( typeof( DetailsColumnNames ), currentPosition );
            } 
           

            // get data from analyze
            string[,] AnalyzeDataTable = Analyze( signals, out GeneralColumnsNames, out ReporterNames, out ColumnsNamesOfEachReporter, out CustomColumnsNames, SampleNameList, SamplePositionList, referenceSignalsData, backgroundSignals );
      
            row = numberOfMarkers * numberOfSampleList;

            column = AnalyzeDataTable.GetLength( COLUMN_SIZE );

            // set the size of DetailsDataTable  
            string[,] DetailsDataTable = new string[numberOfSampleList - NUMBER_OF_SPECIAL_SAMPLES, DetailsColumnHeaders.Length];
       
            for ( numberOfColumns = 0; numberOfColumns < DetailsColumnHeaders.Length; numberOfColumns++ )
            {
                for ( currentSample = 0; currentSample < numberOfSampleList - NUMBER_OF_SPECIAL_SAMPLES; currentSample++ )
                {
                    DetailsDataTable[currentSample, numberOfColumns] = "";
                }
            }


            // insert data to details data table
            for ( currentSample = 0; currentSample < numberOfSampleList - NUMBER_OF_SPECIAL_SAMPLES; currentSample++ )
            {
                // set index
                DetailsDataTable[currentSample, DETAILS_INDEX_COLUMN] = ( currentSample + 1 ).ToString();

                // set position            
                DetailsDataTable[currentSample, DETAILS_POSITION_COLUMN] = getWellPos( SamplePositionList[currentSample + NUMBER_OF_SPECIAL_SAMPLES] );

                // set sampleID
                DetailsDataTable[currentSample, DETAILS_SAMPLE_ID_COLUMN] = SampleNameList[currentSample + NUMBER_OF_SPECIAL_SAMPLES];

                for ( currentMarker = 0; currentMarker < numberOfMarkers; currentMarker++ )
                {

                    // check if there is a marker, if so enter color value
                    if ( AnalyzeDataTable[currentMarker + SPECIAL_SAMPLE_STRUCTURE * NUMBER_OF_SPECIAL_SAMPLES + 4 * currentSample, column - 1] != "" )
                    {

                        DetailsDataTable[currentSample, 2 + currentMarker] = AnalyzeDataTable[currentMarker + currentSample * 4 + SPECIAL_SAMPLE_STRUCTURE * NUMBER_OF_SPECIAL_SAMPLES, column - 1];
                    }
                
                }
  
            }
               

            return DetailsDataTable;
        }



        


        /// <summary>
        /// Summary results of all samples and build summary data table
        /// </summary>
        /// <param name="MarkerTruthTable">Truth Table of the specific Marker</param>
        /// <param name="Data">data of signals values</param>
        /// <param name="RawDataReporterColumnNo">amount of Summary Data Table column</param>
        /// <returns></returns>return the summary data table  

        public override string[,] Summary( double[, , ,] signals, out string[] SummaryColumnHeaders, string[] SampleNameList, int[] SamplePositionList, double[,] referenceSignalsData, double[,] backgroundSignals )
        {

            int currentPosition = 0;
            int currentSample = 0;
            int row = 0;
            int column = 0;
             
            // get number of pads
            int numberOfMarkers = signals.GetLength( MARKER_LIST );

            // get number of samples
            int numberOfSampleList = signals.GetLength( SAMPLE_LIST );


            // init size from enum
            int EnumSizeSummarylColumnsNames = Enum.GetNames( typeof( SummaryColumnsNames ) ).Length;

            // set headlines of the table        
            SummaryColumnHeaders = new string[EnumSizeSummarylColumnsNames];

            // set name for each parameter 
            for ( currentPosition = 0; currentPosition < SummaryColumnHeaders.Length; currentPosition++ )
            {
                SummaryColumnHeaders[currentPosition] = Enum.GetName( typeof( SummaryColumnsNames ), currentPosition );
            }
             
            // get data from analyze
            string[,] DetailsDataTable = Details( signals, out DetailsColumnHeaders, SampleNameList, SamplePositionList, referenceSignalsData, backgroundSignals );
           
            // set the size of SummaryDataTable 
            string[,] SummaryDataTable = new string[numberOfSampleList - NUMBER_OF_SPECIAL_SAMPLES, EnumSizeSummarylColumnsNames];


            for ( column = 0; column < SummaryDataTable.GetLength( COLUMN_SIZE ); column++ )
            {
                for ( row = 0; row < numberOfSampleList - NUMBER_OF_SPECIAL_SAMPLES; row++ )
                {
                    SummaryDataTable[row, column] = "";
                }
            }
             
            string result = "";

            for ( currentSample = 0; currentSample < numberOfSampleList - NUMBER_OF_SPECIAL_SAMPLES; currentSample++ )
            {
                // set index
                SummaryDataTable[currentSample, SUMMARY_INDEX_COLUMN] = ( currentSample + 1 ).ToString();

                // set position
                SummaryDataTable[currentSample, SUMMARY_POSITION_COLUMN] = getWellPos( SamplePositionList[currentSample + NUMBER_OF_SPECIAL_SAMPLES] );

                // set sampleID
                SummaryDataTable[currentSample, SUMMARY_SAMPLE_NAME_COLUMN] = SampleNameList[currentSample + NUMBER_OF_SPECIAL_SAMPLES];

                // check if all 3 columns are negative
                if ( DetailsDataTable[currentSample, DETAILS_MRSA_COLUMN] == NEGATIVE && DetailsDataTable[currentSample, DETAILS_VRE_COLUMN] == NEGATIVE && DetailsDataTable[currentSample, DETAILS_KPC_COLUMN] == NEGATIVE )
                {
                    // if all 3 columns are negative, negative will be written only 1 time (in mrsa column for example)
                    result = DetailsDataTable[currentSample, DETAILS_MRSA_COLUMN];
                }

                // check if one of the 3 columns is invalid sample
                else if ( DetailsDataTable[currentSample, DETAILS_MRSA_COLUMN] == INVALID_SAMPLE || DetailsDataTable[currentSample, DETAILS_VRE_COLUMN] == INVALID_SAMPLE || DetailsDataTable[currentSample, DETAILS_KPC_COLUMN] == INVALID_SAMPLE )
                {
                    // invalid sample will be written only 1 time
                    result = INVALID_SAMPLE_SUMMARY;
                }

                // check if one of the 3 columns is pcr failed  
                else if ( DetailsDataTable[currentSample, DETAILS_MRSA_COLUMN] == PCR_Failed || DetailsDataTable[currentSample, DETAILS_VRE_COLUMN] == PCR_Failed || DetailsDataTable[currentSample, DETAILS_KPC_COLUMN] == PCR_Failed )
                {
                    // pcr failed will be written only 1 time
                    result = PCR_Failed;
                }

                else
                {
                    // if MRSA is negative, don't write it to result 
                    if ( DetailsDataTable[currentSample, DETAILS_MRSA_COLUMN] != NEGATIVE )
                    {
                        result = DetailsDataTable[currentSample, DETAILS_MRSA_COLUMN];
                        result += ", ";
                    }

                    // if VRE is negative, don't write it to result 
                    if ( DetailsDataTable[currentSample, DETAILS_VRE_COLUMN] != NEGATIVE )
                    {
                        result += DetailsDataTable[currentSample, DETAILS_VRE_COLUMN];
                        result += ", ";
                    }

                    // if KPC is negative, don't write it to result 
                    if ( DetailsDataTable[currentSample, DETAILS_KPC_COLUMN] != NEGATIVE )
                    {
                        result += DetailsDataTable[currentSample, DETAILS_KPC_COLUMN];
                    }

                }

                SummaryDataTable[currentSample, SUMMARY_RESULT_COLUMN] = result;
                result = "";
            }
             

            return SummaryDataTable;
        }


          

        /// <summary>
        /// get well position according the well ID
        /// </summary>
        /// <returns></returns>
        public string getWellPos( int wellId )
        {
            //Well position:
            string WellPos = String.Empty;
            int row = Convert.ToInt32( ( wellId - 1 ) / 12/*COLUMNS_IN_SAMPLES_PLATE*/) + ( int )'A';
            int column = ( wellId - 1 ) % 12/*COLUMNS_IN_SAMPLES_PLATE*/ + 1;

            char letter = ( char )( row );
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
   
        public string NVCRatio( string[,] rawDataTable, int row, int column )
        {
            string result = "";

            double NVC = Convert.ToDouble( rawDataTable[row, column + 1] );
            double control = Convert.ToDouble( rawDataTable[row - 3, column + 1] );

            if ( NVC > MINIMUN_SIGNAL_NVC )
            {
                if ( NVC / Math.Abs( control ) > 4 )
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

        public string CalcRatio( string[,] rawDataTable, int row, int column )
        {
            string result = "";
            string markerName = rawDataTable[row, column].ToString();

            string NVC = "";
            double greenSig = 0;
            double control = 0;

            int minimumSignal = 0;
            int minimumRatio = 0;

            // according to the marker name position (row and column) we can set the NVC, green signal and control values 
            switch ( markerName )
            {
                
                case SCCMEC:
                    {
                        NVC = rawDataTable[row + 2, column + 1];
                        greenSig = Convert.ToDouble( rawDataTable[row, column + 1] );
                        control = Convert.ToDouble( rawDataTable[row - 1, column + 1] );
                        minimumSignal = MINIMUN_SIGNAL_SCC_Mec;
                        minimumRatio = MINIMUN_RATIO_SCC_Mec;
                    }
                    break;

                case KPC:
                    {
                        NVC = rawDataTable[row, column + 4];
                        greenSig = Convert.ToDouble( rawDataTable[row, column + 1] );
                        control = Convert.ToDouble( rawDataTable[row - 3, column + 1] );
                        minimumSignal = MINIMUN_SIGNAL_KPC;
                        minimumRatio = MINIMUN_RATIO_KPC;
                    }
                    break;

                case DDL_EFM:
                    {
                        NVC = rawDataTable[row + 1, column + 4];
                        greenSig = Convert.ToDouble( rawDataTable[row, column + 1] );
                        control = Convert.ToDouble( rawDataTable[row - 2, column + 1] );
                        minimumSignal = MINIMUN_SIGNAL_EFM;
                        minimumRatio = MINIMUN_RATIO_EFM;
                    }
                    break;

                case NUC:
                    {
                        NVC = rawDataTable[row + 2, column + 4];
                        greenSig = Convert.ToDouble( rawDataTable[row, column + 1] );
                        control = Convert.ToDouble( rawDataTable[row - 1, column + 1] );
                        minimumSignal = MINIMUN_SIGNAL_nuc;
                        minimumRatio = MINIMUN_RATIO_nuc;
                    }
                    break;

                case PC:
                    {
                        NVC = "";
                        greenSig = Convert.ToDouble( rawDataTable[row, column + 1] );
                        control = Convert.ToDouble( rawDataTable[row - 1, column + 1] );
                        minimumSignal = MINIMUN_SIGNAL_PC;
                        minimumRatio = MINIMUN_RATIO_PC;
                    }
                    break;

                case MECA:
                    {
                        NVC = rawDataTable[row + 2, column + 7];
                        greenSig = Convert.ToDouble( rawDataTable[row, column + 1] );
                        control = Convert.ToDouble( rawDataTable[row - 1, column + 1] );
                        minimumSignal = MINIMUN_SIGNAL_MecA;
                        minimumRatio = MINIMUN_RATIO_MecA;
                    }
                    break;

                case DDL_EFS:
                    {
                        NVC = rawDataTable[row + 1, column + 7];
                        greenSig = Convert.ToDouble( rawDataTable[row, column + 1] );
                        control = Convert.ToDouble( rawDataTable[row - 2, column + 1] );
                        minimumSignal = MINIMUN_SIGNAL_EFS;
                        minimumRatio = MINIMUN_RATIO_EFS;
                    }
                    break;

                case PHOE:
                    {
                        NVC = rawDataTable[row, column + 7];
                        greenSig = Convert.ToDouble( rawDataTable[row, column + 1] );
                        control = Convert.ToDouble( rawDataTable[row - 3, column + 1] );
                        minimumSignal = MINIMUN_SIGNAL_PhoE;
                        minimumRatio = MINIMUN_RATIO_PhoE;
                    }
                    break;

                case SPA:
                    {
                        NVC = rawDataTable[row + 2, column - 2];
                        greenSig = Convert.ToDouble( rawDataTable[row, column + 1] );
                        control = Convert.ToDouble( rawDataTable[row - 1, column + 1] );
                        minimumSignal = MINIMUN_SIGNAL_SPA;
                        minimumRatio = MINIMUN_RATIO_SPA;
                    }
                    break;

                case VANAB:
                    {
                        NVC = rawDataTable[row + 1, column - 2];
                        greenSig = Convert.ToDouble( rawDataTable[row, column + 1] );
                        control = Convert.ToDouble( rawDataTable[row - 2, column + 1] );
                        minimumSignal = MINIMUN_SIGNAL_VAN_A_B;
                        minimumRatio = MINIMUN_RATIO_VAN_A_B;
                    }
                    break;

            }


            if ( NVC == INVALID )
            {
                result = INVALID;
            }

            else if ( greenSig > minimumSignal )
            {
                if ( ( greenSig / Math.Abs( control ) ) > minimumRatio )
                {
                    result = ( greenSig / Math.Abs( control ) ).ToString();
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
            

            if ( ( result != INVALID ) && ( result != NONE ) )
            {
                // based on result value the fixed number of decimal places behind the dot will be chosen.
                if ( Convert.ToDouble( result ) < 10 )
                {
                    // 2 digits after the dot. 
                    result = ( Math.Round( Convert.ToDouble( result ), TWO_DECIMAL ) ).ToString();
                }
                else if ( Convert.ToDouble( result ) < 100 )
                {
                    // 1 digit after the dot.
                    result = ( Math.Round( Convert.ToDouble( result ), ONE_DECIMAL ) ).ToString();
                }
                else
                {
                    // no digits after the dot.
                    result = ( Math.Round( Convert.ToDouble( result ), NO_DECIMAL ) ).ToString();
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

        public string CalcMRSA( string[,] rawDataTable, int row, int column )
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
            checkIfNumeric = double.TryParse( SPA, out number );
            if ( checkIfNumeric == true )
            {
                MRSA = MRSA + 1;
            }

            // check if the string in SCCMec is numeric
            checkIfNumeric = double.TryParse( SCCMec, out number );
            if ( checkIfNumeric == true )
            {
                MRSA = MRSA + 2;
            }

            // check if the string in nuc is numeric
            checkIfNumeric = double.TryParse( nuc, out number );
            if ( checkIfNumeric == true )
            {
                MRSA = MRSA + 4;
            }

            // check if the string in mecA is numeric
            checkIfNumeric = double.TryParse( MecA, out number );
            if ( checkIfNumeric == true )
            {
                MRSA = MRSA + 8;
            }

            // check MRSA value and set the result

            if ( MRSA == 0 )
            {
                result = NEGATIVE;
            }

            else if ( ( MRSA == 1 ) || ( MRSA == 4 ) || ( MRSA == 5 ) )
            {
                result = MSSA;
            }

            else if ( ( MRSA == 2 ) || ( MRSA == 3 ) || ( MRSA == 6 ) || ( MRSA == 7 ) )
            {
                result = SCC_POSITIVE_MSSA;
            }

            else if ( MRSA == 8 )
            {
                result = CON_MR;
            }

            else if ( ( MRSA == 9 ) || ( MRSA == 12 ) || ( MRSA == 13 ) )
            {
                result = CON_MR_MSSA;
            }

            else if ( MRSA == 10 )
            {
                result = POSSIBLE_MRSA;
            }

            else if ( ( MRSA == 11 ) || ( MRSA == 14 ) || ( MRSA == 15 ) )
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

        public string CalcVRE( string[,] rawDataTable, int row, int column )
        {
            string result = "";

            double number = 0;

            bool checkIfNumeric = false;

            int VRE = 0;

            string EFS = rawDataTable[row, column + 2];

            string EFM = rawDataTable[row, column + 5];

            string VAN_A_B = rawDataTable[row, column + 11];

            // check if the string in VAN A B is numeric
            checkIfNumeric = double.TryParse( VAN_A_B, out number );
            if ( checkIfNumeric == true )
            {
                VRE = VRE + 1;
            }

            // check if the string in EFM is numeric
            checkIfNumeric = double.TryParse( EFM, out number );
            if ( checkIfNumeric == true )
            {
                VRE = VRE + 2;
            }

            // check if the string in EFS is numeric
            checkIfNumeric = double.TryParse( EFS, out number );
            if ( checkIfNumeric == true )
            {
                VRE = VRE + 4;
            }

            // check VRE value and set the result

            if ( VRE == 0 )
            {
                result = NEGATIVE;
            }
  
            else if ( VRE == 1 )
            {
                result = NONE_EFS_EFM_VAN;
            }

            else if ( VRE == 2 )
            {
                result = EFM;
            }

            else if ( VRE == 3 )
            {
                result = VRE_EFM;
            }

            else if ( VRE == 4 )
            {
                result = EFS;
            }

            else if ( VRE == 5 )
            {
                result = VRE_EFS;
            }

            else if ( VRE == 6 )
            {
                result = EFS_EFM;
            }

            else if ( VRE == 7 )
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

        public string CalcKPC( string[,] rawDataTable, int row, int column )
        {
            string result = "";

            double number = 0;

            bool checkIfNumeric = false;

            int KPCnum = 0;

            string PhoE = rawDataTable[row, column + 2];

            string KPC = rawDataTable[row, column + 5];

            // check if the string in KPC is numeric
            checkIfNumeric = double.TryParse( KPC, out number );
            if ( checkIfNumeric == true )
            {
                KPCnum = KPCnum + 1;
            }

            // check if the string in PhoE is numeric
            checkIfNumeric = double.TryParse( PhoE, out number );
            if ( checkIfNumeric == true )
            {
                KPCnum = KPCnum + 2;
            }

            // check VRE value and set the result

            if ( KPCnum == 0 )
            {
                result = NEGATIVE;
            }
  
            else if ( KPCnum == 1 )
            {
                result = KPC_NON_KP;
            }

            else if ( KPCnum == 2 )
            {
                result = KP;
            }

            else if ( KPCnum == 3 )
            {
                result = KP_KPC;
            }

            return result;
        }

          
        #endregion

    }

}
 