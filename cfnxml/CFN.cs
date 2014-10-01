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


namespace CFN
{
    public class CFN : TPL_BaseTemplate
    {

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

        /*
         * public struct DTR_ReporterMix
        {
        public int referenceIndex; 	//Sequential #
        public string[,] refSet;	//array of string. Its structure detailed below.
        public double avgGreen;	//Average green background for this reporter mix
        public double avgRed;		//Average red background for this reporter mix
        public bool reporterPassed;	//Define whether reference passed for this reporter mix
        }

         * */
        /// <summary>
        /// Analyze reference data, and update refStatus member that is used in Analyze method.
        /// </summary>
        /// <param name="Data">Raw data of references, Data [ 2* number of references , 2* number of reporter scans]</param>
        /// <returns>Array of reporter mix</returns>
        public DTR_ReporterMix[] RM = new DTR_ReporterMix[4];

        public override DTR_ReporterMix[] reference(double[,] Data)
        {
            RM[0].referenceIndex = 1;
            RM[0].avgGreen = (Data[6, 1] + Data[7, 1]) / 2;
            RM[0].avgRed = (Data[6, 0] + Data[7, 0]) / 2;
            string[,] matrix = new string[10, 7];
            matrix[0, 0] = "Reference set A";
            matrix[0, 1] = "F508del     ";
            matrix[0, 2] = (Data[0, 1] - this.RM[0].avgGreen).ToString("0");
            matrix[0, 3] = (Data[0, 1] / this.RM[0].avgGreen).ToString("0.0");
            matrix[0, 4] = (Data[0, 0] - this.RM[0].avgRed).ToString("0");
            matrix[0, 5] = (Data[0, 0] / this.RM[0].avgRed).ToString("0.0");
            if (Double.Parse(matrix[0, 2]) > 1500 && Double.Parse(matrix[0, 3]) > 3
            && Double.Parse(matrix[0, 4]) > 1500 && Double.Parse(matrix[0, 5]) > 3)
                matrix[0, 6] = "Pass";
            else
                matrix[0, 6] = "FAIL";
            matrix[1, 0] = "Reference set A";
            matrix[1, 1] = "1717-1 G->A ";
            matrix[1, 2] = (Data[2, 1] - this.RM[0].avgGreen).ToString("0");
            matrix[1, 3] = (Data[2, 1] / this.RM[0].avgGreen).ToString("0.0");
            matrix[1, 4] = (Data[2, 0] - this.RM[0].avgRed).ToString("0");
            matrix[1, 5] = (Data[2, 0] / this.RM[0].avgRed).ToString("0.0");
            if (Double.Parse(matrix[1, 2]) > 1500 && Double.Parse(matrix[1, 3]) > 3
            && Double.Parse(matrix[1, 4]) > 1500 && Double.Parse(matrix[1, 5]) > 3)
                matrix[1, 6] = "Pass";
            else
                matrix[1, 6] = "FAIL";
            matrix[2, 0] = "Reference set A";
            matrix[2, 1] = "N1303K      ";
            matrix[2, 2] = (Data[4, 1] - this.RM[0].avgGreen).ToString("0");
            matrix[2, 3] = (Data[4, 1] / this.RM[0].avgGreen).ToString("0.0");
            matrix[2, 4] = (Data[4, 0] - this.RM[0].avgRed).ToString("0");
            matrix[2, 5] = (Data[4, 0] / this.RM[0].avgRed).ToString("0.0");
            if (Double.Parse(matrix[2, 2]) > 1500 && Double.Parse(matrix[2, 3]) > 3
            && Double.Parse(matrix[2, 4]) > 1500 && Double.Parse(matrix[2, 5]) > 3)
                matrix[2, 6] = "Pass";
            else
                matrix[2, 6] = "FAIL";
            matrix[3, 0] = "Reference set A";
            matrix[3, 1] = "3849+10kb C->T";
            matrix[3, 2] = (Data[8, 1] - this.RM[0].avgGreen).ToString("0");
            matrix[3, 3] = (Data[8, 1] / this.RM[0].avgGreen).ToString("0.0");
            matrix[3, 4] = (Data[8, 0] - this.RM[0].avgRed).ToString("0");
            matrix[3, 5] = (Data[8, 0] / this.RM[0].avgRed).ToString("0.0");
            if (Double.Parse(matrix[3, 2]) > 1500 && Double.Parse(matrix[3, 3]) > 3)
                matrix[3, 6] = "Pass";
            else
                matrix[3, 6] = "FAIL";
            matrix[4, 0] = "Reference set A";
            matrix[4, 1] = "3120+1kb del8.6";
            matrix[4, 2] = (Data[10, 1] - this.RM[0].avgGreen).ToString("0");
            matrix[4, 3] = (Data[10, 1] / this.RM[0].avgGreen).ToString("0.0");
            matrix[4, 4] = (Data[10, 0] - this.RM[0].avgRed).ToString("0");
            matrix[4, 5] = (Data[10, 0] / this.RM[0].avgRed).ToString("0.0");
            if (Double.Parse(matrix[4, 2]) > 1500 && Double.Parse(matrix[4, 3]) > 3
            && Double.Parse(matrix[4, 4]) > 1500 && Double.Parse(matrix[4, 5]) > 3)
                matrix[4, 6] = "Pass";
            else
                matrix[4, 6] = "FAIL";

            matrix[5, 0] = "Reference set B";
            matrix[5, 1] = "F508del     ";
            matrix[5, 2] = (Data[1, 1] - this.RM[0].avgGreen).ToString("0");
            matrix[5, 3] = (Data[1, 1] / this.RM[0].avgGreen).ToString("0.0");
            matrix[5, 4] = (Data[1, 0] - this.RM[0].avgRed).ToString("0");
            matrix[5, 5] = (Data[1, 0] / this.RM[0].avgRed).ToString("0.0");
            if (Double.Parse(matrix[5, 2]) > 1500 && Double.Parse(matrix[5, 3]) > 3
            && Double.Parse(matrix[5, 4]) > 1500 && Double.Parse(matrix[5, 5]) > 3)
                matrix[5, 6] = "Pass";
            else
                matrix[5, 6] = "FAIL";

            matrix[6, 0] = "Reference set B";
            matrix[6, 1] = "1717-1 G->A";
            matrix[6, 2] = (Data[3, 1] - this.RM[0].avgGreen).ToString("0");
            matrix[6, 3] = (Data[3, 1] / this.RM[0].avgGreen).ToString("0.0");
            matrix[6, 4] = (Data[3, 0] - this.RM[0].avgRed).ToString("0");
            matrix[6, 5] = (Data[3, 0] / this.RM[0].avgRed).ToString("0.0");
            if (Double.Parse(matrix[6, 2]) > 1500 && Double.Parse(matrix[6, 3]) > 3
            && Double.Parse(matrix[6, 4]) > 1500 && Double.Parse(matrix[6, 5]) > 3)
                matrix[6, 6] = "Pass";
            else
                matrix[6, 6] = "FAIL";
            matrix[7, 0] = "Reference set B";
            matrix[7, 1] = "N1303K      ";
            matrix[7, 2] = (Data[5, 1] - this.RM[0].avgGreen).ToString("0");
            matrix[7, 3] = (Data[5, 1] / this.RM[0].avgGreen).ToString("0.0");
            matrix[7, 4] = (Data[5, 0] - this.RM[0].avgRed).ToString("0");
            matrix[7, 5] = (Data[5, 0] / this.RM[0].avgRed).ToString("0.0");
            if (Double.Parse(matrix[7, 2]) > 1500 && Double.Parse(matrix[7, 3]) > 3
            && Double.Parse(matrix[7, 4]) > 1500 && Double.Parse(matrix[7, 5]) > 3)
                matrix[7, 6] = "Pass";
            else
                matrix[7, 6] = "FAIL";
            matrix[8, 0] = "Reference set B";
            matrix[8, 1] = "3849+10kb C->T";
            matrix[8, 2] = (Data[9, 1] - this.RM[0].avgGreen).ToString("0");
            matrix[8, 3] = (Data[9, 1] / this.RM[0].avgGreen).ToString("0.0");
            matrix[8, 4] = (Data[9, 0] - this.RM[0].avgRed).ToString("0");
            matrix[8, 5] = (Data[9, 0] / this.RM[0].avgRed).ToString("0.0");
            if (Double.Parse(matrix[8, 2]) > 1500 && Double.Parse(matrix[8, 3]) > 3)
                matrix[8, 6] = "Pass";
            else
                matrix[8, 6] = "FAIL";
            matrix[9, 0] = "Reference set B";
            matrix[9, 1] = "3120+1kb del8.6";
            matrix[9, 2] = (Data[11, 1] - this.RM[0].avgGreen).ToString("0");
            matrix[9, 3] = (Data[11, 1] / this.RM[0].avgGreen).ToString("0.0");
            matrix[9, 4] = (Data[11, 0] - this.RM[0].avgRed).ToString("0");
            matrix[9, 5] = (Data[11, 0] / this.RM[0].avgRed).ToString("0.0");
            if (Double.Parse(matrix[9, 2]) > 1500 && Double.Parse(matrix[9, 3]) > 3
            && Double.Parse(matrix[9, 4]) > 1500 && Double.Parse(matrix[9, 5]) > 3)
                matrix[9, 6] = "Pass";
            else
                matrix[9, 6] = "FAIL";
            RM[0].refSet = matrix;
            if ((String.Compare(matrix[0, 6], "FAIL") == 0 && String.Compare(matrix[5, 6], "FAIL") == 0) ||
                (String.Compare(matrix[1, 6], "FAIL") == 0 && String.Compare(matrix[6, 6], "FAIL") == 0) ||
                (String.Compare(matrix[2, 6], "FAIL") == 0 && String.Compare(matrix[7, 6], "FAIL") == 0) ||
                (String.Compare(matrix[3, 6], "FAIL") == 0 && String.Compare(matrix[8, 6], "FAIL") == 0) ||
                (String.Compare(matrix[4, 6], "FAIL") == 0 && String.Compare(matrix[9, 6], "FAIL") == 0))
            {
                RM[0].reporterPassed = false;
                refStatus = refStatus.Replace("A", "0");
            }
            else
            {
                RM[0].reporterPassed = true;
                refStatus = refStatus.Replace("A", "1");
            }
            //Code for reporter 2
            RM[1].referenceIndex = 2;
            RM[1].avgGreen = (Data[4, 5] + Data[5, 5]) / 2;
            RM[1].avgRed = (Data[4, 4] + Data[5, 4]) / 2;
            string[,] matrix2 = new string[10, 7];
            matrix2[0, 0] = "Reference set A";
            matrix2[0, 1] = "G85E        ";
            matrix2[0, 2] = (Data[0, 5] - this.RM[1].avgGreen).ToString("0");
            matrix2[0, 3] = (Data[0, 5] / this.RM[1].avgGreen).ToString("0.0");
            matrix2[0, 4] = (Data[0, 4] - this.RM[1].avgRed).ToString("0");
            matrix2[0, 5] = (Data[0, 4] / this.RM[1].avgRed).ToString("0.0");
            if (Double.Parse(matrix2[0, 2]) > 1500 && Double.Parse(matrix2[0, 3]) > 3
            && Double.Parse(matrix2[0, 4]) > 1500 && Double.Parse(matrix2[0, 5]) > 3)
                matrix2[0, 6] = "Pass";
            else
                matrix2[0, 6] = "FAIL";
            matrix2[1, 0] = "Reference set A";
            matrix2[1, 1] = "G542X       ";
            matrix2[1, 2] = (Data[2, 5] - this.RM[1].avgGreen).ToString("0");
            matrix2[1, 3] = (Data[2, 5] / this.RM[1].avgGreen).ToString("0.0");
            matrix2[1, 4] = (Data[2, 4] - this.RM[1].avgRed).ToString("0");
            matrix2[1, 5] = (Data[2, 4] / this.RM[1].avgRed).ToString("0.0");
            if (Double.Parse(matrix2[1, 2]) > 1500 && Double.Parse(matrix2[1, 3]) > 3
            && Double.Parse(matrix2[1, 4]) > 1500 && Double.Parse(matrix2[1, 5]) > 3)
                matrix2[1, 6] = "Pass";
            else
                matrix2[1, 6] = "FAIL";
            matrix2[2, 0] = "Reference set A";
            matrix2[2, 1] = "I1234V      ";
            matrix2[2, 2] = (Data[6, 5] - this.RM[1].avgGreen).ToString("0");
            matrix2[2, 3] = (Data[6, 5] / this.RM[1].avgGreen).ToString("0.0");
            matrix2[2, 4] = (Data[6, 4] - this.RM[1].avgRed).ToString("0");
            matrix2[2, 5] = (Data[6, 4] / this.RM[1].avgRed).ToString("0.0");
            if (Double.Parse(matrix2[2, 2]) > 1500 && Double.Parse(matrix2[2, 3]) > 3
            && Double.Parse(matrix2[2, 4]) > 1500 && Double.Parse(matrix2[2, 5]) > 3)
                matrix2[2, 6] = "Pass";
            else
                matrix2[2, 6] = "FAIL";
            matrix2[3, 0] = "Reference set A";
            matrix2[3, 1] = "W1282X      ";
            matrix2[3, 2] = (Data[8, 5] - this.RM[1].avgGreen).ToString("0");
            matrix2[3, 3] = (Data[8, 5] / this.RM[1].avgGreen).ToString("0.0");
            matrix2[3, 4] = (Data[8, 4] - this.RM[1].avgRed).ToString("0");
            matrix2[3, 5] = (Data[8, 4] / this.RM[1].avgRed).ToString("0.0");
            if (Double.Parse(matrix2[3, 2]) > 1500 && Double.Parse(matrix2[3, 3]) > 3
            && Double.Parse(matrix2[3, 4]) > 1500 && Double.Parse(matrix2[3, 5]) > 3)
                matrix2[3, 6] = "Pass";
            else
                matrix2[3, 6] = "FAIL";
            matrix2[4, 0] = "Reference set A";
            matrix2[4, 1] = "2183AA>G    ";
            matrix2[4, 2] = (Data[10, 5] - this.RM[1].avgGreen).ToString("0");
            matrix2[4, 3] = (Data[10, 5] / this.RM[1].avgGreen).ToString("0.0");
            matrix2[4, 4] = (Data[10, 4] - this.RM[1].avgRed).ToString("0");
            matrix2[4, 5] = (Data[10, 4] / this.RM[1].avgRed).ToString("0.0");
            if (Double.Parse(matrix2[4, 2]) > 1500 && Double.Parse(matrix2[4, 3]) > 3
            && Double.Parse(matrix2[4, 4]) > 1500 && Double.Parse(matrix2[4, 5]) > 3)
                matrix2[4, 6] = "Pass";
            else
                matrix2[4, 6] = "FAIL";
            matrix2[5, 0] = "Reference set B";
            matrix2[5, 1] = "G85E        ";
            matrix2[5, 2] = (Data[1, 5] - this.RM[1].avgGreen).ToString("0");
            matrix2[5, 3] = (Data[1, 5] / this.RM[1].avgGreen).ToString("0.0");
            matrix2[5, 4] = (Data[1, 4] - this.RM[1].avgRed).ToString("0");
            matrix2[5, 5] = (Data[1, 4] / this.RM[1].avgRed).ToString("0.0");
            if (Double.Parse(matrix2[5, 2]) > 1500 && Double.Parse(matrix2[5, 3]) > 3
            && Double.Parse(matrix2[5, 4]) > 1500 && Double.Parse(matrix2[5, 5]) > 3)
                matrix2[5, 6] = "Pass";
            else
                matrix2[5, 6] = "FAIL";
            matrix2[6, 0] = "Reference set B";
            matrix2[6, 1] = "G542X      ";
            matrix2[6, 2] = (Data[3, 5] - this.RM[1].avgGreen).ToString("0");
            matrix2[6, 3] = (Data[3, 5] / this.RM[1].avgGreen).ToString("0.0");
            matrix2[6, 4] = (Data[3, 4] - this.RM[1].avgRed).ToString("0");
            matrix2[6, 5] = (Data[3, 4] / this.RM[1].avgRed).ToString("0.0");
            if (Double.Parse(matrix2[6, 2]) > 1500 && Double.Parse(matrix2[6, 3]) > 3
            && Double.Parse(matrix2[6, 4]) > 1500 && Double.Parse(matrix2[6, 5]) > 3)
                matrix2[6, 6] = "Pass";
            else
                matrix2[6, 6] = "FAIL";
            matrix2[7, 0] = "Reference set B";
            matrix2[7, 1] = "I1234V      ";
            matrix2[7, 2] = (Data[7, 5] - this.RM[1].avgGreen).ToString("0");
            matrix2[7, 3] = (Data[7, 5] / this.RM[1].avgGreen).ToString("0.0");
            matrix2[7, 4] = (Data[7, 4] - this.RM[1].avgRed).ToString("0");
            matrix2[7, 5] = (Data[7, 4] / this.RM[1].avgRed).ToString("0.0");
            if (Double.Parse(matrix2[7, 2]) > 1500 && Double.Parse(matrix2[7, 3]) > 3
            && Double.Parse(matrix2[7, 4]) > 1500 && Double.Parse(matrix2[7, 5]) > 3)
                matrix2[7, 6] = "Pass";
            else
                matrix2[7, 6] = "FAIL";
            matrix2[8, 0] = "Reference set B";
            matrix2[8, 1] = "W1282X     ";
            matrix2[8, 2] = (Data[9, 5] - this.RM[1].avgGreen).ToString("0");
            matrix2[8, 3] = (Data[9, 5] / this.RM[1].avgGreen).ToString("0.0");
            matrix2[8, 4] = (Data[9, 4] - this.RM[1].avgRed).ToString("0");
            matrix2[8, 5] = (Data[9, 4] / this.RM[1].avgRed).ToString("0.0");
            if (Double.Parse(matrix2[8, 2]) > 1500 && Double.Parse(matrix2[8, 3]) > 3
            && Double.Parse(matrix2[8, 4]) > 1500 && Double.Parse(matrix2[8, 5]) > 3)
                matrix2[8, 6] = "Pass";
            else
                matrix2[8, 6] = "FAIL";
            matrix2[9, 0] = "Reference set B";
            matrix2[9, 1] = "2183AA>G    ";
            matrix2[9, 2] = (Data[11, 5] - this.RM[1].avgGreen).ToString("0");
            matrix2[9, 3] = (Data[11, 5] / this.RM[1].avgGreen).ToString("0.0");
            matrix2[9, 4] = (Data[11, 4] - this.RM[1].avgRed).ToString("0");
            matrix2[9, 5] = (Data[11, 4] / this.RM[1].avgRed).ToString("0.0");
            if (Double.Parse(matrix2[9, 2]) > 1500 && Double.Parse(matrix2[9, 3]) > 3
            && Double.Parse(matrix2[9, 4]) > 1500 && Double.Parse(matrix2[9, 5]) > 3)
                matrix2[9, 6] = "Pass";
            else
                matrix2[9, 6] = "FAIL";
            //for (int x = 0; x <= 6; x++)
            //{
            //    matrix2[8, x] = String.Empty;
            //    matrix2[9, x] = String.Empty;
            //}

            RM[1].refSet = matrix2;
            if ((String.Compare(matrix2[0, 6], "FAIL") == 0 && String.Compare(matrix2[5, 6], "FAIL") == 0) ||
                (String.Compare(matrix2[1, 6], "FAIL") == 0 && String.Compare(matrix2[6, 6], "FAIL") == 0) ||
                (String.Compare(matrix2[2, 6], "FAIL") == 0 && String.Compare(matrix2[7, 6], "FAIL") == 0) ||
                (String.Compare(matrix2[3, 6], "FAIL") == 0 && String.Compare(matrix2[8, 6], "FAIL") == 0) ||
                (String.Compare(matrix2[4, 6], "FAIL") == 0 && String.Compare(matrix2[9, 6], "FAIL") == 0))
            {
                RM[1].reporterPassed = false;
                refStatus = refStatus.Replace("B", "0");
            }
            else
            {
                RM[1].reporterPassed = true;
                refStatus = refStatus.Replace("B", "1");
            }
            //Code for reporter 3
            RM[2].referenceIndex = 3;
            RM[2].avgGreen = (Data[0, 9] + Data[1, 9]) / 2;
            RM[2].avgRed = (Data[0, 8] + Data[1, 8]) / 2;
            string[,] matrix3 = new string[10, 7];
            matrix3[0, 0] = "Reference set A";
            matrix3[0, 1] = "S549R       ";
            matrix3[0, 2] = (Data[2, 9] - this.RM[2].avgGreen).ToString("0");
            matrix3[0, 3] = (Data[2, 9] / this.RM[2].avgGreen).ToString("0.0");
            matrix3[0, 4] = (Data[2, 8] - this.RM[2].avgRed).ToString("0");
            matrix3[0, 5] = (Data[2, 8] / this.RM[2].avgRed).ToString("0.0");
            if (Double.Parse(matrix3[0, 2]) > 1500 && Double.Parse(matrix3[0, 3]) > 3
            && Double.Parse(matrix3[0, 4]) > 1500 && Double.Parse(matrix3[0, 5]) > 3)
                matrix3[0, 6] = "Pass";
            else
                matrix3[0, 6] = "FAIL";
            matrix3[1, 0] = "Reference set A";
            matrix3[1, 1] = "Q359k/T360K ";
            matrix3[1, 2] = (Data[4, 9] - this.RM[2].avgGreen).ToString("0");
            matrix3[1, 3] = (Data[4, 9] / this.RM[2].avgGreen).ToString("0.0");
            matrix3[1, 4] = (Data[4, 8] - this.RM[2].avgRed).ToString("0");
            matrix3[1, 5] = (Data[4, 8] / this.RM[2].avgRed).ToString("0.0");
            if (Double.Parse(matrix3[1, 2]) > 1500 && Double.Parse(matrix3[1, 3]) > 3
            && Double.Parse(matrix3[1, 4]) > 1500 && Double.Parse(matrix3[1, 5]) > 3)
                matrix3[1, 6] = "Pass";
            else
                matrix3[1, 6] = "FAIL";
            matrix3[2, 0] = "Reference set A";
            matrix3[2, 1] = "W1089X      ";
            matrix3[2, 2] = (Data[6, 9] - this.RM[2].avgGreen).ToString("0");
            matrix3[2, 3] = (Data[6, 9] / this.RM[2].avgGreen).ToString("0.0");
            matrix3[2, 4] = (Data[6, 8] - this.RM[2].avgRed).ToString("0");
            matrix3[2, 5] = (Data[6, 8] / this.RM[2].avgRed).ToString("0.0");
            if (Double.Parse(matrix3[2, 2]) > 1500 && Double.Parse(matrix3[2, 3]) > 3
            && Double.Parse(matrix3[2, 4]) > 1500 && Double.Parse(matrix3[2, 5]) > 3)
                matrix3[2, 6] = "Pass";
            else
                matrix3[2, 6] = "FAIL";
            matrix3[3, 0] = "Reference set A";
            matrix3[3, 1] = "CFTR del2.3 ";
            matrix3[3, 2] = (Data[8, 9] - this.RM[2].avgGreen).ToString("0");
            matrix3[3, 3] = (Data[8, 9] / this.RM[2].avgGreen).ToString("0.0");
            matrix3[3, 4] = (Data[8, 8] - this.RM[2].avgRed).ToString("0");
            matrix3[3, 5] = (Data[8, 8] / this.RM[2].avgRed).ToString("0.0");
            if (Double.Parse(matrix3[3, 2]) > 1500 && Double.Parse(matrix3[3, 3]) > 3
            && Double.Parse(matrix3[3, 4]) > 1500 && Double.Parse(matrix3[3, 5]) > 3)
                matrix3[3, 6] = "Pass";
            else
                matrix3[3, 6] = "FAIL";
            matrix3[4, 0] = "Reference set A";
            matrix3[4, 1] = "3121-1 G->A ";
            matrix3[4, 2] = (Data[10, 9] - this.RM[2].avgGreen).ToString("0");
            matrix3[4, 3] = (Data[10, 9] / this.RM[2].avgGreen).ToString("0.0");
            matrix3[4, 4] = (Data[10, 8] - this.RM[2].avgRed).ToString("0");
            matrix3[4, 5] = (Data[10, 8] / this.RM[2].avgRed).ToString("0.0");
            if (Double.Parse(matrix3[3, 2]) > 1500 && Double.Parse(matrix3[2, 3]) > 3
            && Double.Parse(matrix3[3, 4]) > 1500 && Double.Parse(matrix3[2, 5]) > 3)
                matrix3[4, 6] = "Pass";
            else
                matrix3[4, 6] = "FAIL";
            matrix3[5, 0] = "Reference set B";
            matrix3[5, 1] = "S549R       ";
            matrix3[5, 2] = (Data[3, 9] - this.RM[2].avgGreen).ToString("0");
            matrix3[5, 3] = (Data[3, 9] / this.RM[2].avgGreen).ToString("0.0");
            matrix3[5, 4] = (Data[3, 8] - this.RM[2].avgRed).ToString("0");
            matrix3[5, 5] = (Data[3, 8] / this.RM[2].avgRed).ToString("0.0");
            if (Double.Parse(matrix3[5, 2]) > 1500 && Double.Parse(matrix3[5, 3]) > 3
            && Double.Parse(matrix3[5, 4]) > 1500 && Double.Parse(matrix3[5, 5]) > 3)
                matrix3[5, 6] = "Pass";
            else
                matrix3[5, 6] = "FAIL";
            matrix3[6, 0] = "Reference set B";
            matrix3[6, 1] = "Q359k/T360K ";
            matrix3[6, 2] = (Data[5, 9] - this.RM[2].avgGreen).ToString("0");
            matrix3[6, 3] = (Data[5, 9] / this.RM[2].avgGreen).ToString("0.0");
            matrix3[6, 4] = (Data[5, 8] - this.RM[2].avgRed).ToString("0");
            matrix3[6, 5] = (Data[5, 8] / this.RM[2].avgRed).ToString("0.0");
            if (Double.Parse(matrix3[6, 2]) > 1500 && Double.Parse(matrix3[6, 3]) > 3
            && Double.Parse(matrix3[6, 4]) > 1500 && Double.Parse(matrix3[6, 5]) > 3)
                matrix3[6, 6] = "Pass";
            else
                matrix3[6, 6] = "FAIL";
            matrix3[7, 0] = "Reference set B";
            matrix3[7, 1] = "W1089X      ";
            matrix3[7, 2] = (Data[7, 9] - this.RM[2].avgGreen).ToString("0");
            matrix3[7, 3] = (Data[7, 9] / this.RM[2].avgGreen).ToString("0.0");
            matrix3[7, 4] = (Data[7, 8] - this.RM[2].avgRed).ToString("0");
            matrix3[7, 5] = (Data[7, 8] / this.RM[2].avgRed).ToString("0.0");
            if (Double.Parse(matrix3[7, 2]) > 1500 && Double.Parse(matrix3[7, 3]) > 3
            && Double.Parse(matrix3[7, 4]) > 1500 && Double.Parse(matrix3[7, 5]) > 3)
                matrix3[7, 6] = "Pass";
            else
                matrix3[7, 6] = "FAIL";
            matrix3[8, 0] = "Reference set B";
            matrix3[8, 1] = "CFTR del2.3 ";
            matrix3[8, 2] = (Data[9, 9] - this.RM[2].avgGreen).ToString("0");
            matrix3[8, 3] = (Data[9, 9] / this.RM[2].avgGreen).ToString("0.0");
            matrix3[8, 4] = (Data[9, 8] - this.RM[2].avgRed).ToString("0");
            matrix3[8, 5] = (Data[9, 8] / this.RM[2].avgRed).ToString("0.0");
            if (Double.Parse(matrix3[8, 2]) > 1500 && Double.Parse(matrix3[8, 3]) > 3
            && Double.Parse(matrix3[8, 4]) > 1500 && Double.Parse(matrix3[8, 5]) > 3)
                matrix3[8, 6] = "Pass";
            else
                matrix3[8, 6] = "FAIL";
            matrix3[9, 0] = "Reference set B";
            matrix3[9, 1] = "3121-1 G->A ";
            matrix3[9, 2] = (Data[11, 9] - this.RM[2].avgGreen).ToString("0");
            matrix3[9, 3] = (Data[11, 9] / this.RM[2].avgGreen).ToString("0.0");
            matrix3[9, 4] = (Data[11, 8] - this.RM[2].avgRed).ToString("0");
            matrix3[9, 5] = (Data[11, 8] / this.RM[2].avgRed).ToString("0.0");
            if (Double.Parse(matrix3[9, 2]) > 1500 && Double.Parse(matrix3[9, 3]) > 3
            && Double.Parse(matrix3[9, 4]) > 1500 && Double.Parse(matrix3[9, 5]) > 3)
                matrix3[9, 6] = "Pass";
            else
                matrix3[9, 6] = "FAIL";
            //for (int x = 0; x <= 6; x++)
            //{
            //    matrix3[8, x] = String.Empty;
            //    matrix3[9, x] = String.Empty;
            //}

            RM[2].refSet = matrix3;
            if ((String.Compare(matrix3[0, 6], "FAIL") == 0 && String.Compare(matrix3[5, 6], "FAIL") == 0) ||
                (String.Compare(matrix3[1, 6], "FAIL") == 0 && String.Compare(matrix3[6, 6], "FAIL") == 0) ||
                (String.Compare(matrix3[2, 6], "FAIL") == 0 && String.Compare(matrix3[7, 6], "FAIL") == 0) ||
                (String.Compare(matrix3[3, 6], "FAIL") == 0 && String.Compare(matrix3[8, 6], "FAIL") == 0) ||
                (String.Compare(matrix3[4, 6], "FAIL") == 0 && String.Compare(matrix3[9, 6], "FAIL") == 0))
            {
                RM[2].reporterPassed = false;
                refStatus = refStatus.Replace("C", "0");
            }
            else
            {
                RM[2].reporterPassed = true;
                refStatus = refStatus.Replace("C", "1");
            }
            //Code for reporter 4
            RM[3].referenceIndex = 4;
            RM[3].avgGreen = (Data[8, 13] + Data[9, 13]) / 2;
            RM[3].avgRed = (Data[8, 12] + Data[9, 12]) / 2;
            string[,] matrix4 = new string[10, 7];
            matrix4[0, 0] = "Reference set A";
            matrix4[0, 1] = "405+1 G->A  ";
            matrix4[0, 2] = (Data[0, 13] - this.RM[3].avgGreen).ToString("0");
            matrix4[0, 3] = (Data[0, 13] / this.RM[3].avgGreen).ToString("0.0");
            matrix4[0, 4] = (Data[0, 12] - this.RM[3].avgRed).ToString("0");
            matrix4[0, 5] = (Data[0, 12] / this.RM[3].avgRed).ToString("0.0");
            if (Double.Parse(matrix4[0, 2]) > 1500 && Double.Parse(matrix4[0, 3]) > 3
            && Double.Parse(matrix4[0, 4]) > 1500 && Double.Parse(matrix4[0, 5]) > 3)
                matrix4[0, 6] = "Pass";
            else
                matrix4[0, 6] = "FAIL";
            matrix4[1, 0] = "Reference set A";
            matrix4[1, 1] = "D1152H      ";
            matrix4[1, 2] = (Data[2, 13] - this.RM[3].avgGreen).ToString("0");
            matrix4[1, 3] = (Data[2, 13] / this.RM[3].avgGreen).ToString("0.0");
            matrix4[1, 4] = (Data[2, 12] - this.RM[3].avgRed).ToString("0");
            matrix4[1, 5] = (Data[2, 12] / this.RM[3].avgRed).ToString("0.0");
            if (Double.Parse(matrix4[1, 2]) > 1500 && Double.Parse(matrix4[1, 3]) > 3
            && Double.Parse(matrix4[1, 4]) > 1500 && Double.Parse(matrix4[1, 5]) > 3)
                matrix4[1, 6] = "Pass";
            else
                matrix4[1, 6] = "FAIL";
            matrix4[2, 0] = "Reference set A";
            matrix4[2, 1] = "4010 delTATT";
            matrix4[2, 2] = (Data[4, 13] - this.RM[3].avgGreen).ToString("0");
            matrix4[2, 3] = (Data[4, 13] / this.RM[3].avgGreen).ToString("0.0");
            matrix4[2, 4] = (Data[4, 12] - this.RM[3].avgRed).ToString("0");
            matrix4[2, 5] = (Data[4, 12] / this.RM[3].avgRed).ToString("0.0");
            if (Double.Parse(matrix4[2, 2]) > 1500 && Double.Parse(matrix4[2, 3]) > 3
            && Double.Parse(matrix4[2, 4]) > 1500 && Double.Parse(matrix4[2, 5]) > 3)
                matrix4[2, 6] = "Pass";
            else
                matrix4[2, 6] = "FAIL";
            matrix4[3, 0] = "Reference set A";
            matrix4[3, 1] = "Y1092X      ";
            matrix4[3, 2] = (Data[6, 13] - this.RM[3].avgGreen).ToString("0");
            matrix4[3, 3] = (Data[6, 13] / this.RM[3].avgGreen).ToString("0.0");
            matrix4[3, 4] = (Data[6, 12] - this.RM[3].avgRed).ToString("0");
            matrix4[3, 5] = (Data[6, 12] / this.RM[3].avgRed).ToString("0.0");
            if (Double.Parse(matrix4[3, 2]) > 1500 && Double.Parse(matrix4[3, 3]) > 3
            && Double.Parse(matrix4[3, 4]) > 1500 && Double.Parse(matrix4[3, 5]) > 3)
                matrix4[3, 6] = "Pass";
            else
                matrix4[3, 6] = "FAIL";
            matrix4[4, 0] = "Reference set B";
            matrix4[4, 1] = "405+1 G->A  ";
            matrix4[4, 2] = (Data[1, 13] - this.RM[3].avgGreen).ToString("0");
            matrix4[4, 3] = (Data[1, 13] / this.RM[3].avgGreen).ToString("0.0");
            matrix4[4, 4] = (Data[1, 12] - this.RM[3].avgRed).ToString("0");
            matrix4[4, 5] = (Data[1, 12] / this.RM[3].avgRed).ToString("0.0");
            if (Double.Parse(matrix4[4, 2]) > 1500 && Double.Parse(matrix4[4, 3]) > 3
             && Double.Parse(matrix4[4, 4]) > 1500 && Double.Parse(matrix4[4, 5]) > 3)
                matrix4[4, 6] = "Pass";
            else
                matrix4[4, 6] = "FAIL";
            matrix4[5, 0] = "Reference set B";
            matrix4[5, 1] = "D1152H      ";
            matrix4[5, 2] = (Data[3, 13] - this.RM[3].avgGreen).ToString("0");
            matrix4[5, 3] = (Data[3, 13] / this.RM[3].avgGreen).ToString("0.0");
            matrix4[5, 4] = (Data[3, 12] - this.RM[3].avgRed).ToString("0");
            matrix4[5, 5] = (Data[3, 12] / this.RM[3].avgRed).ToString("0.0");
            if (Double.Parse(matrix4[5, 2]) > 1500 && Double.Parse(matrix4[5, 3]) > 3
            && Double.Parse(matrix4[5, 4]) > 1500 && Double.Parse(matrix4[5, 5]) > 3)
                matrix4[5, 6] = "Pass";
            else
                matrix4[5, 6] = "FAIL";
            matrix4[6, 0] = "Reference set B";
            matrix4[6, 1] = "4010 delTATT";
            matrix4[6, 2] = (Data[5, 13] - this.RM[3].avgGreen).ToString("0");
            matrix4[6, 3] = (Data[5, 13] / this.RM[3].avgGreen).ToString("0.0");
            matrix4[6, 4] = (Data[5, 12] - this.RM[3].avgRed).ToString("0");
            matrix4[6, 5] = (Data[5, 12] / this.RM[3].avgRed).ToString("0.0");
            if (Double.Parse(matrix4[6, 2]) > 1500 && Double.Parse(matrix4[6, 3]) > 3
            && Double.Parse(matrix4[6, 4]) > 1500 && Double.Parse(matrix4[6, 5]) > 3)
                matrix4[6, 6] = "Pass";
            else
                matrix4[6, 6] = "FAIL";
            matrix4[7, 0] = "Reference set B";
            matrix4[7, 1] = "Y1092X      ";
            matrix4[7, 2] = (Data[7, 13] - this.RM[3].avgGreen).ToString("0");
            matrix4[7, 3] = (Data[7, 13] / this.RM[3].avgGreen).ToString("0.0");
            matrix4[7, 4] = (Data[7, 12] - this.RM[3].avgRed).ToString("0");
            matrix4[7, 5] = (Data[7, 12] / this.RM[3].avgRed).ToString("0.0");
            if (Double.Parse(matrix4[7, 2]) > 1500 && Double.Parse(matrix4[7, 3]) > 3
            && Double.Parse(matrix4[7, 4]) > 1500 && Double.Parse(matrix4[7, 5]) > 3)
                matrix4[7, 6] = "Pass";
            else
                matrix4[7, 6] = "FAIL";
            for (int x = 0; x <= 6; x++)
            {
                matrix4[8, x] = String.Empty;
                matrix4[9, x] = String.Empty;
            }

            RM[3].refSet = matrix4;
            if ((String.Compare(matrix4[0, 6], "FAIL") == 0 && String.Compare(matrix4[4, 6], "FAIL") == 0) ||
                (String.Compare(matrix4[1, 6], "FAIL") == 0 && String.Compare(matrix4[5, 6], "FAIL") == 0) ||
                (String.Compare(matrix4[2, 6], "FAIL") == 0 && String.Compare(matrix4[6, 6], "FAIL") == 0) ||
                (String.Compare(matrix4[3, 6], "FAIL") == 0 && String.Compare(matrix4[7, 6], "FAIL") == 0))
            {
                RM[3].reporterPassed = false;
                refStatus = refStatus.Replace("D", "0");
            }
            else
            {
                RM[3].reporterPassed = true;
                refStatus = refStatus.Replace("D", "1");
            }
            return RM;
        }


        /// <summary>
        /// Analyze results of one sample
        /// </summary>
        /// <param name="refStr">Reference string</param>
        /// <param name="Data">Raw data of sample: Array size will (X,Y) where X is the number of captures and Y is the number of reporters multiplied by 2(one for green and one for red).</param>
        /// <returns>Method will return an array. Returned array size will be (X,3) where X is number of mutations. Structure of the array will be:(n,0)Mutation Name , (n,1) Scaled Ratio ,(n,2)Call</returns>
        //public override string[,] Analyze(string refStr, double[, ,] Data)
        //{
        //    ////////////////////////////////////////////////////////////////////////
        //    ///                     ADD YOUR CODE                               ///
        //    ///////////////////////////////////////////////////////////////////////
        //    return null;
        //}
        public string[,] myReply = new string[24, 5];
        enum Led
        { LED1, LED2, LED3, LED4 }
        enum Reporter
        { REPORTER_1, REPORTER_2, REPORTER_3, REPORTER_4 }
        enum Capture
        { CAPTURE_1, CAPTURE_2, CAPTURE_3, CAPTURE_4, CAPTURE_5, CAPTURE_6 }
        //public override string[,] Analyze(string refStr, double[, ,] Data)
        //{
            /////Method will get two arguments
            /////1. Reference string 'refStr' to notify reference status
            /////2. Tri-dimensional array 'Data' with signals where dimensions are 
            ///// LED, Reporter, Capture respectivly.
            /////Method will return bi-dimensional array with following data for each mutation:
            /////Mutation name, net green signal, net red signal, scaled ratio, genotype.

            //Led RED = Led.LED1;
            //Led GREEN = Led.LED2;
            //Reporter R1 = Reporter.REPORTER_1;
            //Reporter R2 = Reporter.REPORTER_2;
            //Reporter R3 = Reporter.REPORTER_3;
            //Reporter R4 = Reporter.REPORTER_4;
            //Capture C1 = Capture.CAPTURE_1;
            //Capture C2 = Capture.CAPTURE_2;
            //Capture C3 = Capture.CAPTURE_3;
            //Capture C4 = Capture.CAPTURE_4;
            //Capture C5 = Capture.CAPTURE_5;
            //Capture C6 = Capture.CAPTURE_6;



            //myReply[0, 0] = "F508del";
            //myReply[0, 1] = Data[(int)GREEN, (int)R1, (int)C1].ToString();
            //myReply[0, 2] = Data[(int)RED, (int)R1, (int)C1].ToString();
            //myReply[0, 3] = this.F508del(refStr, Data[(int)GREEN, (int)R1, (int)C1], Data[(int)RED, (int)R1, (int)C1], Data[(int)GREEN, (int)R1, (int)C4], Data[(int)RED, (int)R1, (int)C4]);
            //myReply[0, 4] = this.Genotype(myReply[0, 3]);
            //myReply[1, 0] = "1717-1 G>A";
            //myReply[1, 1] = Data[(int)GREEN, (int)R1, (int)C2].ToString();
            //myReply[1, 2] = Data[(int)RED, (int)R1, (int)C2].ToString();
            //myReply[1, 3] = this._1717(refStr, Data[(int)GREEN, (int)R1, (int)C2], Data[(int)RED, (int)R1, (int)C2], Data[(int)GREEN, (int)R1, (int)C4], Data[(int)RED, (int)R1, (int)C4]);
            //myReply[1, 4] = this.Genotype(myReply[1, 3]);
            //myReply[2, 0] = "N1303K";
            //myReply[2, 1] = Data[(int)GREEN, (int)R1, (int)C3].ToString();
            //myReply[2, 2] = Data[(int)RED, (int)R1, (int)C3].ToString();
            //myReply[2, 3] = this.N1303K(refStr, Data[(int)GREEN, (int)R1, (int)C3], Data[(int)RED, (int)R1, (int)C3], Data[(int)GREEN, (int)R1, (int)C4], Data[(int)RED, (int)R1, (int)C4]);
            //myReply[2, 4] = this.Genotype(myReply[2, 3]);
            //myReply[3, 0] = "Control";
            //myReply[3, 1] = Data[(int)GREEN, (int)R1, (int)C4].ToString();
            //myReply[3, 2] = Data[(int)RED, (int)R1, (int)C4].ToString();
            //myReply[3, 3] = String.Empty;
            //myReply[3, 4] = String.Empty;
            //myReply[4, 0] = "3849+10kb C>T";
            //myReply[4, 1] = Data[(int)GREEN, (int)R1, (int)C5].ToString();
            //myReply[4, 2] = Data[(int)RED, (int)R1, (int)C5].ToString();
            //myReply[4, 3] = this._3849(refStr, Data[(int)GREEN, (int)R1, (int)C5], Data[(int)RED, (int)R1, (int)C5], Data[(int)GREEN, (int)R1, (int)C4], Data[(int)RED, (int)R1, (int)C4]);
            //myReply[4, 4] = this.Genotype(myReply[4, 3]);
            //myReply[5, 0] = "3120+1kb del8.6";
            //myReply[5, 1] = Data[(int)GREEN, (int)R1, (int)C6].ToString();
            //myReply[5, 2] = Data[(int)RED, (int)R1, (int)C6].ToString();
            //myReply[5, 3] = this._3120(refStr, Data[(int)GREEN, (int)R1, (int)C6], Data[(int)RED, (int)R1, (int)C6], Data[(int)GREEN, (int)R1, (int)C4], Data[(int)RED, (int)R1, (int)C4]);
            //myReply[5, 4] = this.Genotype(myReply[5, 3]);
            //myReply[6, 0] = "G85E";
            //myReply[6, 1] = Data[(int)GREEN, (int)R2, (int)C1].ToString();
            //myReply[6, 2] = Data[(int)RED, (int)R2, (int)C1].ToString();
            //myReply[6, 3] = this.G85E(refStr, Data[(int)GREEN, (int)R2, (int)C1], Data[(int)RED, (int)R2, (int)C1], Data[(int)GREEN, (int)R2, (int)C3], Data[(int)RED, (int)R2, (int)C3]);
            //myReply[6, 4] = this.Genotype(myReply[6, 3]);
            //myReply[7, 0] = "G542X";
            //myReply[7, 1] = Data[(int)GREEN, (int)R2, (int)C2].ToString();
            //myReply[7, 2] = Data[(int)RED, (int)R2, (int)C2].ToString();
            //myReply[7, 3] = this.G542X(refStr, Data[(int)GREEN, (int)R2, (int)C2], Data[(int)RED, (int)R2, (int)C2], Data[(int)GREEN, (int)R2, (int)C3], Data[(int)RED, (int)R2, (int)C3]);
            //myReply[7, 4] = this.Genotype(myReply[7, 3]);
            //myReply[8, 0] = "Control";
            //myReply[8, 1] = Data[(int)GREEN, (int)R2, (int)C3].ToString();
            //myReply[8, 2] = Data[(int)RED, (int)R2, (int)C3].ToString();
            //myReply[8, 3] = String.Empty;
            //myReply[8, 4] = String.Empty;
            //myReply[9, 0] = "I1234V";
            //myReply[9, 1] = Data[(int)GREEN, (int)R2, (int)C4].ToString();
            //myReply[9, 2] = Data[(int)RED, (int)R2, (int)C4].ToString();
            //myReply[9, 3] = this.I1234V(refStr, Data[(int)GREEN, (int)R2, (int)C4], Data[(int)RED, (int)R2, (int)C4], Data[(int)GREEN, (int)R2, (int)C3], Data[(int)RED, (int)R2, (int)C3]);
            //myReply[9, 4] = this.Genotype(myReply[9, 3]);
            //myReply[10, 0] = "W1282X";
            //myReply[10, 1] = Data[(int)GREEN, (int)R2, (int)C5].ToString();
            //myReply[10, 2] = Data[(int)RED, (int)R2, (int)C5].ToString();
            //myReply[10, 3] = this.W1282X(refStr, Data[(int)GREEN, (int)R2, (int)C5], Data[(int)RED, (int)R2, (int)C5], Data[(int)GREEN, (int)R2, (int)C3], Data[(int)RED, (int)R2, (int)C3]);
            //myReply[10, 4] = this.Genotype(myReply[10, 3]);
            //myReply[11, 0] = "2183AA>G";
            //myReply[11, 1] = Data[(int)GREEN, (int)R2, (int)C6].ToString();
            //myReply[11, 2] = Data[(int)RED, (int)R2, (int)C6].ToString();
            //myReply[11, 3] = this._2183AA(refStr, Data[(int)GREEN, (int)R2, (int)C6], Data[(int)RED, (int)R2, (int)C6], Data[(int)GREEN, (int)R2, (int)C3], Data[(int)RED, (int)R2, (int)C3]);
            //myReply[11, 4] = this.Genotype(myReply[11, 3]);
            //myReply[12, 0] = "Control";
            //myReply[12, 1] = Data[(int)GREEN, (int)R3, (int)C1].ToString();
            //myReply[12, 2] = Data[(int)RED, (int)R3, (int)C1].ToString();
            //myReply[12, 3] = String.Empty;
            //myReply[12, 4] = String.Empty;
            //myReply[13, 0] = "S549R T>G";
            //myReply[13, 1] = Data[(int)GREEN, (int)R3, (int)C2].ToString();
            //myReply[13, 2] = Data[(int)RED, (int)R3, (int)C2].ToString();
            //myReply[13, 3] = this.S549R(refStr, Data[(int)GREEN, (int)R3, (int)C2], Data[(int)RED, (int)R3, (int)C2], Data[(int)GREEN, (int)R3, (int)C1], Data[(int)RED, (int)R3, (int)C1]);
            //myReply[13, 4] = this.Genotype(myReply[13, 3]);
            //myReply[14, 0] = "Q359/T360K";
            //myReply[14, 1] = Data[(int)GREEN, (int)R3, (int)C3].ToString();
            //myReply[14, 2] = Data[(int)RED, (int)R3, (int)C3].ToString();
            //myReply[14, 3] = this.Q359K(refStr, Data[(int)GREEN, (int)R3, (int)C3], Data[(int)RED, (int)R3, (int)C3], Data[(int)GREEN, (int)R3, (int)C1], Data[(int)RED, (int)R3, (int)C1]);
            //myReply[14, 4] = this.Genotype(myReply[14, 3]);
            //myReply[15, 0] = "W1089X";
            //myReply[15, 1] = Data[(int)GREEN, (int)R3, (int)C4].ToString();
            //myReply[15, 2] = Data[(int)RED, (int)R3, (int)C4].ToString();
            //myReply[15, 3] = this.W1089X(refStr, Data[(int)GREEN, (int)R3, (int)C4], Data[(int)RED, (int)R3, (int)C4], Data[(int)GREEN, (int)R3, (int)C1], Data[(int)RED, (int)R3, (int)C1]);
            //myReply[15, 4] = this.Genotype(myReply[15, 3]);
            //myReply[16, 0] = "CFTR del2.3";
            //myReply[16, 1] = Data[(int)GREEN, (int)R3, (int)C5].ToString();
            //myReply[16, 2] = Data[(int)RED, (int)R3, (int)C5].ToString();
            //myReply[16, 3] = this.CFTRdel23(refStr, Data[(int)GREEN, (int)R3, (int)C5], Data[(int)RED, (int)R3, (int)C5], Data[(int)GREEN, (int)R3, (int)C1], Data[(int)RED, (int)R3, (int)C1]);
            //myReply[16, 4] = this.Genotype(myReply[16, 3]);
            //myReply[17, 0] = "3121-1 G>A";
            //myReply[17, 1] = Data[(int)GREEN, (int)R3, (int)C6].ToString();
            //myReply[17, 2] = Data[(int)RED, (int)R3, (int)C6].ToString();
            //myReply[17, 3] = this._3121(refStr, Data[(int)GREEN, (int)R3, (int)C6], Data[(int)RED, (int)R3, (int)C6], Data[(int)GREEN, (int)R3, (int)C1], Data[(int)RED, (int)R3, (int)C1]);
            //myReply[17, 4] = this.Genotype(myReply[17, 3]);
            //myReply[18, 0] = "405+1 G>A";
            //myReply[18, 1] = Data[(int)GREEN, (int)R4, (int)C1].ToString();
            //myReply[18, 2] = Data[(int)RED, (int)R4, (int)C1].ToString();
            //myReply[18, 3] = this._405(refStr, Data[(int)GREEN, (int)R4, (int)C1], Data[(int)RED, (int)R4, (int)C1], Data[(int)GREEN, (int)R4, (int)C5], Data[(int)RED, (int)R4, (int)C5]);
            //myReply[18, 4] = this.Genotype(myReply[18, 3]);
            //myReply[19, 0] = "D1152H";
            //myReply[19, 1] = Data[(int)GREEN, (int)R4, (int)C2].ToString();
            //myReply[19, 2] = Data[(int)RED, (int)R4, (int)C2].ToString();
            //myReply[19, 3] = this.D1152H(refStr, Data[(int)GREEN, (int)R4, (int)C2], Data[(int)RED, (int)R4, (int)C2], Data[(int)GREEN, (int)R4, (int)C5], Data[(int)RED, (int)R4, (int)C5]);
            //myReply[19, 4] = this.Genotype(myReply[19, 3]);
            //myReply[20, 0] = "4010 delTATT";
            //myReply[20, 1] = Data[(int)GREEN, (int)R4, (int)C3].ToString();
            //myReply[20, 2] = Data[(int)RED, (int)R4, (int)C3].ToString();
            //myReply[20, 3] = this._4010(refStr, Data[(int)GREEN, (int)R4, (int)C3], Data[(int)RED, (int)R4, (int)C3], Data[(int)GREEN, (int)R4, (int)C5], Data[(int)RED, (int)R4, (int)C5]);
            //myReply[20, 4] = this.Genotype(myReply[20, 3]);
            //myReply[21, 0] = "Y1092X";
            //myReply[21, 1] = Data[(int)GREEN, (int)R4, (int)C4].ToString();
            //myReply[21, 2] = Data[(int)RED, (int)R4, (int)C4].ToString();
            //myReply[21, 3] = this.Y1092X(refStr, Data[(int)GREEN, (int)R4, (int)C4], Data[(int)RED, (int)R4, (int)C4], Data[(int)GREEN, (int)R4, (int)C5], Data[(int)RED, (int)R4, (int)C5]);
            //myReply[21, 4] = this.Genotype(myReply[21, 3]);
            //myReply[22, 0] = "Control";
            //myReply[22, 1] = Data[(int)GREEN, (int)R4, (int)C5].ToString();
            //myReply[22, 2] = Data[(int)RED, (int)R4, (int)C5].ToString();
            //myReply[22, 3] = String.Empty;
            //myReply[22, 4] = String.Empty;
            //myReply[23, 0] = String.Empty;
            //myReply[23, 1] = String.Empty;
            //myReply[23, 2] = String.Empty;
            //myReply[23, 3] = String.Empty;
            //myReply[23, 4] = String.Empty;

            ////StreamWriter sw = new StreamWriter("C:\\NCXL_log.txt");
            ////for (int row = 0; row <= myReply.GetUpperBound(0); row++)
            ////{
            ////    for (int column = 0; column <= myReply.GetUpperBound(1); column++)
            ////    {
            ////        sw.Write("{0}\t",myReply[row,column]);
            ////    }
            ////    sw.WriteLine("\r");
            ////}


            //return myReply;
        //}



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


        enum GeneralColumnsNames1 { Number, Position, SampleID };
        enum ReporterNames1 { ReporterMix1, ReporterMix2, ReporterMix3, ReporterMix4 };
        enum ColumnsNamesOfEachReporter1 { Marker, Green, Ratio };
        enum CustomColumnsNames1 { Interpretation };



        /// <summary>
        /// Analyze results of all samples and build raw data table
        /// </summary>
        /// <param name="MarkerTruthTable">Truth Table of the specific Marker</param>
        /// <param name="Data">data of signals values</param>
        /// <param name="RawDataReporterColumnNo">amount of RawData Table column</param>
        /// <returns></returns>return the raw data table [num of sample,num of markers ,RawDataReporterColumnNo]




        public string[,] Analyze( double[, , ,] signals, out string[] GeneralColumnsNames, out string[] ReporterNames, out string[] ColumnsNamesOfEachReporter,
                 out string[] CustomColumnsNames, string[] SampleNameList, int[] SamplePositionList )
        {



            string[,] ICPMarkerName = { { "Histidine",  "Histidine", "Histidine", "Histidine" },
                                        { "MecA",       "nuc",       "SCC Mec",   "SPA" },
                                        { "EFS",        "EFM",       "",          "VAN A B" },
                                        { "PhoE",       "KPC",       "NVC",       "" }};

            int i = 0;
            int j = 0;
            int k = 1;

            // get the calculation of the current row
            int currectRow = 0;
            // get the calculation of the current column
            int currectColumn = 0;
            // get the calculation of the current column for marker 
            int currectColumnForMarker = 0;

            // get number of pads
            int numberOfMarkers = signals.GetLength( 1 );
            // get number of samples
            int numberOfSampleList = signals.GetLength( 0 );

            // init size from enum
            // dov - need to fix so that the data be received from XML
            int EnumSizeGeneralColumnsNames = Enum.GetNames( typeof( GeneralColumnsNames1 ) ).Length;
            int EnumSizeReporterNames = Enum.GetNames( typeof( ReporterNames1 ) ).Length;
            int EnumSizeColumnsNamesOfEachReporter = Enum.GetNames( typeof( ColumnsNamesOfEachReporter1 ) ).Length;
            int EnumSizeCustomColumnsNames = Enum.GetNames( typeof( CustomColumnsNames1 ) ).Length;

            // set headlines of the table        
            GeneralColumnsNames = new string[EnumSizeGeneralColumnsNames];
            ReporterNames = new string[EnumSizeReporterNames];
            ColumnsNamesOfEachReporter = new string[EnumSizeColumnsNamesOfEachReporter];
            CustomColumnsNames = new string[EnumSizeCustomColumnsNames];


            // set name for each parameter 
            for ( i = 0; i < GeneralColumnsNames.Length; i++ )
            {
                GeneralColumnsNames[i] = Enum.GetName( typeof( GeneralColumnsNames1 ), i );
            }

            for ( i = 0; i < ReporterNames.Length; i++ )
            {
                ReporterNames[i] = Enum.GetName( typeof( ReporterNames1 ), i );
            }

            for ( i = 0; i < ColumnsNamesOfEachReporter.Length; i++ )
            {
                ColumnsNamesOfEachReporter[i] = Enum.GetName( typeof( ColumnsNamesOfEachReporter1 ), i );
            }

            for ( i = 0; i < CustomColumnsNames.Length; i++ )
            {
                CustomColumnsNames[i] = Enum.GetName( typeof( CustomColumnsNames1 ), i );
            }




            /////////////////////////////////////////////////////////////////////////////////////////
            // temp value - delete it when the values from the machine will be accurate  
            numberOfMarkers = numberOfMarkers - 2;

            /////////////////////////////////////////////////////////////////////////////////////////


            // size of column is: number of GeneralColumnsNames + ReporterNames * ColumnsNamesOfEachReporter + CustomColumnsNames 
            int column = EnumSizeGeneralColumnsNames + EnumSizeReporterNames * EnumSizeColumnsNamesOfEachReporter + EnumSizeCustomColumnsNames;
            // size of row is: number of pads * number of samples + 2 (sample 1)
            int row = numberOfMarkers * numberOfSampleList + 2 * NUMBER_OF_SPECIAL_SAMPLES;

            string[,] RawDataTable = new string[row, column];


            // set markers (capture / pads) to each reporter (scan)
            // loop that run on all the samples
            for ( i = 0; i < numberOfSampleList; i++ )
            {
                // loop that run on all the markers (or captures / pads)
                for ( j = 0; j < numberOfMarkers; j++ )
                {
                    // loop that run on all the reporters (or scans)
                    for ( k = 0; k < EnumSizeReporterNames; k++ )
                    {
                        currectRow = j + 2 + numberOfMarkers * i;

                        // 1 is represent the marker column (we need to skip on it in order to set the green value in the next column) 
                        currectColumn = 1 + EnumSizeGeneralColumnsNames + EnumSizeColumnsNamesOfEachReporter * k;

                        currectColumnForMarker = EnumSizeGeneralColumnsNames + EnumSizeColumnsNamesOfEachReporter * k;

                        RawDataTable[currectRow, currectColumn] = signals[i, j, k, 0].ToString();

                        // set index - index start from 1
                        RawDataTable[currectRow, 0] = ( i + 1 ).ToString();

                        // set position number
                        RawDataTable[currectRow, 1] = getWellPos( SamplePositionList[i] );

                        // set sample name
                        RawDataTable[currectRow, 2] = SampleNameList[i];

                        // set marker name
                        RawDataTable[currectRow, currectColumnForMarker] = ICPMarkerName[j, k];
                    }



                }

            }

            // need to fix all the values when received currect data
            if ( NUMBER_OF_SPECIAL_SAMPLES > 0 )
            {
                // set marker names
                RawDataTable[0, 7] = "843";
                RawDataTable[1, 7] = "25096";

                // set green values
                RawDataTable[0, 6] = "Bkg.";
                RawDataTable[1, 6] = "PC";

                // set sample index
                RawDataTable[0, 0] = "0";
                RawDataTable[1, 0] = "0";

                // set sample position
                RawDataTable[0, 1] = "A1";
                RawDataTable[1, 1] = "A1";

                // set sample name
                RawDataTable[0, 2] = "Sample 1";
                RawDataTable[1, 2] = "Sample 1";

            }



            // set ratio values
            for ( i = 0; i < column; i++ )
            {
                for ( j = 0; j < row; j++ )
                {

                    if ( RawDataTable[j, i] == "NVC" )
                    {
                        RawDataTable[j, i + 2] = NVCRatio( RawDataTable, j, i );
                    }

                    else if ( ( RawDataTable[j, i] == "SCC Mec" ) || ( RawDataTable[j, i] == "KPC" ) || ( RawDataTable[j, i] == "EFM" )
                           || ( RawDataTable[j, i] == "nuc" ) || ( RawDataTable[j, i] == "MecA" ) || ( RawDataTable[j, i] == "EFS" )
                           || ( RawDataTable[j, i] == "PhoE" ) || ( RawDataTable[j, i] == "SPA" ) || ( RawDataTable[j, i] == "VAN A B" ) )
                    {
                        // set the return value to the appropriate ratio index
                        RawDataTable[j, i + 2] = CalcRatio( RawDataTable, j, i );
                    }

                    else if ( ( RawDataTable[j, i] == "PC" ) )
                    {
                        RawDataTable[j, i + 2] = CalcRatio( RawDataTable, j, i );

                        if ( RawDataTable[j, i + 2] == "None" )
                        {
                            RawDataTable[j, i + 9] = "PC Fail";
                        }

                        else
                        {
                            RawDataTable[j, i + 9] = "PC Pass";
                        }
                    }

                }

            }



            // set interpretation values
            for ( i = 0; i < column; i++ )
            {
                for ( j = 0; j < row; j++ )
                {
                    // check for the marker that is in the row of the needed interpretation

                    if ( RawDataTable[j, i] == "MecA" )
                    {
                        // set the return value to the appropriate interpretation index
                        RawDataTable[j, i + 12] = CalcMRSA( RawDataTable, j, i );
                    }

                    else if ( RawDataTable[j, i] == "EFS" )
                    {
                        // set the return value to the appropriate interpretation index
                        RawDataTable[j, i + 12] = CalcVRE( RawDataTable, j, i );
                    }

                    else if ( RawDataTable[j, i] == "PhoE" )
                    {
                        // set the return value to the appropriate interpretation index
                        RawDataTable[j, i + 12] = CalcKPC( RawDataTable, j, i );
                    }
                }

            }



            string Name = @"C:\dov\testANALYZE.txt";

            // Create a new stream to write to the file

            StreamWriter writer = new StreamWriter( Name );
            try
            {
                for ( int m = 0; m < row; ++m )
                {
                    for ( int n = 0; n < column; ++n )
                    {
                        writer.Write( "{0,2} ", RawDataTable[m, n] );
                    }
                    writer.WriteLine();
                }

                writer.Close();

            }
            catch
            {

            }


            return RawDataTable;
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
                    result = "OK";
                }
                else
                {
                    result = "NOA";
                }
            }
            else
            {
                result = "NOA";
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

            switch ( markerName )
            {
                case "SCC Mec":
                    {
                        NVC = rawDataTable[row + 2, column + 1];
                        greenSig = Convert.ToDouble( rawDataTable[row, column + 1] );
                        control = Convert.ToDouble( rawDataTable[row - 1, column + 1] );
                        minimumSignal = MINIMUN_SIGNAL_SCC_Mec;
                        minimumRatio = MINIMUN_RATIO_SCC_Mec;
                    }
                    break;

                case "KPC":
                    {
                        NVC = rawDataTable[row, column + 4];
                        greenSig = Convert.ToDouble( rawDataTable[row, column + 1] );
                        control = Convert.ToDouble( rawDataTable[row - 3, column + 1] );
                        minimumSignal = MINIMUN_SIGNAL_KPC;
                        minimumRatio = MINIMUN_RATIO_KPC;
                    }
                    break;

                case "EFM":
                    {
                        NVC = rawDataTable[row + 1, column + 4];
                        greenSig = Convert.ToDouble( rawDataTable[row, column + 1] );
                        control = Convert.ToDouble( rawDataTable[row - 2, column + 1] );
                        minimumSignal = MINIMUN_SIGNAL_EFM;
                        minimumRatio = MINIMUN_RATIO_EFM;
                    }
                    break;

                case "nuc":
                    {
                        NVC = rawDataTable[row + 2, column + 4];
                        greenSig = Convert.ToDouble( rawDataTable[row, column + 1] );
                        control = Convert.ToDouble( rawDataTable[row - 1, column + 1] );
                        minimumSignal = MINIMUN_SIGNAL_nuc;
                        minimumRatio = MINIMUN_RATIO_nuc;
                    }
                    break;

                case "PC":
                    {
                        NVC = "";
                        greenSig = Convert.ToDouble( rawDataTable[row, column + 1] );
                        control = Convert.ToDouble( rawDataTable[row - 1, column + 1] );
                        minimumSignal = MINIMUN_SIGNAL_PC;
                        minimumRatio = MINIMUN_RATIO_PC;
                    }
                    break;

                case "MecA":
                    {
                        NVC = rawDataTable[row + 2, column + 7];
                        greenSig = Convert.ToDouble( rawDataTable[row, column + 1] );
                        control = Convert.ToDouble( rawDataTable[row - 1, column + 1] );
                        minimumSignal = MINIMUN_SIGNAL_MecA;
                        minimumRatio = MINIMUN_RATIO_MecA;
                    }
                    break;

                case "EFS":
                    {
                        NVC = rawDataTable[row + 1, column + 7];
                        greenSig = Convert.ToDouble( rawDataTable[row, column + 1] );
                        control = Convert.ToDouble( rawDataTable[row - 2, column + 1] );
                        minimumSignal = MINIMUN_SIGNAL_EFS;
                        minimumRatio = MINIMUN_RATIO_EFS;
                    }
                    break;

                case "PhoE":
                    {
                        NVC = rawDataTable[row, column + 7];
                        greenSig = Convert.ToDouble( rawDataTable[row, column + 1] );
                        control = Convert.ToDouble( rawDataTable[row - 3, column + 1] );
                        minimumSignal = MINIMUN_SIGNAL_PhoE;
                        minimumRatio = MINIMUN_RATIO_PhoE;
                    }
                    break;

                case "SPA":
                    {
                        NVC = rawDataTable[row + 2, column - 2];
                        greenSig = Convert.ToDouble( rawDataTable[row, column + 1] );
                        control = Convert.ToDouble( rawDataTable[row - 1, column + 1] );
                        minimumSignal = MINIMUN_SIGNAL_SPA;
                        minimumRatio = MINIMUN_RATIO_SPA;
                    }
                    break;

                case "VAN A B":
                    {
                        NVC = rawDataTable[row + 1, column - 2];
                        greenSig = Convert.ToDouble( rawDataTable[row, column + 1] );
                        control = Convert.ToDouble( rawDataTable[row - 2, column + 1] );
                        minimumSignal = MINIMUN_SIGNAL_VAN_A_B;
                        minimumRatio = MINIMUN_RATIO_VAN_A_B;
                    }
                    break;

            }


            if ( NVC == "INVALID" )
            {
                result = "Invalid";
            }

            else if ( greenSig > minimumSignal )
            {
                if ( ( greenSig / Math.Abs( control ) ) > minimumRatio )
                {
                    result = ( greenSig / Math.Abs( control ) ).ToString();
                }
                else
                {
                    result = "None";
                }
            }

            else
            {
                result = "None";
            }


            if ( ( result != "Invalid" ) && ( result != "None" ) )
            {
                if ( Convert.ToDouble( result ) < 10 )
                {
                    result = ( Math.Round( Convert.ToDouble( result ), 2 ) ).ToString();
                }
                else if ( Convert.ToDouble( result ) < 100 )
                {
                    result = ( Math.Round( Convert.ToDouble( result ), 1 ) ).ToString();
                }
                else
                {
                    result = ( Math.Round( Convert.ToDouble( result ), 0 ) ).ToString();
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

            // check MRSA

            if ( MRSA == 0 )
            {
                result = "Negative";
            }

            else if ( ( MRSA == 1 ) || ( MRSA == 4 ) || ( MRSA == 5 ) )
            {
                result = "MSSA";
            }

            else if ( ( MRSA == 2 ) || ( MRSA == 3 ) || ( MRSA == 6 ) || ( MRSA == 7 ) )
            {
                result = "Scc Positive MSSA";
            }

            else if ( MRSA == 8 )
            {
                result = "CoN-MR";
            }

            else if ( ( MRSA == 9 ) || ( MRSA == 12 ) || ( MRSA == 13 ) )
            {
                result = "CoN-MR+MSSA";
            }

            else if ( MRSA == 10 )
            {
                result = "Possible MRSA";
            }

            else if ( ( MRSA == 11 ) || ( MRSA == 14 ) || ( MRSA == 15 ) )
            {
                result = "MRSA";
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

            // check VRE

            if ( VRE == 0 )
            {
                result = "Negative";
            }

            else if ( VRE == 1 )
            {
                result = "None efs/efm Van+";
            }

            else if ( VRE == 2 )
            {
                result = "efm";
            }

            else if ( VRE == 3 )
            {
                result = "VRE EFM";
            }

            else if ( VRE == 4 )
            {
                result = "efs";
            }

            else if ( VRE == 5 )
            {
                result = "VRE EFS";
            }

            else if ( VRE == 6 )
            {
                result = "efs/efm";
            }

            else if ( VRE == 7 )
            {
                result = "VRE EFM/EFS";
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

            // check VRE

            if ( KPCnum == 0 )
            {
                result = "Negative";
            }

            else if ( KPCnum == 1 )
            {
                result = "KPC (non KP)";
            }

            else if ( KPCnum == 2 )
            {
                result = "KP";
            }

            else if ( KPCnum == 3 )
            {
                result = "KP-KPC";
            }

            return result;
        }

         







        private string F508del(string refStr, double Green, double Red, double GreenControl, double RedControl)
        {
            const double MinSignal = 1500;
            const double ScaleFactor = 1;
            const double LHET = 0.33;
            const double UHET = 3;
            string ReturnedValue;
            double GreenAboveControl, RedAboveControl, ScaledRatio;
            if (refStr.Substring(refStr.Length - 1, 1) == "0")
                ReturnedValue = "Ref. Fail";
            else
            {
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
                if (Green < -200 || Red < -200 || GreenControl < -200 || RedControl < -200)
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
            }
            return ReturnedValue;
        }
        private string _1717(string refStr, double Green, double Red, double GreenControl, double RedControl)
        {
            const double MinSignal = 1500;
            const double ScaleFactor = 1;
            const double LHET = 0.33;
            const double UHET = 3;
            string ReturnedValue;
            double GreenAboveControl, RedAboveControl, ScaledRatio;
            if (refStr.Substring(refStr.Length - 1, 1) == "0")
                ReturnedValue = "Ref. Fail";
            else
            {
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
                if (Green < -200 || Red < -200 || GreenControl < -200 || RedControl < -200)
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
            }
            return ReturnedValue;
        }
        private string N1303K(string refStr, double Green, double Red, double GreenControl, double RedControl)
        {
            const double MinSignal = 1500;
            const double ScaleFactor = 1.25;
            const double LHET = 0.33;
            const double UHET = 3;
            string ReturnedValue;
            double GreenAboveControl, RedAboveControl, ScaledRatio;
            if (refStr.Substring(refStr.Length - 1, 1) == "0")
                ReturnedValue = "Ref. Fail";
            else
            {
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
                if (Green < -200 || Red < -200 || GreenControl < -200 || RedControl < -200)
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
            }
            return ReturnedValue;
        }
        private string _3849(string refStr, double Green, double Red, double GreenControl, double RedControl)
        {
            const double MinSignal = 1500;
            const double ScaleFactor = 1;
            const double LHET = 0.33;
            const double UHET = 3;
            string ReturnedValue;
            double GreenAboveControl, RedAboveControl, ScaledRatio;
            if (refStr.Substring(refStr.Length - 1, 1) == "0")
                ReturnedValue = "Ref. Fail";
            else
            {
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
                if (Green < -200 || Red < -200 || GreenControl < -200 || RedControl < -200)
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
            }
            return ReturnedValue;
        }
        private string _3120(string refStr, double Green, double Red, double GreenControl, double RedControl)
        {
            const double MinSignal = 1500;
            const double ScaleFactor = 1;
            const double LHET = 0.33;
            const double UHET = 3;
            string ReturnedValue;
            double GreenAboveControl, RedAboveControl, ScaledRatio;
            if (refStr.Substring(refStr.Length - 1, 1) == "0")
                ReturnedValue = "Ref. Fail";
            else
            {
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
                if (Green < -200 || Red < -200 || GreenControl < -200 || RedControl < -200)
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
            }
            return ReturnedValue;
        }
        private string G85E(string refStr, double Green, double Red, double GreenControl, double RedControl)
        {
            const double MinSignal = 1500;
            const double ScaleFactor = 1;
            const double LHET = 0.33;
            const double UHET = 4;
            string ReturnedValue;
            double GreenAboveControl, RedAboveControl, ScaledRatio;
            if (refStr.Substring(refStr.Length - 2, 1) == "0")
                ReturnedValue = "Ref. Fail";
            else
            {
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
                if (Green < -200 || Red < -200 || GreenControl < -200 || RedControl < -200)
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
            }
            return ReturnedValue;
        }
        private string G542X(string refStr, double Green, double Red, double GreenControl, double RedControl)
        {
            const double MinSignal = 1500;
            const double ScaleFactor = 1;
            const double LHET = 0.33;
            const double UHET = 5;
            string ReturnedValue;
            double GreenAboveControl, RedAboveControl, ScaledRatio;
            if (refStr.Substring(refStr.Length - 2, 1) == "0")
                ReturnedValue = "Ref. Fail";
            else
            {
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
                if (Green < -200 || Red < -200 || GreenControl < -200 || RedControl < -200)
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
            }
            return ReturnedValue;
        }
        private string I1234V(string refStr, double Green, double Red, double GreenControl, double RedControl)
        {
            const double MinSignal = 1500;
            const double ScaleFactor = 0.77;
            const double LHET = 0.33;
            const double UHET = 3;
            string ReturnedValue;
            double GreenAboveControl, RedAboveControl, ScaledRatio;
            if (refStr.Substring(refStr.Length - 2, 1) == "0")
                ReturnedValue = "Ref. Fail";
            else
            {
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
                if (Green < -200 || Red < -200 || GreenControl < -200 || RedControl < -200)
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
            }
            return ReturnedValue;
        }
        private string W1282X(string refStr, double Green, double Red, double GreenControl, double RedControl)
        {
            const double MinSignal = 1500;
            const double ScaleFactor = 0.71;
            const double LHET = 0.33;
            const double UHET = 3;
            string ReturnedValue;
            double GreenAboveControl, RedAboveControl, ScaledRatio;
            if (refStr.Substring(refStr.Length - 2, 1) == "0")
                ReturnedValue = "Ref. Fail";
            else
            {
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
                if (Green < -200 || Red < -200 || GreenControl < -200 || RedControl < -200)
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
            }
            return ReturnedValue;
        }
        private string _2183AA(string refStr, double Green, double Red, double GreenControl, double RedControl)
        {
            const double MinSignal = 1500;
            const double ScaleFactor = 0.71;
            const double LHET = 0.33;
            const double UHET = 3;
            string ReturnedValue;
            double GreenAboveControl, RedAboveControl, ScaledRatio;
            if (refStr.Substring(refStr.Length - 2, 1) == "0")
                ReturnedValue = "Ref. Fail";
            else
            {
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
                if (Green < -200 || Red < -200 || GreenControl < -200 || RedControl < -200)
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
            }
            return ReturnedValue;
        }
        private string S549R(string refStr, double Green, double Red, double GreenControl, double RedControl)
        {
            const double MinSignal = 1500;
            const double ScaleFactor = 1.67;
            const double LHET = 0.33;
            const double UHET = 3;
            string ReturnedValue;
            double GreenAboveControl, RedAboveControl, ScaledRatio;
            if (refStr.Substring(refStr.Length - 3, 1) == "0")
                ReturnedValue = "Ref. Fail";
            else
            {
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
                if (Green < -200 || Red < -200 || GreenControl < -200 || RedControl < -200)
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
            }
            return ReturnedValue;
        }
        private string Q359K(string refStr, double Green, double Red, double GreenControl, double RedControl)
        {
            const double MinSignal = 1500;
            const double ScaleFactor = 1.67;
            const double LHET = 0.33;
            const double UHET = 3;
            string ReturnedValue;
            double GreenAboveControl, RedAboveControl, ScaledRatio;
            if (refStr.Substring(refStr.Length - 3, 1) == "0")
                ReturnedValue = "Ref. Fail";
            else
            {
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
                if (Green < -200 || Red < -200 || GreenControl < -200 || RedControl < -200)
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
            }
            return ReturnedValue;
        }
        private string W1089X(string refStr, double Green, double Red, double GreenControl, double RedControl)
        {
            const double MinSignal = 1500;
            const double ScaleFactor = 0.67;
            const double LHET = 0.33;
            const double UHET = 3;
            string ReturnedValue;
            double GreenAboveControl, RedAboveControl, ScaledRatio;
            if (refStr.Substring(refStr.Length - 3, 1) == "0")
                ReturnedValue = "Ref. Fail";
            else
            {
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
                if (Green < -200 || Red < -200 || GreenControl < -200 || RedControl < -200)
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
            }
            return ReturnedValue;
        }
        private string CFTRdel23(string refStr, double Green, double Red, double GreenControl, double RedControl)
        {
            const double MinSignal = 1500;
            const double ScaleFactor = 0.67;
            const double LHET = 0.33;
            const double UHET = 3;
            string ReturnedValue;
            double GreenAboveControl, RedAboveControl, ScaledRatio;
            if (refStr.Substring(refStr.Length - 3, 1) == "0")
                ReturnedValue = "Ref. Fail";
            else
            {
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
                if (Green < -200 || Red < -200 || GreenControl < -200 || RedControl < -200)
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
            }
            return ReturnedValue;
        }
        private string _3121(string refStr, double Green, double Red, double GreenControl, double RedControl)
        {
            const double MinSignal = 1500;
            const double ScaleFactor = 1.11;
            const double LHET = 0.33;
            const double UHET = 3;
            string ReturnedValue;
            double GreenAboveControl, RedAboveControl, ScaledRatio;
            if (refStr.Substring(refStr.Length - 3, 1) == "0")
                ReturnedValue = "Ref. Fail";
            else
            {
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
                if (Green < -200 || Red < -200 || GreenControl < -200 || RedControl < -200)
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
            }
            return ReturnedValue;
        }
        private string _405(string refStr, double Green, double Red, double GreenControl, double RedControl)
        {
            const double MinSignal = 1500;
            const double ScaleFactor = 0.83;
            const double LHET = 0.33;
            const double UHET = 3;
            string ReturnedValue;
            double GreenAboveControl, RedAboveControl, ScaledRatio;
            if (refStr.Substring(refStr.Length - 1, 1) == "0")
                ReturnedValue = "Ref. Fail";
            else
            {
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
                if (Green < -200 || Red < -200 || GreenControl < -200 || RedControl < -200)
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
            }
            return ReturnedValue;
        }
        private string D1152H(string refStr, double Green, double Red, double GreenControl, double RedControl)
        {
            const double MinSignal = 1500;
            const double ScaleFactor = 1.11;
            const double LHET = 0.33;
            const double UHET = 3;
            string ReturnedValue;
            double GreenAboveControl, RedAboveControl, ScaledRatio;
            if (refStr.Substring(refStr.Length - 4, 1) == "0")
                ReturnedValue = "Ref. Fail";
            else
            {
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
                if (Green < -200 || Red < -200 || GreenControl < -200 || RedControl < -200)
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
            }
            return ReturnedValue;
        }
        private string _4010(string refStr, double Green, double Red, double GreenControl, double RedControl)
        {
            const double MinSignal = 1500;
            const double ScaleFactor = 1.11;
            const double LHET = 0.33;
            const double UHET = 3;
            string ReturnedValue;
            double GreenAboveControl, RedAboveControl, ScaledRatio;
            if (refStr.Substring(refStr.Length - 4, 1) == "0")
                ReturnedValue = "Ref. Fail";
            else
            {
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
                if (Green < -200 || Red < -200 || GreenControl < -200 || RedControl < -200)
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
            }
            return ReturnedValue;
        }
        private string Y1092X(string refStr, double Green, double Red, double GreenControl, double RedControl)
        {
            const double MinSignal = 1500;
            const double ScaleFactor = 1.11;
            const double LHET = 0.33;
            const double UHET = 3;
            string ReturnedValue;
            double GreenAboveControl, RedAboveControl, ScaledRatio;
            if (refStr.Substring(refStr.Length - 4, 1) == "0")
                ReturnedValue = "Ref. Fail";
            else
            {
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
                if (Green < -200 || Red < -200 || GreenControl < -200 || RedControl < -200)
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
            }
            return ReturnedValue;
        }
        private string Genotype(string ScaledRatio)
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
                    if (SR > MinWT)
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
        
        #endregion

    }

}
