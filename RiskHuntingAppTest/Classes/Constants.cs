using System.Collections.Generic;

namespace RiskHuntingAppTest
{
	public static class Constants
	{
		public const string SESSION_ACTIVITY_LOG_ID = "__SessionActivityLog";

		public const string CASEREF = "CaseStudy_";
		public const string SOURCE_TYPE = CASEREF;
		public const string CASE_TYPE = "Risk";

		public const string SOURCESPECIFICATION = "SourceSpecification";
		public const string PROBLEM = "Problem";
		public const string SOLUTION = "Solution";
		public const string ADDITIONAL = "Additional";
		public const string PROCESSFOLDER = "_toProcess";

		public const int MaxPromptsAtATime = 8;
		public const float THRESHOLD = 50.0f;
		public const int MaxPastRisksAtATime = 5;


	}
}

