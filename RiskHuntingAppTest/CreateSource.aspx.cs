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
    public partial class CreateSource : System.Web.UI.Page
    {
		protected string sourcesPath = Path.Combine (SettingsTool.GetApplicationPath(), "SearchApp", "xmlFiles", "Sources");
        protected const string SOURCESPECIFICATION = "SourceSpecification";
        protected const string PROBLEM = "Problem";
        protected const string SOLUTION = "Solution";
        protected const string PROCESSFOLDER = "_toProcess";

        protected string sourceId;
        protected int maxId;

		protected const string SOURCE_TYPE = Constants.CASEREF;

        protected const string SOURCE_WATERMARK = "Enter a source or author";
        protected const string NAME_WATERMARK = "Enter a name";
        protected const string PROBLEMDESCR_WATERMARK = "Enter a problem description";
        protected const string TRIGGERINGEVENT_WATERMARK = "Enter the triggering event";
        protected const string MISC_WATERMARK = "Enter some keywords";
        protected const string SOLUTIONDESCR_WATERMARK = "Enter a solution description";
        protected const string MISC2_WATERMARK = "Enter any additional information";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PopulateDropDownLists();
				//Antique.DataSourceParser.DBSelect dbs = new Antique.DataSourceParser.DBSelect();
                //Response.Write(dbs.GetDomainID("Healthcare").ToString());

            }
            PopulateTextBoxesWatermarks();
        }

        #region Initializing
        private void PopulateDropDownLists()
        {
            DomainDropDown.Items.Add(new ListItem("<select domain>", "0"));
            DomainDropDown.Items.Add(new ListItem("Healthcare", "1"));
            DomainDropDown.Items.Add(new ListItem("Policing", "2"));
            DomainDropDown.Items.Add(new ListItem("Teaching", "3"));
            DomainDropDown.Items.Add(new ListItem("Education", "4"));
            DomainDropDown.Items.Add(new ListItem("Travel", "5"));
            DomainDropDown.Items.Add(new ListItem("Social Services", "6"));
            DomainDropDown.Items.Add(new ListItem("Quality of Life", "7"));
            DomainDropDown.Items.Add(new ListItem("Childcare", "8"));
            DomainDropDown.Items.Add(new ListItem("Emergency", "9"));
            DomainDropDown.SelectedValue = "0";

            ObservedBehaviourDropDown.Items.Add(new ListItem("<select observed behaviour>", "1"));
            ObservedBehaviourDropDown.Items.Add(new ListItem("disruptive", "2"));
            ObservedBehaviourDropDown.Items.Add(new ListItem("unwell", "3"));
            ObservedBehaviourDropDown.Items.Add(new ListItem("violent", "4"));
            ObservedBehaviourDropDown.Items.Add(new ListItem("lethargic", "5"));
            ObservedBehaviourDropDown.Items.Add(new ListItem("anti-social", "6"));
            ObservedBehaviourDropDown.Items.Add(new ListItem("new arrival", "7"));
            ObservedBehaviourDropDown.Items.Add(new ListItem("leaving to hospital", "8"));
            ObservedBehaviourDropDown.Items.Add(new ListItem("returned from hospital", "9"));
            ObservedBehaviourDropDown.SelectedValue = "1";

            //TreatmentTypeDropDown.Items.Add(new ListItem("<select treatment type>", "1"));
            //TreatmentTypeDropDown.SelectedValue = "1";

            //AilmentTypeDropDown.Items.Add(new ListItem("<select ailment or disease type>", "1"));
            //AilmentTypeDropDown.SelectedValue = "1";
        }

        private void ResetDropDownLists()
        {
            DomainDropDown.SelectedValue = "0";
            ObservedBehaviourDropDown.SelectedValue = "1";
            //TreatmentTypeDropDown.SelectedValue = "1";
            //AilmentTypeDropDown.SelectedValue = "1";
        }

        private void PopulateTextBoxesWatermarks()
        {
            //SourceTextBox.Text = String.Empty;
            SourceTextBox.WatermarkText = SOURCE_WATERMARK;
            //NameTextBox.Text = String.Empty;
            NameTextBox.WatermarkText = NAME_WATERMARK;
            //ProblemDescrTextBox.Text = String.Empty;
            ProblemDescrTextBox.WatermarkText = PROBLEMDESCR_WATERMARK;
            //TriggeringEventTextBox.Text = String.Empty;
            //TriggeringEventTextBox.WatermarkText = TRIGGERINGEVENT_WATERMARK;
            //MiscellaneousTextBox.Text = String.Empty;
            MiscellaneousTextBox.WatermarkText = MISC_WATERMARK;
            //SolutionDescrTextBox.Text = String.Empty;
            SolutionDescrTextBox.WatermarkText = SOLUTIONDESCR_WATERMARK;
            //Miscellaneous2TextBox.Text = String.Empty;
            //Miscellaneous2TextBox.WatermarkText = MISC2_WATERMARK;
        }

        private void ResetPopulateTextBoxesWatermarks()
        {
            SourceTextBox.Text = String.Empty;
            SourceTextBox.WatermarkText = SOURCE_WATERMARK;
            NameTextBox.Text = String.Empty;
            NameTextBox.WatermarkText = NAME_WATERMARK;
            ProblemDescrTextBox.Text = String.Empty;
            ProblemDescrTextBox.WatermarkText = PROBLEMDESCR_WATERMARK;
            //TriggeringEventTextBox.Text = String.Empty;
            //TriggeringEventTextBox.WatermarkText = TRIGGERINGEVENT_WATERMARK;
            MiscellaneousTextBox.Text = String.Empty;
            MiscellaneousTextBox.WatermarkText = MISC_WATERMARK;
            SolutionDescrTextBox.Text = String.Empty;
            SolutionDescrTextBox.WatermarkText = SOLUTIONDESCR_WATERMARK;
            //Miscellaneous2TextBox.Text = String.Empty;
            //Miscellaneous2TextBox.WatermarkText = MISC2_WATERMARK;
        }

        private void ResetErrorMessages()
        {
            DomainErrorMsg.InnerHtml = String.Empty;
            NameErrorMsg.InnerHtml = String.Empty;
            ProblemDescrErrorMsg.InnerHtml = String.Empty;
            SolutionDescrErrorMsg.InnerHtml = String.Empty;
            SubmitReport.InnerHtml = String.Empty;
            MiscellaneousErrorMsg.InnerHtml = String.Empty;

        }
        #endregion

        protected void checkDomain(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                CheckDomainForHealthcare();
            }
        }

        protected void submitClicked(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (CheckDomainDropDown() == true && CheckNameTextBox() == true && CheckProblemDescrTextBox() == true && CheckSolutionDescrTextBox() == true && CheckMiscTextBox() == true)
                {
                    this.maxId = GetNewSourceId();
                    GenerateXml("SourceSpecification");
                    GenerateXml("Problem");
                    GenerateXml("Solution");
                    if (!DomainDropDown.SelectedItem.Text.Equals("Healthcare"))
                    {
                        antiqueService.AntiqueService ant = new RiskHuntingAppTest.antiqueService.AntiqueService();
                        ant.GeneratePredicates(NameTextBox.Text, maxId.ToString(), SourceTextBox.Text, Convert.ToInt32(DomainDropDown.SelectedItem.Value), ProblemDescrTextBox.Text, SolutionDescrTextBox.Text, MiscellaneousTextBox.Text);
                    }
                    ResetErrorMessages();
                    SubmitReport.InnerHtml = "<span class=\"greentitle\">A new case study was created successfully.</span>";
                }
            }
        }

        #region Xml generation
        private void GenerateXml(string componentType)
        {
            string Ref;
            string xmlUri, xmlUri2;
            if (componentType.Equals("SourceSpecification"))
            {
                XmlProc.SourceSpecificationSerialized.SourceSpecification ss = CreateSourceSpecificationXml();
                Ref = SOURCE_TYPE + this.sourceId + "_" + componentType + ".xml";
                xmlUri = sourcesPath + DomainDropDown.SelectedItem.Text + "/" + SOURCESPECIFICATION + "/" + Ref;
                xmlUri2 = sourcesPath + PROCESSFOLDER + "/" + SOURCESPECIFICATION + "/" + Ref;
                XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Save(ss, xmlUri);
                XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Save(ss, xmlUri2);
            }
            else if (componentType.Equals("Problem"))
            {
                XmlProc.ProblemSerialized.LanguageSpecificSpecification problem = CreateProblemXml();
                Ref = SOURCE_TYPE + this.sourceId + "_" + componentType + ".xml";
                xmlUri = sourcesPath + DomainDropDown.SelectedItem.Text + "/" + PROBLEM + "/" + Ref;
                xmlUri2 = sourcesPath + PROCESSFOLDER + "/" + PROBLEM + "/" + Ref;
                XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Save(problem, xmlUri);
                XmlProc.ObjectXMLSerializer<XmlProc.ProblemSerialized.LanguageSpecificSpecification>.Save(problem, xmlUri2);
            }
            else if (componentType.Equals("Solution"))
            {
                XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = CreateSolutionXml();
                Ref = SOURCE_TYPE + this.sourceId + "_" + componentType + ".xml";
                xmlUri = sourcesPath + DomainDropDown.SelectedItem.Text + "/" + SOLUTION + "/" + Ref;
                xmlUri2 = sourcesPath + PROCESSFOLDER + "/" + SOLUTION + "/" + Ref;
                XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Save(solution, xmlUri);
                XmlProc.ObjectXMLSerializer<XmlProc.SolutionSerialized.LanguageSpecificSpecification>.Save(solution, xmlUri2);
            }

        }

        private XmlProc.SourceSpecificationSerialized.SourceSpecification CreateSourceSpecificationXml()
        {
            XmlProc.SourceSpecificationSerialized.SourceSpecification ss = new XmlProc.SourceSpecificationSerialized.SourceSpecification();

            DateTime now = DateTime.Now;
            ss.SourceId = maxId;
            this.sourceId = maxId.ToString();
            ss.SourceName = NameTextBox.Text;
            ss.SourceType = SOURCE_TYPE;
            ss.Domain = DomainDropDown.SelectedItem.Text;
            ss.Filename = ss.SourceType + maxId + ".docx";
            ss.LaunchDate = now.ToString();
            ss.SourceSpecificationLastEdited = now.ToString();

            XmlProc.SourceSpecificationSerialized.SourceSpecificationFacetFacetSpecification fspecProblem = new XmlProc.SourceSpecificationSerialized.SourceSpecificationFacetFacetSpecification();
            fspecProblem.FacetSpecificationLanguage = "Text";
            fspecProblem.FacetSpecificationLink = ss.SourceType + maxId + "_Problem.xml";

            XmlProc.SourceSpecificationSerialized.SourceSpecificationFacetFacetSpecification fspecSolution = new XmlProc.SourceSpecificationSerialized.SourceSpecificationFacetFacetSpecification();
            fspecSolution.FacetSpecificationLanguage = "Text";
            fspecSolution.FacetSpecificationLink = ss.SourceType + maxId + "_Solution.xml";

            XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet problem = new XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet();
            problem.FacetType = "Problem";
            problem.Author = SourceText();
            problem.FacetSpecification = fspecProblem;

            XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet solution = new XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet();
            solution.FacetType = "Solution";
            solution.Author = SourceText();
            solution.FacetSpecification = fspecSolution;

            ss.Facet = new System.Collections.Generic.List<XmlProc.SourceSpecificationSerialized.SourceSpecificationFacet>();
            ss.Facet.Add(problem);
            ss.Facet.Add(solution);

            return ss;
        }

        private XmlProc.ProblemSerialized.LanguageSpecificSpecification CreateProblemXml()
        {
            XmlProc.ProblemSerialized.LanguageSpecificSpecification problem = new XmlProc.ProblemSerialized.LanguageSpecificSpecification();

            DateTime now = DateTime.Now;
            problem.FacetType = "Problem";
            problem.FacetSpecificationLanguage = "Text";
            problem.Author = SourceTextBox.Text;
            problem.LaunchDate = now.ToString();
            problem.SourceSpecificationLastEdited = now.ToString();

            XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationData fsd = new XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationData();
            fsd.Content = ProblemDescrTextBox.Text;
            //fsd.Observations = new System.Collections.Generic.List<XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationDataObservation>();
            //XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationDataObservation obs = new XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationDataObservation();
            //obs.id = 1;
            //obs.launchDate = now.ToString();
            //obs.Value = "n/A";
            //fsd.Observations.Add(obs);
            fsd.Observations = String.Empty;
            fsd.ObservedBehaviour = ObservedBehaviourText();
            fsd.TreatmentType = TreatmentTypeText();
            fsd.DateOfIncident = String.Empty;
            fsd.AilmentType = AilmentTypeText();
            fsd.TriggeringEvent = TriggeringEventText();
            fsd.Miscellaneous = MiscellaneousTextBox.Text;
            fsd.MatchingDetails = String.Empty;

            problem.FacetSpecificationData = fsd;


            return problem;
        }

        private XmlProc.SolutionSerialized.LanguageSpecificSpecification CreateSolutionXml()
        {
            XmlProc.SolutionSerialized.LanguageSpecificSpecification solution = new XmlProc.SolutionSerialized.LanguageSpecificSpecification();

            DateTime now = DateTime.Now;
            solution.FacetType = "Solution";
            solution.FacetSpecificationLanguage = "Text";
            solution.Author = SourceTextBox.Text;
            solution.LaunchDate = now.ToString();
            solution.SourceSpecificationLastEdited = now.ToString();

            XmlProc.SolutionSerialized.LanguageSpecificSpecificationFacetSpecificationData fsd = new XmlProc.SolutionSerialized.LanguageSpecificSpecificationFacetSpecificationData();
            fsd.Content = SolutionDescrTextBox.Text;
            fsd.Miscellaneous = Miscellaneous2Text();

            solution.FacetSpecificationData = fsd;


            return solution;
        }

        private int GetNewSourceId()
        {
            int maxId = 0;
            string path = FindLatestSourcePath();
            if (!path.Equals(String.Empty))
            {
                if (File.Exists(path))
                {
                    XmlProc.SourceSpecificationSerialized.SourceSpecification ss = XmlProc.ObjectXMLSerializer<XmlProc.SourceSpecificationSerialized.SourceSpecification>.Load(path);
                    maxId = Convert.ToInt32(ss.SourceId);
                    maxId = maxId + 1;
                }
            }
            else
                maxId = 1;
            return maxId;
        }

        private string FindLatestSourcePath()
        {
            string path = String.Empty;
            // get your files (names)
			string[] fileNames = Directory.GetFiles(Path.Combine (sourcesPath, DomainDropDown.SelectedItem.Text, SOURCESPECIFICATION), "*.*");
            if (fileNames.Length > 0)
            {
                // Now read the creation time for each file
                DateTime[] creationTimes = new DateTime[fileNames.Length];
                for (int i = 0; i < fileNames.Length; i++)
                    creationTimes[i] = new FileInfo(fileNames[i]).CreationTime;

                // sort it
                Array.Sort(creationTimes, fileNames);
                path = fileNames[fileNames.Length - 1];
            }

            return path;
        }
        #endregion

        #region check elements content

        private void CheckDomainForHealthcare()
        {
            if (DomainDropDown.SelectedValue.Equals("1"))
            {
                ObservedBehaviourDropDown.Visible = true;
            }
            else
            {
                ObservedBehaviourDropDown.Visible = false;
            }
        }

        private bool CheckDomainDropDown()
        {
            if (DomainDropDown.SelectedValue.Equals("0"))
            {
                DomainErrorMsg.InnerHtml = "<span class=\"redtitle\">please select a domain</span>";
                return false;
            }
            else
            {
                DomainErrorMsg.InnerHtml = String.Empty;
                return true;
            }
        }

        private bool CheckNameTextBox()
        {
            if (NameTextBox.Text.Equals(String.Empty) || NameTextBox.Text.Equals(NAME_WATERMARK))
            {
                NameErrorMsg.InnerHtml = "<span class=\"redtitle\">please enter a name</span>";
                return false;
            }
            else
            {
                NameErrorMsg.InnerHtml = String.Empty;
                return true;
            }
        }

        private bool CheckProblemDescrTextBox()
        {
            if (ProblemDescrTextBox.Text.Equals(String.Empty))
            {
                ProblemDescrErrorMsg.InnerHtml = "<span class=\"redtitle\">please enter some text</span>";
                return false;
            }
            else
            {
                ProblemDescrErrorMsg.InnerHtml = String.Empty;
                return true;
            }
        }

        private bool CheckSolutionDescrTextBox()
        {
            if (SolutionDescrTextBox.Text.Equals(String.Empty))
            {
                SolutionDescrErrorMsg.InnerHtml = "<span class=\"redtitle\">please enter some text</span>";
                return false;
            }
            else
            {
                SolutionDescrErrorMsg.InnerHtml = String.Empty;
                return true;
            }
        }

        private bool CheckMiscTextBox()
        {
            if (MiscellaneousTextBox.Text.Equals(String.Empty))
            {
                MiscellaneousErrorMsg.InnerHtml = "<span class=\"redtitle\">please enter some keywords</span>";
                return false;
            }
            else
            {
                MiscellaneousErrorMsg.InnerHtml = String.Empty;
                return true;
            }
        }


        #endregion

        # region contents

        private string ObservedBehaviourText()
        {
            if (ObservedBehaviourDropDown.SelectedValue.Equals("1"))
                return String.Empty;
            else
                return ObservedBehaviourDropDown.SelectedItem.Text;
        }

        private string TreatmentTypeText()
        {
            //if (TreatmentTypeDropDown.SelectedValue.Equals("1"))
            //    return String.Empty;
            //else
            //    return TreatmentTypeDropDown.SelectedItem.Text;

            return String.Empty;
        }

        private string AilmentTypeText()
        {
            //if (AilmentTypeDropDown.SelectedValue.Equals("1"))
            //    return String.Empty;
            //else
            //    return AilmentTypeDropDown.SelectedItem.Text;

            return String.Empty;
        }

        private string SourceText()
        {
            if (SourceTextBox.Text.Equals(TRIGGERINGEVENT_WATERMARK))
                return String.Empty;
            else
                return SourceTextBox.Text;
        }

        private string TriggeringEventText()
        {
            //if (TriggeringEventTextBox.Text.Equals(TRIGGERINGEVENT_WATERMARK))
            //    return String.Empty;
            //else
            //    return TriggeringEventTextBox.Text;

            return String.Empty;
        }

        private string MiscellaneousText()
        {
            if (MiscellaneousTextBox.Text.Equals(MISC_WATERMARK))
                return String.Empty;
            else
                return MiscellaneousTextBox.Text;
        }

        private string Miscellaneous2Text()
        {
            //if (Miscellaneous2TextBox.Text.Equals(MISC2_WATERMARK))
            //    return String.Empty;
            //else
            //    return Miscellaneous2TextBox.Text;

            return String.Empty;
        }

        #endregion

        protected void reset_Click(object sender, EventArgs e)
        {
            ResetDropDownLists();
            ResetPopulateTextBoxesWatermarks();
            ResetErrorMessages();
        }
    }
}
