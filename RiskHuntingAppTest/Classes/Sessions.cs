using System;
using System.Web;
using System.Collections.Generic;

namespace RiskHuntingAppTest
{
	/// <summary>
	///     All access to Session variables must be through this class.
	/// </summary>
	public static class Sessions
	{
		#region Private Constants
		//---------------------------------------------------------------------
		public const string sortState = "SortBy";
		public const string riskState = "CURRENT_RISK";
		public const string problemDescState = "CURRENT_PROBLEM_DESC";
		public const string pastRiskDescState = "CURRENT_PAST_RISK_DESC";
		public const string creativityPromptsState = "CREATIVITY_PROMPTS";
		public const string creativityPromptsPastRiskState = "CREATIVITY_PROMPTS_PAST_RISK";
		public const string responseUriState = "CurrentResponseUri";
		public const string pastRiskState = "CurrentOriginalID";
		public const string personasState = "CURRENT_PERSONAS";
		public const string personaState = "CURRENT_PERSONA";

		//---------------------------------------------------------------------
		#endregion

		#region Public Properties
		//---------------------------------------------------------------------



		public static string SortState
		{
			get {
				var obj = HttpContext.Current.Session [sortState];
				if (obj == null)
					return String.Empty;
				return (string)obj;
			}

			set
			{
				HttpContext.Current.Session [sortState] = value;
			}
		}

		public static string RiskState
		{
			get 
			{
				var obj = HttpContext.Current.Session [riskState];
				if (obj == null)
					return String.Empty;
				return (string)obj;
			}

			set
			{
				HttpContext.Current.Session [riskState] = value;
			}
		}
			
		public static List<NLResponseToken> ProblemDescState
		{
			get 
			{
				var obj = HttpContext.Current.Session [problemDescState];
				if (obj == null)
					return null;
				return (List<NLResponseToken>)obj;
			}

			set
			{
				HttpContext.Current.Session [problemDescState] = value;
			}
		}

		public static List<NLResponseToken> PastRiskDescState
		{
			get 
			{
				var obj = HttpContext.Current.Session [pastRiskDescState];
				if (obj == null)
					return null;
				return (List<NLResponseToken>)obj;
			}

			set
			{
				HttpContext.Current.Session [pastRiskDescState] = value;
			}
		}

		public static IList<string> CreativityPromptsState
		{
			get 
			{
				var obj = HttpContext.Current.Session [creativityPromptsState];
				if (obj == null)
					return null;
				return (IList<string>)obj;
			}

			set
			{
				HttpContext.Current.Session [creativityPromptsState] = value;
			}
		}

		public static IList<string> CreativityPromptsPastRiskState
		{
			get 
			{
				var obj = HttpContext.Current.Session [creativityPromptsPastRiskState];
				if (obj == null)
					return null;
				return (IList<string>)obj;
			}

			set
			{
				HttpContext.Current.Session [creativityPromptsPastRiskState] = value;
			}
		}

		public static string ResponseUriState
		{
			get 
			{
				var obj = HttpContext.Current.Session [responseUriState];
				if (obj == null)
					return String.Empty;
				return (string)obj;
			}

			set
			{
				HttpContext.Current.Session [responseUriState] = value;
			}
		}

		public static string PastRiskState
		{
			get 
			{
				var obj = HttpContext.Current.Session [pastRiskState];
				if (obj == null)
					return String.Empty;
				return (string)obj;
			}

			set
			{
				HttpContext.Current.Session [pastRiskState] = value;
			}
		}
			
		public static List<Persona> PersonasState
		{
			get {
				var obj = HttpContext.Current.Session [personasState];
				if (obj == null)
					return null;
				return (List<Persona>)obj;
			}

			set
			{
				HttpContext.Current.Session [personasState] = value;
			}
		}

		public static string PersonaState
		{
			get {
				var obj = HttpContext.Current.Session [personaState];
				if (obj == null)
					return String.Empty;
				return (string)obj;
			}

			set
			{
				HttpContext.Current.Session [personaState] = value;
			}
		}


		/// <summary>
		///     The Username is the domain name and username of the current user.
		/// </summary>
		public static string Username
		{
			get { return HttpContext.Current.User.Identity.Name; }
		}


//		/// <summary>
//		///     UserAuthorisation contains the authorisation information for
//		///     the current user.
//		/// </summary>
//		public static UserAuthorisation UserAuthorisation
//		{
//			get 
//			{
//				UserAuthorisation userAuth 
//				= (UserAuthorisation)HttpContext.Current.Session [userAuthorisation];
//
//				// Check whether the UserAuthorisation has expired
//				if (
//					userAuth == null || 
//					(userAuth.Created.AddMinutes(
//						MyApplication.Settings.Caching.AuthorisationCache.CacheExpiryMinutes)) 
//					< DateTime.Now
//				)
//				{
//					userAuth = UserAuthorisation.GetUserAuthorisation(Username);
//					UserAuthorisation = userAuth;
//				}
//
//				return userAuth;
//			}
//
//			private set
//			{
//				HttpContext.Current.Session [userAuthorisation] = value;
//			}
//		}
//
//		/// <summary>
//		///     TeamManagementState is used to store the current state of the 
//		///     TeamManagement.aspx page.
//		/// </summary>
//		public static TeamManagementState TeamManagementState
//		{
//			get 
//			{
//				return (TeamManagementState)HttpContext.Current.Session [teamManagementState];
//			}
//
//			set
//			{
//				HttpContext.Current.Session [teamManagementState] = value;
//			}
//		}
//
//		/// <summary>
//		///     StartDate is the earliest date used to filter records.
//		/// </summary>
//		public static DateTime StartDate
//		{
//			get 
//			{
//				if (HttpContext.Current.Session [startDate] == null)
//					return DateTime.MinValue;
//				else
//					return (DateTime)HttpContext.Current.Session [startDate];
//			}
//
//			set
//			{
//				HttpContext.Current.Session [startDate] = value;
//			}
//		}
//
//		/// <summary>
//		///     EndDate is the latest date used to filter records.
//		/// </summary>
//		public static DateTime EndDate
//		{
//			get 
//			{
//				if (HttpContext.Current.Session [endDate] == null)
//					return DateTime.MaxValue;
//				else
//					return (DateTime)HttpContext.Current.Session [endDate];
//			}
//
//			set
//			{
//				HttpContext.Current.Session [endDate] = value;
//			}
//		}
		//---------------------------------------------------------------------
		#endregion
	}
}

