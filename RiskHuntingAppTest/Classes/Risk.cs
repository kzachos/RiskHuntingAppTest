using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace RiskHuntingAppTest
{
	public enum RiskQueryState {
		[Description("Problem Described")]
		ProblemDescribed,
		[Description("Ideas Generated")]
		IdeasGenerated,
//		[Description("Similar risks found")]
//		SimilarRisksFound,
//		[Description("Actions Formulated")]
//		ActionsFormulated,
//		[Description("Risk Resolved")]
//		RiskResolved
	}

	public enum RiskResolutionType {
		[Description("Resolution Idea")]
		ResolutionIdea,
		[Description("Action")]
		Action
	}

	public struct Action
	{
		public string Id { get; set; }
		public string Content { get; set; }
		public string Owner { get; set; }
		public DateTime ImplementBy {get; set; }
	}

	public struct Risk
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime LaunchDate { get; set; }
		public DateTime LastEdited { get; set; }
		public string Author { get; set; }

		public string Content { get; set; }
		public string InjuryNature { get; set; }
		public string LocationDetail { get; set; }
		public string BodyPart { get; set; }
		public string InjuryCause { get; set; }
		public string IncidentPriority { get; set; }
		public string IncidentStatus {get; set; }
		public string RootCause { get; set; }
		public string RoutineWork { get; set; }
		public string ShiftType { get; set; }
		public string Title {get; set; }
		public string ClosedByOperator { get; set; }
		public string ContractorName { get; set; }
		public DateTime DateClosed {get; set; }
		public DateTime DateIncidentOccurred { get; set; }
		public string Miscellaneous {get; set; }

		public ArrayList Recommendations { get; set; }
		public List<Action> Actions { get; set; }
		public string Recommendation {get; set; }
		public string CorrectiveActions {get; set; }
		public string Countermeasures { get; set; }

		public RiskQueryState State { get; set; }
		public bool SimilarCasesFound { get; set; }
		public string ImageUri { get; set; }


		public Risk (XmlProc.SourceSpecificationSerialized.SourceSpecification ss, 
			XmlProc.ProblemSerialized.LanguageSpecificSpecification p, 
			XmlProc.SolutionSerialized.LanguageSpecificSpecification s) : this ()
		{
//			Console.WriteLine ("ss.SourceId (Risk): " + ss.SourceId.ToString ());
			Id = ss.SourceId;
			Name = ss.SourceName;
			LaunchDate = Util.ConvertDateTime (ss.LaunchDate);
			LastEdited = Util.ConvertDateTime (ss.SourceSpecificationLastEdited);
			Author = ss.Facet [0].Author;

			switch (ss.SourceType)
			{
			case "ProblemDescribed":
				State = RiskQueryState.ProblemDescribed;
				break;
			case "IdeasGenerated":
				State = RiskQueryState.IdeasGenerated;
				break;
//			case "ActionsFormulated":
//				State = RiskQueryState.ActionsFormulated;
//				break;
//			case "RiskResolved":
//				State = RiskQueryState.RiskResolved;
//				break;
			}


			SimilarCasesFound = ss.Filename.Equals("Found")?true:false;

			XmlProc.ProblemSerialized.LanguageSpecificSpecificationFacetSpecificationData pf = p.FacetSpecificationData;
			Content = !pf.Content.Equals (String.Empty) ? pf.Content : String.Empty;
			LocationDetail = !pf.LocationDetail.Equals (String.Empty) ? pf.LocationDetail : String.Empty;
			BodyPart = !pf.BodyPart.Equals (String.Empty) ? pf.BodyPart : String.Empty;
			DateIncidentOccurred = Util.ConvertDateTime (p.SourceSpecificationLastEdited);
			InjuryNature = !pf.InjuryNature.Equals (String.Empty) ? pf.InjuryNature : String.Empty;
			ImageUri = !pf.Miscellaneous.Equals(String.Empty) ? pf.Miscellaneous : String.Empty;


			Recommendations = new ArrayList ();
			Actions = new List<Action> ();
			XmlProc.SolutionSerialized.LanguageSpecificSpecificationFacetSpecificationData sf = s.FacetSpecificationData;
			if (!sf.Content.Equals (String.Empty) 
//				|| !sf.Content.Trim().Equals (";;")
			) {
//				var content = Util.ExtractAttributeContentFromString2 (sf.Content, "Recommendation").Trim ();
//				char[] delim = new char[] {';'};
//				Resolutions.AddRange(content.Split(delim));
				Recommendations = Util.ResolutionsStringToArray (sf.Content, "Recommendation");
//				Console.WriteLine ("Recommendations.Count: " + Recommendations.Count.ToString ());

				var allActionsString = Util.ResolutionsStringToArray (sf.Content, "Corrective Actions");
//				Console.WriteLine ("allActionsString.Count: " + allActionsString.Count);
				if (allActionsString.Count > 0)
					foreach (var a in allActionsString)
						Actions.Add (Util.ActionStringToObject (a.ToString()));
			} else
				Content = String.Empty;

		}


	}
}

