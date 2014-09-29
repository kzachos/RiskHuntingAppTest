using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using World.Code.WebControls;

namespace RiskHuntingAppTest
{
    public partial class ProcessSource : System.Web.UI.Page
    {
        protected string sourcesPath = SettingsTool.GetApplicationPath() + "SearchApp/xmlFiles/Sources/";
        protected const string PROCESSFOLDER = "_toProcess"; 
        protected const string SOURCESPECIFICATION = "SourceSpecification";
        protected const string PROBLEM = "Problem";
        protected const string SOLUTION = "Solution";
		protected const string SOURCE_TYPE = Constants.CASEREF;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void submitClicked(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Process();
            }
        }

        #region Xml generation
        private void Process()
        {
            string[] fileNamesSpec = Directory.GetFiles(sourcesPath + PROCESSFOLDER + "/" + SOURCESPECIFICATION + "/", "*.*");
            string[] fileNamesProblem = Directory.GetFiles(sourcesPath + PROCESSFOLDER + "/" + PROBLEM + "/", "*.*");
            string[] fileNamesSolution = Directory.GetFiles(sourcesPath + PROCESSFOLDER + "/" + SOLUTION + "/", "*.*");
            if (fileNamesSpec.Length > 0)
            {
                for (int i = 0; i < fileNamesSpec.Length; i++)
                {
                    XmlProc.SourceSpecificationSerialized.SourceSpecification spec = XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Load(fileNamesSpec[i]);
                    if (!spec.Domain.Equals("Healthcare"))
                    {
                        XmlProc.ProblemSerialized.LanguageSpecificSpecification problem = XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Load(fileNamesProblem[i]);
                        XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Load(fileNamesSolution[i]);
                        ProcessInfoFromXml(spec, problem, solution);
                    }
                }
            } 
        }

        private void ProcessInfoFromXml(XmlProc.SourceSpecificationSerialized.SourceSpecification spec, XmlProc.ProblemSerialized.LanguageSpecificSpecification problem, XmlProc.SolutionSerialized.LanguageSpecificSpecification solution)
        {
			int domainID;
			switch (spec.Domain){
			case "Healthcare":
				domainID = 1;
				break;
			case "Policing":
				domainID = 2;
				break;
			case "Teaching":
				domainID = 3;
				break;
			case "Education":
				domainID = 4;
				break;
			case "Travel":
				domainID = 5;
				break;
			case "Social Services":
				domainID = 6;
				break;
            case "Quality of Life":
                domainID = 7;
                break;
            case "Childcare":
                domainID = 8;
                break;
            default:
				domainID = 1;
				break;
			}
            RiskHuntingAppTest.antique.AntiqueService antique = new RiskHuntingAppTest.antique.AntiqueService();
            antique.GeneratePredicates (spec.SourceName, spec.SourceId.ToString(), problem.Author, domainID, problem.FacetSpecificationData.Content, solution.FacetSpecificationData.Content, String.Empty);

        }

        #endregion
    }
}
