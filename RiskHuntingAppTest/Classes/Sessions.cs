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
		private const string sortBy = "SortBy";
		private const string currentRisk = "CURRENT_RISK";
		private const string currentProblemDesc = "CURRENT_PROBLEM_DESC";
		private const string currentPastRiskDesc = "CURRENT_PAST_RISK_DESC";
		private const string creativityPrompts = "CREATIVITY_PROMPTS";
		private const string creativityPromptsPastRisk = "CREATIVITY_PROMPTS_PAST_RISK";
		private const string currentResponseUri = "CurrentResponseUri";
		private const string currentOriginalID = "CurrentOriginalID";
		//---------------------------------------------------------------------
		#endregion

		#region Public Properties
		//---------------------------------------------------------------------


		public static string SortState
		{
			get {
				var obj = HttpContext.Current.Session [sortBy];
				if (obj == null)
					return String.Empty;
				return (string)obj;
			}

			set
			{
				HttpContext.Current.Session [sortBy] = value;
			}
		}

		public static string RiskState
		{
			get 
			{
				var obj = HttpContext.Current.Session [currentRisk];
				if (obj == null)
					return String.Empty;
				return (string)obj;
			}

			set
			{
				HttpContext.Current.Session [currentRisk] = value;
			}
		}
			
		public static List<NLResponseToken> ProblemDescState
		{
			get 
			{
				var obj = HttpContext.Current.Session [currentProblemDesc];
				if (obj == null)
					return new List<NLResponseToken>();
				return (List<NLResponseToken>)obj;
			}

			set
			{
				HttpContext.Current.Session [currentProblemDesc] = value;
			}
		}

		public static List<NLResponseToken> PastRiskDescState
		{
			get 
			{
				var obj = HttpContext.Current.Session [currentPastRiskDesc];
				if (obj == null)
					return new List<NLResponseToken>();
				return (List<NLResponseToken>)obj;
			}

			set
			{
				HttpContext.Current.Session [currentPastRiskDesc] = value;
			}
		}

		public static IList<string> CreativityPromptsState
		{
			get 
			{
				var obj = HttpContext.Current.Session [creativityPrompts];
				if (obj == null)
					return null;
				return (IList<string>)obj;
			}

			set
			{
				HttpContext.Current.Session [creativityPrompts] = value;
			}
		}

		public static IList<string> CreativityPromptsPastRiskState
		{
			get 
			{
				var obj = HttpContext.Current.Session [creativityPromptsPastRisk];
				if (obj == null)
					return null;
				return (IList<string>)obj;
			}

			set
			{
				HttpContext.Current.Session [creativityPromptsPastRisk] = value;
			}
		}

		public static string ResponseUriState
		{
			get 
			{
				var obj = HttpContext.Current.Session [currentResponseUri];
				if (obj == null)
					return String.Empty;
				return (string)obj;
			}

			set
			{
				HttpContext.Current.Session [currentResponseUri] = value;
			}
		}

		public static string PastRiskState
		{
			get 
			{
				var obj = HttpContext.Current.Session [currentOriginalID];
				if (obj == null)
					return String.Empty;
				return (string)obj;
			}

			set
			{
				HttpContext.Current.Session [currentOriginalID] = value;
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

