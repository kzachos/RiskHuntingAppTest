using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace RiskHuntingAppTest
{
	public struct ProcessGuidance
	{
		public List<string> ProblemDescription { get; set; }
		public List<string> RiskResolutions { get; set; }
		public List<string> RiskDescription { get; set; }
		public List<string> RiskResolutionsApplied { get; set; }
		public List<string> CreativeGuidance { get; set; }
		public List<string> Summary { get; set; }
	}
}

