using System.Collections.Generic;

namespace RiskHuntingAppTest
{
	public static class Constants
	{
		public const string SESSION_ACTIVITY_LOG_ID = "__SessionActivityLog";

		public const string EDDIE_URI = "http://achernar.soi.city.ac.uk/ESD/WebServices/EDDiE_WS/EddieService.asmx";
		public const string ANTIQUE_URI = "http://achernar.soi.city.ac.uk/ESD/ClassLibraries/Antique/Antique.AntiqueService/AntiqueService.asmx";
		public const string HALLOFFAME_URI = "http://achernar.soi.city.ac.uk/HallOfFame/HallOfFameService/Service1.asmx";

		public const string CASEREF = "CaseStudy";
		public const string SOURCE_TYPE = CASEREF;
		public const string CASE_TYPE = "Risk";
		public const string AUTHOR = "Bassildon Trial";

		public const string PERSONA_TYPE = "Superhero";

		public const string SOURCESPECIFICATION = "SourceSpecification";
		public const string PROBLEM = "Problem";
		public const string SOLUTION = "Solution";
		public const string ADDITIONAL = "Additional";
		public const string PROCESSFOLDER = "_toProcess";
		public const string IMAGESFOLDER = "Images";

		public const int MaxPromptsAtATime = 8;
		public const float THRESHOLD = 50.0f;
		public const int MaxPastRisksAtATime = 5;

		public const string SESSION_EXPIRED_TEXT = "Your session expired";
		public const string SESSION_EXPIRED_LABEL = "sessionexpired";

		public const string PIN = "1234";

		public const string PDF_TEMPLATE = "OHSREP12_Template.pdf";
		public const string PDF_OUTPUT = "Risk.pdf";
	}
}

